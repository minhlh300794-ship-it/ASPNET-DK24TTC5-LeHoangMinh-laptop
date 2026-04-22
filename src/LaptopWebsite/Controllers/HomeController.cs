using LaptopWebsite.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LaptopWebsite.Controllers
{
    public class HomeController : Controller
    {
        // Khởi tạo DbContext
        private LaptopDbContext db = new LaptopDbContext();

        public ActionResult Index(string searchString, int? categoryId)
        {
            // 1. Chuẩn bị dữ liệu cho Dropdown List (Combobox) chọn danh mục
            ViewBag.categoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");

            // Lưu lại từ khóa tìm kiếm để hiển thị lại trên ô input
            ViewBag.CurrentSearch = searchString;

            // 2. Lấy toàn bộ sản phẩm (chưa thực thi truy vấn ngay nhờ AsQueryable)
            var products = db.Products.Include("Category").AsQueryable();

            // 3. Lọc theo từ khóa (Tên sản phẩm hoặc Mô tả)
            if (!string.IsNullOrEmpty(searchString))
            {
                // Chuyển về chữ thường để tìm kiếm không phân biệt hoa thường
                products = products.Where(p => p.ProductName.ToLower().Contains(searchString.ToLower())
                                            || p.Description.ToLower().Contains(searchString.ToLower()));
            }

            // 4. Lọc theo Danh mục (Hãng)
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            // 5. Trả về kết quả cuối cùng (lúc này ToList() mới chính thức gọi xuống Database)
            return View(products.OrderByDescending(p => p.ProductId).ToList());
        }

        public ActionResult Details(int? id)
        {
            // Kiểm tra nếu không có ID truyền vào
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Tìm sản phẩm theo ID, bao gồm cả thông tin Danh mục
            Product product = db.Products.Include("Category").FirstOrDefault(p => p.ProductId == id);

            // Nếu không tìm thấy sản phẩm
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // Đừng quên giải phóng kết nối database
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}