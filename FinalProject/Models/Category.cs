using FinalProject.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Category
    {
        public int id { get; set; }
        [MinLength(3, ErrorMessage = "Name must be more than 3 char !")]
        [MaxLength(50, ErrorMessage = "Name must be Less than 50 char !")]
        [UniqueCategory]
        public string name { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
