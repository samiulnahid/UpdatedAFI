using System;
using AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HomenonownerForm;
using FTData = AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class HomenonownerFormDbMapper
    {
        public static void Map(this QuoteHomenonowner quote, HomenonownerFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteHomenonowner();
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

        public static void ReverseMap(this HomenonownerFormSaveModel model, FTData.Quote quote, QuoteContact contact, QuoteHomenonowner homeowner)
        {
            if (model == null)
            {
                model = new HomenonownerFormSaveModel();
            }

            model.CommonReverseMap(quote, contact, null, null);

            if (homeowner != null)
            {
                model.PolicyRenewalDate = homeowner.PurchaseDate != null ? DateTime.Parse(homeowner.PurchaseDate) : (DateTime?)null;
                model.AddressToBeQuotedAddress = homeowner.Address;
                model.AddressToBeQuotedCity = homeowner.City;
                model.AddressToBeQuotedState = homeowner.State;
                model.AddressToBeQuotedZip = homeowner.Zip;
                model.PropertyConstructionType = homeowner.Type;
                model.PropertyStyleOfHouse = homeowner.Style;
                model.PropertyStoryCount = homeowner.NumberOfStories;
                model.PropertyUnitCount = homeowner.NumberOfUnits;
                model.TotalLivingAreaSqFt = homeowner.TotalLivingArea;
                model.PropertyConstructionType = homeowner.ConstructionType;
                model.PropertyRoofAge = homeowner.RoofAge;
                model.PropertyRoofMaterial = homeowner.RoofMaterial;
                model.ResidenceDwellingAmountToBeQuoted = homeowner.QuoteAmount;
                model.CurrentInsuranceCompany = homeowner.CurrentCarrier;
                model.ResidenceOccupancy = homeowner.NumberOfOccupants;
                model.ResidenceNearFireStation = homeowner.MilesToFireDept;
                model.ResidenceNearFireHydrant = homeowner.DistanceToHydrant;
                model.ResidenceWithinCityLimits = homeowner.CityLimits;
                model.PropertyClaimsCount = homeowner.RecentLossesAmount;
                model.PropertyClaimsDetails = homeowner.RecentLossesDetails;
                model.NonSmokingHousehold = homeowner.IsOccupantSmoke;
                model.PropertyBathroomCount = homeowner.NumbersOfBedrooms;
                model.PropertyBedroomCount = homeowner.NumbersOfBathrooms;
                model.MonitoredAlarmSystem = homeowner.AlarmSystemType;
                model.PropertyAttachedGarage = homeowner.AttachedGarage;
                model.NewPurchaseDiscount = homeowner.IsNewHomePurchase;
                model.PropertyYearBuilt = homeowner.YearBuilt;
                model.PropertyLengthWidth = homeowner.MobileHouseStyle;
                model.NewPurchaseDiscountAnticipatedClosingDate = homeowner.PurchaseClosingDate?.ToString();
                model.AreTenantsRequiredToCarryLiabilityCoverage = homeowner.AreTenantsRequiredToCarryLiabilityCoverage;
                model.LengthOfLeasingAgreement = homeowner.LengthOfLeasingAgreement;
                model.PropertyReasonForVacancy = homeowner.PropertyReasonForVacancy;
                model.Occupation = homeowner.Occupation;
                model.Education = homeowner.Education;
                model.LeakDetectionSystem = homeowner.LeakDetectionSystem;
            }
        }
    }
}