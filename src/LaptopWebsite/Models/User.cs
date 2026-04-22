using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaptopWebsite.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Role { get; set; } // "Admin" hoặc "Customer"

        public virtual ICollection<Order> Orders { get; set; }
    }
}