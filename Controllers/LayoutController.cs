using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyShop.Controllers
{
    public class LayoutController : Controller
    {
        // GET: Layout
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NavbarMenu()
        {
            ViewBag.MostProduct = new ProductDAO().GetListProduct(6);
            ViewBag.MostCategory = new CategoryDAO().GetCategoriesName(4);
            return PartialView();
        }
    }
}