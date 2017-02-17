namespace Fibonacci.Initiator
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Fibonacci.BusinessLogic;

    public class CalculationController : ApiController
    {
        private readonly ICalculatorPool calculatorPool;

        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        public CalculationController(ICalculatorPool calculatorPool)
        {
            this.calculatorPool = calculatorPool;
        }

        [HttpPost]
        public async Task Calculate(Guid correlationId, string value)
        {
            await this.calculatorPool.Receive(new CalculationRequest(correlationId, BigInteger.Parse(value)));
        }
    }
}