using ecommerce.Models;
using System.Collections.Generic;

namespace ecommerce.ViewModels
{
    public class MainProductsViewModel
    {
        public IEnumerable<CategoryType> CategoryTypes { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}