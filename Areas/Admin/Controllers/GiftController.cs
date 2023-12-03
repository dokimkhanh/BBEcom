using KeyShop.Common;
using Models.DAO;
using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace KeyShop.Areas.Admin.Controllers
{
    public class GiftController : BaseController
    {
        // GET: Admin/Gift
        public ActionResult Index(string txtSearch, int page = 1, int pageSize = 10)
        {
            var dao = new GiftcardDAO();
            var model = dao.GetGifts(txtSearch, page, pageSize);
            ViewBag.Search = txtSearch;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.productID = new SelectList(new ProductDAO().GetListProduct(null), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string codeList, int productId)
        {
            var dao = new GiftcardDAO();
            if (ModelState.IsValid)
            {
                string[] separators = { "\r\n" };
                string[] codeArray = codeList.Split(separators, StringSplitOptions.RemoveEmptyEntries);


                foreach (var item in codeArray)
                {
                    var res = dao.AddGift(new Giftcard()
                    {
                        ProductId = productId,
                        Code = item
                    });
                    if (res < 0)
                    {
                        SetAlert("Thêm thất bại", "error");
                        ModelState.AddModelError("", "Thêm thất bại, có lỗi xảy ra");
                    }
                }
                SetAlert("Thêm giftcode thành công", "success");
                return RedirectToAction("Index", "Gift");

            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var gift = new GiftcardDAO().GetGiftbyId(id);
            ViewBag.productID = new SelectList(new ProductDAO().GetListProduct(null), "Id", "Name");
            return View(gift);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(Giftcard model)
        {
            var dao = new GiftcardDAO();
            if (ModelState.IsValid)
            {
                var res = new GiftcardDAO().UpdateGift(model);

                if (!res)
                {
                    SetAlert("Thêm thất bại", "error");
                    ModelState.AddModelError("", "Thêm thất bại, có lỗi xảy ra");
                }
                else
                {
                    SetAlert("Sửa dữ liệu giftcode thành công", "success");
                    return RedirectToAction("Index", "Gift");
                }
            }
            return View("Index");

        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var res = new GiftcardDAO().DeleteGift(id);
            if (res)
            {
                SetAlert("Xoá dữ liệu thành công", "warning");
            }
            else
            {
                SetAlert("Thao tác thất bại", "error");
            }
            return RedirectToAction("Index");
        }
    }
}