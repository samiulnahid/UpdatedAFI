using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models
{
    public class SideBar
    {
        public string triggerText { get; set; }
        public string heading { get; set; }
        public List<SideBarContent> content { get; set; }
    }
}