namespace LaptopWebsite.Migrations
{
    using LaptopWebsite.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LaptopWebsite.Models.LaptopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LaptopWebsite.Models.LaptopDbContext context)
        {
            // Thêm danh mục mẫu
            context.Categories.AddOrUpdate(c => c.CategoryName,
                new Category { CategoryName = "Dell" },
                new Category { CategoryName = "Asus" },
                new Category { CategoryName = "MacBook" },
                new Category { CategoryName = "MSI" },
                new Category { CategoryName = "Lenovo" },
                new Category { CategoryName = "Acer" },
                new Category { CategoryName = "HP" }
            );
            context.SaveChanges(); // Lưu để lấy ID danh mục

            // Thêm sản phẩm mẫu
            context.Products.AddOrUpdate(p => p.ProductName,
                new Product
                {
                    ProductName = "Dell XPS 13",
                    CategoryId = 1, // Dell
                    Price = 25000000,
                    StockQuantity = 10,
                    Description = "Intel Core i5, 8GB RAM, 256GB SSD",
                    ImageUrl = "https://i.ibb.co/p6hZDFST/dell-xps-13.jpg" // Dùng ảnh tạm
                },
                new Product
                {
                    ProductName = "Asus ROG Strix",
                    CategoryId = 2, // Asus
                    Price = 30000000,
                    StockQuantity = 5,
                    Description = "Intel Core i7, 16GB RAM, 512GB SSD, RTX 3060",
                    ImageUrl = "https://via.placeholder.com/300x200?text=Asus+ROG"
                },
                new Product
                {
                    ProductName = "MSI Gaming Thin 15",
                    CategoryId = 3, // MSI
                    Price = 22000000,
                    StockQuantity = 10,
                    Description = "Intel Core i5, 16GB RAM, 512GB SSD",
                    ImageUrl = "https://i.ibb.co/9RgH69w/MSI-Thin-15.jpg"
                },
                new Product
                {
                ProductName = "Acer Aspire Lite 14",
                    CategoryId = 4, // Acer
                    Price = 15000000,
                    StockQuantity = 5,
                    Description = "Intel Core i5, 4GB RAM, 256GB SSD",
                    ImageUrl = "https://via.placeholder.com/300x200?text=Asus+ROG"
                }
            );
            context.SaveChanges();

            // Thêm tài khoản Admin mẫu
            context.Users.AddOrUpdate(u => u.Email,
                new User
                {
                    FullName = "Quản trị viên",
                    Email = "admin@laptop.com",
                    Password = "123", // Tạm thời để mật khẩu gốc để dễ test, sau này có thể thêm hàm mã hóa MD5/BCrypt
                    Role = "Admin",
                    Phone = "0987654321",
                    Address = "Mỹ Tho, Tiền Giang"
                },
                new User
                {
                    FullName = "Khách hàng test",
                    Email = "khach@laptop.com",
                    Password = "123",
                    Role = "Customer"
                }
            );
            context.SaveChanges();
        }
    }
}
