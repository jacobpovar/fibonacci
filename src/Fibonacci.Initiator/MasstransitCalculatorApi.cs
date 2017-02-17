namespace Fibonacci.Initiator
{
    using System.Threading.Tasks;

    using Fibonacci.BusinessLogic;

    using MassTransit;

    internal class MasstransitCalculatorApi : ICalculatorApi
    {
        private readonly IBus bus;

        public MasstransitCalculatorApi(IBus bus)
        {
            this.bus = bus;
        }

        public async Task Send(CalculationRequest calculationRequest)
        {
            await this.bus.Publish(calculationRequest).ConfigureAwait(false);
        }
    }
}