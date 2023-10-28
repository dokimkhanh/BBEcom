using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KeyShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "Login",
              url: "dang-nhap",
              defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "KeyShop.Controllers" }
          );

            routes.MapRoute(
               name: "Profile",
               url: "trang-ca-nhan",
               defaults: new { controller = "Profile", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "KeyShop.Controllers" }
           );

            routes.MapRoute(
              name: "OrderHistory",
              url: "lich-su-mua-hang",
              defaults: new { controller = "Profile", action = "OrderHistory", id = UrlParameter.Optional },
              namespaces: new[] { "KeyShop.Controllers" }
          );

            routes.MapRoute(
               name: "Payment",
               url: "thanh-toan",
               defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional },
               namespaces: new[] { "KeyShop.Controllers" }
           );

            routes.MapRoute(
               name: "CartAddItem",
               url: "gio-hang/them-{productID}-{quantity}",
               defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
               namespaces: new[] { "KeyShop.Controllers" }
           );

            routes.MapRoute(
               name: "Cart",
               url: "gio-hang",
               defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "KeyShop.Controllers" }
           );

            routes.MapRoute(
               name: "ProductDetail",
               url: "san-pham/{alias}-{id}",
               defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
               namespaces: new[] { "KeyShop.Controllers" }
           );

            routes.MapRoute(
                name: "ProductCategory",
                url: "danh-muc/{alias}-{id}",
                defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                namespaces: new[] { "KeyShop.Controllers" }
            );

            routes.MapRoute(
               name: "404",
               url: "page-404",
               defaults: new { controller = "Error", action = "PageNotFound", id = UrlParameter.Optional },
               namespaces: new[] { "KeyShop.Controllers" }
           );

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "KeyShop.Controllers" }
           );
        }
    }
}
