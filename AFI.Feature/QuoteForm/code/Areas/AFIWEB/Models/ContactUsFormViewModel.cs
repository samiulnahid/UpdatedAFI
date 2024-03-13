using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;

namespace AFI.Foundation.QuoteForm.Models
{
    public class ContactUsFormViewModel
    {
		public string RecaptchaKey { get; set; }
		public string PhoneHelpText { get; set; }
        public IEnumerable<SelectListItem> Subjects { get; set; }
    }
}