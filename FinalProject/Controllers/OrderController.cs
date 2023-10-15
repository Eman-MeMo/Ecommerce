using FinalProject.Entities;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FinalProject.Controllers
{
    public class OrderController : Controller
    {
        EcommerceContext DB = new EcommerceContext();
        public IActionResult Index()
        {
            var usernames = DB.users.ToDictionary(u => u.id, u => u.name);
            var productNames = DB.products.ToDictionary(p => p.id, p => p.name);
            ViewBag.usernames = usernames;
            ViewBag.productNames = productNames;
            List<(string Name, string Link)> navItems = new List<(string, string)>();
            navItems.Add(("Products", "/Product/ShowForAdmin"));
            navItems.Add(("Categories", "/Category/Index"));
            navItems.Add(("Customers", "/User/Index"));
            navItems.Add(("Orders", "/Order/Index"));
            ViewBag.navItems = navItems;
            ViewBag.orderItem = DB.orderItems.ToList();

            return View(DB.orders.ToList());
        }
    }
}
