using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayWatcher.Converters
{
    public class WishlistItemConverter
    {
        internal static Models.WishlistItem Convert(Entities.WishlistItem a)
        {
            return new Models.WishlistItem
            {
                Id = a.Id,
                Name = a.Name,
                CategoryName = a.Category.Name,
                CategoryFullName = a.Category.FullName,
                Notes = a.Notes,
                Status = a.Status
            };
        }
    }
}