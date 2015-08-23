using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBayWatcher.Core.Models
{
    public class User
    {
        public string Id { get; set; }
        public string[] Tokens { get; set; }
    }
}
