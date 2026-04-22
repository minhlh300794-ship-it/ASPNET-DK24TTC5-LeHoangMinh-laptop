using LaptopWebsite.Filters;
using LaptopWebsite.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LaptopWebsite.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class ReportsController : Controller
    {
        private LaptopDbContext db = new LaptopDbContext();

        public ActionResult Index()
        {
            // 1. Chỉ lấy những đơn hàng đã giao thành công
            // (Dùng .ToList() ở đây để kéo dữ liệu về bộ nhớ trước khi xử lý ngày tháng)
            var completedOrders = db.Orders.Where(o => o.Status == "Đã giao").ToList();

            // 2. Nhóm dữ liệu theo Tháng và Năm, tính tổng tiền và số đơn
            var reportData = completedOrders
                .GroupBy(o => new { o.OrderDate.Month, o.OrderDate.Year })
                .Select(g => new RevenueReport
                {
                    MonthYear = g.Key.Month.ToString("00") + "/" + g.Key.Year,
                    TotalRevenue = g.Sum(o => o.TotalAmount),
                    TotalOrders = g.Count()
                })
                .OrderBy(r => r.MonthYear)
                .ToList();

            // 3. Chuẩn bị chuỗi dữ liệu (Labels và Data) để gửi sang View vẽ Chart.js
            ViewBag.Labels = string.Join(",", reportData.Select(r => $"\"{r.MonthYear}\""));
            ViewBag.Data = string.Join(",", reportData.Select(r => r.TotalRevenue));

            return View(reportData);
        }
    }
}