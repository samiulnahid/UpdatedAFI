using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm
{
    public class OverallCoverageAuto
    {
        public SideBar @default { get; set; }
        public SideBar bodilyInjuryLiability { get; set; }
        public SideBar propertyDamageLiability { get; set; }
        public SideBar medicalPayment { get; set; }
        public SideBar uninsuredMotoristBodilyInjury { get; set; }
        public SideBar personalInjuryProtection { get; set; }
    }
    
}