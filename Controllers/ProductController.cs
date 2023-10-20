using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Category(int id)
        {
            var dao = new CategoryDAO();
            var cateName = dao.GetCategoryById(id);
            ViewBag.CategoryProductCount = dao.GetCountProductCategory(id);
            return View(cateName);
        }

        public ActionResult Detail(int id)
        {
            var product = new ProductDAO().GetProductById(id);
            return View(product);
        }

        public ActionResult Products(int id, int page = 1, int pageSize = 8)
        {
            var product = new ProductDAO().GetListProductByCategory(id, page, pageSize);
            return PartialView(product);
        }
    }
}