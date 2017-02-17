namespace Fibonacci.Tests
{
    using System.Threading.Tasks;

    using Fibonacci.BusinessLogic;
    using Fibonacci.Initiator;
    using Fibonacci.Responder.WebApiServer;

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

            await sut.Calculate(request.CorrelationId, request.Value.ToString());

            pool.Verify(
                x =>
                    x.Receive(
                        It.Is<CalculationRequest>(
                            r => r.CorrelationId == request.CorrelationId && r.Value == request.Value)));
        }
    }
}