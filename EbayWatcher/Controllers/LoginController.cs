using EbayWatcher.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EbayWatcher.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.LoginUrl = Ebay.GetLoginUrl();
            Session["AuthenticatingWithEbay"] = true;
            return Redirect(Ebay.GetLoginUrl());
        }
    }
}