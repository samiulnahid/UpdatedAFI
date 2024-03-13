using System;
using System.Collections.Generic;
using System.Linq;
using AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.QuoteLeadForm;
using FTData = AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class QuoteLeadFormDbMapper
    {
        public static void Map(this QuoteLead quote, QuoteLeadFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteLead();
            }

            quote.Key = quoteKey;
            quote.FirstName = model.PolicyHolderFirstName;
            quote.LastName = model.PolicyHolderLastName;
            quote.Suffix = model.PolicyHolderSuffix;
            quote.SpouseFirstName = model.SpouseFirstName;
            quote.SpouseLastName = model.SpouseLastName;
            quote.EmailAddress = model.PolicyHolderEmail;
            quote.PhoneNumber = model.PolicyHolderPhoneNumber;           
            quote.PolicyHolderMailingAddress = model.PolicyHolderMailingAddress;
            quote.PolicyHolderMailingAddress2 = model.PolicyHolderMailingAddress2;
            quote.PolicyHolderCity = model.City;
            quote.PolicyHolderState = model.State;
            quote.PolicyHolderZip = model.Zip;
            quote.PropertyAddress2 = model.PropertyAddress2;
        }

        public static void ReverseMap(this QuoteLeadFormSaveModel model, FTData.Quote quote, QuoteContact contact, QuoteLead data)
        {
            if (model == null)
            {
                model = new QuoteLeadFormSaveModel();
            }

            model.CommonReverseMap(quote, contact, null, null);
            
            if (data != null)
            {
                model.PolicyHolderFirstName = data.FirstName;
                model.PolicyHolderLastName = data.LastName;
                model.PolicyHolderSuffix = data.Suffix;
                model.SpouseFirstName = data.SpouseFirstName;
                model.SpouseLastName = data.SpouseLastName;
                model.PolicyHolderPhoneNumber = data.PhoneNumber;
                model.PolicyHolderEmail = data.EmailAddress;
                model.PolicyHolderMailingAddress = data.PolicyHolderMailingAddress;
                model.PolicyHolderMailingAddress2 = data.PolicyHolderMailingAddress2;
                model.City = data.PolicyHolderCity;
                model.State = data.PolicyHolderState;
                model.Zip = data.PolicyHolderZip;
                model.PropertyAddress2 = data.PropertyAddress2;
            }
        }
    }
}