using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaptopWebsite.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Thành tiền của một loại sản phẩm
        public decimal TotalPrice => Price * Quantity;
    }
}