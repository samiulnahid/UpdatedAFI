using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
using FTData = AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.BusinessForm;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class BusinessFormDbMapper
    {
        public static void Map(this QuoteCommercial quote, BusinessFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteCommercial();
            }

            quote.Key = quoteKey;
            quote.FirstName = model.PolicyHolderFirstName;
            quote.LastName = model.PolicyHolderLastName;
            quote.PhoneNumber = model.PolicyHolderPhoneNumber;
            quote.EmailAddress = model.PolicyHolderEmail;
            quote.BusinessAddress = model.BusinessPhysicalAddress;
            quote.BusinessCity = model.City;
            quote.BusinessState = model.State;
            quote.BusinessZip = model.Zip;
            quote.BusinessName = model.BusinessName;
            quote.BusinessType = model.BusinessType;
            quote.BusinessWebsiteUrl = model.BusinessWebsite;
            quote.BusinessTaxID = model.BusinessTaxId;
            quote.InsuranceTypeWanted = model.TypeOfInsurance;
            quote.CurrentInsuranceCompany = model.CurrentInsuranceCompany;
            quote.InsuranceCompany = model.InsuranceCompany;
            quote.PolicyRenewalDate = model.PolicyRenewalDate;
            quote.PolicyRenewalType = model.PolicyRenewalType;
        }

        public static void ReverseMap(this BusinessFormSaveModel model, FTData.Quote quote, QuoteContact contact, QuoteCommercial commercial)
        {
            if (model == null)
            {
                model = new BusinessFormSaveModel();
            }

            model.CommonReverseMap(quote, contact, null, null);
            
            if (commercial != null)
            {
                model.PolicyHolderFirstName = commercial.FirstName;
                model.PolicyHolderLastName = commercial.LastName;
                model.PolicyHolderPhoneNumber = commercial.PhoneNumber;
                model.PolicyHolderEmail = commercial.EmailAddress;
                model.BusinessPhysicalAddress = commercial.BusinessAddress;
                model.City = commercial.BusinessCity;
                model.State = commercial.BusinessState;
                model.Zip = commercial.BusinessZip;
                model.BusinessName = commercial.BusinessName;
                model.BusinessType = commercial.BusinessType;
                model.BusinessWebsite = commercial.BusinessWebsiteUrl;
                model.BusinessTaxId = commercial.BusinessTaxID;
                model.TypeOfInsurance = commercial.InsuranceTypeWanted;
                model.CurrentInsuranceCompany = commercial.CurrentInsuranceCompany;
                model.InsuranceCompany = commercial.InsuranceCompany;
                model.PolicyRenewalDate = commercial.PolicyRenewalDate;
                model.PolicyRenewalType = commercial.PolicyRenewalType;
            }
        }
    }
}