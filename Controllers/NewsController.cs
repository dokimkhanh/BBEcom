using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyShop.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult View(int id)
        {
            var news = new NewsDAO().GetNewsById(id);
            return View(news);
        }
    }
}