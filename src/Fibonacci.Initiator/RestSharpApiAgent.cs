namespace Fibonacci.Initiator
{
    using System;
    using System.Threading.Tasks;

    using Fibonacci.BusinessLogic;
    using Fibonacci.Contracts;

    using Polly;

    using RestSharp;

    public class RestSharpApiAgent : ICalculatorApi
    {
        public async Task Send(CalculationRequest calculationRequest)
        {
            var retryForever = Policy.Handle<Exception>().RetryForeverAsync();
            await retryForever.ExecuteAsync(async () => await CallApi(calculationRequest));
        }

        private static async Task CallApi(CalculationRequest calculationRequest)
        {
            var client = new RestClient(Settings.ApiAddress);

            var request = new RestRequest("api/calculation", Method.POST);
            request.Timeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;

            request.AddParameter("correlationId", calculationRequest.CorrelationId, ParameterType.QueryString);
            request.AddParameter("value", calculationRequest.Value.ToString(), ParameterType.QueryString);

            var restResponse = await client.ExecutePostTaskAsync(request).ConfigureAwait(false);
            if (restResponse.ErrorException != null)
            {
                throw new Exception("failed to call other side", restResponse.ErrorException);
            }
        }
    }
}
