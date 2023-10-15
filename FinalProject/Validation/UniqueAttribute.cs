using FinalProject.Entities;
using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Validation
{
    public class UniqueAttribute : ValidationAttribute
    {
        EcommerceContext DB = new EcommerceContext();
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string username = value?.ToString();
            User user = validationContext.ObjectInstance as User;
            User userFromDB = DB.users.FirstOrDefault(s => s.username == username);
            if (userFromDB != null && user.name == null)//login case
            {
                return ValidationResult.Success;
            }
            else if (userFromDB == null && user.name != null)//sign up with unique username case
            {
                return ValidationResult.Success;
            }
            else if (username == null) //Edit User Profile case
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Username already Exists !");
            }
        }
    }
}
