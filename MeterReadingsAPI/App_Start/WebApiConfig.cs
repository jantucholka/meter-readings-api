using System.Web.Http;
using MeterReading.Logic;
using MeterReading.Logic.Validators;
using MeterReadings.Repository;
using Unity;
using Unity.AspNet.WebApi;

namespace MeterReadingsAPI
{
    public static class WebApiConfig
    {
        public static IUnityContainer Container => CreateContainer();

        private static IUnityContainer CreateContainer()
        {
            var container = new UnityContainer();

            container.RegisterSingleton<IMeterReadingFacade, MeterReadingFacade>();
            container.RegisterSingleton<IMeterReadingsRepository, MeterReadingsRepository>();
            container.RegisterSingleton<IMeterReadingLenientValidator, MeterReadingLenientValidator>();

            return container;
        }

        public static void Register(HttpConfiguration config)
        {
            config.DependencyResolver = new UnityDependencyResolver(Container);

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
