namespace Fibonacci.Tests
{
    using System;
    using System.Threading.Tasks;

    using Fibonacci.BusinessLogic;

    using Moq;

    using Xunit;

    public class Calculator_Specs
    {
        private readonly Mock<ICalculatorApi> apiAgent;

        private readonly Calculator sut;

        public Calculator_Specs()
        {
            this.apiAgent = new Mock<ICalculatorApi>();
            this.sut = new Calculator(this.apiAgent.Object);
        }

        [Fact]
        public async Task CallsCalculatorApiWithStartDataOnInit()
        {
            await this.sut.Recieve(CalculationRequest.Initial);

            this.apiAgent.Verify(x => x.Send(It.Is<CalculationRequest>(r => r.Value == 1)));
        }

        [Theory]
        [InlineData(new long[] { 1 }, 2)]
        [InlineData(new long[] { 1, 3 }, 5)]
        [InlineData(new long[] { 1, 3, 8 }, 13)]
        public async Task CalculatesNextDigitOnReceive(long[] receiveInput, long expectedNext)
        {
            await this.sut.Recieve(CalculationRequest.Initial);

            foreach (var input in receiveInput)
            {
                await this.sut.Recieve(new CalculationRequest(Guid.NewGuid(), input));
            }

            this.apiAgent.Verify(x => x.Send(It.Is<CalculationRequest>(r => r.Value == expectedNext)));
        }
    }
}
