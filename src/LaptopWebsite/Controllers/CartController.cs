using LaptopWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LaptopWebsite.Controllers
{
    public class CartController : Controller
    {
        private LaptopDbContext db = new LaptopDbContext();

        // Lấy giỏ hàng hiện tại từ Session hoặc tạo mới nếu chưa có
        public List<CartItem> GetCart()
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }

        // Thêm sản phẩm vào giỏ
        public ActionResult AddToCart(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(p => p.ProductId == id);

            if (item == null) // Nếu chưa có sản phẩm này trong giỏ
            {
                var product = db.Products.Find(id);
                cart.Add(new CartItem
                {
                    ProductId = id,
                    ProductName = product.ProductName,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = 1
                });
            }
            else // Nếu đã có thì tăng số lượng
            {
                item.Quantity++;
            }
            return RedirectToAction("Index");
        }

        // Trang hiển thị giỏ hàng
        public ActionResult Index()
        {
            var cart = GetCart();
            ViewBag.TotalAmount = cart.Sum(s => s.TotalPrice);
            return View(cart);
        }

        // Xóa sản phẩm khỏi giỏ
        public ActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(p => p.ProductId == id);
            if (item != null) cart.Remove(item);
            return RedirectToAction("Index");
        }

        // 1. GET: Hiển thị trang xác nhận thanh toán
        public ActionResult Checkout()
        {
            if (Session["UserId"] == null)
            {
                // Bắt buộc đăng nhập mới được thanh toán
                //return RedirectToAction("Login", "Account");
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Url.PathAndQuery });
            }

            var cart = GetCart();
            if (cart.Count == 0) return RedirectToAction("Index", "Home");

            ViewBag.TotalAmount = cart.Sum(s => s.TotalPrice);
            return View();
        }

        // 2. POST: Xử lý lưu đơn hàng vào Database
        [HttpPost]
        public ActionResult ProcessCheckout(string shipAddress, string shipPhone)
        {
            var cart = GetCart();
            int userId = (int)Session["UserId"];

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Bước 1: Tạo mới Đơn hàng
                    var order = new Order
                    {
                        UserId = userId,
                        OrderDate = DateTime.Now,
                        TotalAmount = cart.Sum(s => s.TotalPrice),
                        Status = "Chờ xử lý"
                    };
                    db.Orders.Add(order);
                    db.SaveChanges(); // Lưu để lấy OrderId

                    // Bước 2: Tạo Chi tiết đơn hàng cho từng món trong giỏ
                    foreach (var item in cart)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.Price
                        };
                        db.OrderDetails.Add(orderDetail);
                    }
                    db.SaveChanges();

                    // Bước 3: Hoàn tất
                    transaction.Commit();
                    Session["Cart"] = null; // Xóa giỏ hàng sau khi đặt thành công

                    return RedirectToAction("OrderSuccess");
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return View("Error");
                }
            }
        }

        public ActionResult OrderSuccess()
        {
            return View();
        }
    }
}