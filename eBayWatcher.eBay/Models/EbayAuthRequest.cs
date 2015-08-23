using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eBayWatcher.eBay.Models
{
    public class EbayAuthRequest
    {
        public string LoginUrl { get; internal set; }
        public string SessionId { get; internal set; }

        public override string ToString()
        {
            return "Session ID: " + SessionId;
        }
    }
}
