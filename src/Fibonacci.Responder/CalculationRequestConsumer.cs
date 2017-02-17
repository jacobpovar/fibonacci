namespace Fibonacci.Responder
{
    using System.Threading.Tasks;

    using Fibonacci.BusinessLogic;

    using MassTransit;

    public class CalculationRequestConsumer : IConsumer<CalculationRequest>
    {
        private readonly ICalculatorPool pool;

        public CalculationRequestConsumer(ICalculatorPool pool)
        {
            this.pool = pool;
        }

        public async Task Consume(ConsumeContext<CalculationRequest> context)
        {
            await this.pool.Receive(context.Message).ConfigureAwait(false);
        }
    }
}