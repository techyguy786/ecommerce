using System.Threading.Tasks;
using ecommerce.Models;

namespace ecommerce.ViewModels
{
    public class ProductDetailViewModel
    {
        public string CategoryName { get; set; }
        public Product Product { get; set; }
        public FileDetail FileDetail { get; set; }
        public bool CartItem { get; set; }
        public bool WishItem { get; set; }
    }
}