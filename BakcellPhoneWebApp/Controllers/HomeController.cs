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
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = _db.Users.FirstOrDefault(x => x.Id == currentUserId);
            ViewBag.Balance = currentUser.Balance;
            return View();
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
            var orders = _db.Orders.Where(x => x.Status != OrderStatus.Gözləmədə).ToList();
            return View(orders);
        }

        [Authorize(Roles = "Kuryer")]
        public ActionResult BeingDeliveredOrders()
        {
            string currentUserId = User.Identity.GetUserId();
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