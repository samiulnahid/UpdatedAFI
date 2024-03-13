using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.FloodForm;
using FTData = AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class FloodFormDbMapper
    {
        public static void Map(this QuoteContact quote, FloodFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteContact();
            }

            quote.Key = quoteKey;
            quote.CNTCGroupID = model.MemberNumber;
            quote.FirstName = model.PolicyHolderFirstName;
            quote.LastName = model.PolicyHolderLastName;
            quote.Suffix = model.PolicyHolderSuffix;
            quote.Street = model.PolicyHolderMailingAddress;
            quote.State = model.PolicyHolderState;
            quote.City = model.PolicyHolderCity;
            quote.ZipCode = model.PolicyHolderZip;
            quote.ServiceBranch = model.BranchOfService;
            quote.ServiceStatus = model.MilitaryStatus;
            quote.HowToContact = model.SubmitQuoteContactMethod;
            quote.PhoneType = model.PolicyHolderPhoneType;
            quote.Email = model.PolicyHolderEmail;
            quote.PhoneNumber = model.PolicyHolderPhoneNumber;
            quote.SSN = model.PolicyHolderSsn;
            quote.MaritalStatus = model.PolicyHolderMaritalStatus;
            quote.BirthDate = model.PolicyHolderDob;
            quote.Gender = model.PolicyHolderGender;
            quote.ResidenceStatus = model.PolicyHolderResidenceStatus;
            if (string.Equals(quote.HowToContact, "Phone", StringComparison.InvariantCultureIgnoreCase))
            {
                quote.CallForReview = true;
                quote.ReviewPhoneNum = model.SubmitQuoteContactInfo;
            }
            else
            {
                quote.CallForReview = false;
                quote.ReviewEmail = model.SubmitQuoteContactInfo;
            }

            quote.ServiceRank = model.MilitaryRank;

            if (model.PropertyAddressSameAsMailing.HasValue && model.PropertyAddressSameAsMailing.Value)
            {
                quote.PropertyCity = quote.City = model.PolicyHolderCity;
                quote.PropertyState = quote.State = model.PolicyHolderState;
                quote.PropertyStreet = quote.Street = model.PolicyHolderMailingAddress;
                quote.PropertyZipCode = quote.ZipCode = model.PolicyHolderZip;
            }
            else
            {
                quote.City = model.PolicyHolderCity;
                quote.State = model.PolicyHolderState;
                quote.Street = model.PolicyHolderMailingAddress;
                quote.ZipCode = model.PolicyHolderZip;

                quote.PropertyCity = model.PropertyCity;
                quote.PropertyState = model.PropertyState;
                quote.PropertyStreet = model.PropertyAddress;
                quote.PropertyZipCode = model.PropertyZip;
            }

            

            quote.FirstCommandAdvisorName = model.FirstCommandAdvisorName;
            quote.SpouseOfMilitary = string.Equals(model.EligibilityStatus, "MilitarySpouse", StringComparison.InvariantCultureIgnoreCase);
            quote.UnderMoratorium = false;
            quote.WantToReceiveInfo = model.SubmitQuoteMoreInfo;

            if (string.Equals(model.EligibilityStatus, "child", StringComparison.InvariantCultureIgnoreCase))
            {
                quote.InsuredParent = $"{model.EligibilityLastName}, {model.EligibilityFirstName}";
            }
            if (string.Equals(model.EligibilityStatus, "parent", StringComparison.InvariantCultureIgnoreCase))
            {
                quote.InsuredParent = $"{model.EligibilityLastName}, {model.EligibilityFirstName}";
            }
            if (string.Equals(model.EligibilityStatus, "MilitarySpouse", StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(model.PolicyHolderMaritalStatus, "married", StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(model.PolicyHolderMaritalStatus, "civil union or domestic partner", StringComparison.InvariantCultureIgnoreCase))
            {
                quote.ServiceSpouseFirstName = !string.IsNullOrWhiteSpace(model.SpouseFirstName) ? model.SpouseFirstName : model.EligibilityFirstName;
                quote.ServiceSpouseLastName = !string.IsNullOrWhiteSpace(model.SpouseLastName) ? model.SpouseLastName : model.EligibilityLastName;
                quote.SpouseFirstName = model.SpouseFirstName ?? model.CohabitantFirstName;
                quote.SpouseLastName = model.SpouseLastName ?? model.CohabitantLastName;
                quote.SpouseSuffix = model.SpouseSuffix ?? model.CohabitantSuffix;
                quote.SpouseBirthDate = model.SpouseDob ?? model.CohabitantDob;
                quote.SpouseGender = model.SpouseGender ?? model.CohabitantGender;
                quote.SpouseSSN = model.SpouseSsn ?? model.CohabitantSsn;
            }

            if (string.Equals(model.PolicyHolderMaritalStatus, "cohabitant", StringComparison.InvariantCultureIgnoreCase))
            {
                quote.ServiceSpouseFirstName = !string.IsNullOrWhiteSpace(model.CohabitantFirstName) ? model.CohabitantFirstName : model.EligibilityFirstName;
                quote.ServiceSpouseLastName = !string.IsNullOrWhiteSpace(model.CohabitantLastName) ? model.CohabitantLastName : model.EligibilityLastName;
                quote.SpouseFirstName = model.CohabitantFirstName;
                quote.SpouseLastName = model.CohabitantLastName;
                quote.SpouseSuffix = model.CohabitantSuffix;
                quote.SpouseBirthDate = model.CohabitantDob;
                quote.SpouseGender = model.CohabitantGender;
                quote.SpouseSSN = model.CohabitantSsn;
            }
        }
        public static void Map(this QuoteFlood quote, FloodFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteFlood();
            }

            quote.Key = quoteKey;
            quote.LocationDifferentThanMailing = !model.PropertyAddressSameAsMailing ?? false;
            quote.Address = model.PropertyAddress;
            quote.City = model.PropertyCity;
            quote.State = model.PropertyState;
            quote.ZipCode = model.PropertyZip;
            quote.IsCondo = model.IsStructureACondominium;
            quote.CondoFloor = model.WhatFloorIsYourCondominiumOn;
            quote.FloorsInStructure = model.FloorsInStructure;
            quote.FoundationType = model.FloodFoundation;
            quote.ConstructionDate = model.PropertyYearBuilt;
            quote.OccupiedType = model.PropertyOccupancy;
            quote.TotalLivingArea = model.TotalLivingAreaSqFt;
            quote.StructureValue = model.StructuralValue;
            quote.GarageType = model.GarageType;
            quote.GarageValue = model.GarageValue;
            quote.FloodZone = model.FloodZone;
            quote.AwareOfFloodLosses = model.AwareOfFloodLossesOnProperty;
            quote.NumberOfLosses = model.HowManyLossesHaveOccurred;
            quote.MortgageInsAmount = model.HowMuchMortgageInsuranceRequired;
        }

        public static void ReverseMap(this FloodFormSaveModel model, FTData.Quote quote, QuoteContact contact, QuoteFlood flood)
        {
            if (model == null)
            {
                model = new FloodFormSaveModel();
            }

            model.CommonReverseMap(quote, contact, null, null);

            if (flood != null)
            {
                model.PropertyAddressSameAsMailing = !flood.LocationDifferentThanMailing;
                model.IsStructureACondominium = flood.IsCondo;
                model.WhatFloorIsYourCondominiumOn = flood.CondoFloor;
                model.FloorsInStructure = flood.FloorsInStructure;
                model.FloodFoundation = flood.FoundationType;
                model.PropertyYearBuilt = flood.ConstructionDate;
                model.PropertyOccupancy = flood.OccupiedType;
                model.TotalLivingAreaSqFt = flood.TotalLivingArea;
                model.StructuralValue = flood.StructureValue;
                model.GarageType = flood.GarageType;
                model.GarageValue = flood.GarageValue;
                model.FloodZone = flood.FloodZone;
                model.AwareOfFloodLossesOnProperty = flood.AwareOfFloodLosses;
                model.HowManyLossesHaveOccurred = flood.NumberOfLosses;
                model.HowMuchMortgageInsuranceRequired = flood.MortgageInsAmount;
            }
        }
    }
}