using FinalProject.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.ViewModels.CategoryViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [MinLength(3, ErrorMessage = "Name must be more than 3 char !")]
        [MaxLength(50, ErrorMessage = "Name must be Less than 50 char !")]
        [UniqueCategory]
        public string Name { get; set; }
    }
}
