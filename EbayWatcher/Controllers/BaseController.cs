using EbayWatcher.BusinessLogic;
using EbayWatcher.Entities;
using EbayWatcher.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;

namespace EbayWatcher.Controllers
{
    public abstract class BaseController : Controller
    {
        private ApplicationUserManager _userManager;
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
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public BaseController()
        {
        }

        public BaseController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            //if (!BusinessLogic.Ebay.ApiSettingsValid())
            //{
            //    filterContext.Result = RedirectToAction("Index", "Setup");
            //    return;
            //}

            // Don't need to require login yet, since we're just using the search
            //if (!BusinessLogic.Users.IsLoggedIn())
            //{
            //    filterContext.Result = RedirectToAction("Index", "Login");
            //}

            base.OnActionExecuting(filterContext);
        }
    }
}