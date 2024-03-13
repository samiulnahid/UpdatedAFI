using System.Runtime.Serialization;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MobilehomeForm
{
    [DataContract]
    public class UniqueMobilehomeFormData
    {
        [DataMember(Name = "propertyYearBuiltStartYear")]
        public string PropertyYearBuiltStartYear { get; set; }
        [DataMember(Name = "propertyYearBuiltBeforeText")]
        public string PropertyYearBuiltBeforeText { get; set; }
        [DataMember(Name = "propertyAgeOfRoofStart")]
        public string PropertyAgeOfRoofStart { get; set; }
        [DataMember(Name = "propertyAgeOfRoofEnd")]
        public string PropertyAgeOfRoofEnd { get; set; }
        [DataMember(Name = "propertyAgeOfRoofBeforeText")]
        public string PropertyAgeOfRoofBeforeText { get; set; }
    }
}