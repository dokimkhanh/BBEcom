using KeyShop.Common;
using Models.DAO;
using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyShop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index(string txtSearch, int page = 1, int pageSize = 10)
        {
            SetViewBag();
            var list = new ProductDAO().GetProducts(txtSearch, page, pageSize);
            ViewBag.Search = txtSearch;
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product model)
        {
            SetViewBag();
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.Alias = CommonHelper.ChuyenCoDauThanhKhongDau(model.Name);
                var res = new ProductDAO().AddProduct(model);
                if (res > 0)
                {
                    SetAlert("Thêm người dùng thành công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    SetAlert("Thêm thất bại", "error");
                    ModelState.AddModelError("", "Thêm người dùng thất bại");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new ProductDAO();
            var product = dao.GetProductById(id);
            SetViewBag(product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDAO();
                model.Alias = CommonHelper.ChuyenCoDauThanhKhongDau(model.Name);
                var res = dao.UpdateProduct(model);
                if (res)
                {
                    SetAlert("Sửa thông tin sản phẩm thành công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    SetAlert("Thao tác thất bại", "error");
                    ModelState.AddModelError("", "Không thể sửa thông tin");
                }
            }
            SetViewBag(model.CategoryId);
            return View();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var res = new ProductDAO().DeleteProduct(id);
            if (res)
            {
                SetAlert("Xoá thành công", "warning");
                return RedirectToAction("Index", "Product");
            }
            else
            {
                SetAlert("Thao tác thất bại", "error");
                ModelState.AddModelError("", "Không thể sửa thông tin");
            }
            return View("Index");
        }

        public void SetViewBag(int? selectedID = null)
        {
            var dao = new CategoryDAO();
            ViewBag.productCategoryID = new SelectList(dao.GetCategoriesName(null), "Id", "Name", selectedID);
        }
    }
}