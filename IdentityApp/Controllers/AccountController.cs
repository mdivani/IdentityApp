using IdentityApp.Identity;
using IdentityApp.Models;
using IdentityService.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdentityApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _manager;
        private readonly SignInManager<IdentityUser, string> _signIn;
        private readonly RoleManager<IdentityRole, int> _roleManager;

        public AccountController(
ApplicationUserManager manager,
SignInManager<IdentityUser, string> signIn,
RoleManager<IdentityRole, int> roleManager
)
        {
            _manager = manager;
            _signIn = signIn;
            _roleManager = roleManager;
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _manager.FindAsync(model.Email, model.Password);
                if (user != null)
                {
                    if (!_manager.IsEmailConfirmed(user.Id))
                    {
                        return Content("Confirm Your email before sign in!");
                    }
                    var identity = _manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Email, model.Email));
                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                    return redirectToLocal(returnUrl);
                }
                else
                {
                    //TODO: display 'incorrect user!' error message/view
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser(model.Email);
                var result = await _manager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var code = await _manager.GenerateEmailConfirmationTokenAsync(model.Email); // maili unda iyos aq imito ro jer ar icis appma basam ra id mianicha
                    var callbackUrl = Url.Action(
                         "ConfirmEmail", "Account",
                         new { userId = model.Email, code = code },
                         protocol: Request.Url.Scheme);

                    await _manager.SendEmailAsync(model.Email,
                          "Confirm your account",
                          "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    return Content("Please confirm your account. Link was sent to " + model.Email);
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)  return View("Register"); //error
            var result = await _manager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
              return View();
            }
            return View("Register");
        }

        public ActionResult Logout()
        {
            _signIn.AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home", null);
        }

        #region Helpers
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult redirectToLocal(string returnUrl = "")
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home", null);
            }
        }
#endregion
    }
}