using InTimePro.Api.Services;
using Newtonsoft.Json;
using Owin;
using Swashbuckle.Application;
using System.Web.Http;

namespace InTimePro.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            var authService = new AuthService();
            config.DependencyResolver = new SimpleResolver(authService);

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            config.EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "InTimePro API");
                    c.DescribeAllEnumsAsStrings();
                })
                .EnableSwaggerUi();

            app.UseWebApi(config);
        }
    }
}
