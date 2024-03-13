using System;

namespace AFI.Feature.WebQuoteService.Models
{
    public class AFIForm
    {
        public int? CNTCGroupId { get; set; }
        public string QuoteId { get; set; }
        public int Priority { get; set; }
        public string StateAbbrv { get; set; }
        public bool Expidite { get; set; }
        public string DisplayInformation { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreationUser { get; set; }
        public string UpdateUser { get; set; }
        public string Email { get; set; }
        public string Eligibility { get; set; }
        public bool CallForReview { get; set; }
        public bool CurrentlyAutoInsured { get; set; }
        public bool IsFirstCommand { get; set; }
        public bool ReceivedPremium { get; set; }
        public bool IsFinished { get; set; }
        public bool IsMobileManufactured { get; set; }
        public bool IsSlt { get; set; }
        public bool IsRental { get; set; }
        public bool IsBusinessTest { get; set; } = true;
        public bool IsTest { get; set; } = true;
        public bool NeedsUnderwriting { get; set; }
        public bool IssuedOnline { get; set; }
        public bool ApplicationCompleted { get; set; }
        public bool IsInterested { get; set; }
        public bool IsCondo { get; set; }
        public bool IsVacant { get; set; }
        public int FormTypeId { get; set; }
        public int CoverageTypeId { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public bool IsPossibleDup { get; set; }
    }
}