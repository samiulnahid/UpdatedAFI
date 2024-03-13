using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.RenterForm
{
    public class RenterFormSaveModel : CommonFormSaveModel
    {
        public bool AddressToBeQuotedAddressSameAsMailing { get; set; }
        public string AddressToBeQuotedAddress { get; set; }
        public string AddressToBeQuotedCity { get; set; }
        public string AddressToBeQuotedState { get; set; }
        public string AddressToBeQuotedZip { get; set; }
        public string CoverageInfoAmountRequested { get; set; }
        public string CoverageInfoDeductibleRequested { get; set; }
        public bool CoverageInfoIncludeReplacementCost { get; set; }
        public bool CoverageInfoIncludeReplacementCostUnderstand { get; set; }
        public bool CoverageInfoIncludeComprehensiveCoverageEndorsement { get; set; }
        public bool CoverageInfoIncludeAdditionalCoverageEndorsement { get; set; }
        public bool CoverageInfoIncludeIdentityFraudExpenseCoverageEndorsement { get; set; }
        public bool CoverageInfoIncludeBusinessPropertyEndorsement { get; set; }
        public string PropertyType { get; set; }
        public string HouseholdGoodsQuality { get; set; }
        public string FurnishedRoomsCount { get; set; }
        public string PersonalProperty { get; set; }
    }
}