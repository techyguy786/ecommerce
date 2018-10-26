using ecommerce.Models;
using System.Collections.Generic;

namespace ecommerce.ViewModels
{
    public class CategoriesViewModel
    {
        public IEnumerable<CategoryType> Types { get; set; }
        public Category Category { get; set; }
    }
}