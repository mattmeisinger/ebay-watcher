using EbayWatcher.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EbayWatcher.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            if (Users.IsLoggedIn())
            {
                return Content("Already logged in");
            }
            else
            {
                return View();
            }
        }
        public ActionResult GoToEbayLogin()
        {
            var sessionId = Ebay.GetNewSessionId();
            HttpContext.Session["EbaySessionId"] = sessionId;
            var ebayLoginUrl = Ebay.GetLoginUrl(sessionId);
            return Redirect(ebayLoginUrl);
        }
        public ActionResult Logout()
        {
            Users.LogOut();
            return RedirectToAction("Login");
        }
    }
}