using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class ContactUsPostModel
    {
        public string token { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string message { get; set; }
        public string phone { get; set; }
        public string subject { get; set; }
    }
}