using FinalProject.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.ViewModels.UserViewModels
{
    public class UserLoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
