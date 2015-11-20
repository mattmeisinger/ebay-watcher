using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace System
{
    public static class ExtensionMethods
    {
        public static string GetHeader(this HttpRequestMessage req, string headerName)
        {
            return req.Headers.Where(a => a.Key == headerName).Select(a => a.Value.First()).FirstOrDefault();
        }
    }
}