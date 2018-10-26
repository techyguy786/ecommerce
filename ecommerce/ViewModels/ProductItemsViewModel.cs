using ecommerce.Models;
using System.Collections.Generic;

namespace ecommerce.ViewModels
{
    public class ProductItemsViewModel
    {
        public IEnumerable<WishList> WishLists { get; set; }
        public IEnumerable<CartItem> Items { get; set; }
        public decimal Sum { get; set; }
    }
}