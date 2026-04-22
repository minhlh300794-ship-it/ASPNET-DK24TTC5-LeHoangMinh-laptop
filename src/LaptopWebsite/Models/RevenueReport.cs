using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaptopWebsite.Models
{
    public class RevenueReport
    {
        public string MonthYear { get; set; }  // Ví dụ: "04/2026"
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
    }
}