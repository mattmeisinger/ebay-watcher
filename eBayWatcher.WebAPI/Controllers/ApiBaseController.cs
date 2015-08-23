using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace eBayWatcher.WebAPI.Controllers
{
    public class ApiBaseController : ApiController
    {
        /// <summary>
        /// Retrieves an individual cookie from the cookies collection
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public string GetCookie(string cookieName)
        {
            var cookie = Request.Headers.GetCookies(cookieName).FirstOrDefault();
            if (cookie != null)
                return cookie[cookieName].Value;

            return null;
        }
        
        /// <summary>
        /// Returns an individual HTTP Header value
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetHeader(string key)
        {
            IEnumerable<string> keys = null;
            if (!Request.Headers.TryGetValues(key, out keys))
                return null;

            return keys.First();
        }
    }
}