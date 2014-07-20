using StampFinder.Entities;
using StampFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StampFinder.Controllers
{
    public class StampsController : Controller
    {
        // GET: Stamps
        public ActionResult Index()
        {
            using (var context = new StampContext())
            {
                if (!context.Stamps.Any())
                {
                    context.Stamps.Add(new Models.Stamp
                    {
                        Status = "Need",
                        ScottId = "1"
                    });
                    context.SaveChanges();
                }
                return View(context.Stamps.ToArray());
            }
        }

        [HttpGet]
        public ActionResult Edit(int? Id = null)
        {
            using (var context = new StampContext())
            {
                if (Id.HasValue)
                    return View(context.Stamps.Single(a => a.Id == Id));
                else
                    return View(new Stamp { });
            }
        }

        [HttpPost]
        public ActionResult Edit(Stamp o)
        {
            using (var context = new StampContext())
            {
                Stamp s = null;
                if (o.Id == 0)
                {
                    s = new Stamp { };
                    context.Stamps.Add(s);
                }
                else
                {
                    s = context.Stamps.Single(a => a.Id == o.Id);
                }

                s.ScottId = o.ScottId;
                s.Status = o.Status;
                s.CatValueMint = o.CatValueMint;
                s.CatValueUsed = o.CatValueUsed;

                context.SaveChanges();
                o.Id = s.Id;

                return RedirectToAction("Edit", new { Id = o.Id });
            }
        }
    }
}