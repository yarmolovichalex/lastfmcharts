using System.Net.Http.Headers;
using System.Web.Http;

namespace LastFMCharts
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{input}",
                defaults: new { input = RouteParameter.Optional }
            );
        }
    }
}