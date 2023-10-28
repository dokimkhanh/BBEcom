using KeyShop.Common;
using KeyShop.Models;
using Microsoft.Ajax.Utilities;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace KeyShop.Controllers
{
    public class CartController : MemberBaseController
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

        [HttpGet]
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

        [HttpPost]
        public ActionResult Payment(Order _order)
        {
            int price = 0;
            var code = new { Status = false, Url = "" };

            Random r = new Random();
            var orderCode = "DH" + r.Next(0, 99) + r.Next(0, 99) + r.Next(0, 99);
            var cart = Session["CART_SESSION"];
            var list = new List<Cart>();
            if (cart != null)
            {
                list = (List<Cart>)cart;

                foreach (var item in list)
                {
                    int money = item.product.PriceSale > 0 ? item.product.PriceSale.GetValueOrDefault(0) : item.product.Price;
                    price += money * item.Quantity;
                }
                var url = UrlPayment(price, orderCode);
                code = new { Status = true, Url = url };
            }
            return Json(code);
        }

        public string UrlPayment(int price, string orderCode)
        {
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (price * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_Locale", "vn");
            //vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            //vnpay.AddRequestData("vnp_BankCode", "VNBANK");

            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + orderCode);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", orderCode.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return paymentUrl;
        }

        [HttpGet]
        public ActionResult VnpayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var dao = new OrderDAO();
                        var sess = (UserLogin)Session[KeyShop.Common.CommonConstants.USER_SESSION];
                        var customerID = sess.UserId;

                        var cart = Session["CART_SESSION"];
                        var list = new List<Cart>();
                        if (cart != null)
                        {
                            list = (List<Cart>)cart;
                        }

                        Order order = new Order();
                        order.OrderCode = orderCode;
                        order.CustomerID = customerID;
                        order.TotalAmount = (int)vnp_Amount;
                        order.Status = true;
                        order.CreatedDate = DateTime.Now;

                        foreach (var item in list)
                        {
                            for (int i = 0; i < item.Quantity; i++)
                            {
                                var newOrderDetail = new OrderDetail
                                {
                                    ProductID = item.product.Id,
                                    Price = item.product.PriceSale > 0 ? item.product.PriceSale.GetValueOrDefault(0) : item.product.Price,
                                    Quantity = item.Quantity,
                                    Code = new GiftcardDAO().TakeAndDelete(item.product.Id).Code
                                };

                                order.OrderDetail.Add(newOrderDetail);
                            }
                            
                        }

                        dao.AddOrder(order);

                        //list.ToList().ForEach(x => order.OrderDetail.Add(new OrderDetail
                        //{

                        //    ProductID = x.product.Id,
                        //    Price = x.product.PriceSale > 0 ? x.product.PriceSale.GetValueOrDefault(0) : x.product.Price,
                        //    Quantity = x.Quantity,
                        //    Code = new GiftcardDAO().TakeAndDelete(x.product.Id).Code
                        //}));
                        Session["CART_SESSION"] = null;

                        //var itemOrder = db.Orders.FirstOrDefault(x => x.Code == orderCode);
                        //if (itemOrder != null)
                        //{
                        //    itemOrder.Status = 2;//đã thanh toán
                        //    db.Orders.Attach(itemOrder);
                        //    db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                        //    db.SaveChanges();
                        //}

                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        ViewBag.SoTien = vnp_Amount.ToString();
                        ViewBag.TrangThai = true;
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.TrangThai = false;
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý. Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();

                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
            }
            return View();
        }
    }
}