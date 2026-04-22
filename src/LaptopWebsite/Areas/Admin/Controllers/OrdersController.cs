using LaptopWebsite.Filters; // Chứa ổ khóa AdminAuthorize
using LaptopWebsite.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace LaptopWebsite.Areas.Admin.Controllers
{
    [AdminAuthorize] // Khóa chặn người lạ
    public class OrdersController : Controller
    {
        private LaptopDbContext db = new LaptopDbContext();

        // 1. Danh sách đơn hàng
        public ActionResult Index()
        {
            // Lấy danh sách đơn hàng kèm thông tin Khách hàng, đơn mới nhất xếp lên đầu
            var orders = db.Orders.Include(o => o.User).OrderByDescending(o => o.OrderDate).ToList();
            return View(orders);
        }

        // 2. Chi tiết đơn hàng
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            // Tìm đơn hàng
            var order = db.Orders.Include(o => o.User).FirstOrDefault(o => o.OrderId == id);
            if (order == null) return HttpNotFound();

            // Lấy danh sách sản phẩm nằm trong đơn hàng này gửi qua ViewBag
            ViewBag.OrderDetails = db.OrderDetails.Include(od => od.Product)
                                     .Where(od => od.OrderId == id).ToList();

            return View(order);
        }

        // 3. Action xử lý Cập nhật trạng thái
        [HttpPost]
        public ActionResult UpdateStatus(int orderId, string status)
        {
            var order = db.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = status;
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = orderId });
        }
    }
}