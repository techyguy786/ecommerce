using ecommerce.Models;
using ecommerce.Services;
using ecommerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ecommerce.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly ApplicationDbContext _context;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public ProductController() : this(new ProductService())
        {
            _context = new ApplicationDbContext();
        }

        // GET: Admin/Product
        public async Task<ActionResult> Index()
        {
            var products = await _productService.GetProductsWithCategories();
            return View(products);
        }

        public async Task<ActionResult> AddProduct()
        {
            var viewModel = new ProductViewModel
            {
                Product = new Product(),
                Categories = await _productService.GetCategories(),
                CategoryTypes = await _productService.GetCategoryTypes()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProduct(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new ProductViewModel
                {
                    Product = model.Product,
                    Categories = await _productService.GetCategories(),
                    CategoryTypes = await _productService.GetCategoryTypes()
                };
                return View(viewModel);
            }

            var fileDetails = FileDetails();

            Product product = new Product
            {
                FileDetails = fileDetails,
                Name = model.Product.Name,
                BrandName = model.Product.BrandName,
                CategoryId = model.CategoryId,
                DiscountPercentage = model.Product.DiscountPercentage,
                Details = model.Product.Details,
                Featured = model.Product.Featured,
                FinalPrice = model.Product.FinalPrice,
                Gender = model.Product.Gender,
                Quantity = model.Product.Quantity,
                Price = model.Product.Price
            };

            if (model.Date != null)
            {
                product.ArrivalTime = Convert.ToDateTime(model.Date);
            }
            else
            {
                product.ArrivalTime = DateTime.Now;
            }

            await _productService.StoreProduct(product);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> PopulateCategoryById(int id)
        {
            _productService.MakeProxyCreationFalse();
            var categories = await _productService.GetCategoriesByCategoryTypeId(id);
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        private List<FileDetail> FileDetails()
        {
            List<FileDetail> fileDetails = new List<FileDetail>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    FileDetail fileDetail = new FileDetail
                    {
                        Id = new Guid(Guid.NewGuid().ToString("N")),
                        FileName = fileName,
                        Extension = Path.GetExtension(fileName)
                    };
                    fileDetails.Add(fileDetail);
                    var path = Path.Combine(Server.MapPath("~/Uploads/"), fileDetail.Id + fileDetail.Extension);
                    file.SaveAs(path);
                }
            }

            return fileDetails;
        }

        public async Task<ActionResult> EditProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = await _productService
                .GetProductWithFileDetailsById(Convert.ToInt32(id));
            if (product == null)
            {
                return HttpNotFound();
            }

            var c = await _productService.GetCategoryById(product.CategoryId);
            var typeId = c.CategoryTypeId;
            var viewModel = new ProductViewModel
            {
                Scheduled = true,
                Date = product.ArrivalTime.ToString(),
                Product = product,
                CategoryId = product.CategoryId,
                Categories = await _productService.GetCategoriesByCategoryTypeId(typeId),
                CategoryTypeId = typeId,
                CategoryTypes = await _productService.GetCategoryTypes()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProduct(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    FileDetail fileDetail = new FileDetail
                    {
                        Id = new Guid(Guid.NewGuid().ToString("N")),
                        FileName = fileName,
                        Extension = Path.GetExtension(fileName),
                        ProductId = model.Product.ProductId
                    };
                    var path = Path.Combine(Server.MapPath("~/Uploads/"), fileDetail.Id + fileDetail.Extension);
                    file.SaveAs(path);

                    _context.Entry(fileDetail).State = EntityState.Added;
                }
            }

            Product product = model.Product;

            if (model.Date != null)
            {
                product.ArrivalTime = Convert.ToDateTime(model.Date);
            }
            else
            {
                product.ArrivalTime = DateTime.Now;
            }

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteProduct(int id)
        {
            try
            {
                Product product = _context.Products.Find(id);
                if (product == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { Result = "Error" });
                }

                foreach (var file in product.FileDetails)
                {
                    string path = Path.Combine(Server.MapPath("~/Uploads/"), file.Id + file.Extension);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                _context.Products.Remove(product);
                _context.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { Result = "Error", Message = e.Message });
            }
        }

        public ActionResult Download(string p, string d)
        {
            return File(Path.Combine(Server.MapPath("~/Uploads/"), p),
                System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }

        [HttpPost]
        public ActionResult DeleteFile(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {
                Guid guid = new Guid(id);
                FileDetail fileDetail = _context.FileDetails.Find(guid);
                if (fileDetail == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { Result = "Error" });
                }

                //Remove from database
                _context.FileDetails.Remove(fileDetail);
                _context.SaveChanges();

                //Delete file from the file system
                var path = Path.Combine(Server.MapPath("~/Uploads/"), fileDetail.Id + fileDetail.Extension);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}