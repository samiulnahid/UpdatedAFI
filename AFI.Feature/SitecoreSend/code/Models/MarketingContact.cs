using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.SitecoreSend.Models
{
    public class MarketingContact
    {
        public string OPID { get; set; }
        public string Title { get; set; }
        public string Extension { get; set; }
        public string Department { get; set; }
        public string MemberNumber { get; set; }
        public string ContactID { get; set; }
        public string AFIConnectStatus { get; set; }

        public string CarrierName { get; set; }
        public string CarrierServiceBillingPhone { get; set; }
        public string CarrierClaimsPhone { get; set; }
        public string CarrierRoadsidePhone { get; set; }
        public string CarrierURL { get; set; }
        public string CarrierURLUserFriendly { get; set; }
        public string PolicyNumber { get; set; }
        public string LineofBusiness { get; set; }
        public string CarrierCode { get; set; }
        public string IssuingAgent { get; set; }
        public string WhenBoundDate { get; set; }
        public string PolicyEffectiveDate { get; set; }
        public string PolicyCarrier { get; set; }
        public string PhoneNumber { get; set; }
        public string Salutation { get; set; }

        public string MembershipDate { get; set; }
        public string DoNotSolicitYN { get; set; }
        public string DOB { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AgentTitle { get; set; }
        public string AgentName { get; set; }
        public string AgentEmail { get; set; }
        public string AgentText { get; set; }
        public string AgentPhone { get; set; }
        public string AgentExt { get; set; }
        public string ConditionalSalutationOrFirstName { get; set; }
        public string UnsubscriptionUrl { get; set; }
        public string ListId { get; set; }
        public string MemberBusname { get; set; }
        public string MemberAddr { get; set; }
        public string MemberCity { get; set; }
        public string MemberState { get; set; }
        public string MemberPostalCode { get; set; }
        public string ConditionalAFIConnect { get; set; }
        public string ConditionalContactList { get; set; }
        public string EXMDomainUrl { get; set; }
        public string SocialFB { get; set; }
        public string SocialTW { get; set; }
        public string SocialLN { get; set; }
        public string ServiceEmail { get; set; }
        public string DeliveryEmail { get; set; }
        public string PrivacyUrl { get; set; }
        public string TermsUrl { get; set; }
        public string ReferralUrl { get; set; }
        public string FeedbackUrl { get; set; }
    }
}