using ecommerce.Models;
using ecommerce.Services;
using ecommerce.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ecommerce.Controllers
{
    public class CartController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var cart = new CartService(HttpContext);
            var items = await cart.GetCartItemsAsync();
            var model = new CartItemsViewModel
            {
                Items = items,
                Sum = CalculateCart(items)
            };

            return View(model);
        }

        private decimal CalculateCart(IEnumerable<CartItem> items)
        {
            return items.Sum(item => item.Product.FinalPrice * item.Count);
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(int id, int quantity)
        {
            var cart = new CartService(HttpContext);
            cart.MakeProxyCreationFalse();
            await cart.AddItemsInCart(id, quantity);
            var items = await cart.GetCartItemsAsync();
            var totalsum = CalculateCart(items);

            var model = new CartItemsViewModel
            {
                Items = items,
                Sum = totalsum
            };

            var result = JsonConvert.SerializeObject(model, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AddWishItemsInCart(int productId, int quantity)
        {
            var cart = new CartService(HttpContext);
            cart.MakeProxyCreationFalse();
            await cart.AddItemsInCart(productId, quantity);

            var wish = new CartService(HttpContext, true);
            wish.MakeProxyCreationFalse();
            await wish.RemoveWishList(productId);
            var items = await cart.GetCartItemsAsync();
            //wish.MakeProxyCreation();

            var viewModel = new ProductItemsViewModel
            {
                WishLists = await wish.GetWishListAsync(),
                Items = items,
                Sum = CalculateCart(items)
            };
            var result = JsonConvert.SerializeObject(viewModel, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCart(int id, int count)
        {
            var cart = new CartService(HttpContext);
            await cart.UpdateCart(id, count);
            var items = await cart.GetCartItemsAsync();
            var totalsum = CalculateCart(items);

            var model = new CartItemsViewModel
            {
                Items = items,
                Sum = totalsum
            };

            var result = JsonConvert.SerializeObject(model, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> RemoveFromCart(int productId)
        {
            var cart = new CartService(HttpContext);
            await cart.RemoveAync(productId);
            var items = await cart.GetCartItemsAsync();
            var totalsum = CalculateCart(items);

            var model = new CartItemsViewModel
            {
                Items = items,
                Sum = totalsum
            };

            var result = JsonConvert.SerializeObject(model, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public async Task<ActionResult> Checkout()
        {
            var cart = new CartService(HttpContext);
            var items = await cart.GetCartItemsAsync();
            var totalsum = CalculateCart(items);

            var viewModel = new CheckoutViewModel
            {
                CartItems = items,
                Sum = totalsum
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Checkout(CheckoutViewModel model)
        {
            var cart = new CartService(HttpContext);
            var items = await cart.GetCartItemsAsync();
            var totalsum = CalculateCart(items);

            if (!ModelState.IsValid)
            {
                model.CartItems = items;
                model.Sum = totalsum;
                return View(model);
            }

            var result = await cart.CheckoutAsync(model);

            if (result.Succeeded)
            {
                TempData["transactionId"] = result.TransactionId;
                return RedirectToAction("Complete");
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        public ActionResult Complete()
        {
            ViewBag.TransactionId = (string)TempData["transactionId"];

            return View();
        }

        [HttpPost]
        public async Task AddToWishList(int id)
        {
            var wish = new CartService(HttpContext, true);
            await wish.AddItemToWishList(id);
        }

        public async Task<ActionResult> WishList()
        {
            var wish = new CartService(HttpContext, true);
            var products = await wish.GetWishListAsync();
            return View(products);
        }

        [HttpPost]
        public async Task<ActionResult> RemoveWishList(int id)
        {
            var wish = new CartService(HttpContext, true);
            wish.MakeProxyCreationFalse();
            await wish.RemoveWishList(id);
            var products = await wish.GetWishListAsync();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}