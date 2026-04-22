using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaptopWebsite.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100)]
        public string CategoryName { get; set; }

        // Navigation property
        public virtual ICollection<Product> Products { get; set; }
    }
}