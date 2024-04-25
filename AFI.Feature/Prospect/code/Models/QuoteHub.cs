using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.Prospect.Models
{
    public class QuoteHub
    {
        public int IdQuote { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string MailingAddr1 { get; set; }
        public string MailingAddr2 { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingZip { get; set; }
        public int ChipMemberId { get; set; }
        public string EmailId { get; set; }
        public string LOB { get; set; }
        public int IdSuffix { get; set; }
        public int CreatedBy { get; set; } = 1;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string FNameSoundex { get; set; }
        public string LNameSoundex { get; set; }
        public string UserType { get; set; }
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public string MailingCounty { get; set; }
        public int IdMailingState { get; set; }
        public string MailingStateName { get; set; }
        public int IdMailingCountry { get; set; }
        public string MailingCountryName { get; set; }
        public string MailingCountryAbbrv { get; set; }
        public string QuoteCreatedBy { get; set; }
        public string QuoteModifiedBy { get; set; }
        public bool IsTestAccount { get; set; } = false;
        public int IdPrimarySalutation { get; set; }
        public char PrimaryGender { get; set; }
        public string OnePage { get; set; }
    }
}