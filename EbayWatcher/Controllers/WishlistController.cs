using StampFinder.Entities;
using StampFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StampFinder.Controllers
{
    public class WishlistController : Controller
    {
        // GET: Stamps
        public ActionResult Index()
        {
            using (var context = new EbayWatcherContext())
            {
                if (!context.Stamps.Any())
                {
                    context.Stamps.Add(new Models.WishlistItem
                    {
                        Status = "Need",
                        Name = "1"
                    });
                    context.SaveChanges();
                }
                return View(context.Stamps.ToArray());
            }
        }

        [HttpGet]
        public ActionResult Edit(int? Id = null)
        {
            using (var context = new EbayWatcherContext())
            {
                if (Id.HasValue)
                    return View(context.Stamps.Single(a => a.Id == Id));
                else
                    return View(new WishlistItem { });
            }
        }

        [HttpPost]
        public ActionResult Edit(WishlistItem o)
        {
            using (var context = new EbayWatcherContext())
            {
                WishlistItem s = null;
                if (o.Id == 0)
                {
                    s = new WishlistItem { };
                    context.Stamps.Add(s);
                }
                else
                {
                    s = context.Stamps.Single(a => a.Id == o.Id);
                }

                s.Name = o.Name;
                s.Category = o.Category;
                s.Status = o.Status;
                s.Notes = o.Notes;

                context.SaveChanges();
                o.Id = s.Id;

                return RedirectToAction("Edit", new { Id = o.Id });
            }
        }
    }
}