using FinalProject.Entities;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace FinalProject.Controllers
{
    public class CartController : Controller
    {
        EcommerceContext DB = new EcommerceContext();
        public IActionResult Index()
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
                Cart cart = DB.carts.FirstOrDefault(c => c.UserId == userId);

                if (cart != null)
                {
                    var cartProducts = DB.cartProducts.Where(cp => cp.CartId == cart.id).ToList();
                    var productIds = cartProducts.Select(cp => cp.ProductId).ToList();
                    var productsInCart = DB.products.Where(p => productIds.Contains(p.id)).ToList();
                    Dictionary<int, int> productQuantities = cartProducts.ToDictionary(cp => cp.ProductId, cp => cp.quantity);
                    ViewBag.productQuantities = productQuantities;
                    return View(productsInCart);
                }
                else
                {
                    return View("EmptyCart");
                }
            }
            else
            {
                return RedirectToAction("login", "User");
            }

        }

        public IActionResult AddToCart(Product p)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {

                Cart cart = DB.carts.FirstOrDefault(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new Cart { UserId = userId.Value };
                    DB.carts.Add(cart);
                    DB.SaveChanges();
                }

                CartProduct cartProduct = DB.cartProducts.FirstOrDefault(cpp => cpp.CartId == cart.id && cpp.ProductId == p.id);

                if (cartProduct == null)
                {
                    CartProduct cp = new CartProduct
                    {
                        CartId = cart.id,
                        ProductId = p.id,
                        quantity = 1
                    };
                    DB.cartProducts.Add(cp);
                    DB.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("login", "User");
            }

        }
        [HttpPost]
        public IActionResult EditQuantity(int productId, int quantity)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");

                if (!userId.HasValue)
                {
                    return Json(new { success = false, message = "User not authenticated." });
                }

                Cart cart = DB.carts.FirstOrDefault(c => c.UserId == userId);
                if (cart == null)
                {
                    return Json(new { success = false, message = "Cart not found." });
                }

                CartProduct cartProduct = DB.cartProducts.FirstOrDefault(c => c.CartId == cart.id && c.ProductId == productId);
                if (cartProduct == null)
                {
                    return Json(new { success = false, message = "Product not found in the cart." });
                }



                else
                {
                    cartProduct.quantity += quantity;
                    if (cartProduct.quantity == 0)
                    {
                        DB.cartProducts.Remove(cartProduct);
                        DB.SaveChanges();
                    }
                }

                DB.SaveChanges();

                return Json(new { success = true, newQuantity = cartProduct.quantity });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "An error occurred while processing the request." });
            }
        }
        [HttpPost]
        public IActionResult Checkout(decimal totalPrice)
        {
            List<(string Name, string Link)> navItems = new List<(string, string)>();
            navItems.Add(("Home", "/Product/ShowForUser"));
            navItems.Add(("Shop", "/Product/ShowForUser"));
            navItems.Add(("Cart", "/Cart/Index"));
            navItems.Add(("Profile", "/User/Profile"));
            ViewBag.navItems = navItems;

            bool allProductIsAvailable = true;
            int? userId = HttpContext.Session.GetInt32("UserId");
            Cart cart = DB.carts.FirstOrDefault(c => c.UserId == userId);
            var cartProducts = DB.cartProducts.Where(cp => cp.CartId == cart.id).ToList();

            foreach (var cartProduct in cartProducts)
            {
                Product product = DB.products.FirstOrDefault(p => p.id == cartProduct.ProductId);

                if (product != null && cartProduct.quantity > product.mount)
                {
                    allProductIsAvailable = false;
                    TempData["ErrorMessage"] = $"The product '{product.name}' is not available in the desired quantity. Available amount: {product.mount}.";
                    TempData["ProductId"] = product.id;
                    return RedirectToAction("Index");
                }
            }
            if (allProductIsAvailable)
            {
                Order order = new Order();
                order.UserId = userId.Value;
                order.date = DateTime.Now;
                order.totalPrice = totalPrice;
                DB.orders.Add(order);
                DB.SaveChanges();
                foreach (var cartProductt in cartProducts)
                {
                    DB.products.FirstOrDefault(p => p.id == cartProductt.ProductId).mount -= cartProductt.quantity;
                    OrderItem orderItem = new OrderItem();
                    orderItem.quantity = cartProductt.quantity;
                    orderItem.ProductId = cartProductt.ProductId;
                    orderItem.OrderId = order.id;
                    DB.orderItems.Add(orderItem);
                    DB.SaveChanges();
                    DB.cartProducts.Remove(cartProductt);
                    DB.SaveChanges();
                }
                DB.carts.Remove(cart);
                DB.SaveChanges();
                return View();
            }
            return View();
        }


    }
}
