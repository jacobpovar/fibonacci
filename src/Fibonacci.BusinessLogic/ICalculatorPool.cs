namespace Fibonacci.BusinessLogic
{
    using System.Threading.Tasks;

    public interface ICalculatorPool
    {
        Task Receive(CalculationRequest calculationRequest);
    }
}