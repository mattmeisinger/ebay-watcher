using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EbayWatcher.Controllers
{
    public class SetupController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string DevID, string AppID, string RuName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(DevID) || string.IsNullOrWhiteSpace(AppID) || string.IsNullOrWhiteSpace(RuName))
                {
                    throw new Exception("DevID and AppID must contain values.");
                }

                // Save the settings entered
                BusinessLogic.AppSettings.Set("DevID", DevID);
                BusinessLogic.AppSettings.Set("AppID", AppID);
                BusinessLogic.AppSettings.Set("RuName", RuName);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                ViewBag.DevID = DevID;
                ViewBag.AppID = AppID;
                ViewBag.RuName = RuName;
                return View();
            }

            return Redirect("~/");
        }
    }
}