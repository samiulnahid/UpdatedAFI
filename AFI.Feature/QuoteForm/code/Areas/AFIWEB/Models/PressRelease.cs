using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class PressRelease
    {
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public DateTime PublishDate { get; set; }

    }
}