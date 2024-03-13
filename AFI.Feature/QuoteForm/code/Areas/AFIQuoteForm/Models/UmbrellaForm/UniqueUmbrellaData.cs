using System.Collections.Generic;
using System.Runtime.Serialization;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.UmbrellaForm
{
    [DataContract]
    public class UniqueUmbrellaData
    {
        [DataMember(Name = "addViolationButtonText")]
        public string AddViolationText { get; set; }

        [DataMember(Name = "addAnotherRecreationalVehicleText")]
        public string AddAnotherRecreationalVehicleText { get; set; }

        [DataMember(Name = "addAnotherWatercraftText")]
        public string AddAnotherWatercraftText { get; set; }

        [DataMember(Name = "householdViolationTypes")]
        public List<Options> HouseholdViolationTypes { get; set; }

    }
}