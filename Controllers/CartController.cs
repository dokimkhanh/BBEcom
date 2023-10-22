using KeyShop.Models;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace KeyShop.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CART_SESSION";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session["CART_SESSION"];
            var list = new List<Cart>();
            if (cart != null)
            {
                list = (List<Cart>)cart;
            }
            return View(list);
        }

        public ActionResult AddItem(int productID, int quantity)
        {
            var product = new ProductDAO().GetProductById(productID);
            var cart = Session["CART_SESSION"];
            if (cart != null)
            {
                var list = (List<Cart>)cart;
                if (list.Exists(x => x.product.Id == productID))
                {
                    foreach (var item in list)
                    {
                        if (item.product.Id == productID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    var item = new Cart();
                    item.product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session["CART_SESSION"] = list;

            }
            else
            {
                var item = new Cart();
                item.product = product;
                item.Quantity = quantity;

                var list = new List<Cart>();
                list.Add(item);
                Session["CART_SESSION"] = list;
            }
            return RedirectToAction("Index");
        }

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<Cart>>(cartModel);
            var sesCart = (List<Cart>)Session["CART_SESSION"];
            foreach (var item in sesCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.product.Id == item.product.Id);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;

                }
            }
            Session["CART_SESSION"] = sesCart;

            return Json(new { status = true });

        }

        public JsonResult DeleteAllCart()
        {
            Session["CART_SESSION"] = null;
            return Json(new { status = true });
        }

        public JsonResult Delete(long id)
        {
            var sesCart = (List<Cart>)Session["CART_SESSION"];
            sesCart.RemoveAll(x => x.product.Id == id);
            Session["CART_SESSION"] = sesCart;
            return Json(new { status = true });
        }

        public ActionResult Payment()
        {
            var cart = Session["CART_SESSION"];
            var list = new List<Cart>();
            if (cart != null)
            {
                list = (List<Cart>)cart;
            }
            return View(list);
        }
    }
}