using ecommerce.Models;
using ecommerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ecommerce.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _cartId;
        private readonly string _wishlistId;

        public CartService(HttpContextBase httpContext,
            ApplicationDbContext context, bool wishlist)
        {
            if (wishlist)
            {
                _context = context;
                _wishlistId = GetWishListId(httpContext);
            }
        }

        public CartService(HttpContextBase httpContext,
            ApplicationDbContext context)
        {
            _context = context;
            _cartId = GetCartId(httpContext);
        }

        public CartService(HttpContextBase context, bool wishlist)
            : this(context, new ApplicationDbContext(), wishlist)
        {
        }

        public CartService(HttpContextBase context) :
            this(context, new ApplicationDbContext())
        {
        }


        private string GetCartId(HttpContextBase httpContext)
        {
            var cookie = httpContext.Request.Cookies.Get("ecmCart");
            string cartId;

            if (string.IsNullOrWhiteSpace(cookie?.Value))
            {
                cookie = new HttpCookie("ecmCart");
                cartId = Guid.NewGuid().ToString("N");
                cookie.Value = cartId;
                cookie.Expires = DateTime.Now.AddDays(7);
                httpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                cartId = cookie.Value;
                cookie.Expires = DateTime.Now.AddDays(3);
            }

            return cartId;
        }

        private string GetWishListId(HttpContextBase httpContext)
        {
            var cookie = httpContext.Request.Cookies.Get("ecmWishList");
            string wishId;

            if (string.IsNullOrWhiteSpace(cookie?.Value))
            {
                cookie = new HttpCookie("ecmWishList");
                wishId = Guid.NewGuid().ToString("N");
                cookie.Value = wishId;
                cookie.Expires = DateTime.Now.AddDays(7);
                httpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                wishId = cookie.Value;
                cookie.Expires = DateTime.Now.AddDays(3);
            }

            return wishId;
        }

        public async Task AddItemsInCart(int id, int quantity)
        {
            var product = await _context.Products
                .Include(x => x.FileDetails)
                .SingleOrDefaultAsync(p => p.ProductId == id);
            FileDetail first = new FileDetail();

            if (product == null)
            {
                throw new NullReferenceException("Product Doesn't Exists");
            }

            var images = product.FileDetails.ToList();
            first = images.First();

            var userCart = await _context.CartItems
                .SingleOrDefaultAsync(p => p.ProductId == id &&
                                           p.CartId == _cartId);

            if (userCart != null)
            {
                userCart.Count = quantity;
                userCart.DateCreated = DateTime.Now;
                userCart.ImageUrl = first.Id + first.Extension;
                _context.Entry(userCart).State = EntityState.Modified;
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = _cartId,
                    ProductId = id,
                    Count = quantity,
                    ImageUrl = first.Id + first.Extension,
                    DateCreated = DateTime.Now
                };
                _context.CartItems.Add(cartItem);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync()
        {
            return await _context.CartItems
                .Where(x => x.CartId == _cartId)
                .Include(x => x.Product)
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();
        }

        public void MakeProxyCreationFalse()
        {
            _context.Configuration.ProxyCreationEnabled = false;
        }

        public void MakeProxyCreation()
        {
            _context.Configuration.ProxyCreationEnabled = true;
        }

        public async Task<IEnumerable<FileDetail>> GetImages(int id)
        {
            return await _context.FileDetails.Where(x => x.ProductId == id)
                .ToListAsync();
        }

        public async Task UpdateCart(int productId, int count)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId &&
                                          c.CartId == _cartId);

            if (cartItem != null)
            {
                cartItem.Count = count;
                _context.Entry(cartItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> RemoveAync(int productId)
        {
            // Confirmation: if cartitem really exist or not
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId &&
                                           c.CartId == _cartId);
            var itemCount = 0;
            if (cartItem == null)
            {
                return itemCount;
            }

            _context.CartItems.Remove(cartItem);

            //if (cartItem.Count > 1)
            //{
            //    cartItem.Count--;
            //    itemCount = cartItem.Count;
            //}
            //else
            //{

            //}

            await _context.SaveChangesAsync();
            return itemCount;
        }

        public async Task<PaymentResult> CheckoutAsync(CheckoutViewModel viewModel)
        {
            var items = await GetCartItemsAsync();
            var order = new Order
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Address = viewModel.Address,
                City = viewModel.City,
                State = viewModel.State,
                PostalCode = viewModel.PostalCode,
                Country = viewModel.Country,
                Phone = viewModel.Phone,
                Email = viewModel.Email,
                OrderDate = DateTime.Now
            };

            foreach (var item in items)
            {
                var detail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Count
                };

                order.Total += (item.Product.FinalPrice * item.Count);

                order.OrderDetails.Add(detail);
            }

            viewModel.Sum = order.Total;

            var gateway = new PaymentGateway();

            var result = gateway.ProcessPayment(viewModel);

            if (result.Succeeded)
            {
                order.TransactionId = result.TransactionId;
                _context.Orders.Add(order);
                _context.CartItems.RemoveRange(items);
                await _context.SaveChangesAsync();
            }

            return result;
        }

        public async Task AddItemToWishList(int id)
        {
            var product = await _context.Products
                .Include(x => x.FileDetails)
                .SingleOrDefaultAsync(p => p.ProductId == id);
            FileDetail first = new FileDetail();

            if (product == null)
            {
                throw new NullReferenceException("Product Doesn't Exists");
            }

            var images = product.FileDetails.ToList();
            first = images.First();

            var wishList = await _context.WishLists
                .SingleOrDefaultAsync(p => p.ProductId == id &&
                                           p.wishId == _wishlistId);

            if (wishList != null)
            {
                wishList.DateCreated = DateTime.Now;
                wishList.ImageUrl = first.Id + first.Extension;
                _context.Entry(wishList).State = EntityState.Modified;
            }
            else
            {
                var wish = new WishList
                {
                    wishId = _wishlistId,
                    ProductId = id,
                    ImageUrl = first.Id + first.Extension,
                    DateCreated = DateTime.Now
                };
                _context.WishLists.Add(wish);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WishList>> GetWishListAsync()
        {
            return await _context.WishLists
                .Where(x => x.wishId == _wishlistId)
                .Include(x => x.Product)
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();
        }

        public async Task RemoveWishList(int id)
        {
            // Confirmation: if cartitem really exist or not
            var wishItem = await _context.WishLists
                .FirstOrDefaultAsync(c => c.ProductId == id &&
                                          c.wishId == _wishlistId);
            

            _context.WishLists.Remove(wishItem);

            await _context.SaveChangesAsync();
        }
    }
}