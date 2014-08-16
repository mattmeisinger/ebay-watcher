using EbayWatcher.BusinessLogic;
using EbayWatcher.Entities.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EbayWatcher.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        public async Task<ActionResult> Login()
        {
            if (!Users.IsLoggedIn())
            {
                if (Ebay.IsAuthenticatedWithEbay())
                {
                    var username = Users.GetCurrentUser().UserName;

                    // Create user if it doesn't already exist
                    var user = UserManager.Users.SingleOrDefault(a => a.UserName == username);
                    if (user == null)
                    {
                        user = new ApplicationUser() { UserName = username };
                        UserManager.Create(user);
                    }

                    // Log into identity
                    var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

                    return RedirectToAction("Index", "Wishlist");
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return Content("Logged out");
        }

        public ActionResult ToGoEbayLogin()
        {
            return Redirect(Ebay.GetLoginUrl());
        }
    }
}