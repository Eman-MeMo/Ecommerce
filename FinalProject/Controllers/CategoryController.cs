using FinalProject.Entities;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class CategoryController : Controller
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
            return View(DB.categories.ToList());
        }
        public IActionResult New()
        {
            List<(string Name, string Link)> navItems = new List<(string, string)>();
            navItems.Add(("Products", "/Product/ShowForAdmin"));
            navItems.Add(("Categories", "/Category/Index"));
            navItems.Add(("Customers", "/User/Index"));
            navItems.Add(("Orders", "/Order/Index"));
            ViewBag.navItems = navItems;
            return View();
        }
        [HttpPost]
        public IActionResult New(Category c)
        {
            if (ModelState.IsValid)
            {
                DB.categories.Add(c);
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
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
            return View(DB.categories.Find(id));
        }
        [HttpPost]
        public IActionResult Edit(Category c)
        {
            if (ModelState.IsValid)
            {
                DB.categories.Update(c);
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c);
        }
        public IActionResult Delete(int id)
        {
            var c = DB.categories.Find(id);
            DB.categories.Remove(c);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
