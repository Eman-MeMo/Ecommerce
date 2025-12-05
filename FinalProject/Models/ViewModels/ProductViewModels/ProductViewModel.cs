using FinalProject.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.ViewModels.ProductViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [MinLength(3, ErrorMessage = "Name must be more than 3 char !")]
        [MaxLength(50, ErrorMessage = "Name must be Less than 50 char !")]
        [UniqueProduct]
        public string Name { get; set; }
        [MinLength(5, ErrorMessage = "Name must be more than 5 char !")]
        [MaxLength(5000, ErrorMessage = "Name must be Less than 5000 char !")]
        public string Description { get; set; }

        [Range(10, 100000, ErrorMessage = "Price must be between 10 and 100000!")]
        public int Price { get; set; }
        [RegularExpression(@"\w*\.(png|jpg)", ErrorMessage = "Image must end with png or jpg extenstion !")]

        public string imag { get; set; }
        [RegularExpression(@"\w*\.(png|jpg)", ErrorMessage = "Image must end with png or jpg extenstion !")]

        public int mount { get; set; }
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
