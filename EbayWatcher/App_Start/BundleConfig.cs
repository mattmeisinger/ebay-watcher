using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace EbayWatcher
{
    public class BundleConfig
    {
        internal static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/js")
                .Include("~/Scripts/jquery-ui-1.10.4.js")
                .IncludeDirectory("~/Scripts/EbayWatcher", "*.js"));
            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/themes/base/jquery-ui.css")
                .Include("~/Content/Site.css"));
        }
    }
}