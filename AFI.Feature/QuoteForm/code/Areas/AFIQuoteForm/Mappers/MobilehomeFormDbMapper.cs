using System;
using AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MobilehomeForm;
using FTData = AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class MobilehomeFormDbMapper
    {
        public static void Map(this QuoteMobilehome quote, MobilehomeFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteMobilehome();
            }

            quote.Key = quoteKey;
            quote.Address = model.AddressToBeQuotedAddress;
            quote.City = model.AddressToBeQuotedCity;
            quote.State = model.AddressToBeQuotedState;
            quote.Zip = model.AddressToBeQuotedZip;
            quote.Type = model.PropertyConstructionType;
            quote.Style = model.PropertyStyleOfHouse;
            quote.NumberOfStories = model.PropertyStoryCount;
            quote.NumberOfUnits = model.PropertyUnitCount;
            quote.TotalLivingArea = model.PropertyTotalLivingArea;
            quote.ConstructionType = model.PropertyConstructionType;
            quote.RoofAge = model.PropertyRoofAge;
            quote.RoofMaterial = model.PropertyRoofMaterial;
            quote.CurrentCarrier = model.CurrentInsuranceCompany;
            quote.MobileHouseStyle = model.PropertyLengthWidth;
            quote.PurchaseDate = model.PolicyRenewalDate?.ToString();
            DateTime purchaseClosingDate;
            quote.PurchaseClosingDate = DateTime.TryParse(model.NewPurchaseDiscountAnticipatedClosingDate, out purchaseClosingDate) ? purchaseClosingDate : (DateTime?) null;
            quote.QuoteAmount = model.ResidenceDwellingAmountToBeQuoted;
            quote.NumberOfOccupants = model.ResidenceOccupancy;
            quote.RecentLossesAmount = model.PropertyClaimsCount;
            quote.RecentLossesDetails = model.PropertyClaimsDetails;
            quote.CityLimits = model.ResidenceWithinCityLimits;
            quote.DistanceToHydrant = model.ResidenceNearFireHydrant;
            quote.MilesToFireDept = model.ResidenceNearFireStation;
            quote.IsOccupantSmoke = model.NonSmokingHousehold;
            quote.NumbersOfBathrooms = model.PropertyBathroomCount;
            quote.NumbersOfBedrooms = model.PropertyBedroomCount;
            quote.AlarmSystemType = model.MonitoredAlarmSystem;
            quote.AttachedGarage = model.PropertyAttachedGarage;
            quote.IsNewHomePurchase = model.NewPurchaseDiscount;
            quote.YearBuilt = model.PropertyYearBuilt;
        }

        public static void ReverseMap(this MobilehomeFormSaveModel model, FTData.Quote quote, QuoteContact contact, QuoteMobilehome mobilehome)
        {
            if (model == null)
            {
                model = new MobilehomeFormSaveModel();
            }

            model.CommonReverseMap(quote, contact, null, null);

            if (mobilehome != null)
            {
                model.PolicyRenewalDate = mobilehome.PurchaseDate != null ? DateTime.Parse(mobilehome.PurchaseDate) : (DateTime?)null;
                model.AddressToBeQuotedAddress = mobilehome.Address;
                model.AddressToBeQuotedCity = mobilehome.City;
                model.AddressToBeQuotedState = mobilehome.State;
                model.AddressToBeQuotedZip = mobilehome.Zip;
                model.PropertyConstructionType = mobilehome.Type;
                model.PropertyStyleOfHouse = mobilehome.Style;
                model.PropertyStoryCount = mobilehome.NumberOfStories;
                model.PropertyUnitCount = mobilehome.NumberOfUnits;
                model.PropertyTotalLivingArea = mobilehome.TotalLivingArea;
                model.PropertyConstructionType = mobilehome.ConstructionType;
                model.PropertyRoofAge = mobilehome.RoofAge;
                model.PropertyRoofMaterial = mobilehome.RoofMaterial;
                model.ResidenceDwellingAmountToBeQuoted = mobilehome.QuoteAmount;
                model.CurrentInsuranceCompany = mobilehome.CurrentCarrier;
                model.ResidenceOccupancy = mobilehome.NumberOfOccupants;
                model.ResidenceNearFireStation = mobilehome.MilesToFireDept;
                model.ResidenceNearFireHydrant = mobilehome.DistanceToHydrant;
                model.ResidenceWithinCityLimits = mobilehome.CityLimits;
                model.PropertyClaimsCount = mobilehome.RecentLossesAmount;
                model.PropertyClaimsDetails = mobilehome.RecentLossesDetails;
                model.NonSmokingHousehold = mobilehome.IsOccupantSmoke;
                model.PropertyBathroomCount = mobilehome.NumbersOfBedrooms;
                model.PropertyBedroomCount = mobilehome.NumbersOfBathrooms;
                model.MonitoredAlarmSystem = mobilehome.AlarmSystemType;
                model.PropertyAttachedGarage = mobilehome.AttachedGarage;
                model.NewPurchaseDiscount = mobilehome.IsNewHomePurchase;
                model.PropertyYearBuilt = mobilehome.YearBuilt;
                model.PropertyLengthWidth = mobilehome.MobileHouseStyle;
                model.NewPurchaseDiscountAnticipatedClosingDate = mobilehome.PurchaseClosingDate?.ToString();
            }
        }
    }
}