using EbayWatcher.BusinessLogic;
using EbayWatcher.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EbayWatcher.Controllers
{
    public abstract class AuthenticatedController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // If user isn't logged in, see if they are on the second step of ebay authentication
            if (!Users.IsLoggedIn())
            {
                // If they are on the second stage of ebay authentication, see if it is complete
                if (Users.GetCurrentSessionId() != null)
                {
                    var authenticationDidSucceed = Ebay.CompleteEbayAuthentication();
                    if (authenticationDidSucceed)
                    {
                        // Do nothing. The authentication is complete so they can see any page now.
                    }
                    else
                    {
                        filterContext.Result = RedirectToAction("Login", "Account");
                    }
                }
                else
                {
                    filterContext.Result = RedirectToAction("Login", "Account");
                }
            }
            else
            {
                // Do nothing.  The user is already logged in.
            }

            base.OnActionExecuting(filterContext);
        }
    }
}