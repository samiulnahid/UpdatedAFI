using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.JsonConverters;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using Newtonsoft.Json;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.UmbrellaForm
{
    public class UmbrellaFormSaveModel : CommonFormSaveModel
    {
        public string ResidenceStatus { get; set; }
        public string PropertyCity { get; set; }
        public string PropertyState { get; set; }
        public string PropertyStreet { get; set; }
        public string PropertyZip { get; set; }
        public bool MailingIsDifferent { get; set; }
        public string QuoteAmount { get; set; }
        public string UnderlyingPersonalLiabilityLimits { get; set; }
        public string NumberOfDrivers { get; set; }
        public string NumberOfDriversUnderAge22 { get; set; }
        public string NumberOfDriversUnderAge25 { get; set; }
        public string NumberOfDriversOverAge70 { get; set; }
        public string NumberOfVehicles { get; set; }
        public string AutoLiabilityCoverageLimits { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool DoYouOwnAnyRentalProperty { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool DoYouOwnAnyRecreationalVehicles { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool DoYouOwnAnyWatercraft { get; set; }
        public string NumberOfRentalProperties { get; set; }
        public string RentalUnderlyingInsurance { get; set; }
        public string Type0OfRecreationalVehicle { get; set; }
        public string Type1OfRecreationalVehicle { get; set; }
        public string Type2OfRecreationalVehicle { get; set; }
        public string Type3OfRecreationalVehicle { get; set; }
        public string Type4OfRecreationalVehicle { get; set; }
        public string Type5OfRecreationalVehicle { get; set; }
        public string Recreational0VehicleLiability { get; set; }
        public string Recreational1VehicleLiability { get; set; }
        public string Recreational2VehicleLiability { get; set; }
        public string Recreational3VehicleLiability { get; set; }
        public string Recreational4VehicleLiability { get; set; }
        public string Recreational5VehicleLiability { get; set; }
        public string Type0OfWatercraft { get; set; }
        public string Length0OfWatercraft { get; set; }
        public string Horsepower0OfWatercraft { get; set; }
        public string Watercraft0Liability { get; set; }

        public string Type1OfWatercraft { get; set; }
        public string Length1OfWatercraft { get; set; }
        public string Horsepower1OfWatercraft { get; set; }
        public string Watercraft1Liability { get; set; }

        public string Type2OfWatercraft { get; set; }
        public string Length2OfWatercraft { get; set; }
        public string Horsepower2OfWatercraft { get; set; }
        public string Watercraft2Liability { get; set; }

        public string Type3OfWatercraft { get; set; }
        public string Length3OfWatercraft { get; set; }
        public string Horsepower3OfWatercraft { get; set; }
        public string Watercraft3Liability { get; set; }

        public string Type4OfWatercraft { get; set; }
        public string Length4OfWatercraft { get; set; }
        public string Horsepower4OfWatercraft { get; set; }
        public string Watercraft4Liability { get; set; }

        public string Type5OfWatercraft { get; set; }
        public string Length5OfWatercraft { get; set; }
        public string Horsepower5OfWatercraft { get; set; }
        public string Watercraft5Liability { get; set; }
    }
}