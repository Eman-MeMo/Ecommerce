using FinalProject.Entities;
using FinalProject.Interfaces;
using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Validation
{
    public class UniqueCategoryAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var unitOfWork = validationContext.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            if (unitOfWork == null)
                return new ValidationResult("UnitOfWork not available.");

            string uniqueName = value.ToString() ?? "";

            var categoryInstance = validationContext.ObjectInstance;
            int idToIgnore = 0;

            var idProp = categoryInstance.GetType().GetProperty("id");
            if (idProp != null)
            {
                var idVal = idProp.GetValue(categoryInstance);
                if (idVal != null)
                    idToIgnore = (int)idVal;
            }

            var categoryFromDB = unitOfWork.CategoryRepository.GetCategoryByName(uniqueName).Result;

            if (categoryFromDB == null || categoryFromDB.id == idToIgnore)
                return ValidationResult.Success;

            return new ValidationResult("Category name already exists!");
        }
    }
}
