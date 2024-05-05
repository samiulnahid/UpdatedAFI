using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
	public class VotingMemberCSV
	{

		public int MemberId { get; set; }
		public string MemberNumber { get; set; }
		public string PIN { get; set; }
		public int VotingPeriodId { get; set; }
		public string MarketingCode { get; set; }
		public string Salutation { get; set; }
		public string Prefix { get; set; }
		public string ProperFirstName { get; set; }
		public string InsuredFirstName { get; set; }
		public string MiddleName { get; set; }
		public string InsuredLastName { get; set; }
		public string Suffix { get; set; }
		public string ServiceStatus { get; set; }
		public string MailingAddressLine1 { get; set; }
		public string MailingAddressLine2 { get; set; }
		public string MailingCityName { get; set; }
		public string MailingStateAbbreviation { get; set; }
		public string MailingZip { get; set; }
		public string MailingCountry { get; set; }
		public string EmailAddress { get; set; }

		public string ClientType { get; set; }
		public string MailingCountyName { get; set; }
		public string MembershipDate { get; set; }
		public string YearsAsMember { get; set; }
		public string Gender { get; set; }

		public string VotingPeriod { get; set; }
		public bool IsActive { get; set; }
		public bool IsEmailUpdated { get; set; }

	}
}