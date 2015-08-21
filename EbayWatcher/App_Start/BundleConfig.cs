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
            bundles.Add(new ScriptBundle("~/bundles/scripts")
                .Include("~/Scripts/libs/jquery-{version}.js")
                .Include("~/Scripts/libs/angular.js")
                .IncludeDirectory("~/Scripts", "*.js", false)
                );

            bundles.Add(new StyleBundle("~/bundles/styles")
                .Include("~/Content/Site.css"));
        }
    }
}