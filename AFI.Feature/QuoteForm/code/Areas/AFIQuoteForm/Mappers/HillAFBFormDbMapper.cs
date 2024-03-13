using AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HillAFBForm;
using FTData = AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class HillAFBFormDbMapper
    {
        public static void Map(this QuoteHillAFB quote, HillAFBFormSaveModel model, int quoteKey)
        {
            if (quote == null)
            {
                quote = new QuoteHillAFB();
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

        public static void ReverseMap(this HillAFBFormSaveModel model, FTData.Quote quote, QuoteContact contact, QuoteHillAFB hillAFB)
        {
            if (model == null)
            {
                model = new HillAFBFormSaveModel();
            }

            model.CommonReverseMap(quote, contact, null, null);
            
            if (hillAFB != null)
            {
                model.PolicyHolderFirstName = hillAFB.FirstName;
                model.PolicyHolderLastName = hillAFB.LastName;
                model.PolicyHolderSuffix = hillAFB.Suffix;
                model.SpouseFirstName = hillAFB.SpouseFirstName;
                model.SpouseLastName = hillAFB.SpouseLastName;
                model.PolicyHolderPhoneNumber = hillAFB.PhoneNumber;
                model.PolicyHolderEmail = hillAFB.EmailAddress;
                model.PolicyHolderMailingAddress = hillAFB.PolicyHolderMailingAddress;
                model.PolicyHolderMailingAddress2 = hillAFB.PolicyHolderMailingAddress2;
                model.City = hillAFB.PolicyHolderCity;
                model.State = hillAFB.PolicyHolderState;
                model.Zip = hillAFB.PolicyHolderZip; 
            }
        }
    }
}