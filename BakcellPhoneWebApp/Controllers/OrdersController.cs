using BakcellPhoneWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BakcellPhoneWebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrdersController()
        {
            _db = new ApplicationDbContext();
        }
        // GET: Orders
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var Orders = _db.Orders.ToList();
            return View(Orders);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult OnWayStatus()
        {
            var couriers =
              _db.Couriers
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name + " " + s.Surname + " " + s.PhoneNumber
                })
                .ToList();
            ViewBag.Couriers = new SelectList(couriers, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult OnWayStatus(int id)
        {
            var couriers =
              _db.Couriers
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name + " " + s.Surname + " " + s.PhoneNumber
                })
                .ToList();

            var order = _db.Orders.Find(id);
            if (order == null)
            {
                HttpNotFound();
            }

            string courierid = Request.Form["Kuryer"].ToString();

            order.CourierId = courierid;
            order.Status = OrderStatus.Yolda;

            var token = "1798612318:AAE3L9DaxUpym5i0IH2dBxOEAmfHYoz1uaM";
            var chat_id = "-1001162743010";
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            var link = baseUrl + "Home/BeingDeliveredOrders";
            var user = _db.Couriers.Find(order.CourierId);

            var text = "<b> Yeni sifariş </b>: %0A - <b> Tg istifadəçi adı:</b><i> @" + user.TgUsername + " </i> %0A - <b> Ad:</b><i> " + user.Name + " </i> %0A - <b> Soyad:</b><i> " + user.Surname + " </i> %0A - <b> Ünvan:</b><i> " + user.Location + " </i> %0A - <b> Link:</b><i> " + link + " </i>";

            var url = "https://api.telegram.org/bot"+token+"/sendMessage?chat_id="+chat_id+"&text="+text+"&parse_mode=html";

            var request = WebRequest.Create(url);
            request.Method = "GET";

            using var webResponse = request.GetResponse();

            _db.Entry(order).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("PendingOrders", "Home");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminDeliver(int id)
        {
            var order = _db.Orders.Find(id);
            if (order == null)
            {
                HttpNotFound();
            }
            order.Status = OrderStatus.Çatdırıldı;
            _db.Entry(order).State = System.Data.Entity.EntityState.Modified;

            var price = order.NumberPrice;
            var manager = _db.Users.Find(order.ManagerId);
            var vendor = _db.Users.Find(order.VendorId);

            manager.Balance += 2.00m;
            vendor.Balance += price - 10;

            _db.Entry(manager).State = System.Data.Entity.EntityState.Modified;
            _db.Entry(vendor).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("PendingOrders", "Home");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ConfirmOrder(int id)
        {
            var order = _db.Orders.Find(id);
            if (order == null)
            {
                HttpNotFound();
            }
            order.Status = OrderStatus.Çatdırıldı;
            _db.Entry(order).State = System.Data.Entity.EntityState.Modified;

            var price = order.NumberPrice;
            var courier = _db.Users.Find(order.CourierId);
            var manager = _db.Users.Find(order.ManagerId);
            var vendor = _db.Users.Find(order.VendorId);

            courier.Balance += 2.50m;
            manager.Balance += 2.00m;
            vendor.Balance += price - 10;

            _db.Entry(courier).State = System.Data.Entity.EntityState.Modified;
            _db.Entry(manager).State = System.Data.Entity.EntityState.Modified;
            _db.Entry(vendor).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("WaitingOrders", "Home");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RejectOrder(int id)
        {
            var order = _db.Orders.Find(id);
            if (order == null)
            {
                HttpNotFound();
            }

            var filePath = Server.MapPath("~/Uploads/Confirmation/" + order.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            order.Status = OrderStatus.Yolda;
            order.Image = null;
            _db.Entry(order).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            var token = "1798612318:AAE3L9DaxUpym5i0IH2dBxOEAmfHYoz1uaM";
            var chat_id = "-1001162743010";
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            var link = baseUrl + "Home/BeingDeliveredOrders";
            var user = _db.Couriers.Find(order.CourierId);

            var text = "<b> Təsdiqlənməmiş sifariş </b>: %0A - <b> Tg istifadəçi adı:</b><i> @" + user.TgUsername + " </i> %0A - <b> Ad:</b><i> " + user.Name + " </i> %0A - <b> Soyad:</b><i> " + user.Surname + " </i> %0A - <b> Ünvan:</b><i> " + user.Location + " </i> %0A - <b> Link:</b><i> " + link + " </i>";

            var url = "https://api.telegram.org/bot" + token + "/sendMessage?chat_id=" + chat_id + "&text=" + text + "&parse_mode=html";

            var request = WebRequest.Create(url);
            request.Method = "GET";

            using var webResponse = request.GetResponse();

            return RedirectToAction("WaitingOrders", "Home");
        }


        [HttpPost]
        [Authorize(Roles = "Kuryer")]
        public ActionResult DeliveredStatus(int id)
        {
            var order = _db.Orders.Find(id);
            if (order == null)
            {
                HttpNotFound();
            }
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                                                            //Use the following properties to get file's name, size and MIMEType
                int fileSize = file.ContentLength;
                //var fileName = DateTime.Now.ToLocalTime() + Path.GetExtension(file.FileName);
                var fileName = Path.GetFileName(file.FileName);
                string mimeType = file.ContentType;
                Stream fileContent = file.InputStream;
                //To save file, use SaveAs method
                file.SaveAs(Path.Combine(Server.MapPath("~/Uploads/Confirmation/"), DateTime.Now.ToString("yyyy_MM_dd_mm_ss") + "_" + fileName));
                order.Image = fileName;
                order.Status = OrderStatus.Yoxlanılır;
                _db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();

                var token = "1798612318:AAE3L9DaxUpym5i0IH2dBxOEAmfHYoz1uaM";
                var chat_id = "-1001176204874";
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                var link = baseUrl + "Home/WaitingOrders";
                var user = _db.Couriers.Find(order.CourierId);

                var text = "<b> Sifariş </b>: %0A - <b> Kuryer:</b><i> " + user.Name + " " + user.Surname + " </i> %0A - <b> Link:</b><i> " + link + " </i>";

                var url = "https://api.telegram.org/bot" + token + "/sendMessage?chat_id=" + chat_id + "&text=" + text + "&parse_mode=html";

                var request = WebRequest.Create(url);
                request.Method = "GET";

                using var webResponse = request.GetResponse();
            }
            return Json(Request.Files.Count + " şəkil yükləndi.");
        }

        [Authorize(Roles = "Menecer")]
        public ActionResult List()
        {
            var userid = User.Identity.GetUserId();
            var Orders = _db.Orders.Where(x=>x.ManagerId==userid).ToList();
            return View(Orders);
        }

        [Authorize(Roles = "Satıcı")]
        public ActionResult OrderList()
        {
            var userid = User.Identity.GetUserId();
            var Orders = _db.Orders.Where(x => x.VendorId == userid && x.Status == OrderStatus.Çatdırıldı).ToList();
            return View(Orders);
        }

        [Authorize(Roles = "Menecer")]
        // GET: Orders/Create
        public ActionResult Create()
        {
            var vendors =
              _db.Vendors
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name + " " + s.Surname + " " + s.PhoneNumber
                })
                .ToList();
            ViewBag.Vendors = new SelectList(vendors, "Id", "Name");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [Authorize(Roles = "Menecer")]
        public ActionResult Create(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var vendors =
                  _db.Vendors
                    .Select(s => new
                    {
                        Id = s.Id,
                        Name = s.Name + " " + s.Surname + " " + s.PhoneNumber
                    })
                    .ToList();
                ViewBag.Vendors = new SelectList(vendors, "Id", "Name");
                var order = new Order { 
                    Address = model.Address,
                    City = model.City,
                    CreatedDate = DateTime.Now,
                    CustomerInfo = model.CustomerInfo,
                    CustomerPhoneNumber = model.CustomerPhoneNumber,
                    DeliveryTime = model.DeliveryTime,
                    District = model.District,
                    NumberPrice = model.NumberPrice,
                    OrderedPhoneNumber = model.OrderedPhoneNumber,
                    Status = OrderStatus.Gözləmədə,
                    ManagerId = User.Identity.GetUserId(),
                    VendorId = model.VendorId
                };
                _db.Orders.Add(order);
                _db.SaveChanges();

                var token = "1798612318:AAE3L9DaxUpym5i0IH2dBxOEAmfHYoz1uaM";
                var chat_id = "-1001178088564";
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                var link = baseUrl + "Home/SentOrders";
                var user = _db.Managers.Find(order.ManagerId);

                var text = "<b> Yeni sifariş </b>: %0A - <b> Menecer:</b><i> " + user.Name + " " + user.Surname + " </i> %0A - <b> Link:</b><i> " + link + " </i>";

                var url = "https://api.telegram.org/bot" + token + "/sendMessage?chat_id=" + chat_id + "&text=" + text + "&parse_mode=html";

                var request = WebRequest.Create(url);
                request.Method = "GET";

                using var webResponse = request.GetResponse();

                return RedirectToAction("List", "Orders");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var vendors =
              _db.Vendors
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name + " " + s.Surname + " " + s.PhoneNumber
                })
                .ToList();
            ViewBag.Vendors = new SelectList(vendors, "Id", "Name");

            var managers =
              _db.Managers
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name + " " + s.Surname
                })
                .ToList();
            ViewBag.Managers = new SelectList(managers, "Id", "Name");
            var order = _db.Orders.Find(id);
            if (order == null)
            {
                HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Address,City,CustomerInfo,CustomerPhoneNumber,DeliveryTime,CreatedDate,District,NumberPrice,OrderedPhoneNumber,Status,ManagerId,VendorId")] Order order)
        {
            var vendors =
              _db.Vendors
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name + " " + s.Surname + " " + s.PhoneNumber
                })
                .ToList();
            ViewBag.Vendors = new SelectList(vendors, "Id", "Name");

            var managers =
              _db.Managers
                .Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name + " " + m.Surname
                })
                .ToList();
            ViewBag.Managers = new SelectList(managers, "Id", "Name");
            if (ModelState.IsValid)
            {
                _db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var order = _db.Orders.Find(id);
            if (order == null)
            {
                HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Order order)
        {
            var myorder = _db.Orders.Find(order.Id);
            _db.Orders.Remove(myorder);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
