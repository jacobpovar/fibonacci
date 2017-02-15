using System;
using System.Net.Http;
using log4net;
using log4net.Config;

namespace Fibonacci.Initiator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var log = LogManager.GetLogger("default");

            log.Info("Initiator A started");

            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();

                var response = client.GetAsync(baseAddress + "api/values").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }

            log.Info("Enter smth to stop");

            Console.ReadLine();
        }
    }

    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }
}