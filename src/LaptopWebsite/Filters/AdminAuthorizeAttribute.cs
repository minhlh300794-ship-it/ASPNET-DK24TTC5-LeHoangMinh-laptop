using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LaptopWebsite.Filters
{
    // Kế thừa từ ActionFilterAttribute để can thiệp vào trước khi Action chạy
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Kiểm tra Session Role
            var role = HttpContext.Current.Session["Role"];

            // Nếu chưa đăng nhập hoặc không phải Admin
            if (role == null || role.ToString() != "Admin")
            {
                // Đá về trang Đăng nhập
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Account", action = "Login", area = "" })
                );
            }

            base.OnActionExecuting(filterContext);
        }
    }
}