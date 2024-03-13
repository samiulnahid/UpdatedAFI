using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common
{
    [DataContract]
    public class OverallCoveragesSidebars
    {
        [DataMember(Name = "default")]
        public SideBar DefaultSidebar { get; set; }
        [DataMember(Name = "bodilyInjuryLiability")]
        public SideBar BodilyInjuryLiability { get; set; }
        [DataMember(Name = "propertyDamageLiability")]
        public SideBar PropertyDamageLiability { get; set; }
        [DataMember(Name = "medicalPayment")]
        public SideBar MedicalPayment { get; set; }
        [DataMember(Name = "uninsuredMotoristBodilyInjury")]
        public SideBar UninsuredMotoristBodilyInjury { get; set; }
        [DataMember(Name = "personalInjuryProtection")]
        public SideBar PersonalInjuryProtection { get; set; }
    }

    [DataContract]
    public class OverallCoveragesWatercraftSidebars
    {
        [DataMember(Name = "default")]
        public SideBar DefaultSidebar { get; set; }
        [DataMember(Name = "bodilyInjuryPropertyDamage")]
        public SideBar BodilyInjuryPropertyDamage { get; set; }
        [DataMember(Name = "medicalCoverage")]
        public SideBar MedicalCoverage { get; set; }
        [DataMember(Name = "uninsuredBoatersCoverage")]
        public SideBar UninsuredBoatersCoverage { get; set; }
    }
}