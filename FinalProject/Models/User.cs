using FinalProject.Validation;

using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class User
    {
        public int id { get; set; }
        [Required]
        [Unique]
        public string username { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Password must be more than 5 char !")]
        public string password { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Name must be more than 3 char !")]
        [MaxLength(10, ErrorMessage = "Name must be Less than 10 char !")]
        public string name { get; set; }
        [Range(15, 100, ErrorMessage = "Age must be between 15 and 100!")]
        public int age { get; set; }
        [MinLength(3, ErrorMessage = "Address must be more than 3 char !")]
        [MaxLength(30, ErrorMessage = "Address must be Less than 30 char !")]
        public string address { get; set; }
        public string Gender { get; set; }
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Mobile number must be exactly 11 digits.")]
        public string mobileNum { get; set; }
        public bool customer { get; set; }
        public virtual ICollection<Cart>? Carts { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
