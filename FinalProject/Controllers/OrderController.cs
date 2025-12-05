using FinalProject.Entities;
using FinalProject.Helpers;
using FinalProject.Interfaces;
using FinalProject.Models;
using FinalProject.Models.ViewModels;
using FinalProject.Models.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var users = await unitOfWork.UserRepository.GetAllAsync();
            var products = await unitOfWork.ProductRepository.GetAllAsync();
            var orders = await unitOfWork.OrderRepository.GetAllAsync();
            var orderItems = await unitOfWork.OrderItemRepository.GetAllAsync();

            var viewModel = new OrderViewModel
            {
                Orders = orders,
                Usernames = users.ToDictionary(u => u.id, u => u.name),
                ProductNames = products.ToDictionary(p => p.id, p => p.name),
                OrderItems = orderItems,
            };

            ViewBag.navItems = NavigationHelper.GetNavItems(false);

            return View(viewModel);
        }
    }
}
