using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EbayWatcher.Controllers
{
    public abstract class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!BusinessLogic.Ebay.ApiSettingsValid())
            {
                filterContext.Result = RedirectToAction("Index", "Setup");
                return;
            }

            // Don't need to require login yet, since we're just using the search
            //if (!BusinessLogic.Users.IsLoggedIn())
            //{
            //    filterContext.Result = RedirectToAction("Index", "Login");
            //}

            base.OnActionExecuting(filterContext);
        }
    }
}