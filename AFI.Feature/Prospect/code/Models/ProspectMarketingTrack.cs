using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.Prospect.Models
{
    public class ProspectMarketingTrack
    {
        public int Id { get; set; }
        public int ProspectID { get; set; }
        public string Source { get; set; }
        public int Status { get; set; }
        public string CoverageType { get; set; }
        public string CreatedBy { get; set; } = null;
        public DateTime? CreatedDate { get; set; }
    }
}