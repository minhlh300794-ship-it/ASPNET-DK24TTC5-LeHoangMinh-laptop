using LaptopWebsite.Models;
using System.Data.Entity;

namespace LaptopWebsite.Models
{
    public class LaptopDbContext : DbContext
    {
        // "LaptopDbConnection" là tên chuỗi kết nối sẽ khai báo trong Web.config
        public LaptopDbContext() : base("name=LaptopDbConnection")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}