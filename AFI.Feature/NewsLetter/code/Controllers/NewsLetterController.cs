using AFI.Feature.NewsLetter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFI.Feature.NewsLetter.Repository;

namespace AFI.Feature.NewsLetter.Controllers
{
    public class NewsLetterController : Controller
    {
        private readonly INewsLetterRepository _newsLetterRepository;
        public NewsLetterController(INewsLetterRepository newsLetterRepository)
        {
            _newsLetterRepository = newsLetterRepository;        
        }
        public ActionResult MoreNewsLetters()
        {
            var guid = Sitecore.Context.Item.ID.Guid;
           
            var newslettersList  = _newsLetterRepository.GetMoreNewsLetters(guid);         
            return View("/Views/AFI/NewsLetter/MoreNewsLetters.cshtml", newslettersList);
        }
        public ActionResult ArchivedNewsLetterListing(int page = 1)
        {
            var archivedNewsLetterList = _newsLetterRepository.GetArchivedNewsletters(page);
            return View("/Views/AFI/NewsLetter/ArchivedNewsLetters.cshtml", archivedNewsLetterList);
        }

    }
}