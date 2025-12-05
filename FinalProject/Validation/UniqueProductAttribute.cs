using FinalProject.Entities;
using FinalProject.Interfaces;
using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Validation
{
    public class UniqueProductAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var unitOfWork = validationContext.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            if (unitOfWork == null)
                return new ValidationResult("UnitOfWork not available.");

            string uniqueName = value.ToString() ?? "";

            var productInstance = validationContext.ObjectInstance;
            int idToIgnore = -1;

            var idProp = productInstance.GetType().GetProperty("Id");
            if (idProp != null)
            {
                var idVal = idProp.GetValue(productInstance);
                if (idVal != null)
                    idToIgnore = (int)idVal;
            }

            var productFromDB = unitOfWork.ProductRepository.GetProductByName(uniqueName).Result;

            if (productFromDB == null || productFromDB.id == idToIgnore)
                return ValidationResult.Success;

            return new ValidationResult("Product name already exists!");
        }
    }
}
