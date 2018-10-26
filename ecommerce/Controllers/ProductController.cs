using ecommerce.Services;
using ecommerce.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public ProductController() : this(new ProductService())
        {
        }

        // GET: Product
        public async Task<ActionResult> Index()
        {
            var viewModel = new MainProductsViewModel
            {
                CategoryTypes = await _productService.GetCategoryTypes(),
                Products = await _productService.GetProducts()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ProductItems(string name)
        {
            var products = await _productService.GetProductsByName(name);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        //public async Task<ActionResult> ProductItems()
        //{
        //    var viewModel = new MainProductsViewModel
        //    {
        //        CategoryTypes = await _productService.GetCategoryTypes(),
        //        Products = await _productService.GetProducts()
        //    };

        //    var result = JsonConvert.SerializeObject(viewModel, Formatting.Indented,
        //        new JsonSerializerSettings
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //        });
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public async Task<ActionResult> Detail(int id)
        {
            var product = await _productService.GetProductById(id);
            var cart = await _productService.CheckProductInCart(HttpContext, id);
            var wish = await _productService.CheckProductInWish(HttpContext, id);
            var categoryId = product.CategoryId;
            var category = await _productService.GetCategoryById(categoryId);
            var viewModel = new ProductDetailViewModel
            {
                CategoryName = category.CategoryName,
                Product = product,
                CartItem = cart,
                WishItem = wish,
                FileDetail = await _productService.GetFileDetailByProductId(product.ProductId)
            };
            return View(viewModel);
        }

        public async Task<ActionResult> CategoryProducts(int id)
        {
            var products = await _productService.ProductsByCategory(id);
            var viewModel = new MainProductsViewModel
            {
                Products = products,
                CategoryTypes = await _productService.GetCategoryTypes()
            };
            return View("Index", viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    _context.Dispose();
            //}
            base.Dispose(disposing);
        }
    }
}