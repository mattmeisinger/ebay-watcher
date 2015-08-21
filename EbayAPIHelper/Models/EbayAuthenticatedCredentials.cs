using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbayAPIHelper.Models
{
    public class EbayAuthenticatedCredentials
    {
        public string EbayUsername { get; set; }
        public string SessionId { get; set; }
        public string Token { get; set; }

        public override string ToString()
        {
            var token = string.IsNullOrWhiteSpace(Token) ? "(No Token)" : "(Has Token)";
            return $"{EbayUsername} - {SessionId} - {token}";
        }
    }
}
