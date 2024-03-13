using System;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
using AFI.Feature.WebQuoteService.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.WebQuoteMappers
{
    class ContactInfoModelHelper
    {
        public string PrimaryFirst { get; set; }
        public string PrimaryLast { get; set; }
        public DateTime? PrimaryDob { get; set; }
        public string PrimarySuffix { get; set; }
        public string PrimaryGender { get; set; }
        public string PrimarySsn { get; set; }
        public string PrimaryPrefix { get; set; }

        public string SpouseFirst { get; set; }
        public string SpouseLast { get; set; }
        public DateTime? SpouseDob { get; set; }
        public string SpouseSuffix { get; set; }
        public string SpouseGender { get; set; }
        public string SpouseSsn { get; set; }
        public string SpousePrefix { get; set; }

    }
    public static class CommonFormWebQuoteMapper
    {
        private static readonly string DEFAULT_MARKETING_CODE = "8896";

        private static BranchOfServiceContainer GetBranchOfService(CommonFormSaveModel model)
        {
            var container = new BranchOfServiceContainer()
            {
                BranchOfServiceInfo = new BranchOfServiceClass()
            };

            switch (model.EligibilityStatus.ToLowerInvariant())
            {
                case "military":
                case "spouse":
                    container.BranchOfServiceInfo.BranchOfService = model.BranchOfService.ToUpper();
                    container.BranchOfServiceInfo.BranchOfServiceStatus = model.MilitaryStatus.ReverseNormalizeEnum<WebQuoteMapperEnums.DataCacheBranchOfServiceStatusTypes>()
                        .To<WebQuoteMapperEnums.BranchOfServiceStatusTypes>()
                        .ToString().ToUpper();
                    container.BranchOfServiceInfo.PayGrade = model.MilitaryRank.Substring(0, model.MilitaryRank.IndexOf(' ')).Trim().ToUpper();
                    string rankEnum = model.MilitaryRank.Substring(model.MilitaryRank.IndexOf(' ')).Trim()
                        .ReverseNormalizeEnum<WebQuoteMapperEnums.RankTypes>()
                        .ToString();
                    container.BranchOfServiceInfo.Rank = model.MilitaryRank.Substring(model.MilitaryRank.IndexOf(' ')).Trim().ToUpper();
                    container.BranchOfServiceInfo.RankAbbreviation = rankEnum.ToEnum<WebQuoteMapperEnums.RankTypes>()
                        .To<WebQuoteMapperEnums.RankABRVTypes>()
                        .ToString().ToUpper();
                    break;
                case "dod":
                    container.BranchOfServiceInfo.BranchOfServiceStatus = WebQuoteMapperEnums.DataCacheBranchOfServiceStatusTypes.Active.ToString().ToUpper();
                    container.BranchOfServiceInfo.PayGrade = WebQuoteMapperEnums.PayGradeTypes.AD_dash_1.ToString().ToUpper();
                    container.BranchOfServiceInfo.Rank = WebQuoteMapperEnums.RankTypes.ADMINISTRATIVELY_DETERMINED__popen_AD_pclose_.ToString().ToUpper();
                    container.BranchOfServiceInfo.RankAbbreviation = WebQuoteMapperEnums.RankTypes.ADMINISTRATIVELY_DETERMINED__popen_AD_pclose_.To<WebQuoteMapperEnums.RankABRVTypes>().ToString().ToUpper();
                    break;
            }

            return container;
        }

        private static WebQuoteMapperEnums.GenderABVRTypes? GetGender(string gender)
        {
            if (string.IsNullOrWhiteSpace(gender)) return null;
            if (string.Equals(gender, "Male", StringComparison.InvariantCultureIgnoreCase)) return WebQuoteMapperEnums.GenderABVRTypes.M;
            if (string.Equals(gender, "Female", StringComparison.InvariantCultureIgnoreCase)) return WebQuoteMapperEnums.GenderABVRTypes.F;
            return WebQuoteMapperEnums.GenderABVRTypes.U;
        }

        private static WebQuoteMapperEnums.MaritalStatusABRVTypes? GetMaritalStatus(string maritalStatus)
        {
            if (string.IsNullOrEmpty(maritalStatus)) return null;
            if (string.Equals(maritalStatus, "Married", StringComparison.InvariantCultureIgnoreCase)) return WebQuoteMapperEnums.MaritalStatusABRVTypes.M;
            if (string.Equals(maritalStatus, "Cohabitant", StringComparison.InvariantCultureIgnoreCase)) return WebQuoteMapperEnums.MaritalStatusABRVTypes.C;
            if (string.Equals(maritalStatus, "Single", StringComparison.InvariantCultureIgnoreCase)) return WebQuoteMapperEnums.MaritalStatusABRVTypes.S;
            if (string.Equals(maritalStatus, "Divorced", StringComparison.InvariantCultureIgnoreCase)) return WebQuoteMapperEnums.MaritalStatusABRVTypes.D;
            if (string.Equals(maritalStatus, "Widowed", StringComparison.InvariantCultureIgnoreCase)) return WebQuoteMapperEnums.MaritalStatusABRVTypes.W;
            return WebQuoteMapperEnums.MaritalStatusABRVTypes.U;
        }

        public static void Map(this AFIForm afiForm, CommonFormSaveModel model, AFIFormsMapID form, bool isPossibleDuplicate, string coverageType, string[] testingIpAddresses)
        {
            string currentIpAddress = string.Empty;
            try
            {
                currentIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                Sitecore.Diagnostics.Log.Info($"AFI Form current IP Address {currentIpAddress}", Constants.QUOTE_FORMS_LOGGER_NAME);
                Sitecore.Diagnostics.Log.Info($"AFI Form IP Addresses to test with {Newtonsoft.Json.JsonConvert.SerializeObject(testingIpAddresses)}", Constants.QUOTE_FORMS_LOGGER_NAME);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("AFI Form current IP Address cannot be aquired", ex, Constants.QUOTE_FORMS_LOGGER_NAME);
            }

            if (afiForm == null)
            {
                afiForm = new AFIForm();
            }
            afiForm.CNTCGroupId = model.MemberNumber == 0 ? (int?)null : model.MemberNumber;
            afiForm.QuoteId = model.QuoteId.ToString();
            afiForm.Priority = 5; //INFO: This value comes from AFI code
            afiForm.StateAbbrv = model.PolicyHolderState ?? model.State;
            afiForm.Address1 = model.PolicyHolderMailingAddress ?? model.BusinessPhysicalAddress;
            afiForm.Expidite = false;
            afiForm.DisplayInformation = $"{model.PolicyHolderFirstName} {model.PolicyHolderLastName}";
            afiForm.CreatedDate = model.StartDate;
            afiForm.UpdatedDate = DateTime.Now;
            afiForm.CreationUser = afiForm.UpdateUser = afiForm.Email = model.PolicyHolderEmail;
            afiForm.Eligibility = model.EligibilityStatus;
            afiForm.CallForReview = string.Equals(model.SubmitQuoteContactMethod, "Phone", StringComparison.InvariantCultureIgnoreCase);
            if (string.Equals(coverageType, CoverageTypes.Auto, StringComparison.InvariantCultureIgnoreCase))
            {
                afiForm.CurrentlyAutoInsured = !string.IsNullOrWhiteSpace(model.CurrentInsuranceCompany) &&
                    !string.Equals(model.CurrentInsuranceCompany, "None", StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                afiForm.CurrentlyAutoInsured = true;
            }

            afiForm.IsFirstCommand = !string.IsNullOrWhiteSpace(model.FirstCommandAdvisorName);
            afiForm.ReceivedPremium = false;
            afiForm.IsFinished = true;
            afiForm.IsMobileManufactured = false;
            afiForm.IsSlt = false;
            afiForm.IsRental = false;
            afiForm.IsTest = !string.IsNullOrWhiteSpace(currentIpAddress) && testingIpAddresses.Any() && testingIpAddresses.Contains(currentIpAddress.Trim());
            Sitecore.Diagnostics.Log.Info($"AFI Form IsTest value {afiForm.IsTest}", Constants.QUOTE_FORMS_LOGGER_NAME);
            afiForm.IsBusinessTest = afiForm.IsTest && (string.Equals(coverageType, "Commercial", StringComparison.InvariantCultureIgnoreCase) || string.Equals(coverageType, "Business", StringComparison.InvariantCultureIgnoreCase));
            Sitecore.Diagnostics.Log.Info($"AFI Form IsBusinessTest value {afiForm.IsTest}", Constants.QUOTE_FORMS_LOGGER_NAME);
            afiForm.NeedsUnderwriting = false;
            afiForm.IssuedOnline = false;
            afiForm.ApplicationCompleted = false;
            afiForm.IsInterested = false;
            afiForm.IsCondo = false;
            afiForm.IsVacant = false;
            afiForm.FormTypeId = form?.FormTypeID ?? 0;
            afiForm.CoverageTypeId = form?.CoverageTypeID ?? 0;
            afiForm.City = model.PolicyHolderCity ?? model.City;
            afiForm.IsPossibleDup = isPossibleDuplicate;
        }

        public static void Map(this ContactInformation contactInformation, CommonFormSaveModel model)
        {
            if (contactInformation == null)
            {
                contactInformation = new ContactInformation();
            }
            var contactInfoHelper = new ContactInfoModelHelper();
            
            if (string.Equals(model.EligibilityStatus, "MilitarySpouse", StringComparison.InvariantCultureIgnoreCase))
            {
                contactInfoHelper.PrimaryPrefix = "RANK";
                contactInfoHelper.PrimaryFirst = model.SpouseFirstName ?? model.EligibilityFirstName ?? string.Empty;
                contactInfoHelper.PrimaryLast = model.SpouseLastName ?? model.EligibilityLastName ?? string.Empty;
                contactInfoHelper.PrimaryDob = model.SpouseDob ?? model.CohabitantDob;
                contactInfoHelper.PrimarySuffix = model.SpouseSuffix ?? model.CohabitantSuffix ?? string.Empty;
                contactInfoHelper.PrimaryGender = model.SpouseGender ?? model.CohabitantGender ?? string.Empty;
                contactInfoHelper.PrimarySsn = model.SpouseSsn ?? model.CohabitantSsn ?? string.Empty;
            }
            else
            {
                contactInfoHelper.PrimaryFirst = model.PolicyHolderFirstName ?? string.Empty;
                contactInfoHelper.PrimaryLast = model.PolicyHolderLastName ?? string.Empty;
                contactInfoHelper.PrimaryDob = model.PolicyHolderDob;
                contactInfoHelper.PrimarySuffix = model.PolicyHolderSuffix ?? string.Empty;
                contactInfoHelper.PrimaryGender = model.PolicyHolderGender ?? string.Empty;
                contactInfoHelper.PrimarySsn = model.PolicyHolderSsn ?? string.Empty;

                if (!string.Equals(model.EligibilityStatus, "Military", StringComparison.InvariantCultureIgnoreCase) || string.Equals(model.MilitaryStatus, "Separated", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!string.IsNullOrWhiteSpace(model.PolicyHolderGender))
                    {
                        if (string.Equals(model.PolicyHolderMaritalStatus, "Married", StringComparison.InvariantCultureIgnoreCase))
                        {
                            contactInfoHelper.PrimaryPrefix = string.Equals(model.PolicyHolderGender, "Male", StringComparison.InvariantCultureIgnoreCase) ? "MR" : "MRS";
                        }
                        else
                        {
                            contactInfoHelper.PrimaryPrefix = string.Equals(model.PolicyHolderGender, "Male", StringComparison.InvariantCultureIgnoreCase) ? "MR" : "MS";
                        }
                    }
                }
                else
                { 
                    contactInfoHelper.PrimaryPrefix = string.Empty;
                    if (string.Equals(model.EligibilityStatus, "Military", StringComparison.InvariantCultureIgnoreCase))
                    {
                        contactInfoHelper.PrimaryPrefix = "RANK";
                        if(string.Equals(model.MilitaryStatus, "Separated", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (!string.IsNullOrWhiteSpace(model.PolicyHolderGender))
                            {
                                if (string.Equals(model.PolicyHolderMaritalStatus, "Married", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    contactInfoHelper.PrimaryPrefix = string.Equals(model.PolicyHolderGender, "Male", StringComparison.InvariantCultureIgnoreCase) ? "MR" : "MRS";
                                }
                                else
                                {
                                    contactInfoHelper.PrimaryPrefix = string.Equals(model.PolicyHolderGender, "Male", StringComparison.InvariantCultureIgnoreCase) ? "MR" : "MS";
                                }
                            }
                        }
                    }
                }

                if (string.Equals(model.PolicyHolderMaritalStatus, "Married", StringComparison.InvariantCultureIgnoreCase) || string.Equals(model.PolicyHolderMaritalStatus, "Cohabitant", StringComparison.InvariantCultureIgnoreCase) || string.Equals(model.PolicyHolderMaritalStatus, "Civil Union Or Domestic Partner", StringComparison.InvariantCultureIgnoreCase))
                {
                    contactInfoHelper.SpouseFirst = model.SpouseFirstName ?? model.CohabitantFirstName ?? string.Empty;
                    contactInfoHelper.SpouseLast = model.SpouseLastName ?? model.CohabitantLastName ?? string.Empty;
                    contactInfoHelper.SpouseDob = model.SpouseDob ?? model.CohabitantDob;
                    contactInfoHelper.SpouseSuffix = model.SpouseSuffix ?? model.CohabitantSuffix ?? string.Empty;
                    contactInfoHelper.SpouseGender = model.SpouseGender ?? model.CohabitantGender ?? string.Empty;
                    contactInfoHelper.SpouseSsn = model.SpouseSsn ?? model.CohabitantSsn ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(contactInfoHelper.SpouseGender))
                    {
                        contactInfoHelper.SpousePrefix = string.Equals(contactInfoHelper.SpouseGender, "Male", StringComparison.InvariantCultureIgnoreCase) ? "MR" : "MRS";
                    }
                    else
                    {
                        contactInfoHelper.SpousePrefix = contactInfoHelper.SpouseGender ?? string.Empty;
                    }
                }
            }

            var primaryContactInfo = new ContactInfo(ContactType.Primary);
            primaryContactInfo.ClientType = model.EligibilityStatus.ToUpper().ReverseNormalizeEnum<WebQuoteMapperEnums.dataCacheClientTypes>().To<WebQuoteMapperEnums.ClientTypes>().ToString() ?? string.Empty;
            primaryContactInfo.AlreadySent = false;
            primaryContactInfo.NamePrefix = contactInfoHelper.PrimaryPrefix ?? string.Empty;
            primaryContactInfo.DOB = contactInfoHelper.PrimaryDob;
            primaryContactInfo.FirstName = contactInfoHelper.PrimaryFirst ?? string.Empty;
            primaryContactInfo.LastName = contactInfoHelper.PrimaryLast ?? string.Empty;
            primaryContactInfo.NameSuffix = contactInfoHelper.PrimarySuffix ?? string.Empty;
            primaryContactInfo.SSN = string.IsNullOrWhiteSpace(contactInfoHelper.PrimarySsn) ? null : contactInfoHelper.PrimarySsn.Replace("-", string.Empty);
            primaryContactInfo.Gender = GetGender(contactInfoHelper.PrimaryGender).ToString() ?? string.Empty;
            primaryContactInfo.MaritalStatus = GetMaritalStatus(model.PolicyHolderMaritalStatus).ToString() ?? string.Empty;
            primaryContactInfo.MarketingSource = model.ResponseType ?? string.Empty;
            primaryContactInfo.MarketingOfferCode = model.OfferDescription ?? string.Empty;
            primaryContactInfo.MarketingResponseCode = string.IsNullOrWhiteSpace(model.ResponseDescription) ? DEFAULT_MARKETING_CODE : model.ResponseDescription;
            primaryContactInfo.BranchOfServiceInformation = GetBranchOfService(model);
            primaryContactInfo.PhoneInformation = new PhoneInfo[]
            {
                new PhoneInfo
                {
                    PhoneNumber = model.PolicyHolderPhoneNumber.DigitsOnly() ?? string.Empty,
                    PhoneType = GetPhoneType(model.PolicyHolderPhoneType) ?? string.Empty
                }
            };
            primaryContactInfo.AddressInformation = new AddressInfo[]
            {
                new AddressInfo
                {
                    LocationType = "MAILING",
                    Country = "USA",
                    PerformAddressScrub = true,
                    County = string.Empty,
                    AddressLine1 = model.PolicyHolderMailingAddress ?? model.BusinessPhysicalAddress ?? string.Empty,
                    State = model.PolicyHolderState ?? model.State ?? string.Empty,
                    City = model.PolicyHolderCity ?? model.City ?? string.Empty,
                    PostalCode = model.PolicyHolderZip ?? model.Zip ?? string.Empty
                }
            };
            primaryContactInfo.EmailInformation = new EmailInfo[]
            {
                new EmailInfo()
                {
                    EmailAddress = model.PolicyHolderEmail ?? string.Empty,
                    EmailType = "PRIMARY"
                }
            };
            primaryContactInfo.PartnerEmployeeId = model.PartnerEmployeeId;
            //TODO: MFNumber? -> UPDATE: According to Amy we will not use this for now
            //TODO: MFSuffix? -> UPDATE: According to Amy we will not use this for now
            contactInformation.ContactInfo = new ContactInfo[]{};
            contactInformation.Add(primaryContactInfo);

            if (string.IsNullOrWhiteSpace(contactInfoHelper.SpouseFirst) || string.IsNullOrWhiteSpace(contactInfoHelper.SpouseLast)) return;

            var spouseSecondaryContactInfo = new ContactInfo(ContactType.SpouseSecondary);
            spouseSecondaryContactInfo.ClientType = WebQuoteMapperEnums.ClientTypes.Spouse.ToString();
            spouseSecondaryContactInfo.FirstName = contactInfoHelper.SpouseFirst ?? string.Empty;
            spouseSecondaryContactInfo.LastName = contactInfoHelper.SpouseLast ?? string.Empty;
            spouseSecondaryContactInfo.Gender = GetGender(contactInfoHelper.SpouseGender).ToString() ?? string.Empty;
            spouseSecondaryContactInfo.NameSuffix = contactInfoHelper.SpouseSuffix ?? string.Empty;
            spouseSecondaryContactInfo.SSN = contactInfoHelper.SpouseSsn.Replace("-", string.Empty) ?? null;
            spouseSecondaryContactInfo.DOB = contactInfoHelper.SpouseDob;
            spouseSecondaryContactInfo.NamePrefix = contactInfoHelper.SpousePrefix ?? string.Empty;
            spouseSecondaryContactInfo.MaritalStatus = GetMaritalStatus(model.PolicyHolderMaritalStatus).ToString() ?? string.Empty;
            spouseSecondaryContactInfo.MarketingSource = model.ResponseType ?? string.Empty;
            spouseSecondaryContactInfo.MarketingOfferCode = model.OfferDescription ?? string.Empty;
            spouseSecondaryContactInfo.MarketingResponseCode = string.IsNullOrWhiteSpace(model.ResponseDescription) ? DEFAULT_MARKETING_CODE : model.ResponseDescription;
            spouseSecondaryContactInfo.AlreadySent = false;
            contactInformation.Add(spouseSecondaryContactInfo);
        }

        private static string GetPhoneType(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            switch (input.ToLower())
            {
                case "cell":
                    return input.ToUpper();

                case "business":
                    return "PRIMARY WORK";

                default:
                    return $"PRIMARY {input.ToUpper()}";
            }
        }
    }
}