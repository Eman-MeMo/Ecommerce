using FinalProject.Entities;
using FinalProject.Helpers;
using FinalProject.Interfaces;
using FinalProject.Models;
using FinalProject.Models.ViewModels.CategoryViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.navItems = NavigationHelper.GetNavItems(false);

            var categories = await unitOfWork.CategoryRepository.GetAllAsync();

            var model = categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.id,
                    Name = c.name
                }).ToList();

            return View(model);
        }
        public IActionResult New()
        {
            ViewBag.navItems = NavigationHelper.GetNavItems(false);
            return View(new CategoryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> New(CategoryViewModel vm)
        {
            ViewBag.navItems = NavigationHelper.GetNavItems(false);

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var entity = new Category
            {
                name = vm.Name
            };

            await unitOfWork.CategoryRepository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.navItems = NavigationHelper.GetNavItems(false);

            var category = await unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (category == null) return NotFound();

            var vm = new CategoryViewModel
            {
                Id = category.id,
                Name = category.name
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel vm)
        {
            ViewBag.navItems = NavigationHelper.GetNavItems(false);

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var category = await unitOfWork.CategoryRepository.GetByIdAsync(vm.Id);
            if (category == null) return Json(new { success = false, message = "Category not found" });

            category.name = vm.Name;

            unitOfWork.CategoryRepository.Update(category);
            await unitOfWork.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (category == null) return NotFound();

            unitOfWork.CategoryRepository.Delete(category);
            await unitOfWork.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
