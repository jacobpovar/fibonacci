namespace Fibonacci.BusinessLogic
{
    using System;
    using System.Numerics;

    public class CalculationRequest
    {
        public CalculationRequest(Guid correlationId, BigInteger value)
        {
            this.CorrelationId = correlationId;
            this.Value = value;
        }

        public Guid CorrelationId { get; set; }

        public BigInteger Value { get; set; }

        public static CalculationRequest Initial => new CalculationRequest(Guid.NewGuid(), 1);

        public CalculationRequest WithValue(BigInteger value)
        {
            return new CalculationRequest(this.CorrelationId, value);
        }
    }
}