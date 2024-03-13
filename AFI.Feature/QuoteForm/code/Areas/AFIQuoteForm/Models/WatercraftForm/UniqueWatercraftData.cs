using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common.Vehicle;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.WatercraftForm
{
    public class UniqueWatercraftData
    {
        public string addDriverButtonText { get; set; }
        public string addVehicleButtonText { get; set; }
        public string addViolationButtonText { get; set; }
        public Heading addDriverHeadings { get; set; }
        public List<Options> householdViolationTypes { get; set; }
        public List<PipAmount> pipAmounts { get; set; }
        public List<Options> hullMaterials { get; set; }
        public List<Options> propulsionTypes { get; set; }
        public OverallCoveragesWatercraftSidebars overallCoveragesSidebars { get; set; }
        public string vehicleStartYear { get; set; }
        public List<Options> physicalDamageDeductibles { get; set; }
        public List<Options> physicalDamageComprehensiveDeductibles { get; set; }
        public string bodilyInjuryPropertyDamageSublabel { get; set; }
        public string medicalCoverageSublabel { get; set; }
        public string uninsuredBoatersCoverageSublabel { get; set; }
    }
}