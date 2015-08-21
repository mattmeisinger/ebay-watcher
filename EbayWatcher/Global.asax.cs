using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using System.Configuration;
using EbayWatcher.BusinessLogic;

namespace EbayWatcher
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            EbayAPIHelper.EbaySettings.AppID = AppSettings.Get("AppID");
            EbayAPIHelper.EbaySettings.CertID = AppSettings.Get("CertID");
            EbayAPIHelper.EbaySettings.DevID = AppSettings.Get("DevID");
            EbayAPIHelper.EbaySettings.EBayToken = AppSettings.Get("EBayToken");
            EbayAPIHelper.EbaySettings.RuName = AppSettings.Get("RuName");
            EbayAPIHelper.EbaySettings.TradingServerAddress = ConfigurationManager.AppSettings["TradingServerAddress"];
            EbayAPIHelper.EbaySettings.FindingServerAddress = ConfigurationManager.AppSettings["FindingServerAddress"];
            EbayAPIHelper.EbaySettings.EbayXMLAPIURL = ConfigurationManager.AppSettings["EbayXMLAPIURL"];
            
        }
    }
}
