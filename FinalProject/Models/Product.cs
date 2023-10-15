using FinalProject.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Product
    {
        public int id { get; set; }
        [MinLength(3, ErrorMessage = "Name must be more than 3 char !")]
        [MaxLength(50, ErrorMessage = "Name must be Less than 50 char !")]
        [UniqueProduct]
        public string name { get; set; }
        [Range(10, 100000, ErrorMessage = "Price must be between 10 and 100000!")]
        public int price { get; set; }
        [MinLength(5, ErrorMessage = "Name must be more than 5 char !")]
        [MaxLength(5000, ErrorMessage = "Name must be Less than 5000 char !")]
        public string description { get; set; }
        [RegularExpression(@"\w*\.(png|jpg)", ErrorMessage = "Image must end with png or jpg extenstion !")]
        public string imag { get; set; }
        [Range(0, 1000, ErrorMessage = "Quantity must be between 0 and 1000!")]
        public int mount { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<CartProduct>? CartProducts { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}
