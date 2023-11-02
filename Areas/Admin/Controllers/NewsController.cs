using KeyShop.Common;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyShop.Areas.Admin.Controllers
{
    public class NewsController : BaseController
    {
        // GET: Admin/News
        public ActionResult Index(string txtSearch, int page = 1, int pageSize = 10)
        {
            var dao = new NewsDAO();
            var model = dao.GetAllNews(txtSearch, page, pageSize);
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
        public ActionResult Create(News model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.Alias = CommonHelper.ChuyenCoDauThanhKhongDau(model.Title);
                var res = new NewsDAO().AddNews(model);
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
            var news = new NewsDAO().GetNewsById(id);
            return View(news);
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News model)
        {
            if (ModelState.IsValid)
            {
                model.Alias = CommonHelper.ChuyenCoDauThanhKhongDau(model.Title);
                var res = new NewsDAO().UpdateNews(model);
                if (res)
                {
                    SetAlert("Sửa thông tin thành công", "success");
                    return RedirectToAction("Index", "News");
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
            var res = new NewsDAO().DeleteNews(id);
            if (res)
            {
                SetAlert("Xoá tin tức thành công", "warning");
            }
            else
            {
                SetAlert("Thao tác thất bại", "error");
            }
            return RedirectToAction("Index");
        }
    }
}