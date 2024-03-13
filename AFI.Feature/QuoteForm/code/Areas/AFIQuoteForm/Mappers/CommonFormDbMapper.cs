using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using FTData = AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class CommonFormDbMapper
    {
        public static void CommonReverseMap(this CommonFormSaveModel model, FTData.Quote quote, QuoteContact contact, List<QuoteAutoIncident> incidents, QuoteAuto auto)
        {
            if (model == null)
            {
                model = new CommonFormSaveModel();
            }

            if (quote != null)
            {
                model.QuoteId = quote.Key;
                model.EligibilityStatus = quote.Eligibility;
                model.ResponseType = quote.ResponseType;
                model.PolicyHolderZip = quote.ZipCode;
                model.OfferDescription = quote.OfferDescription;
                model.ResponseDescription = quote.ResponseDescription;
                model.ResponseType = quote.ResponseType;
            }

            if (contact != null)
            {
                model.PolicyHolderFirstName = model.EligibilityFirstName = contact.FirstName;
                model.PolicyHolderLastName = model.EligibilityLastName = contact.LastName;
                model.PolicyHolderSuffix = contact.Suffix;
                model.PolicyHolderDob = contact.BirthDate;
                model.PolicyHolderGender = contact.Gender;
                model.PolicyHolderResidenceStatus = contact.ResidenceStatus;
                model.PolicyHolderMaritalStatus = contact.MaritalStatus;
                model.PolicyHolderEmail = contact.Email;
                model.PolicyHolderPhoneNumber = contact.PhoneNumber;
                model.PolicyHolderPhoneType = contact.PhoneType;
                model.PolicyHolderSsn = contact.SSN;
                model.PolicyHolderMailingAddress = contact.Street;
                model.PolicyHolderState = contact.State;
                model.PolicyHolderCity = contact.City;
                model.PolicyHolderPrimaryResidenceAddress = contact.PropertyStreet;
                model.PolicyHolderPrimaryResidenceCity = contact.PropertyCity;
                model.PolicyHolderPrimaryResidenceState = contact.PropertyState;
                model.PolicyHolderPrimaryResidenceZip = contact.PropertyZipCode;
                model.BranchOfService = contact.ServiceBranch;
                model.MilitaryStatus = contact.ServiceStatus;
                model.MilitaryRank = contact.ServiceRank;
                model.SubmitQuoteMoreInfo = contact.WantToReceiveInfo;
                model.SubmitQuoteContactMethod = contact.CallForReview ? "Phone" : "Email";
                model.SubmitQuoteContactInfo = string.Equals(model.SubmitQuoteContactMethod, "Phone", StringComparison.InvariantCultureIgnoreCase) ? contact.ReviewPhoneNum : contact.ReviewEmail;
                model.FirstCommandAdvisorName = contact.FirstCommandAdvisorName;
                if (string.Equals(quote.Eligibility, "MilitarySpouse", StringComparison.InvariantCultureIgnoreCase) || 
                    string.Equals(contact.MaritalStatus, "Married", StringComparison.InvariantCultureIgnoreCase) ||
                    string.Equals(contact.MaritalStatus, "Cohabitant", StringComparison.InvariantCultureIgnoreCase))
                {
                    model.EligibilityFirstName = contact.ServiceSpouseFirstName;
                    model.EligibilityLastName = contact.ServiceSpouseLastName;
                    model.SpouseFirstName = model.CohabitantFirstName = contact.SpouseFirstName;
                    model.SpouseLastName = model.CohabitantLastName = contact.SpouseLastName;
                    model.SpouseSuffix = model.CohabitantSuffix = contact.SpouseSuffix;
                    model.SpouseDob = model.CohabitantDob = contact.SpouseBirthDate;
                    model.SpouseGender = model.CohabitantGender = contact.SpouseGender;
                    model.SpouseSsn = model.CohabitantSsn = contact.SpouseSSN;
                }

                if (string.Equals(model.EligibilityStatus, "child", StringComparison.InvariantCultureIgnoreCase))
                {
                    string[] names = contact.InsuredParent.Split(',').Select(c => c.Trim()).ToArray();
                    if (names.Any())
                    {
                        model.EligibilityLastName = names[0];
                        if (names.Length > 1)
                            model.EligibilityFirstName = names[1];
                    }
                }
            }

            if (incidents != null && incidents.Any())
            {
                model.HouseholdViolationsPreviousClaims = true;
                string commonPrefix = "HouseholdViolations";
                for (var index = 0; index < incidents.Count; index++)
                {
                    QuoteAutoIncident autoIncident = incidents[index];
                    model.SetPropertyValueByIndex(commonPrefix, "DriverKey", index, autoIncident.Key);
                    model.SetPropertyValueByIndex(commonPrefix, "Driver", index, autoIncident.DriverName);
                    model.SetPropertyValueByIndex(commonPrefix, "Type", index, autoIncident.Incident);
                    model.SetPropertyValueByIndex(commonPrefix, "Date", index, autoIncident.Date);
                }
            }

            if (auto != null)
            {
                model.BodilyInjuryLiability = auto.BodilyInjury;
                model.PropertyDamageLiability = auto.PropertyDamage;
                model.MedicalPayment = auto.MedicalCoverage;
                model.UninsuredMotoristBodilyInjury = auto.UninsuredBodilyInjury;
                model.PersonalInjuryProtection = auto.PersonalInjury;
                model.CurrentInsuranceCompany = auto.CurrentInsurance;
                model.PolicyRenewalDate = auto.CurrentPolicyDate;
                model.LicensedDriverCount = auto.NumberOfLicensedDrivers;
                model.PolicyRenewalType = auto.CurrentPolicyAction;
            }
        }

        public static void Map(this FTData.Quote quote, string coverageType, CommonFormSaveModel model, bool isSubmitted, bool returningVisit)
        {
            if (quote == null)
            {
                quote = new Quote();
            }

            quote.Key = model.QuoteId ?? 0;
            quote.CoverageType = coverageType;
            quote.Eligibility = model.EligibilityStatus.ReverseNormalizeEnum<AFIQuoteForm.Helpers.Enums.UI_EligibilityStatus>().To<AFIQuoteForm.Helpers.Enums.DB_EligibilityStatus>().ToString();
            quote.ResponseType = model.ResponseType;
            quote.ZipCode = model.PolicyHolderZip ?? "99999";
            quote.OfferDescription = model.OfferDescription;
            quote.ResponseType = model.ResponseType ?? "AFI WEBSITE";
            quote.ResponseDescription = String.IsNullOrEmpty(model.ResponseType) && String.IsNullOrEmpty(model.ResponseDescription) ? "8896 - WEBSITE" : model.ResponseDescription;
            quote.ReadDisclaimer = isSubmitted;
            quote.Started = model.StartDate;

         
            if (isSubmitted)
            {
                quote.Finished = DateTime.UtcNow;
            }
            else
            {
                quote.SaveForLaterCreateDate = DateTime.UtcNow;
                quote.SaveForLaterKey = Guid.NewGuid();
            }
        }

        public static void Map(this QuoteContact quote, CommonFormSaveModel model, int quoteKey)
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
            quote.PropertyCity = model.PolicyHolderPrimaryResidenceCity ?? model.PolicyHolderCity;
            quote.PropertyState = model.PolicyHolderPrimaryResidenceState ?? model.PolicyHolderState;
            quote.PropertyStreet = model.PolicyHolderPrimaryResidenceAddress ?? model.PolicyHolderMailingAddress;
            quote.PropertyZipCode = model.PolicyHolderPrimaryResidenceZip ?? model.PolicyHolderZip;
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
                string.Equals(model.PolicyHolderMaritalStatus, "cohabitant", StringComparison.InvariantCultureIgnoreCase) ||
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
        }

        public static void Map(this QuoteAutoIncident quote, CommonFormSaveModel model, int quoteKey, int incidentIndex)
        {
            if (quote == null)
            {
                quote = new QuoteAutoIncident();
            }

            quote.QuoteKey = quoteKey;
            quote.Key = incidentIndex;
            string householdPrefix = "HouseholdViolations";
            quote.DriverKey = model.GetPropertyValueByIndex<CommonFormSaveModel, int?>(householdPrefix, "DriverKey", incidentIndex);
            quote.DriverName = model.GetPropertyValueByIndex<CommonFormSaveModel, string>(householdPrefix, "Driver", incidentIndex);
            quote.Incident = model.GetPropertyValueByIndex<CommonFormSaveModel, string>(householdPrefix, "Type", incidentIndex);
            quote.Date = model.GetPropertyValueByIndex<CommonFormSaveModel, DateTime?>(householdPrefix, "Date", incidentIndex);
        }

        public static void Map(this QuoteAuto quote, CommonFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteAuto();
            }

            quote.Key = quoteKey;
            quote.BodilyInjury = model.BodilyInjuryLiability;
            quote.PropertyDamage = model.PropertyDamageLiability;
            quote.MedicalCoverage = model.MedicalPayment;
            quote.UninsuredBodilyInjury = model.UninsuredMotoristBodilyInjury;
            quote.PersonalInjury = model.PersonalInjuryProtection;
            quote.CurrentInsurance = string.Equals(model.CurrentInsuranceCompany, "Other", StringComparison.InvariantCultureIgnoreCase) ? model.OtherInsuranceCompany : model.CurrentInsuranceCompany;
            quote.CurrentPolicyDate = model.PolicyRenewalDate;
            quote.NumberOfLicensedDrivers = model.LicensedDriverCount;
            quote.CurrentPolicyAction = model.PolicyRenewalType;
        }
    }
}