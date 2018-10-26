using ecommerce.Models;
using System.Collections.Generic;

namespace ecommerce.ViewModels
{
    public class ProductTypesViewModel
    {
        public IEnumerable<Product> NewArrivals { get; set; }
        public IEnumerable<Product> PopularProducts { get; set; }
        public IEnumerable<Product> FeaturedProducts { get; set; }
    }
}