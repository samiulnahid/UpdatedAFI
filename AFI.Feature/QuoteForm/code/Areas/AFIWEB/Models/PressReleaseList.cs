using System.Collections.Generic;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class PressReleaseList : IPaginated
    {
        public List<PressRelease> PressReleases { get; set; }
        public List<int> Years { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public string SelectedYear { get; set; }
    }
}