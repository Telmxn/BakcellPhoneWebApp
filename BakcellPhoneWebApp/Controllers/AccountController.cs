using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BakcellPhoneWebApp.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BakcellPhoneWebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _db;

        public AccountController()
        {
            _db = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Managers()
        {
            return View(_db.Managers.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditManager(string id)
        {
            ApplicationUser appUser = new ApplicationUser();
            appUser = UserManager.FindById(id);
            ManagerEdit user = new ManagerEdit();
            user.Id = appUser.Id;
            user.Name = appUser.Name;
            user.Surname = appUser.Surname;

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Vendors()
        {
            return View(_db.Vendors.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditVendor(string id)
        {
            ApplicationUser appUser = new ApplicationUser();
            appUser = UserManager.FindById(id);
            VendorEdit user = new VendorEdit();
            user.Id = appUser.Id;
            user.Name = appUser.Name;
            user.PhoneNumber = appUser.PhoneNumber;
            user.Surname = appUser.Surname;

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditVendor(VendorEdit model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var store = new UserStore<ApplicationUser>(_db);
            var manager = new UserManager<ApplicationUser>(store);
            var currentUser = manager.FindById(model.Id);
            currentUser.Name = model.Name;
            currentUser.Surname = model.Surname;
            await manager.UpdateAsync(currentUser);
            var ctx = store.Context;
            ctx.SaveChanges();
            return RedirectToAction("Vendors");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditManager(ManagerEdit model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var store = new UserStore<ApplicationUser>(_db);
            var manager = new UserManager<ApplicationUser>(store);
            var currentUser = manager.FindById(model.Id);
            currentUser.Name = model.Name;
            currentUser.Surname = model.Surname;
            await manager.UpdateAsync(currentUser);
            var ctx = store.Context;
            ctx.SaveChanges();
            return RedirectToAction("Managers");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Couriers()
        {
            return View(_db.Couriers.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditCourier(string id)
        {
            Courier appUser = new Courier();
            appUser = (Courier)UserManager.FindById(id);
            CourierEdit user = new CourierEdit();
            user.Id = appUser.Id;
            user.TgUsername = appUser.TgUsername;
            user.Location = appUser.Location;
            user.Name = appUser.Name;
            user.Surname = appUser.Surname;

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditCourier(CourierEdit model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var store = new UserStore<Courier>(_db);
            var manager = new UserManager<Courier>(store);
            var currentUser = manager.FindById(model.Id);
            currentUser.TgUsername = model.TgUsername;
            currentUser.Location = model.Location;
            currentUser.Name = model.Name;
            currentUser.Surname = model.Surname;
            await manager.UpdateAsync(currentUser);
            var ctx = store.Context;
            ctx.SaveChanges();
            return RedirectToAction("Couriers");
        }

        public async Task<ActionResult> DeleteVendor(string id)
        {
            var user = await _db.Vendors.FindAsync(id);
            _db.Vendors.Remove(user);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> DeleteCourier(string id)
        {
            var user = await _db.Couriers.FindAsync(id);
            _db.Couriers.Remove(user);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> DeleteManager(string id)
        {
            var user = await _db.Managers.FindAsync(id);
            _db.Managers.Remove(user);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Yanlış giriş cəhdi.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.UserType = new SelectList(_db.Roles.ToList(), "Name", "Name");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.UserType = new SelectList(_db.Roles.ToList(), "Name", "Name");
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    await UserManager.AddToRoleAsync(user.Id, model.UserType);

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/AddCourier
        [Authorize(Roles = "Admin")]
        public ActionResult AddCourier()
        {
            return View();
        }

        //
        // POST: /Account/AddCourier
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCourier(CourierViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Courier { UserName = model.UserName, Name = model.Name, Surname = model.Surname, TgUsername = model.TgUsername, Location = model.Location };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //Don't login at that moment.
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    await UserManager.AddToRoleAsync(user.Id, "Kuryer");

                    return RedirectToAction("Couriers", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/AddVendor
        [Authorize(Roles = "Admin")]
        public ActionResult AddVendor()
        {
            return View();
        }

        //
        // POST: /Account/AddVendor
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddVendor(VendorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Vendor { Name = model.Name, Surname = model.Surname, PhoneNumber = model.PhoneNumber, UserName = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //Don't login at that moment.
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    await UserManager.AddToRoleAsync(user.Id, "Satıcı");

                    return RedirectToAction("Vendors", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/AddManager
        [Authorize(Roles = "Admin")]
        public ActionResult AddManager()
        {
            return View();
        }

        //
        // POST: /Account/AddManager
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddManager(ManagerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Manager { UserName = model.UserName, Name = model.Name, Surname = model.Surname };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //Don't login at that moment.
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    await UserManager.AddToRoleAsync(user.Id, "Menecer");

                    return RedirectToAction("Managers", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}