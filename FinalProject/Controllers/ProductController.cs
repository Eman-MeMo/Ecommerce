using FinalProject.Entities;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class ProductController : Controller
    {
        EcommerceContext DB = new EcommerceContext();
        public IActionResult ShowForUser()
        {
            ViewBag.Categories = DB.categories.ToList();
            ViewBag.Products = DB.products.ToList();
            List<(string Name, string Link)> navItems = new List<(string, string)>();
            navItems.Add(("Home", "/Product/ShowForUser"));
            navItems.Add(("Shop", "/Product/ShowForUser"));
            navItems.Add(("Cart", "/Cart/Index"));
            navItems.Add(("Profile", "/User/Profile"));
            ViewBag.navItems = navItems;
            return View(DB.products.ToList());
        }
        public IActionResult ShowForAdmin()
        {
            var categoryNames = DB.categories.ToDictionary(d => d.id, d => d.name);
            ViewBag.CategoryNames = categoryNames;
            List<(string Name, string Link)> navItems = new List<(string, string)>();
            navItems.Add(("Products", "/Product/ShowForAdmin"));
            navItems.Add(("Categories", "/Category/Index"));
            navItems.Add(("Customers", "/User/Index"));
            navItems.Add(("Orders", "/Order/Index"));
            ViewBag.navItems = navItems;
            return View(DB.products.ToList());
        }
        public IActionResult DetailForUser(int id)
        {
            List<(string Name, string Link)> navItems = new List<(string, string)>();
            navItems.Add(("Home", "/Product/ShowForUser"));
            navItems.Add(("Shop", "/Product/ShowForUser"));
            navItems.Add(("Cart", "/Cart/Index"));
            navItems.Add(("Profile", "/User/Profile"));
            ViewBag.navItems = navItems;
            Product product = DB.products.Find(id);
            ViewBag.CategoryName = DB.categories.Find(product.CategoryId).name;
            return View(product);
        }
        public IActionResult DetailForAdmin(int id)
        {
            List<(string Name, string Link)> navItems = new List<(string, string)>();
            navItems.Add(("Products", "/Product/ShowForAdmin"));
            navItems.Add(("Categories", "/Category/Index"));
            navItems.Add(("Customers", "/User/Index"));
            navItems.Add(("Orders", "/Order/Index"));
            ViewBag.navItems = navItems;
            Product product = DB.products.Find(id);
            ViewBag.CategoryName = DB.categories.Find(product.CategoryId).name;
            return View(product);
        }
        public IActionResult New()
        {
            List<(string Name, string Link)> navItems = new List<(string, string)>();
            navItems.Add(("Products", "/Product/ShowForAdmin"));
            navItems.Add(("Categories", "/Category/Index"));
            navItems.Add(("Customers", "/User/Index"));
            navItems.Add(("Orders", "/Order/Index"));
            ViewBag.navItems = navItems;
            ViewBag.categoryNames = DB.categories.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult New(Product p)
        {
            if (ModelState.IsValid)
            {
                DB.products.Add(p);
                DB.SaveChanges();
                return RedirectToAction("ShowForAdmin");
            }
            ViewBag.categoryNames = DB.categories.ToList();
            return View();
        }
        public IActionResult Edit(int id)
        {
            List<(string Name, string Link)> navItems = new List<(string, string)>();
            navItems.Add(("Products", "/Product/ShowForAdmin"));
            navItems.Add(("Categories", "/Category/Index"));
            navItems.Add(("Customers", "/User/Index"));
            navItems.Add(("Orders", "/Order/Index"));
            ViewBag.navItems = navItems;
            ViewBag.categoryNames = DB.categories.ToList();
            return View(DB.products.Find(id));
        }
        [HttpPost]
        public IActionResult Edit(Product p)
        {
            if (ModelState.IsValid)
            {
                DB.products.Update(p);
                DB.SaveChanges();
                return RedirectToAction("ShowForAdmin");
            }
            ViewBag.categoryNames = DB.categories.ToList();
            return View(p);
        }
        public IActionResult Delete(int id)
        {
            var product = DB.products.Find(id);
            DB.products.Remove(product);
            DB.SaveChanges();
            return RedirectToAction("ShowForAdmin");
        }
    }
}
