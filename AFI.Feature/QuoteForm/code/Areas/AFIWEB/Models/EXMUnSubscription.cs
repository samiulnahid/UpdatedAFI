using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class EXMUnSubscription
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string MemberID { get; set; }
        public string Campaign { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}