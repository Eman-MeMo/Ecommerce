using FinalProject.Interfaces;
using FinalProject.Models;
using FinalProject.Helpers;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Models.ViewModels;
using FinalProject.Models.ViewModels.ProductViewModels;

namespace FinalProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<IActionResult> ShowForUser()
        {
            var products = await unitOfWork.ProductRepository.GetAllAsync();
            var categories = await unitOfWork.CategoryRepository.GetAllAsync();

            var result = products.Select(p => new ProductViewModel
            {
                Id = p.id,
                Name = p.name,
                Description = p.description,
                Price = p.price,
                CategoryId = p.CategoryId,
                CategoryName = categories.First(c => c.id == p.CategoryId).name,
                imag = p.imag,
                mount = p.mount
            }).ToList();

            ViewBag.navItems = NavigationHelper.GetNavItems(true);
            ViewBag.Categories = categories;

            return View(result);
        }

        public async Task<IActionResult> ShowForAdmin()
        {
            var products = await unitOfWork.ProductRepository.GetAllAsync();
            var categories = await unitOfWork.CategoryRepository.GetAllAsync();

            var result = products.Select(p => new ProductViewModel
            {
                Id = p.id,
                Name = p.name,
                Description = p.description,
                Price = p.price,
                CategoryId = p.CategoryId,
                CategoryName = categories.First(c => c.id == p.CategoryId).name,
                imag = p.imag,
                mount = p.mount
            }).ToList();

            ViewBag.navItems = NavigationHelper.GetNavItems(false);
            return View(result);
        }

        public async Task<IActionResult> DetailForUser(int id)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(product.CategoryId);

            var vm = new ProductViewModel
            {
                Id = product.id,
                Name = product.name,
                Description = product.description,
                Price = product.price,
                CategoryId = product.CategoryId,
                CategoryName = category.name,
                imag = product.imag,
                mount = product.mount
            };

            ViewBag.navItems = NavigationHelper.GetNavItems(true);
            return View(vm);
        }

        public async Task<IActionResult> DetailForAdmin(int id)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(product.CategoryId);

            var vm = new ProductViewModel
            {
                Id = product.id,
                Name = product.name,
                Description = product.description,
                Price = product.price,
                CategoryId = product.CategoryId,
                CategoryName = category.name,
                imag = product.imag,
                mount = product.mount
            };
            ViewBag.navItems = NavigationHelper.GetNavItems(false);
            return View(vm);
        }

        public async Task<IActionResult> New()
        {
            ViewBag.categoryNames = await unitOfWork.CategoryRepository.GetAllAsync();
            ViewBag.navItems = NavigationHelper.GetNavItems(false);
            return View(new ProductFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> New(ProductFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.categoryNames = await unitOfWork.CategoryRepository.GetAllAsync();
                return View(vm);
            }

            var product = new Product
            {
                name = vm.Name,
                description = vm.Description,
                price = vm.Price,
                CategoryId = vm.CategoryId,
                imag = vm.imag,
                mount = vm.mount
            };

            await unitOfWork.ProductRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();

            return RedirectToAction("ShowForAdmin");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(id);

            var vm = new ProductFormViewModel
            {
                Id = product.id,
                Name = product.name,
                Description = product.description,
                Price = product.price,
                CategoryId = product.CategoryId,
                imag = product.imag,
                mount = product.mount
            };

            ViewBag.categoryNames = await unitOfWork.CategoryRepository.GetAllAsync();
            ViewBag.navItems = NavigationHelper.GetNavItems(false);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.categoryNames = await unitOfWork.CategoryRepository.GetAllAsync();
                return View(vm);
            }

            var product = await unitOfWork.ProductRepository.GetByIdAsync(vm.Id);

            product.name = vm.Name;
            product.description = vm.Description;
            product.price = vm.Price;
            product.CategoryId = vm.CategoryId;
            product.mount = vm.mount;

            unitOfWork.ProductRepository.Update(product);
            await unitOfWork.SaveChangesAsync();

            return RedirectToAction("ShowForAdmin");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            unitOfWork.ProductRepository.Delete(product);
            await unitOfWork.SaveChangesAsync();

            return RedirectToAction("ShowForAdmin");
        }
    }
}
