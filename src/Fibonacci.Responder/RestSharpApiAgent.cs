namespace Fibonacci.Responder
{
    using System.Threading.Tasks;

    using Fibonacci.BusinessLogic;
    using Fibonacci.Contracts;

    using RestSharp;

    public class RestSharpApiAgent : ICalculatorApi
    {
        public async Task Send(CalculationRequest calculationRequest)
        {
            var client = new RestClient(Settings.ApiAddress);

            var request = new RestRequest("api/calculation", Method.POST);

            request.AddJsonBody(calculationRequest);

            await client.ExecutePostTaskAsync(request).ConfigureAwait(false);
        }
    }
}
