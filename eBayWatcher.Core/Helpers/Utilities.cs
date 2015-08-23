using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace eBayWatcher.Core.Helpers
{
    public class Utilities
    {
        public static string PostXML(string url, string body, WebHeaderCollection headers)
        {
            var req = WebRequest.Create(url);
            req.ContentType = "text/xml; encoding='utf-8'";
            req.Method = "POST";
            if (headers != null)
            {
                req.Headers = headers;
            }
            byte[] bytes = Encoding.ASCII.GetBytes(body);
            req.ContentLength = bytes.Length;

            using (Stream os = req.GetRequestStream())
            {
                os.Write(bytes, 0, bytes.Length);
            }

            using (var resp = req.GetResponse())
            using (var sr = new StreamReader(resp.GetResponseStream()))
            {
                return sr.ReadToEnd().Trim();
            }
        }
    }
}
