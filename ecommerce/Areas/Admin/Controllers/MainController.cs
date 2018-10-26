using System.Web.Mvc;

namespace ecommerce.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MainController : Controller
    {
        // GET: Admin/Main
        public ActionResult Index()
        {
            return View();
        }
    }
}