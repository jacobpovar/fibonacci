namespace Fibonacci.Tests
{
    using System.Threading.Tasks;

    using Fibonacci.BusinessLogic;
    using Fibonacci.Initiator;

    using Moq;

    using Xunit;

    public class CalculationController_Specs
    {
        [Fact]
        public async Task CallsCalculatorPool_With_ReceivedRequest()
        {
            var pool = new Mock<ICalculatorPool>();
            var sut = new CalculationController(pool.Object);
            var request = CalculationRequest.Initial;

            await sut.Calculate(request);

            pool.Verify(x => x.Receive(request));
        }
    }
}