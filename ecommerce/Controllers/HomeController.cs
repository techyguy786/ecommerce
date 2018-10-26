using ecommerce.Models;
using ecommerce.Services;
using ecommerce.ViewModels;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HomeService _service;

        public HomeController(HomeService service, ApplicationDbContext context)
        {
            _context = context;
            _service = service;
        }

        public HomeController() : this(new HomeService(), new ApplicationDbContext())
        {
        }

        public async Task<ActionResult> Index()
        {
            var viewModel = new HomeProductsViewModel
            {
                NewArrivals = await _service.NewArrivalProducts(),
                FeaturedProducts = await _service.FeaturedProducts()
            };
            return View(viewModel);
        }

        [ChildActionOnly]
        public ActionResult FeaturedItems()
        {
            var products = _context
                .Products.Where(x => x.Featured)
                .OrderByDescending(x => x.ProductId)
                .Include(x => x.FileDetails)
                .Take(9)
                .ToList();
            return PartialView(products);
        }

        [ChildActionOnly]
        public ActionResult Notification()
        {
            var cartId = Request.Cookies["ecmCart"]?.Value;
            IEnumerable<CartItem> items;
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                items = context.CartItems.Where(x => x.CartId == cartId)
                    .Include(x => x.Product)
                    .OrderByDescending(x => x.DateCreated)
                    .ToList();
            }

            var totalsum = items.Sum(x => x.Product.FinalPrice * x.Count);

            var model = new CartItemsViewModel
            {
                Items = items,
                Sum = totalsum
            };

            return PartialView(model);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return View(contact);
            }

            MailMessage mail;
            var client = SmtpClient(out mail);
            mail.Subject = contact.FirstName;
            mail.Body = "Hi, I'm" + contact.FirstName + ", \r\n" +
                        contact.Message + "\r\n\r\n" +
                        contact.FirstName + "\r\n" +
                        contact.LastName + "\r\n" +
                        contact.Email + "\r\n" +
                        contact.Phone1 + "\r\n" +
                        contact.Phone2 + "\r\n" +
                        contact.Address;
            client.Send(mail);

            ViewBag.Message = "Thanks For Your Response, We'll reach you very soon...";
            return PartialView("_EmailSent");
        }

        private static SmtpClient SmtpClient(out MailMessage mail)
        {
            string from = ConfigurationManager.AppSettings["from"];
            string password = ConfigurationManager.AppSettings["pwd"];
            string to = ConfigurationManager.AppSettings["to"];

            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(from, password);

            mail = new MailMessage(from, to);
            return client;
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