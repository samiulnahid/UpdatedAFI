using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class NewsLetterList : IPaginated
    {
        public List<NewsLetter> NewsLetters { get; set; }
        public string Headline { get; set; }
        public string Date { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}