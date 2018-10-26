using System.Web.Mvc;
using System.Web.Routing;

namespace ecommerce
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Cart",
                "{controller}/{action}/{id}/{quantity}",
                new { controller = "Cart", action = "AddToCart" },
                new[] { "ecommerce.Controllers" }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "ecommerce.Controllers" }
            );
        }
    }
}
