using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using KeyShop.Common;
using Models.DAO;
using Models.EF;
using PagedList;

namespace KeyShop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index(string txtSearch, int page = 1, int pageSize = 10)
        {
            var dao = new AccountDAO();
            var users = dao.GetListAccounts(txtSearch, page, pageSize);
            ViewBag.Search = txtSearch;
            return View(users);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Account acc)
        {
            if (ModelState.IsValid)
            {
                var dao = new AccountDAO();
                acc.Password = Encryptor.MD5Hash(acc.Password);
                int id = dao.InsertUser(acc);
                if (id > 0)
                {
                    SetAlert("Thêm người dùng thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    SetAlert("Thêm thất bại", "error");
                    ModelState.AddModelError("", "Thêm người dùng thất bại");
                }
               
            }
            return View("Index");

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var userInfo = new AccountDAO().GetAccountById(id);
            return View(userInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account acc)
        {
            if (ModelState.IsValid)
            {
                var dao = new AccountDAO();

                if (!string.IsNullOrEmpty(acc.Password))
                {
                    var encrypPass = Encryptor.MD5Hash(acc.Password);
                    acc.Password = encrypPass;
                }

                var res = dao.UpdateUser(acc);
                if (res)
                {
                    SetAlert("Sửa thông tin người dùng thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    SetAlert("Thao tác thất bại", "error");
                    ModelState.AddModelError("", "Không thể sửa thông tin");
                }
            }
            return View("Index");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var dao = new AccountDAO();
            var res = dao.DeleteUser(id);
            if (res)
            {
                SetAlert("Xoá người dùng thành công", "warning");
            }
            else
            {
                SetAlert("Thao tác thất bại", "error");
            }
            return RedirectToAction("Index");
        }

    }
}