using AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.LeadGenerationForm;
using FTData = AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class LeadGenerationFormDbMapper
    {
        public static void Map(this QuoteLeadGeneration quote, LeadGenerationFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteLeadGeneration();
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
        }

        public static void ReverseMap(this LeadGenerationFormSaveModel model, FTData.Quote quote, QuoteContact contact, QuoteLeadGeneration leadGeneration)
        {
            if (model == null)
            {
                model = new LeadGenerationFormSaveModel();
            }

            model.CommonReverseMap(quote, contact, null, null);
            
            if (leadGeneration != null)
            {
                model.PolicyHolderFirstName = leadGeneration.FirstName;
                model.PolicyHolderLastName = leadGeneration.LastName;
                model.PolicyHolderSuffix = leadGeneration.Suffix;
                model.SpouseFirstName = leadGeneration.SpouseFirstName;
                model.SpouseLastName = leadGeneration.SpouseLastName;
                model.PolicyHolderPhoneNumber = leadGeneration.PhoneNumber;
                model.PolicyHolderEmail = leadGeneration.EmailAddress;
                model.PolicyHolderMailingAddress = leadGeneration.PolicyHolderMailingAddress;
                model.PolicyHolderMailingAddress2 = leadGeneration.PolicyHolderMailingAddress2;
                model.City = leadGeneration.PolicyHolderCity;
                model.State = leadGeneration.PolicyHolderState;
                model.Zip = leadGeneration.PolicyHolderZip; 
            }
        }
    }
}