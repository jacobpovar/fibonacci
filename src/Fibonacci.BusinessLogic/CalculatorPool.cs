namespace Fibonacci.BusinessLogic
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using log4net;

    public class CalculatorPool : ICalculatorPool
    {
        private readonly Func<Calculator> calculatorFactory;

        private readonly ILog log;

        private readonly ConcurrentDictionary<Guid, Calculator> dictionary = new ConcurrentDictionary<Guid, Calculator>();

        public CalculatorPool(Func<Calculator> calculatorFactory, ILog log)
        {
            this.calculatorFactory = calculatorFactory;
            this.log = log;
        }

        public async Task Receive(CalculationRequest calculationRequest)
        {
            var calculator = this.dictionary.GetOrAdd(calculationRequest.CorrelationId, _ => this.calculatorFactory());
            this.log.Info($"Calculating next value for {calculationRequest.Value}");
            await calculator.Recieve(calculationRequest);
        }
    }
}
