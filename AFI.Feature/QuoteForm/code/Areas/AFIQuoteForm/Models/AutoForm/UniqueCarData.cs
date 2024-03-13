using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common.Vehicle;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm
{
    public class UniqueCarData : UniqueVehicleData
    {
        public string enterByVinText { get; set; }
        public string enterByDetailsText { get; set; }
        public string vehicleStartYear { get; set; }
        public string firstVinLookupYear { get; set; }
        public OverallCoverageAuto overallCoveragesSidebars { get; set; }
        public List<Options> vehicleUses { get; set; }
        public List<Options> physicalDamageDeductibles { get; set; }
        public List<Options> physicalDamageComprehensiveDeductibles { get; set; }
        public string personalInjuryProtectionSublabel { get; set; }

    }
}