using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class AddressBook
    {
        public IEnumerable<string> ToList { get; set; }
        public IEnumerable<string> CCList { get; set; }
        public IEnumerable<string> BCCList { get; set; }
        public IEnumerable<string> FromList { get; set; }
    }
}