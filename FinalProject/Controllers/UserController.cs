using FinalProject.Entities;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class UserController : Controller
    {
        EcommerceContext DB = new EcommerceContext();
        public IActionResult Index()
        {
            List<(string Name, string Link)> navItems = new List<(string, string)>();
            navItems.Add(("Products", "/Product/ShowForAdmin"));
            navItems.Add(("Categories", "/Category/Index"));
            navItems.Add(("Customers", "/User/Index"));
            navItems.Add(("Orders", "/Order/Index"));
            ViewBag.navItems = navItems;
            return View(DB.users.ToList());
        }
        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult login(User s)
        {
            var user = DB.users.FirstOrDefault(a => a.username == s.username);
            if (user == null || user.password != s.password)
            {
                TempData["LoginError"] = "Invalid username or password.";
                return RedirectToAction("login", "User");
            }
            else
            {
                // Store the user's ID in a session variable
                HttpContext.Session.SetInt32("UserId", user.id);

                if (user.customer)
                {
                    return RedirectToAction("ShowForUser", "Product");
                }
                else
                {
                    return RedirectToAction("ShowForAdmin", "Product");
                }
            }
        }

        public IActionResult signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult signup(User s)
        {
            if (ModelState.IsValid)
            {
                s.customer = true;
                DB.users.Add(s);
                DB.SaveChanges();

                // Store the newly created user's ID in a session variable
                HttpContext.Session.SetInt32("UserId", s.id);

                return RedirectToAction("ShowForUser", "Product");
            }
            else
            {
                return View();
            }
        }
        public IActionResult profile()
        {
            List<(string Name, string Link)> navItems = new List<(string, string)>();
            navItems.Add(("Home", "/Product/ShowForUser"));
            navItems.Add(("Shop", "/Product/ShowForUser"));
            navItems.Add(("Cart", "/Cart/Index"));
            navItems.Add(("Profile", "/User/Profile"));
            ViewBag.navItems = navItems;
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                return View(DB.users.Find(userId));
            }
            else
            {
                return View(DB.users.Find(ViewBag.id));
            }
        }
        public IActionResult EditProfile(int id)
        {
            List<(string Name, string Link)> navItems = new List<(string, string)>();
            navItems.Add(("Home", "/Product/ShowForUser"));
            navItems.Add(("Shop", "/Product/ShowForUser"));
            navItems.Add(("Cart", "/Cart/Index"));
            navItems.Add(("Profile", "/User/Profile"));
            ViewBag.navItems = navItems;
            return View(DB.users.Find(id));
        }
        [HttpPost]
        public IActionResult EditProfile(User s)
        {
            ModelState.Remove("username");
            if (ModelState.IsValid)
            {
                s.customer = true;
                DB.users.Update(s);
                DB.SaveChanges();
                ViewBag.id = s.id;
                return RedirectToAction("Profile");

            }
            return View();
        }
    }
}

