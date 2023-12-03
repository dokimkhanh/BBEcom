using KeyShop.Common;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyShop.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
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
        public ActionResult Register(Account model)
        {
            model.Status = true;
            model.IsAdmin = false;
            model.Password = Encryptor.MD5Hash(model.Password);
            if (ModelState.IsValid)
            {
                var dao = new AccountDAO();
                var checkExitsUser = dao.GetAccountByUsername(model.UserName);
                if (checkExitsUser == null)
                {
                    var res = dao.InsertUser(model);
                    if (res > 0)
                    {
                        ModelState.AddModelError("", "Đăng ký thành viên mới thành công!");
                        return View("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể đăng ký tài khoản");
                    }
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập này đã tồn tại!");
                }
            }
            return View("Index");
        }
    }
}