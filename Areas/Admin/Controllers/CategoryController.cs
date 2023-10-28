using KeyShop.Areas.Admin.Models;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using KeyShop.Common;

namespace KeyShop.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        public ActionResult Index(string txtSearch, int page = 1, int pageSize = 10)
        {
            var dao = new CategoryDAO();
            var list = dao.GetCategories(txtSearch, page, pageSize);
            ViewBag.Search = txtSearch;
            return View(list);
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = new CategoryDAO();
                    collection.Alias = CommonHelper.ChuyenCoDauThanhKhongDau(collection.Name);
                    collection.CreatedDate = DateTime.Now;
                    int res = model.AddCategory(collection);
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
                }
                return View(collection);
            }
            catch
            {

            }
            return View();
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int id)
        {
            var categoryInfo = new CategoryDAO().GetCategoryById(id);
            return View(categoryInfo);
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var dao = new CategoryDAO();
                category.Alias = CommonHelper.ChuyenCoDauThanhKhongDau(category.Name);
                var res = dao.UpdateCategory(category);
                if (res)
                {
                    SetAlert("Sửa thông tin danh mục thành công", "success");
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    SetAlert("Thao tác thất bại", "error");
                    ModelState.AddModelError("", "Không thể sửa thông tin");
                }
            }
            return View("Index");
        }

        // POST: Admin/Category/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var res = new CategoryDAO().DeleteCategory(id);
            if (res)
            {
                SetAlert("Xoá danh mục thành công", "warning");
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
