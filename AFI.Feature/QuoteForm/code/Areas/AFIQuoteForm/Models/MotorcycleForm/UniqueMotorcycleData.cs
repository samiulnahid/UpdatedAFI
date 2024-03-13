using System.Collections.Generic;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common.Vehicle;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MotorcycleForm
{
    public class UniqueMotorcycleData : UniqueVehicleData
    {
        public List<Options> vehicleTypes { get; set; }
        public OverallCoveragesSidebars overallCoveragesSidebars { get; set; }
        public List<Options> physicalDamageDeductibles { get; set; }
        public List<Options> physicalDamageComprehensiveDeductibles { get; set; }
        public string personalInjuryProtectionSublabel { get; set; }
    }
}