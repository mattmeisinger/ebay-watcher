using EbayWatcher.BusinessLogic;
using EbayWatcher.Entities.Models;
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
                if (await Ebay.IsAuthenticatedWithEbay(UserManager, AuthenticationManager))
                {
                    return RedirectToAction("Index", "Wishlist");
                }
            }
            else
            {
                return Content("Already logged in");
            }

            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session["EbaySessionId"] = null;
            HttpContext.Session["EbayToken"] = null;
            HttpContext.GetOwinContext().Authentication.SignOut();
            return Content("Logged out");
        }

        public ActionResult ToGoEbayLogin()
        {
            var sessionId = Ebay.GetNewSessionId();
            HttpContext.Session["EbaySessionId"] = sessionId;
            var ebayLoginUrl = Ebay.GetLoginUrl(sessionId);
            return Redirect(ebayLoginUrl);
        }
    }
}