using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class QHAPIPaymentUpdate
    {
        public int IdRiskAddress { get; set; } = 0;
        public int IdQuote { get; set; } = 0;
        public string ScheduleDate { get; set; } = "";
        public string ScheduleTime { get; set; } = "";
    }
    public class QHAPIPaymentThanks
    {
        public int IdRiskAddress { get; set; } = 0;
        public int IdQuote { get; set; } = 0;
        public string txtCallDate { get; set; } = "";
        public string drpHH { get; set; } = "";
        public string drpMM { get; set; } = "";
        public string hdnAMPM { get; set; } = "";
    }
    public class QHAPIPayment
    {
        public int IdPayment { get; set; } = 0;
        public int IdRiskAddress { get; set; } = 0;
        public int IdQuote { get; set; } = 0;
        public string confirmation_number { get; set; } = "";
        public string payment_status { get; set; } = "";
        public string Amount { get; set; } = "";
        public string payment_request_date { get; set; } = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        public string payment_date { get; set; } = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        public string error_code { get; set; } = "";
        public string error_message { get; set; } = "";
        public string CreatedBy { get; set; } = "1";
        public string CreatedOn { get; set; } = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        public string PaymentPlan { get; set; } = "";
    }
    public class QHAPIModel
    {
        public string InsFirstName { get; set; } = "";
        public string InsLastName { get; set; } = "";
        public string InsDOB { get; set; } = "";
        public string InsSocialSecNumber { get; set; } = "";
        public string InsPhoneNumber { get; set; } = "";
        public string InsEmailAddress { get; set; } = "";
        public int InsIdEligibility { get; set; } = 0;
        public string InsSpouseFirstName { get; set; } = "";
        public string InsSpouseLastName { get; set; } = "";
        public string InsSpouseDOB { get; set; } = "";
        public string InsSpouseSocialSecNumber { get; set; } = "";
        public string InsMailAddress1 { get; set; } = "";
        public string InsMailAddress2 { get; set; } = "";
        public string InsMailZip { get; set; } = "";
        public string InsMailCity { get; set; } = "";
        public string InsMailState { get; set; } = "";
        public string InsCovToBegin { get; set; } = "";
        public int IsBusinessOrRevenueOnPremises { get; set; } = 0;
        public string OpCovOption { get; set; } = "";
        public bool CancDec5Yrs { get; set; } = false;
        public bool LockingSafety { get; set; } = false;
        public bool AggressiveDogsBreed { get; set; } = false;
        public bool IsCompany { get; set; } = false;
        public bool OtherLocations { get; set; } = false;
        public string ScheduleDate { get; set; } = "";
        public string ScheduleTime { get; set; } = "";
        public int SusLoss3Yrs { get; set; } = 0;
        public int NumOfLosses { get; set; } = 0;
        public int HasOpenClaims { get; set; } = 0;
        public string BranchOfService { get; set; } = "";
        public string MilitaryStatus { get; set; } = "";
        public string MilitaryRank { get; set; } = "";
        public string MaritalStatus { get; set; } = "";
        public string Salutation { get; set; } = "";
        public string Suffix { get; set; } = "";
        public string SpsSalutation { get; set; } = "";
        public string SpsSuffix { get; set; } = "";
        public bool IsRankAsSalutation { get; set; } = false;

        // Default
        public int IdQuote { get; set; } = 0;
        public int InsIdMaritalStatus { get; set; } = 0;
        public int InsIdRatedMaritalStatus { get; set; } = 0;
        public int InsIdBranchOfService { get; set; } = 0;
        public int InsIdMilitaryStatus { get; set; } = 0;
        public int InsIdRank { get; set; } = 0;
        public string LOB { get; set; } = "";
        public int CreatedBy { get; set; } = 0;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int ModifiedBy { get; set; } = 0;
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        public string QuoteNumber { get; set; } = "";
        public int QuoteValuation { get; set; } = 0;
        public int InsIdSpouseBrOfSer { get; set; } = 0;
        public int InsIdSpouseMilSts { get; set; } = 0;
        public int InsIdSpouseRank { get; set; } = 0;
        public int InsIdSpouseSuffix { get; set; } = 0;
        public string InsMailCounty { get; set; } = "";
        public int IdMailingState { get; set; } = 0;
        public string MailingStateName { get; set; } = "";
        public int InsMailCountry { get; set; } = 0;
        public string MailingCountryName { get; set; } = "";
        public string MailingCountryAbbrv { get; set; } = "";
        public string InsPayGrade { get; set; } = "";
        public string InsSpousePayGrade { get; set; } = "";
        public string ChipMemberId { get; set; } = "";
        public int IdSuffix { get; set; } = 0;
        public int AFI_COUNTRY_ID { get; set; } = 0;
        public int AFI_COUNTY_ID { get; set; } = 0;
        public int AFI_STATE_ID { get; set; } = 0;
        public int LOC_TYPE_ID { get; set; } = 0;
        public bool IsTestAccount { get; set; } = true;
        public int AmountofCoverageRequested { get; set; } = 0;
        public string OnePage { get; set; } = "";
        public bool QuoteStatus { get; set; } = true;
        public int Deductible { get; set; } = 0;
        public int PersonalLiabilityCoverageAmount { get; set; } = 0;
        public int MedicalPaymentstoOthersCoverageAmount { get; set; } = 0;
        public bool IsAppSubmit { get; set; } = true;
        public int CNTC_CONTACT_TYPE_ID { get; set; } = 0;
        public int CNTC_CLIENT_TYPE_ID { get; set; } = 0;
        public string SSN { get; set; } = "";
        public string SpouseSSN { get; set; } = "";
    }
    public class InsuranceApplication
    {

        public string Salutation { get; set; }
        public string InsFirstName { get; set; }
        public string InsLastName { get; set; }
        public string Suffix { get; set; }
        public string InsEmailAddress { get; set; }
        public string InsMailAddress1 { get; set; }
        public string InsMailAddress2 { get; set; }
        public string InsMailCity { get; set; }
        public string InsMailState { get; set; }
        public string InsMailZip { get; set; }
        public string InsDOB { get; set; }
        public string InsSocialSecNumber { get; set; }
        public string InsPhoneNumber { get; set; }
        public string MaritalStatus { get; set; }
        public string InsIdEligibility { get; set; }
        public string SpsSalutation { get; set; }
        public string InsSpouseFirstName { get; set; }
        public string InsSpouseLastName { get; set; }
        public string SpsSuffix { get; set; }
        public string InsSpouseDOB { get; set; }
        public string InsSpouseSocialSecNumber { get; set; }
        public string BranchOfService { get; set; }
        public string MilitaryStatus { get; set; }
        public string MilitaryRank { get; set; }
        public string IsRankAsSalutation { get; set; }
        public string OpCovOption { get; set; }
        public string InsCovToBegin { get; set; }
        public string CancDec5Yrs { get; set; }
        public string LockingSafety { get; set; }
        public string AggressiveDogsBreed { get; set; }
        public string SusLoss3Yrs { get; set; }
        public string NumOfLosses { get; set; }
        public string HasOpenClaims { get; set; }
        public string OtherLocations { get; set; }
        public string IsCompany { get; set; }
        public string IsBusinessOrRevenueOnPremises { get; set; }
        public string ScheduleDate { get; set; }
        public string ScheduleTime { get; set; }
        public string InsSpouceSocialSecNumber { get; set; }
        public string EligibilityMilitary { get; set; }
        public string CurrentMilitarySpouse { get; set; }
        public string Militarysalutation { get; set; }
        public string Spousesalutation { get; set; }
        public string MaritalStatusFirstName { get; set; }
        public string MaritalStatusLastName { get; set; }
        public string Suffixdetailsspouse { get; set; }
        public string MaritalStatusInformationBirthDate { get; set; }
        public string MaritalStatusssn { get; set; }
        public string SpousebranchOfService { get; set; }
        public string spousemilitaryStatus { get; set; }
        public string spousemilitaryRank { get; set; }
        public string currentmilitarysalutation { get; set; }

        public string CoverageType { get; set; }
        public string CoverageMonth { get; set; }
        public string CoverageYear { get; set; }

        public string Amount { get; set; }
        public bool IsPayment { get; set; }
        public string PaymentPlan { get; set; }
        public string token { get; set; }
        public string digiSign { get; set; }
        public string QuoteId { get; set; }
        public string RiskAddressId { get; set; }
    }

    public class CertificateRenter
    {
        public int IdRiskAddreess { get; set; }

        public string CompletionDate { get; set; }

        public string DateOfIssue { get; set; }

        public string MemberNumber { get; set; }

        public string PolicyNumber { get; set; }

        public string EffectiveDate { get; set; }

        public string TermDate { get; set; }

        public string PrimaryName { get; set; }

        public string PrimaryStreet1 { get; set; }

        public string PrimaryStreet2 { get; set; }

        public string PrimaryCity { get; set; }

        public string PrimaryStateAbrev { get; set; }

        public string PrimaryZip { get; set; }

        public string AdditionalInsuredName { get; set; }

        public string AdditionalInsuredStreet1 { get; set; }

        public string AdditionalInsuredStreet2 { get; set; }

        public string AdditionalInsuredCity { get; set; }

        public string AdditionalInsuredStateAbrev { get; set; }

        public string AdditionalInsuredZip { get; set; }

        public string EachOccurrenceAmount { get; set; }

        public string DamageToPremisesAmount { get; set; }

        public string MedExpAmount { get; set; }

        public string PersAdvInjuryAmount { get; set; }

        public string GenAggregateAmount { get; set; }

        public string CompOpAggAmount { get; set; }

        public string PersonalPropertyLimit { get; set; }
    }

}