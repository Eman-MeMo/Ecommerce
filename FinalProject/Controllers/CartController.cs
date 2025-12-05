using FinalProject.Entities;
using FinalProject.Helpers;
using FinalProject.Interfaces;
using FinalProject.Models;
using FinalProject.Models.ViewModels.CartViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using FinalProject.Stripe;
using Microsoft.Extensions.Options;

namespace FinalProject.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly StripeSettings stripeSettings;

        public CartController(IUnitOfWork _unitOfWork, IOptions<StripeSettings> _stripeSettings)
        {
            unitOfWork = _unitOfWork;
            stripeSettings = _stripeSettings.Value;
            StripeConfiguration.ApiKey = stripeSettings.SecretKey;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.navItems = NavigationHelper.GetNavItems(true);
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
                return RedirectToAction("login", "User");

            Cart cart = await unitOfWork.CartRepository.GetByUserIdAsync(userId.Value);

            CartViewModel vm = new CartViewModel();

            if (cart != null)
            {
                var cartProducts = await unitOfWork.CartProductRepository
                    .GetProductsByCartIdAsync(cart.id);

                foreach (var cp in cartProducts)
                {
                    var product = await unitOfWork.ProductRepository.GetByIdAsync(cp.ProductId);

                    vm.Items.Add(new CartItemViewModel
                    {
                        ProductId = cp.ProductId,
                        Quantity = cp.quantity,
                        Name = product.name,
                        Image = product.imag,
                        Price = product.price,
                        Description = product.description,
                        CategoryId = product.CategoryId,
                        mount = product.mount
                    });
                }
            }

            if (vm.Items.Count == 0 || cart==null)
                return View("EmptyCart");

            return View(vm);
        }
        public async Task<IActionResult> AddToCart([FromRoute(Name = "id")] int productId)
        {
            if (productId <= 0)
                return Json(new { success = false, message = "Something error at product info!" });

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("login", "User");

            Models.Product product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
                return Json(new { success = false, message = "Product not found" });

            Cart cart = await unitOfWork.CartRepository.GetByUserIdAsync(userId.Value);
            if (cart == null)
            {
                cart = new Cart { UserId = userId.Value };
                await unitOfWork.CartRepository.AddAsync(cart);
                await unitOfWork.SaveChangesAsync();
            }

            CartProduct cartProduct =
                await unitOfWork.CartProductRepository.GetByIdAsync(cart.id, productId);

            if (cartProduct == null)
            {
                cartProduct = new CartProduct
                {
                    CartId = cart.id,
                    ProductId = productId,
                    quantity = 1
                };
                await unitOfWork.CartProductRepository.AddAsync(cartProduct);
            }

            await unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditQuantity(int productId, int quantity)
        {
            if (!(quantity == 1 || quantity == -1))
                return Json(new { success = false, message = "Invalid quantity change" });

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return Json(new { success = false, message = "Not authenticated" });

            Cart cart = await unitOfWork.CartRepository.GetByUserIdAsync(userId.Value);
            if (cart == null)
                return Json(new { success = false, message = "Cart not found" });

            CartProduct cp =
                await unitOfWork.CartProductRepository.GetByIdAsync(cart.id, productId);

            if (cp == null)
                return Json(new { success = false, message = "Product not found" });

            cp.quantity += quantity;

            if (cp.quantity <= 0)
                unitOfWork.CartProductRepository.Delete(cp);

            await unitOfWork.SaveChangesAsync();

            return Json(new { success = true, newQuantity = Math.Max(cp.quantity, 0) });
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(decimal totalPrice, string paymentMethod)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("login", "User");

            Cart cart = await unitOfWork.CartRepository.GetByUserIdAsync(userId.Value);
            if (cart == null)
                return RedirectToAction("Index");

            var cartProducts = await unitOfWork.CartProductRepository.GetProductsByCartIdAsync(cart.id);

            foreach (var cp in cartProducts)
            {
                var product = await unitOfWork.ProductRepository.GetByIdAsync(cp.ProductId);
                if (cp.quantity > product.mount)
                {
                    TempData["ProductError"] = $"Product '{product.name}' is not available in this quantity.";
                    TempData["ProductId"] = cp.ProductId;
                    return RedirectToAction("Index");
                }
            }

            if (paymentMethod == "Cash")
            {
                return await CreateOrderAfterPayment(userId.Value, cart, cartProducts);
            }
            else if (paymentMethod == "Card")
            {
                return await CreateStripePaymentSession(cartProducts);
            }
            else
            {
                TempData["PaymentError"] = "Invalid payment method selected.";
                return RedirectToAction("Index");
            }
        }
        private async Task<IActionResult> CreateOrderAfterPayment(int userId, Cart cart, IEnumerable<CartProduct> cartProducts)
        {
            var order = new Models.Order
            {
                UserId = userId,
                date = DateTime.Now,
                totalPrice = cartProducts.Sum(cp => cp.quantity * cp.Product.price)
            };

            await unitOfWork.OrderRepository.AddAsync(order);
            await unitOfWork.SaveChangesAsync();

            foreach (var cp in cartProducts)
            {
                var product = await unitOfWork.ProductRepository.GetByIdAsync(cp.ProductId);
                product.mount -= cp.quantity;

                await unitOfWork.OrderItemRepository.AddAsync(new OrderItem
                {
                    OrderId = order.id,
                    ProductId = cp.ProductId,
                    quantity = cp.quantity
                });

                unitOfWork.CartProductRepository.Delete(cp);
            }

            unitOfWork.CartRepository.Delete(cart);
            await unitOfWork.SaveChangesAsync();

            ViewBag.navItems = NavigationHelper.GetNavItems(true);
            return View("Checkout");
        }
        private async Task<IActionResult> CreateStripePaymentSession(IEnumerable<CartProduct> cartProducts)
        {
            var lineItems = new List<SessionLineItemOptions>();

            foreach (var cp in cartProducts)
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(cp.Product.price * 100), 
                        Currency = "EGP",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = cp.Product.name
                        }
                    },
                    Quantity = cp.quantity
                });
            }

            decimal fees = 5m; 
            if (fees > 0)
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(fees * 100),
                        Currency = "EGP",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Service Fees"
                        }
                    },
                    Quantity = 1
                });
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = Url.Action("StripeSuccess", "Cart", null, Request.Scheme),
                CancelUrl = Url.Action("Index", "Cart", null, Request.Scheme),
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Redirect(session.Url);
        }

        public async Task<IActionResult> StripeSuccess()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("login", "User");

            Cart cart = await unitOfWork.CartRepository.GetByUserIdAsync(userId.Value);
            var cartProducts = await unitOfWork.CartProductRepository.GetProductsByCartIdAsync(cart.id);

            return await CreateOrderAfterPayment(userId.Value, cart, cartProducts);
        }
    }
}
