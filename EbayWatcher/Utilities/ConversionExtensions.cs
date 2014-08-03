using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayWatcher.Utilities
{
    public static class ConversionExtensions
    {
        public static int? ToIntOrDefault(this object o)
        {
            if (o == null)
            {
                return null;
            }
            else
            {
                int i;
                if (int.TryParse(o.ToString(), out i))
                {
                    return i;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}