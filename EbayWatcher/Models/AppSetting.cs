using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EbayWatcher.Models
{
    public class AppSetting
    {
        [Key]
        public string Name { get; set; }
        public string Value { get; set; }
    }
}