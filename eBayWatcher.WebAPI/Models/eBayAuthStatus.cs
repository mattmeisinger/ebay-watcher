using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBayWatcher.WebAPI.Models
{
    public class EBayAuthStatus
    {
        public string Description { get; set; }
        public string SessionId { get; set; }
        public string LoginUrl { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
    }
}