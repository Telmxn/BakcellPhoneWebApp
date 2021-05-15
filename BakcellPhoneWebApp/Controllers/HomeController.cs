using BakcellPhoneWebApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BakcellPhoneWebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController()
        {
            _db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            string ReturnUrl;
            if (User.IsInRole("Admin"))
            {
                ReturnUrl = baseUrl + "Home/PendingOrders";
            }
            else if (User.IsInRole("Menecer"))
            {
                ReturnUrl = baseUrl + "Orders/List";
            }
            else if (User.IsInRole("Satıcı"))
            {
                ReturnUrl = baseUrl + "Orders/OrderList";
            }
            else
            {
                ReturnUrl = baseUrl + "Home/BeingDeliveredOrders";
            }
            return Redirect(ReturnUrl);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult PendingOrders()
        {
            var orders = _db.Orders.Where(x => x.Status == OrderStatus.Gözləmədə).ToList();
            return View(orders);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SentOrders()
        {
            var orders = _db.Orders.Where(x => x.Status == OrderStatus.Yolda || x.Status == OrderStatus.Çatdırıldı).ToList();
            return View(orders);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult WaitingOrders()
        {
            var orders = _db.Orders.Where(x => x.Status == OrderStatus.Yoxlanılır).ToList();
            return View(orders);
        }

        [Authorize(Roles = "Kuryer")]
        public ActionResult BeingDeliveredOrders()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = _db.Users.FirstOrDefault(x => x.Id == currentUserId);
            ViewBag.Balance = currentUser.Balance;
            var orders = _db.Orders.Where(x => x.CourierId == currentUserId && x.Status == OrderStatus.Yolda).ToList();
            return View(orders);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}