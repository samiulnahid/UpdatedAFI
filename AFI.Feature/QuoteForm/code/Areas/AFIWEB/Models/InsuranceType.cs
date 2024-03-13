using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class InsuranceType
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAltText { get; set; }
        public string URL { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public string PreviewTitle { get; set; }
        public string QuoteFormLink { get; set; }


        public List<InsuranceType> RelatedInsuranceTypes { get; set; } = new List<InsuranceType>();
    }
}