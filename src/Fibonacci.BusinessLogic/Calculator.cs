namespace Fibonacci.BusinessLogic
{
    using System.Threading.Tasks;

    public class Calculator
    {
        private long previous = 0;

        private readonly ICalculatorApi apiAgent;

        public Calculator(ICalculatorApi apiAgent)
        {
            this.apiAgent = apiAgent;
        }

        public async Task InitializeCalculation()
        {
            await this.Recieve(1);
        }

        public async Task Recieve(long next)
        {
            var value = this.previous + next;
            this.previous = value;
            await this.apiAgent.Send(new CalculateRequest { Value = value });
        }
    }
}