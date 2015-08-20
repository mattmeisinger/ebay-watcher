using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EbayAPIHelper;
using EbayWatcher.BusinessLogic;

namespace EbayWatcher.Controllers
{
    public class AccountController : Controller
    {
        public JsonResult GetStatus()
        {
            return Json(new
            {
                LoggedIn = Users.IsLoggedIn()
            });
        }

        public JsonResult NewAuthRequest() => Json(EbayAuth.CreateNewAuthRequest());
        public JsonResult LogOut()
        {
            Users.LogOut();
            return Success();
        }
    }
}