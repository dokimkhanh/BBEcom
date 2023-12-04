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
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            var sess = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (sess != null) //had login
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {

                var dao = new AccountDAO();
                var res = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (res == 1)
                {
                    var user = dao.GetAccountByUsername(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserId = user.Id;
                    userSession.Name = user.Name;
                    userSession.IsAdmin = user.IsAdmin;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (res == -1)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn đã bị khoá");

                }
                else
                {
                    ModelState.AddModelError("", "Thông tin đăng nhập không hợp lệ");

                }
            }
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session.Remove(CommonConstants.USER_SESSION);
            return RedirectToAction("Index", "Login");
        }
    }
}