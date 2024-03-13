using System;
using AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.CondoForm;
using FTData = AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class CondoFormDbMapper
    {
        public static void Map(this QuoteCondo quote, CondoFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteCondo();
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
            quote.TotalLivingArea = model.TotalLivingAreaSqFt;
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
            quote.AreTenantsRequiredToCarryLiabilityCoverage = model.AreTenantsRequiredToCarryLiabilityCoverage;
            quote.LengthOfLeasingAgreement = model.LengthOfLeasingAgreement;
            quote.PropertyReasonForVacancy = model.PropertyReasonForVacancy;
            quote.LeakDetectionSystem = model.LeakDetectionSystem;
            quote.Occupation = model.Occupation;
            quote.Education = model.Education; 
        }

        public static void ReverseMap(this CondoFormSaveModel model, FTData.Quote quote, QuoteContact contact, QuoteCondo condo)
        {
            if (model == null)
            {
                model = new CondoFormSaveModel();
            }

            model.CommonReverseMap(quote, contact, null, null);

            if (condo != null)
            {
                model.PolicyRenewalDate = condo.PurchaseDate != null ? DateTime.Parse(condo.PurchaseDate) : (DateTime?)null;
                model.AddressToBeQuotedAddress = condo.Address;
                model.AddressToBeQuotedCity = condo.City;
                model.AddressToBeQuotedState = condo.State;
                model.AddressToBeQuotedZip = condo.Zip;
                model.PropertyConstructionType = condo.Type;
                model.PropertyStyleOfHouse = condo.Style;
                model.PropertyStoryCount = condo.NumberOfStories;
                model.PropertyUnitCount = condo.NumberOfUnits;
                model.TotalLivingAreaSqFt = condo.TotalLivingArea;
                model.PropertyConstructionType = condo.ConstructionType;
                model.PropertyRoofAge = condo.RoofAge;
                model.PropertyRoofMaterial = condo.RoofMaterial;
                model.ResidenceDwellingAmountToBeQuoted = condo.QuoteAmount;
                model.CurrentInsuranceCompany = condo.CurrentCarrier;
                model.ResidenceOccupancy = condo.NumberOfOccupants;
                model.ResidenceNearFireStation = condo.MilesToFireDept;
                model.ResidenceNearFireHydrant = condo.DistanceToHydrant;
                model.ResidenceWithinCityLimits = condo.CityLimits;
                model.PropertyClaimsCount = condo.RecentLossesAmount;
                model.PropertyClaimsDetails = condo.RecentLossesDetails;
                model.NonSmokingHousehold = condo.IsOccupantSmoke;
                model.PropertyBathroomCount = condo.NumbersOfBedrooms;
                model.PropertyBedroomCount = condo.NumbersOfBathrooms;
                model.MonitoredAlarmSystem = condo.AlarmSystemType;
                model.PropertyAttachedGarage = condo.AttachedGarage;
                model.NewPurchaseDiscount = condo.IsNewHomePurchase;
                model.PropertyYearBuilt = condo.YearBuilt;
                model.PropertyLengthWidth = condo.MobileHouseStyle;
                model.NewPurchaseDiscountAnticipatedClosingDate = condo.PurchaseClosingDate?.ToString();
                model.AreTenantsRequiredToCarryLiabilityCoverage = condo.AreTenantsRequiredToCarryLiabilityCoverage;
                model.LengthOfLeasingAgreement = condo.LengthOfLeasingAgreement;
                model.PropertyReasonForVacancy = condo.PropertyReasonForVacancy;
                model.Occupation = condo.Occupation;
                model.Education = condo.Education;
                model.LeakDetectionSystem = condo.LeakDetectionSystem;
            }
        }
    }
}