using LaptopWebsite.Models;
using System.Linq;
using System.Web.Mvc;

namespace LaptopWebsite.Controllers
{
    public class AccountController : Controller
    {
        private LaptopDbContext db = new LaptopDbContext();

        // Bản GET: Nhận returnUrl từ URL
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // Bản POST: Xử lý đăng nhập
        [HttpPost]
        public ActionResult Login(string email, string password, string returnUrl)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                Session["UserId"] = user.UserId;
                Session["FullName"] = user.FullName;
                Session["Role"] = user.Role;

                // Nếu có returnUrl thì quay lại trang đó, không thì mới về Home/Admin
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return user.Role == "Admin" ? RedirectToAction("Index", "Products", new { area = "Admin" }) : RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Sai thông tin!";
            return View();
        }

        // 3. Đăng xuất
        public ActionResult Logout()
        {
            Session.Clear(); // Xóa toàn bộ Session
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}