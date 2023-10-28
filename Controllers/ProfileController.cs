using KeyShop.Areas.Admin.Models;
using KeyShop.Common;
using Models.DAO;
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
            var member = (UserLogin)Session[KeyShop.Common.CommonConstants.USER_SESSION];
            if (member == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var data = new AccountDAO().GetAccountById(member.UserId);
                return View(data);
            }
        }

        public ActionResult ProfileMenu()
        {
            return PartialView();
        }

        public ActionResult OrderHistory()
        {
            var user = (UserLogin)Session[KeyShop.Common.CommonConstants.USER_SESSION];
            if (user != null)
            {
                var dataOrder = new OrderDAO().GetListOrderByCustomerID(user.UserId);
                return View(dataOrder);
            }
            return View();
        }
    }
}