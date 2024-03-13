using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Foundation.Helper.Models;


namespace AFI.Feature.NewsLetter.Models
{
    public class ArchivedNewsLetters : IPaginated
    {
        public Dictionary<DateTime, List<NewsLetter>> ArchivedList { get; set; }
        public string link { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}