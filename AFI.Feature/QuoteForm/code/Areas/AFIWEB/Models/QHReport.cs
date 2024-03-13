using System;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class QHReport
    {

        public string ID { get; set; }
        public string QuoteID { get; set; }
        public string RiskAddressID { get; set; }
        public string CoverageType { get; set; }
        public string UserFName { get; set; }
        public string UserLName { get; set; }
        public string emailID { get; set; }
        public string PhoneNumber { get; set; }
        public string Eligibility { get; set; }
        public string MailingstreeAddress { get; set; }
        public string MailingState { get; set; }
        public string MailingCity { get; set; }
        public string MailingZipCode { get; set; }
        public string UserType { get; set; }
        public string QuoteNo { get; set; }
        public string LastSaveDate { get; set; }
        public string QuoteStatus { get; set; }
        public string FormState { get; set; }
        public string CreatedOn { get; set; }
        public int Completed { get; set; }
        public string CompletedDate { get; set; }
        public string ResponseType { get; set; }
        public string ResponseDescription { get; set; }
        public string WebCampaign { get; set; }
        public string WebContent { get; set; }
        public string WebMedium { get; set; }
        public string WebSource { get; set; }
        public string WebTerm { get; set; }
        public string WebClickID { get; set; }
        public string gclid { get; set; }
        public string msclkid { get; set; }
        public string fbclid { get; set; }
        public string ChipMemberId { get; set; }
        public string InsIdBranchOfService { get; set; }
        public string InsIdMilitaryStatus { get; set; }
        public string InsIdRank { get; set; }
        public string InsRiskAddress1 { get; set; }
        public string InsRiskAddress2 { get; set; }
        public string InsRiskZip { get; set; }
        public string InsIdRiskCity { get; set; }
        public string InsIdRiskState { get; set; }
        public string InsIdRiskCounty { get; set; }

        public string QuotesStarted { get; set; }
        public string BasicsInfo { get; set; }
        public string PropertyAddressInfo { get; set; }
        public int IncompleteQuote { get; set; }
        public int NoQuote { get; set; }
        public string Premium { get; set; }
        public string PremiumInterested { get; set; }
        public string NoQuoteReason { get; set; }
        public string RiskState { get; set; }
        public string LOB { get; set; }
        public string QuoteNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DOB { get; set; }
        public string WhoEntered { get; set; }
        public string AgentName { get; set; }
        public string ProgramState { get; set; }
        public int Total { get; set; }

        public string BirthDate
        {
            get
            {
                return DOB.ToShortDateString();
            }
        }
        public string Created_Date
        {
            get
            {
                return CreatedDate.ToShortDateString();
            }
        }
    }
}