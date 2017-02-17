namespace Fibonacci.Responder.WebApiServer
{
    using System;
    using System.Web.Http;

    using Fibonacci.Contracts;

    using Microsoft.Owin.Hosting;

    using Owin;

    using StructureMap;

    public class CalculationApiServer
    {
        private readonly Container container;

        public CalculationApiServer(Container container)
        {
            this.container = container;
        }

        public IDisposable Start()
        {
            return WebApp.Start(new StartOptions { Urls = { Settings.ApiAddress } }, this.Configure);
        }

        private void Configure(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration
                             {
                                 DependencyResolver =
                                     new StructureMapWebApiDependencyResolver(this.container)
                             };

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            appBuilder.UseWebApi(config);
        }
    }
}