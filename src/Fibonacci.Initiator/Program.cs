namespace Fibonacci.Initiator
{
    using System;
    using System.Threading.Tasks;

    using Fibonacci.BusinessLogic;

    using log4net;
    using log4net.Config;

    using MassTransit;

    using StructureMap;

    public class Program
    {
        private static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var threadsCount = GetThreadsCount(args);

            var bus = CreateBus();
            var container = new Container(
                c =>
                    {
                        c.For<ILog>().Use(_ => LogManager.GetLogger("default"));
                        c.ForSingletonOf<IBus>().Use(bus).Singleton();
                        c.For<ICalculatorPool>().Use<CalculatorPool>().Singleton();
                        c.For<ICalculatorApi>().Use<MasstransitCalculatorApi>();
                    });

            bus.Start();

            var log = container.GetInstance<ILog>();
            log.Info($"Initiator A started, using {threadsCount} threads");

            var pool = container.GetInstance<ICalculatorPool>();

            using (new CalculationApiServer(container).Start())
            {
                log.Debug("starting calculation");

                Parallel.For(0, threadsCount, _ => pool.Receive(CalculationRequest.Initial));

                log.Info("Enter smth to stop");

                Console.ReadLine();
            }
        }

        private static IBusControl CreateBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(
                configurator =>
                    {
                        configurator.Host(
                            new Uri("rabbitmq://localhost"),
                            hostConfigurator =>
                                {
                                    hostConfigurator.Username("guest");
                                    hostConfigurator.Password("guest");
                                });
                    });
        }

        private static int GetThreadsCount(string[] args)
        {
            int threadsCount;

            if (args.Length == 1 && int.TryParse(args[0], out threadsCount) && threadsCount > 0)
            {
                return threadsCount;
            }

            return Environment.ProcessorCount;
        }
    }
}