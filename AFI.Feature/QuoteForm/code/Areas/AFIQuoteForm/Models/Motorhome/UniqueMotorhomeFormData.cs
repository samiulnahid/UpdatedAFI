using System.Collections.Generic;
using System.Runtime.Serialization;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common.Vehicle;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Motorhome
{
    public class UniqueMotorhomeFormData : UniqueVehicleData
    {
        public string personalInjuryProtectionSublabel { get; set; }
        public OverallCoveragesSidebars overallCoveragesSidebars { get; set; }
        public List<Options> physicalDamageDeductibles { get; set; }
        public List<Options> physicalDamageComprehensiveDeductibles { get; set; }
        public List<Options> vehicleTypes { get; set; }
        public List<Options> vehicleUses { get; set; }
    }
}