using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using eBayWatcher.WebAPI.Core;

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
