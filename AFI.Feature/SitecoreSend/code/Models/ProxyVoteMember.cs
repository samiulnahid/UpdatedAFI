using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.SitecoreSend.Models
{
	
	public class ProxyVoteMember
	{
		public int MemberId { get; set; } 
		public string MemberNumber { get; set; } = string.Empty;
		public string PIN { get; set; } = string.Empty;
		public int VotingPeriodId { get; set; }
		public bool Enabled { get; set; } = false;
		public string EmailAddress { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;
		public bool ResidentialOccupied { get; set; } = false;
		public bool ResidentialDwelling { get; set; } = false;
		public bool Renters { get; set; } = false;
		public bool Flood { get; set; } = false;
		public bool Life { get; set; } = false;
		public bool PersonalLiabilityRenters { get; set; } = false;
		public bool PersonalLiabilityCatastrophy { get; set; } = false;
		public bool Auto { get; set; } = false;
		public bool RV { get; set; } = false;
		public bool Watercraft { get; set; } = false;
		public bool Motorcycle { get; set; } = false;
		public bool Supplemental { get; set; } = false;
		public bool AnnualReport { get; set; } = false;
		public bool StatutoryFinancialStatements { get; set; } = false;
		public bool MobileHome { get; set; } = false;
		public bool PetHealth { get; set; } = false;
		public bool Business { get; set; } = false;
		public bool LongTermCare { get; set; } = false;
		public bool MailFinancials { get; set; } = false;
		public bool EmailFinancials { get; set; } = false;
		public bool IsEmailUpdated { get; set; } = false;
		public string Prefix { get; set; } = string.Empty;
		public string Salutation { get; set; } = string.Empty;
		public string InsuredFirstName { get; set; } = string.Empty;
		public string InsuredLastName { get; set; } = string.Empty;
		public string ClientType { get; set; } = string.Empty;
		public string ServiceStatus { get; set; } = string.Empty;
		public string MailingAddressLine1 { get; set; } = string.Empty;
		public string MailingAddressLine2 { get; set; } = string.Empty;
		public string MailingCityName { get; set; } = string.Empty;
		public string MailingCountyName { get; set; } = string.Empty;
		public string MailingStateAbbreviation { get; set; } = string.Empty;
		public string MailingZip { get; set; } = string.Empty;
		public string MailingCountry { get; set; } = string.Empty;
		public string MembershipDate { get; set; } = string.Empty;
		public string YearsAsMember { get; set; } = string.Empty;
		public string Gender { get; set; } = string.Empty;
		public bool Deceased { get; set; } = false;

		public string MarketingCode { get; set; }
		public string ProperFirstName { get; set; }
		public string MiddleName { get; set; }
		public string Suffix { get; set; }

		public DateTime CreateDate { get; set; } = DateTime.Now;
	}
}