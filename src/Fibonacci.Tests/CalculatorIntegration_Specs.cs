namespace Fibonacci.Tests
{
    using System.Numerics;
    using System.Threading.Tasks;

    using Fibonacci.BusinessLogic;

    using MassTransit.Logging;

    using Moq;

    using Xunit;

    public class CalculatorIntegration_Specs
    {
        private int count = 1;

        private readonly Mock<ICalculatorApi> firstApi;

        private readonly Mock<ICalculatorApi> secondApi;

        private readonly Calculator first;

        private readonly Calculator second;

        private readonly TaskCompletionSource<BigInteger> calculationResult;

        public CalculatorIntegration_Specs()
        {
            this.firstApi = new Mock<ICalculatorApi>();
            this.secondApi = new Mock<ICalculatorApi>();
            this.first = new Calculator(this.firstApi.Object);
            this.second = new Calculator(this.secondApi.Object);
            this.calculationResult = new TaskCompletionSource<BigInteger>();
        }

        [Theory]
        [InlineData(10, 55)]
        [InlineData(20, 6765)]
        [InlineData(50, 12586269025)]
        public async Task TwoCalculatorsCanCountUpTo(int max, long expected)
        {
            this.WireupCalculators(max);

            await this.first.Recieve(CalculationRequest.Initial);

            var result = await this.calculationResult.Task;
            Assert.Equal(expected, result);
        }

        private void WireupCalculators(int max)
        {
            this.firstApi.Setup(x => x.Send(It.IsAny<CalculationRequest>()))
                .Returns(Task.CompletedTask)
                .Callback(
                    async (CalculationRequest request) =>
                        {
                            if (this.count < max)
                            {
                                this.count++;
                                await this.second.Recieve(request);
                            }
                            else
                            {
                                this.calculationResult.SetResult(request.Value);
                            }
                        });

            this.secondApi.Setup(x => x.Send(It.IsAny<CalculationRequest>()))
                .Returns(Task.CompletedTask)
                .Callback(
                    async (CalculationRequest request) =>
                        {
                            if (this.count < max)
                            {
                                this.count++;
                                await this.first.Recieve(request);
                            }
                            else
                            {
                                this.calculationResult.SetResult(request.Value);
                            }
                        });
        }
    }
}