using KeyShop.Common;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyShop.Areas.Admin.Controllers
{
    public class FaqController : BaseController
    {
        // GET: Admin/Faq
        public ActionResult Index(string txtSearch, int page = 1, int pageSize = 5)
        {
            var dao = new FaqDAO();
            var model = dao.GetFaqs(txtSearch, page, pageSize);
            ViewBag.Search = txtSearch;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Faq model)
        {
            if (ModelState.IsValid)
            {
                var res = new FaqDAO().AddFAQ(model);
                if (res > 0)
                {
                    SetAlert("Thêm danh mục thành công", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Thêm thất bại", "error");
                    ModelState.AddModelError("", "Không thể thêm");
                }
                return View(model);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new FaqDAO().GetFaq(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Faq model)
        {
            if (ModelState.IsValid)
            {
                var res = new FaqDAO().UpdateFAQ(model);
                if (res)
                {
                    SetAlert("Sửa thông tin FAQ thành công", "success");
                    return RedirectToAction("Index", "Faq");
                }
                else
                {
                    SetAlert("Thao tác thất bại", "error");
                    ModelState.AddModelError("", "Không thể sửa thông tin");
                }
            }
            return View();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var res = new FaqDAO().DeleteFaq(id);
            if (res)
            {
                SetAlert("Xoá thành công", "warning");
                return RedirectToAction("Index", "Category");
            }
            else
            {
                SetAlert("Thao tác thất bại", "error");
                ModelState.AddModelError("", "Không thể sửa thông tin");
            }
            return View("Index");
        }
    }
}