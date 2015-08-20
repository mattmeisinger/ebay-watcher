using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbayAPIHelper.Models
{
    public class EbayAuthRequest
    {
        public string LoginUrl { get; internal set; }
        public string SessionId { get; internal set; }
    }
}
