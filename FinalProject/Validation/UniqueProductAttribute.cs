using FinalProject.Entities;
using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Validation
{
    public class UniqueProductAttribute : ValidationAttribute
    {
        EcommerceContext DB = new EcommerceContext();

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            string uniqueName = value.ToString();
            Product product = validationContext.ObjectInstance as Product;
            int idToIgnore = product.id;
            Product productFromDB = DB.products.FirstOrDefault(p => p.name == uniqueName);
            if (productFromDB == null || idToIgnore != 0)
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
