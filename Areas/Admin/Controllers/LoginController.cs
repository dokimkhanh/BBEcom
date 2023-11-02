using KeyShop.Areas.Admin.Models;
using KeyShop.Common;
using Microsoft.Ajax.Utilities;
using Models;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace KeyShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        [HttpGet]
        public ActionResult Index()
        {
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