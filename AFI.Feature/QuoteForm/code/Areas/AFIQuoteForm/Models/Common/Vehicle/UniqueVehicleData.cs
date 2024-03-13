using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common.Vehicle
{
    public class UniqueVehicleData
    {
        public string addDriverButtonText { get; set; }
        public string addVehicleButtonText { get; set; }
        public string addViolationButtonText { get; set; }
        public Heading addDriverHeadings { get; set; }
        public List<Options> householdViolationTypes { get; set; }
        public string bodilyInjuryLiabilitySublabel { get; set; }
        public string propertyDamageLiabilitySublabel { get; set; }
        public string medicalPaymentSublabel { get; set; }
        public string uninsuredMotoristBodilyInjurySublabel { get; set; }
        public List<PipAmount> pipAmounts { get; set; }
    }
}