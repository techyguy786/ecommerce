using ecommerce.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.Services
{
    public class HomeService
    {
        private readonly ApplicationDbContext _context;

        public HomeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public HomeService() : this(new ApplicationDbContext())
        {
        }

        public async Task<IEnumerable<Product>> NewArrivalProducts()
        {
            return await _context
                .Products.OrderByDescending(x => x.ArrivalTime)
                .Include(x => x.FileDetails)
                .Include(x => x.Category)
                .Take(5)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> FeaturedProducts()
        {
            return await _context
                .Products.Where(x => x.Featured)
                .Include(x => x.FileDetails)
                .Include(x => x.Category)
                .Take(5)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> PopularProducts()
        {
            return await _context.OrderDetails
                .Where(x => x.Quantity > 100)
                .Take(5)
                .Select(x => x.Product)
                .ToListAsync();
        }
    }
}