using System;
using System.Collections.Generic;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{

    public class ArchivedNewsLetters:IPaginated
    {
        public Dictionary<DateTime,List<NewsLetter>> ArchivedList { get; set; }
        public string link { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}