namespace Fibonacci.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using Fibonacci.BusinessLogic;

    using log4net;

    using Moq;

    using Xunit;

    public class CalculatorPool_Specs
    {
        [Fact]
        public async Task CanAddNewCalculator()
        {
            var api = new Mock<ICalculatorApi>();
            var calculator = new Calculator(api.Object);
            var sut = new CalculatorPool(() => calculator, Mock.Of<ILog>());

            await sut.Receive(CalculationRequest.Initial);

            api.Verify(x => x.Send(It.IsAny<CalculationRequest>()));
        }

        [Fact]
        public async Task LogsNextReceivedRequest()
        {
            var api = new Mock<ICalculatorApi>();
            var calculator = new Calculator(api.Object);
            var log = new Mock<ILog>();
            var sut = new CalculatorPool(() => calculator, log.Object);

            var request = CalculationRequest.Initial;
            await sut.Receive(request);

            log.Verify(x => x.Info($"Calculating next value for {request.Value}"));
        }

        [Fact]
        public async Task UsesExistingCalculatorOnNextMessage()
        {
            var api = new Mock<ICalculatorApi>();
            api.Setup(x => x.Send(It.IsAny<CalculationRequest>())).Returns(Task.CompletedTask);

            var sut = new CalculatorPool(() => new Calculator(api.Object), Mock.Of<ILog>());

            var calculationRequest = CalculationRequest.Initial;
            await sut.Receive(calculationRequest);
            var nextRequest = calculationRequest.WithValue(42);

            await sut.Receive(nextRequest);

            api.Verify(x => x.Send(It.Is<CalculationRequest>(r => r.CorrelationId == calculationRequest.CorrelationId &&
                r.Value == calculationRequest.Value + nextRequest.Value)));
        }

        [Fact]
        public async Task CanAddNewCalculatorParallely()
        {
            var api = new Mock<ICalculatorApi>();
            api.Setup(x => x.Send(It.IsAny<CalculationRequest>())).Returns(Task.CompletedTask);

            var sut = new CalculatorPool(() => new Calculator(api.Object), Mock.Of<ILog>());
            const int InstanceCount = 42;

            var tasks = Enumerable.Range(0, InstanceCount).Select(_ => sut.Receive(CalculationRequest.Initial)).ToArray();

            await Task.WhenAll(tasks);

            api.Verify(x => x.Send(It.IsAny<CalculationRequest>()), Times.Exactly(InstanceCount));
        }
    }
}