using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using eBayWatcher.Core;
using eBayWatcher.Core.eBay;

namespace eBayWatcher.WebAPI
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            EbaySettings.AppID = AppSettings.Get("AppID");
            EbaySettings.CertID = AppSettings.Get("CertID");
            EbaySettings.DevID = AppSettings.Get("DevID");
            EbaySettings.RuName = AppSettings.Get("RuName");
            EbaySettings.TradingServerAddress = ConfigurationManager.AppSettings["TradingServerAddress"];
            EbaySettings.FindingServerAddress = ConfigurationManager.AppSettings["FindingServerAddress"];
            EbaySettings.EbayXMLAPIURL = ConfigurationManager.AppSettings["EbayXMLAPIURL"];
        }
    }
}
