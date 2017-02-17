namespace Fibonacci.Tests
{
    using System.Threading.Tasks;

    using Fibonacci.BusinessLogic;
    using Fibonacci.Initiator;
    using Fibonacci.Responder;

    using Moq;

    using StructureMap;

    using Xunit;

    public class ApiIntegration_Specs
    {
        [Fact]
        public async Task ApiCanBeCalled()
        {
            var pool = new Mock<ICalculatorPool>();
            var container =
                new Container(expression => { expression.For<ICalculatorPool>().Use(pool.Object); });

            using (new CalculationApiServer(container).Start())
            {
                var sut = new RestSharpApiAgent();
                var calculationRequest = CalculationRequest.Initial;

                await sut.Send(calculationRequest);

                pool.Verify(
                    x =>
                        x.Receive(
                            It.Is<CalculationRequest>(
                                r =>
                                    r.CorrelationId == calculationRequest.CorrelationId
                                    && r.Value == calculationRequest.Value)));
            }
        }
    }
}