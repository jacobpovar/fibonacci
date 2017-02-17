namespace Fibonacci.BusinessLogic
{
    using System;

    public class CalculationRequest
    {
        public CalculationRequest(Guid correlationId, long value)
        {
            this.CorrelationId = correlationId;
            this.Value = value;
        }

        public Guid CorrelationId { get; set; }

        public long Value { get; set; }

        public static CalculationRequest Initial => new CalculationRequest(Guid.NewGuid(), 1);

        public CalculationRequest WithValue(long value)
        {
            return new CalculationRequest(this.CorrelationId, value);
        }
    }
}