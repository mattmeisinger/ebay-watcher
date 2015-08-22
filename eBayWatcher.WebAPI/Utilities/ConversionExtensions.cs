using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBayWatcher
{
    public static class ConversionExtensions
    {
        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static string ToStringOrDefault(this object o)
        {
            if (o == null)
                return null;
            else
                return o.ToString();
        }

        public static string ToLowerOrDefault(this string s)
        {
            if (s == null)
                return null;
            else
                return s.ToLower();
        }

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