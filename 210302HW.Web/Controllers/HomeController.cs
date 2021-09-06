using _210302HW.Web.Models;
using _210302HW.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace _210302HW.Web.Controllers
{
    public class HomeController : Controller
    {

        private string _connectionString = "Data Source=.\\sqlexpress; Initial Catalog=SimpleAds;Integrated Security=true;";
        public IActionResult Index()
        {
            var avm = new AdsViewModel();
            var manager = new AdsManager(_connectionString);
            avm.Ads = manager.GetAds();
            var ids = HttpContext.Session.Get<List<int>>("ListingIds");

            foreach (Ad ad in avm.Ads)
            {
                ad.CanDelete = ids != null && ids.Contains(ad.Id);
            }
            return View(avm);
        }

        [HttpPost]
        public ActionResult NewAd(Ad ad)
        {
            AdsManager am = new(_connectionString);
            am.AddAd(ad);

            List<int> ids = HttpContext.Session.Get<List<int>>("ListingIds");
            if (ids == null)
            {
                ids = new();
            }

            ids.Add(ad.Id);
            HttpContext.Session.Set("ListingIds", ids);

            return Redirect("/home/index");
        }

        public IActionResult NewAd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteAd()
        {
            return View();
        }

    }
}
