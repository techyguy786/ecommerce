using System.Collections.Generic;
using ecommerce.Models;

namespace ecommerce.ViewModels
{
    public class HomeProductsViewModel
    {
        public IEnumerable<Product> NewArrivals { get; set; }
        public IEnumerable<Product> PopularProducts { get; set; }
        public IEnumerable<Product> FeaturedProducts { get; set; }
        public IEnumerable<Product> ScheduledProducts { get; set; }
    }
}