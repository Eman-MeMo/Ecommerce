using FinalProject.Entities;
using FinalProject.Interfaces;
using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Validation
{
    public class UniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            string username = value.ToString();

            var unitOfWork = validationContext.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            if (unitOfWork == null)
                return new ValidationResult("Service not available.");

            var objectInstance = validationContext.ObjectInstance;
            int id = 0;

            var idProp = objectInstance.GetType().GetProperty("Id");
            if (idProp != null)
            {
                var idVal = idProp.GetValue(objectInstance);
                if (idVal != null) id = (int)idVal;
            }

            var userFromDB = unitOfWork.UserRepository.GetUserByUsernameAsync(username).Result;

            if (id == 0)
            {
                if (userFromDB != null)
                    return new ValidationResult("Username already exists!");
            }
            else 
            {
                if (userFromDB != null && userFromDB.id != id)
                    return new ValidationResult("Username already exists!");
            }

            return ValidationResult.Success;
        }
    }
}
