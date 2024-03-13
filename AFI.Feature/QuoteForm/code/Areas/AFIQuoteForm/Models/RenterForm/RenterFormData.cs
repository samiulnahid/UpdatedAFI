using System.Runtime.Serialization;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.RenterForm
{
    public class RenterFormData : FormData
    {
        public UniqueRenterFormData unique { get; set; }
        public RenterFormData() { }
    }

    [DataContract]
    public class UniqueRenterFormData
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