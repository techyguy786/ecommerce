using ecommerce.Models;
using ecommerce.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ecommerce.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Admin/Category
        public ActionResult CategoryTypes()
        {
            return View(_context.CategoryTypes.ToList());
        }

        public ActionResult AddCategoryType()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategoryType(CategoryType model)
        {
            if (ModelState.IsValid)
            {
                _context.CategoryTypes.Add(model);
                _context.SaveChanges();
                return RedirectToAction("AddCategoryType");
            }
            return View(model);
        }

        public ActionResult EditCategoryType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var categoryType = _context.CategoryTypes.SingleOrDefault(x => x.CategoryTypeId == id);
            if (categoryType == null)
            {
                return HttpNotFound();
            }
            return View(categoryType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategoryType(CategoryType category)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(category).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("CategoryTypes");
            }
            return View(category);
        }

        public ActionResult DeleteCategoryType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryType category = _context.CategoryTypes.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteCategoryType")]
        public ActionResult DeleteCategoryTypeConfirmed(int id)
        {
            CategoryType category = _context.CategoryTypes.Find(id);
            _context.CategoryTypes.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("CategoryTypes");
        }

        public ActionResult Categories()
        {
            var categories = _context.Categories.Include(x => x.Type).ToList();
            return View(categories);
        }

        public ActionResult AddCategories()
        {
            var model = new CategoriesViewModel
            {
                Types = _context.CategoryTypes.ToList(),
                Category = new Category()
            };
            return View("_Categories", model);
        }

        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
            }

            Category category = _context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var viewModel = new CategoriesViewModel
            {
                Category = category,
                Types = _context.CategoryTypes.ToList()
            };
            return View("_Categories", viewModel);
        }

        public ActionResult DeleteCategory(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteCategory")]
        public ActionResult DeleteCategoryConfirmed(int id)
        {
            Category category = _context.Categories.Find(id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Categories");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCategories(Category category)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CategoriesViewModel
                {
                    Category = category,
                    Types = _context.CategoryTypes.ToList()
                };
                return View("_Categories", viewModel);
            }

            var id = category.CategoryId;
            if (id == 0)
            {
                _context.Categories.Add(category);
            }
            else
            {
                _context.Entry(category).State = EntityState.Modified;
            }
            _context.SaveChanges();
            //return RedirectToAction("Categories");
            return RedirectToAction("AddCategories");
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