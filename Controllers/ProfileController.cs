using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyShop.Controllers
{
    public class ProfileController : MemberBaseController
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
    }
}