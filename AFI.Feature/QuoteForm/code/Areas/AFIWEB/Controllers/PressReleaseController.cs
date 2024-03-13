using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using Sitecore.Data.Templates;
using Sitecore.Links;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Controllers
{
    public class PressReleaseController : Controller
    {
        private readonly IPressReleaseRepository _pressReleaseRepository;

        public PressReleaseController(IPressReleaseRepository pressReleaseRepository)
        {
            _pressReleaseRepository = pressReleaseRepository;
        }
        public ActionResult PressReleaseListing(int year = Int32.MinValue, int page = 1)
        {
            return View(_pressReleaseRepository.GetPressReleases(Sitecore.Context.Database, year, page));
        }
    }
}