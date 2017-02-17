namespace Fibonacci.Tests
{
    using System.Threading.Tasks;

    using Fibonacci.BusinessLogic;
    using Fibonacci.Responder;

    using MassTransit;
    using MassTransit.TestFramework;

    using Moq;

    using Xunit;

    public class CalculationRequestConsumer_Specs
    {
        [Fact]
        public async Task CallsCalculatorPool_With_ReceivedRequest()
        {
            var pool = new Mock<ICalculatorPool>();
            IConsumer<CalculationRequest> sut = new CalculationRequestConsumer(pool.Object);
            var request = CalculationRequest.Initial;

            await sut.Consume(new TestConsumeContext<CalculationRequest>(request));

            pool.Verify(x => x.Receive(request));
        }
    }
}
