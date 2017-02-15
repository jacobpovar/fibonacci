namespace Fibonacci.Initiator
{
    using System;
    using System.Net.Http;

    using log4net;
    using log4net.Config;

    using Microsoft.Owin.Hosting;

    internal class Program
    {
        private static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var log = LogManager.GetLogger("default");
            log.Info("Initiator A started");

            const string BaseAddress = "http://localhost:9000/";

            using (WebApp.Start<Startup>(BaseAddress))
            {
                // Create HttpCient and make a request to api/values
                var client = new HttpClient();

                var response = client.GetAsync(BaseAddress + "api/values").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }

            log.Info("Enter smth to stop");

            Console.ReadLine();
        }
    }
}