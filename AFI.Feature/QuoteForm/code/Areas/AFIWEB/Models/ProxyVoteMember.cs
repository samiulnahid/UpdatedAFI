using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
	public class ProxyVoteMember
	{

		public int MemberId { get; set; }
		public string MemberNumber { get; set; }
		public string PIN { get; set; }
		public int VotingPeriodId { get; set; }		
		public bool Enabled { get; set; }
		public string EmailAddress { get; set; }
		public string FullName { get; set; }
		public bool ResidentialOccupied { get; set; }
		public bool ResidentialDwelling { get; set; }
		public bool Renters { get; set; }
		public bool Flood { get; set; }
		public bool Life { get; set; }
		public bool PersonalLiabilityRenters { get; set; }
		public bool PersonalLiabilityCatastrophy { get; set; }
		public bool Auto { get; set; }
		public bool RV { get; set; }
		public bool Watercraft { get; set; }
		public bool Motorcycle { get; set; }
		public bool Supplemental { get; set; }
		public bool AnnualReport { get; set; }
		public bool StatutoryFinancialStatements { get; set; }
	
		
		public bool MobileHome { get; set; }
		public bool PetHealth { get; set; }
		public bool Business { get; set; }
		public bool LongTermCare { get; set; }
		public bool MailFinancials { get; set; }
		public bool EmailFinancials { get; set; }

		public bool IsEmailUpdated { get; set; }

		public string Prefix { get; set; }
		public string Salutation { get; set; }
		public string InsuredFirstName { get; set; }
		public string InsuredLastName { get; set; }
		public string ClientType { get; set; }
		public string ServiceStatus { get; set; }
		public string MailingAddressLine1 { get; set; }
		public string MailingAddressLine2 { get; set; }
		public string MailingCityName { get; set; }
		public string MailingCountyName { get; set; }
		public string MailingStateAbbreviation { get; set; }
		public string MailingZip { get; set; }
		public string MailingCountry { get; set; }
		public string MembershipDate { get; set; }
		public string YearsAsMember { get; set; }
		public string Gender { get; set; }
		public string Deceased { get; set; }
		
		public string MarketingCode { get; set; }
		public string ProperFirstName { get; set; }
		public string MiddleName { get; set; }
		public string Suffix { get; set; }
		public DateTime CreateDate { get; set; } = DateTime.Now;


	}
}