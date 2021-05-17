using System.Web.Http;
using System.Web.Http.Cors;
using MeterReading.Logic;
using MeterReading.Logic.Facades;
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

            RegisterFacades(container);
            RegisterRepositories(container);
            RegisterValidators(container);
            RegisterOther(container);

            return container;
        }

        private static void RegisterOther(UnityContainer container)
        {
            container.RegisterSingleton<ICsvHelper, CsvHelper>();
        }

        private static void RegisterValidators(UnityContainer container)
        {
            container.RegisterSingleton<IMeterReadingLenientValidator, MeterReadingLenientValidator>();
        }

        private static void RegisterRepositories(UnityContainer container)
        {
            container.RegisterSingleton<IMeterReadingsRepository, MeterReadingsRepository>();
            container.RegisterSingleton<IAccountRepository, AccountRepository>();
        }

        private static void RegisterFacades(UnityContainer container)
        {
            container.RegisterSingleton<IMeterReadingFacade, MeterReadingFacade>();
            container.RegisterSingleton<IAccountFacade, AccountFacade>();
        }

        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("http://localhost:3000", "*", "*");
            config.EnableCors(cors);

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
