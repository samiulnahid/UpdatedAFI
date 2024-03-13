using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Controllers
{
    public class SiteframeController : Controller
    {
        public ActionResult Careers()
        {
            string newtonSoftwareDomain = Sitecore.Configuration.Settings.GetSetting("CareersNewtonSoftwareDomain", string.Empty);
            CareersFrameViewModel model = new CareersFrameViewModel();
            model.NewtonSoftwareDomain = newtonSoftwareDomain;
            return View(model);
        }
    }
}