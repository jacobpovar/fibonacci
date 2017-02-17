namespace Fibonacci.Responder
{
    using System;
    using System.Threading.Tasks;

    using Fibonacci.BusinessLogic;
    using Fibonacci.Responder.WebApiServer;

    using log4net;
    using log4net.Config;

    using MassTransit;
    using MassTransit.Log4NetIntegration;

    using StructureMap;

    public class Program
    {
        private static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var container = new Container();

            var bus = CreateBus();

            container.Configure(c =>
            {
                c.For<ILog>().Use(_ => LogManager.GetLogger("default"));
                c.ForSingletonOf<IBus>().Use(() => bus).Singleton();
                c.For<ICalculatorPool>().Use<CalculatorPool>().Singleton();
                c.For<ICalculatorApi>().Use<MasstransitCalculatorApi>();
            });

            bus.Start();

            var log = container.GetInstance<ILog>();

            using (new CalculationApiServer(container).Start())
            {
                log.Info("Enter smth to stop");

                Console.ReadLine();
            }

            log.Info("Enter smth to stop");
            Console.ReadLine();
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

                    configurator.UseLog4Net();
                });
        }
    }
}
