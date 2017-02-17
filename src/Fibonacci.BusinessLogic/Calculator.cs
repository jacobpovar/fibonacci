namespace Fibonacci.BusinessLogic
{
    using System.Numerics;
    using System.Threading.Tasks;

    public class Calculator
    {
        private BigInteger previous = 0;

        private readonly ICalculatorApi apiAgent;

        public Calculator(ICalculatorApi apiAgent)
        {
            this.apiAgent = apiAgent;
        }

        public async Task Recieve(CalculationRequest request)
        {
            var value = this.previous + request.Value;
            this.previous = value;
            await this.apiAgent.Send(new CalculationRequest(request.CorrelationId, value));
        }
    }
}