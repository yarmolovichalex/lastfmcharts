using System.Web.Mvc;
using System.Web.Routing;

namespace LastFMCharts
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
            //);

            routes.MapRoute(
                name: "Listen",
                url: "listen/{artistName}",
                defaults: new { controller = "Listen", action = "Index", artistName = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Compare",
                url: "{controller}/{action}",
                defaults: new { controller = "Chart", action = "Compare" }
            );
        }
    }
}
