using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.Prospect.Models
{
    public class SuspectTempPagination
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItem { get; set; }
        public bool Success { get; set; } = true;
        public IEnumerable<SuspectMarketingTemp> ProspectTempList { get; set; }
        public IEnumerable<SuspectMarketing> ProspectList { get; set; }

    }
}