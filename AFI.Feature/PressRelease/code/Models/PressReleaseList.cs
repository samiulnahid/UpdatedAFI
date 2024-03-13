using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Foundation.Helper.Models;

namespace AFI.Feature.PressRelease.Models
{
    public class PressReleaseList : IPaginated
    {
        public List<PressReleases> PressRelease { get; set; }
        public List<Yearlist> YearList { get; set; }
        public string Headline { get; set; }
        public string Date { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string Title { get; set; }
        public string ReadMoreTitle { get; set; }
    }
}