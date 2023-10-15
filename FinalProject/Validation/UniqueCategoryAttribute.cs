using FinalProject.Entities;
using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Validation
{
    public class UniqueCategoryAttribute : ValidationAttribute
    {
        EcommerceContext DB = new EcommerceContext();

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string uniqueName = value.ToString();
            Category category = validationContext.ObjectInstance as Category;
            int idToIgnore = category.id;
            Category categoryFromDB = DB.categories.FirstOrDefault(c => c.name == uniqueName);
            if (categoryFromDB == null || idToIgnore != 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Name already Exists !");
            }
        }
    }
}
