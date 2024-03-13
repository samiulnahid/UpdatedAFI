using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Models
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
	}
}