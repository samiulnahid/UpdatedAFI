using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFI.Feature.PressRelease.Repository;
using AFI.Feature.PressRelease.Models;

namespace AFI.Feature.PressRelease.Controllers
{
    public class PressRealeaseController : Controller
    {
        // GET: PressRealease
        private readonly IpressreleaseRepository  _pressrelease;
        public PressRealeaseController(IpressreleaseRepository PressReleaseRepository)
        {
            _pressrelease = PressReleaseRepository;
        }
        // GET: PressRealease
        [HttpGet]
        public ActionResult FilterPressRelease(string year, int page = 1)
        {

            var guid = Sitecore.Context.Item.ID.Guid;
            PressReleaseList pressreleaseList = new PressReleaseList();
            var pressreleaselistitem = _pressrelease.GetFilterPressRelease(guid, year, page);
            if (!string.IsNullOrEmpty(year))
            {
                foreach (var yearname in pressreleaselistitem.YearList)
                {
                    if (yearname.yearname == year)
                    {
                        yearname.selected = "selected";
                    }
                }
            }
            return View("/Views/AFI/PressRelease/FilterPressRelease.cshtml", pressreleaselistitem);
        }
    }
}