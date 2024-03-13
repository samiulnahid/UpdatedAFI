using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models
{
    public class Section
    {
        public string id { get; set; }
        public string wayfinderId { get; set; }
        public string heading { get; set; }
        public string subheading { get; set; }
        public string[] description { get; set; }
        public string disclaimer { get; set; }
        public SideBar sidebar { get; set; }
        public  List<ISectionField> fields { get; set; }
    }
}