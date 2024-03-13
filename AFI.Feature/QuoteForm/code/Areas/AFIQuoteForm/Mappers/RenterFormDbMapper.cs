using AFI.Feature.Data.DataModels;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.RenterForm;
using FTData = AFI.Feature.Data.DataModels;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers
{
    public static class RenterFormDbMapper
    {
        public static void Map(this QuoteRenter quoteRenter, RenterFormSaveModel model, int quoteKey)
        {
            if (quoteRenter == null)
            {
                quoteRenter = new QuoteRenter();
            }

            quoteRenter.Key = quoteKey;
            quoteRenter.LivingQuarters = model.PropertyType;
            quoteRenter.Quality = model.HouseholdGoodsQuality;
            quoteRenter.TotalNumberOfFurnishedRooms = model.FurnishedRoomsCount;
            quoteRenter.PersonalPropertyValue = model.PersonalProperty;
            quoteRenter.Deductible = model.CoverageInfoDeductibleRequested;
            quoteRenter.Comprehensive = model.CoverageInfoIncludeComprehensiveCoverageEndorsement;
            quoteRenter.Replacement = model.CoverageInfoIncludeReplacementCost;
            quoteRenter.IdentityFraud = model.CoverageInfoIncludeIdentityFraudExpenseCoverageEndorsement;
            quoteRenter.AdditionalCoverages = model.CoverageInfoIncludeAdditionalCoverageEndorsement;
            quoteRenter.BusinessProperty = model.CoverageInfoIncludeBusinessPropertyEndorsement;
            quoteRenter.ReplacementCostITV = model.CoverageInfoAmountRequested;
        }

        public static void ReverseMap(this RenterFormSaveModel model, FTData.Quote quote, QuoteContact contact, QuoteRenter renter)
        {
            if (model == null)
            {
                model = new RenterFormSaveModel();
            }

            model.CommonReverseMap(quote, contact, null, null);

            if (renter != null)
            {
                model.PropertyType = renter.LivingQuarters;
                model.HouseholdGoodsQuality = renter.Quality;
                model.FurnishedRoomsCount = renter.TotalNumberOfFurnishedRooms;
                model.PersonalProperty = renter.PersonalPropertyValue;
                model.CoverageInfoDeductibleRequested = renter.Deductible;
                model.CoverageInfoIncludeComprehensiveCoverageEndorsement = renter.Comprehensive;
                model.CoverageInfoIncludeReplacementCost = renter.Replacement;
                model.CoverageInfoIncludeIdentityFraudExpenseCoverageEndorsement = renter.IdentityFraud;
                model.CoverageInfoIncludeAdditionalCoverageEndorsement = renter.AdditionalCoverages;
                model.CoverageInfoIncludeBusinessPropertyEndorsement = renter.BusinessProperty;
                model.CoverageInfoAmountRequested = renter.ReplacementCostITV;
            }
        }
    }
}