namespace Fibonacci.BusinessLogic
{
    using System.Threading.Tasks;

    public interface ICalculatorApi
    {
        Task Send(CalculationRequest calculationRequest);
    }
}