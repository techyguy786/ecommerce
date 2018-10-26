using ecommerce.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ecommerce.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ProductService() : this(new ApplicationDbContext())
        {
        }

        public async Task<IEnumerable<Product>> GetProductsWithCategories()
        {
            return await _context
                .Products.Include(x => x.Category).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<CategoryType>> GetCategoryTypes()
        {
            return await _context.CategoryTypes.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.SingleAsync(x => x.ProductId == id);
        }

        public async Task<IEnumerable<FileDetail>> GetFileDetails()
        {
            return await _context.FileDetails.ToListAsync();
        }

        public async Task<FileDetail> GetFileDetailByProductId(int id)
        {
            return await _context.FileDetails.FirstAsync(x => x.ProductId == id);
        }

        public async Task<IEnumerable<Category>> GetCategoriesByCategoryTypeId(int id)
        {
            return await _context.Categories.Where(x => x.CategoryTypeId == id).ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.SingleAsync(x => x.CategoryId == id);
        }

        public void MakeProxyCreationFalse()
        {
            _context.Configuration.ProxyCreationEnabled = false;
        }

        public async Task StoreProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetProductWithFileDetailsById(int id)
        {
            return await _context.Products
                .Include(x => x.FileDetails)
                .SingleAsync(x => x.ProductId == id);
        }

        public async Task StoreFileDetail(FileDetail fileDetail)
        {
            _context.FileDetails.Add(fileDetail);
            await _context.SaveChangesAsync();
        }

        public async Task ModifyProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveProduct(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> ProductsByCategory(int id)
        {
            var products = await _context.Products.Where(x => x.CategoryId == id).ToListAsync();
            return products;
        }

        public async Task<bool> CheckProductInCart(HttpContextBase httpContext, int id)
        {
            var cart = httpContext.Request.Cookies.Get("ecmCart");
            CartItem cartItem;
            if (cart != null)
            {
                cartItem = await _context.CartItems
                    .SingleOrDefaultAsync(x => x.CartId == cart.Value && x.ProductId == id);
            }
            else
            {
                cartItem = null;
            }

            if (cartItem != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CheckProductInWish(HttpContextBase httpContext, int id)
        {
            var wish = httpContext.Request.Cookies.Get("ecmWishList");
            WishList wishItem;
            if (wish != null)
            {
                wishItem = await _context.WishLists
                    .SingleOrDefaultAsync(x => x.wishId == wish.Value && x.ProductId == id);
            }
            else
            {
                wishItem = null;
            }

            if (wishItem != null)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            return await _context.Products
                .Where(x => x.Name.StartsWith(name))
                .ToListAsync();
        }
    }
}