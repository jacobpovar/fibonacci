namespace Fibonacci.Initiator
{
    using System.Collections.Generic;
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
        public async Task Calculate([FromBody] CalculationRequest request)
        {
            await this.calculatorPool.Receive(request);
        }
    }
}