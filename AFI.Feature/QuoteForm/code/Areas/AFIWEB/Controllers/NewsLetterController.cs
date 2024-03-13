using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers.Configurations;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using Sitecore.Data.Templates;
using Sitecore.Links;
using Templates = AFI.Feature.Identifiers.Templates;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Controllers
{
    public class NewsLetterController : Controller
    {
        private readonly INewsLetterRepository _newsLetterRepository;

        public NewsLetterController(INewsLetterRepository newsLetterRepository)
        {
            _newsLetterRepository = newsLetterRepository;
        }
        public ActionResult NewsLetterListing(int page = 1)
        {
            var newslettersList = _newsLetterRepository.GetNewsLetters(page);
            return View(newslettersList);
        }

        public ActionResult MoreNewsLetters()
        {
            var guid = Sitecore.Context.Item.ID.Guid;
            var newslettersList = new NewsLetterList
            {
                NewsLetters = _newsLetterRepository.GetMoreNewsLetters(guid)
            };
            return View(newslettersList);
        }

        public ActionResult ArchivedNewsLetterListing(int page = 1)
        {
            return View(_newsLetterRepository.GetArchivedNewsletters(page));
        }

    }
}