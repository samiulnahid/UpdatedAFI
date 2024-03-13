using System.Collections.Generic;
using System.Runtime.Serialization;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.CollectorForm
{
    [DataContract]
    public class UniqueCollectorVehicleData
    {
        [DataMember(Name="addVehicleButtonText")]
        public string AddVehicleButtonText { get; set; }
        [DataMember(Name = "addViolationButtonText")]   
        public string AddViolationButtonText { get; set; }
        [DataMember(Name = "addDriverHeadings")]
        public Heading AddDriverHeadings { get; set; }
        [DataMember(Name = "overallCoveragesSidebars")]
        public OverallCoveragesSidebars OverallCoveragesSidebars { get; set; }
        [DataMember(Name = "householdViolationTypes")]
        public List<Options> HouseholdViolationTypes { get; set; }
        [DataMember(Name = "pipAmounts")]
        public List<PipAmount> PipAmounts { get; set; }
        [DataMember(Name = "physicalDamageDeductibles")]
        public List<Options> PhysicalDamageDeductibles { get; set; }
        [DataMember(Name = "physicalDamageComprehensiveDeductibles")]
        public List<Options> PhysicalDamageComprensiveDeductibles { get; set; }
        [DataMember(Name = "bodilyInjuryLiabilitySublabel")]
        public string BodilyInjuryLiabilitySublabel { get; set; }
        [DataMember(Name = "propertyDamageLiabilitySublabel")]
        public string PropertyDamageLiabilitySublabel { get; set; }
        [DataMember(Name = "medicalPaymentSublabel")]
        public string MedicalPaymentSublabel { get; set; }
        [DataMember(Name = "uninsuredMotoristBodilyInjurySublabel")]
        public string UninsuredMotoristBodilyInjurySublabel { get; set; }
        [DataMember(Name = "personalInjuryProtectionSublabel")]
        public string PersonalInjuryProtectionSublabel { get; set; }
    }
}