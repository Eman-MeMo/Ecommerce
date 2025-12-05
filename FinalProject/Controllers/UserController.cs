using FinalProject.Helpers;
using FinalProject.Interfaces;
using FinalProject.Models;
using FinalProject.Models.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public UserController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.navItems = NavigationHelper.GetNavItems(false);
            var users = await unitOfWork.UserRepository.GetAllAsync();
            return View(users);
        }

        public IActionResult Login()
        {
            return View(new UserLoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(vm.Username);

            if (user == null || user.password != vm.Password)
            {
                TempData["LoginError"] = "Invalid username or password.";
                return View(vm);
            }

            HttpContext.Session.SetInt32("UserId", user.id);

            if (user.customer)
                return RedirectToAction("ShowForUser", "Product");

            return RedirectToAction("ShowForAdmin", "Product");
        }

        public IActionResult Signup()
        {
            return View(new UserSignupViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Signup(UserSignupViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var newUser = new User
            {
                username = vm.Username,
                password = vm.Password,
                name = vm.Name,
                age = vm.Age,
                address = vm.Address,
                Gender = vm.Gender,
                mobileNum = vm.MobileNum,
                customer = true
            };

            await unitOfWork.UserRepository.AddAsync(newUser);
            await unitOfWork.SaveChangesAsync();

            HttpContext.Session.SetInt32("UserId", newUser.id);

            return RedirectToAction("ShowForUser", "Product");
        }

        public async Task<IActionResult> Profile()
        {
            ViewBag.navItems = NavigationHelper.GetNavItems(true);

            int? id = HttpContext.Session.GetInt32("UserId");
            if (!id.HasValue)
                return RedirectToAction("Login");

            var user = await unitOfWork.UserRepository.GetByIdAsync(id.Value);

            var vm = new UserProfileViewModel
            {
                Id = user.id,
                Username = user.username,
                Name = user.name,
                Age = user.age,
                Address = user.address,
                Gender = user.Gender,
                MobileNum = user.mobileNum
            };

            return View(vm);
        }

        public async Task<IActionResult> EditProfile(int id)
        {
            ViewBag.navItems = NavigationHelper.GetNavItems(false);

            var user = await unitOfWork.UserRepository.GetByIdAsync(id);

            var vm = new UserEditProfileViewModel
            {
                Id = user.id,
                Username = user.username,
                Password = user.password,
                Name = user.name,
                Age = user.age,
                Address = user.address,
                Gender = user.Gender,
                MobileNum = user.mobileNum
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditProfileViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await unitOfWork.UserRepository.GetByIdAsync(vm.Id);

            user.name = vm.Name;
            user.age = vm.Age;
            user.address = vm.Address;
            user.Gender = vm.Gender;
            user.mobileNum = vm.MobileNum;

            unitOfWork.UserRepository.Update(user);
            await unitOfWork.SaveChangesAsync();

            return RedirectToAction("Profile");
        }
    }
}
