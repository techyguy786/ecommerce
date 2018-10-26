using System.Collections.Generic;
using ecommerce.Models;

namespace ecommerce.ViewModels
{
    public class CartItemsViewModel
    {
        public IEnumerable<CartItem> Items { get; set; }
        public decimal Sum { get; set; }
    }
}