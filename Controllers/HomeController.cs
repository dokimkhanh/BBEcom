using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyShop.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Categories()
        {
            var model = new CategoryDAO().GetCategoriesName();
            return PartialView(model);
        }

        public PartialViewResult NewsPost()
        {
            var model = new NewsDAO().GetNews();
            return PartialView(model);
        }

        public PartialViewResult ListProduct()
        {
           var list = new ProductDAO().GetListProduct(20);
            return PartialView(list);
        }
    }
}