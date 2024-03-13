using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.JsonConverters;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using Newtonsoft.Json;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MobilehomeForm
{
	public class MobilehomeFormSaveModel : CommonFormSaveModel
	{
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool? AddressToBeQuotedAddressSameAsMailing { get; set; } = false;
        public string AddressToBeQuotedAddress { get; set; }
        public string AddressToBeQuotedCity { get; set; }
        public string AddressToBeQuotedState { get; set; }
        public string AddressToBeQuotedZip { get; set; }
		[JsonConverter(typeof(YesNoBooleanConverter))]
        public bool ResidenceWithinCityLimits { get; set; }
        public string ResidenceNearFireStation { get; set; }
        public string ResidenceNearFireHydrant { get; set; }
        public string ResidenceOccupancy { get; set; }
        public string ResidenceDwellingAmountToBeQuoted { get; set; }
        public string PropertyClaimsCount { get; set; }
        public string PropertyClaimsDetails { get; set; }
        public string PropertyStyleOfHouse { get; set; }
        public string PropertyLengthWidth { get; set; }
        public string PropertyStoryCount { get; set; }
        public string PropertyBedroomCount { get; set; }
        public string PropertyBathroomCount { get; set; }
        public string PropertyUnitCount { get; set; }
        public string PropertyConstructionType { get; set; }
        public string PropertyYearBuilt { get; set; }
        public string PropertyUpdates { get; set; }
        public string PropertyTotalLivingArea { get; set; }
        public string PropertyAttachedGarage { get; set; }
        public string PropertyRoofAge { get; set; }
        public string PropertyRoofMaterial { get; set; }
        public string MonitoredAlarmSystem { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))] 
        public bool LeakDetectionSystem { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool NewPurchaseDiscount { get; set; }
        public string NewPurchaseDiscountAnticipatedClosingDate { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool NonSmokingHousehold { get; set; }
        public string Occupation { get; set; }
        public string Education { get; set; }
    }
}