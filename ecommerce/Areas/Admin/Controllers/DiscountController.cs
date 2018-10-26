using System.Web.Mvc;

namespace ecommerce.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DiscountController : Controller
    {
        // GET: Admin/Discount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return RedirectToAction("Index");
        }
    }
}