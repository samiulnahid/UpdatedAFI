using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteContact
	{
		public int Key { get; set; }
		public byte[] changeset { get; set; }
		public string FirstName { get; set; }
		public string MiddleInitial { get; set; }
		public string LastName { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string PhoneType { get; set; }
		public string HowToContact { get; set; }
		public DateTime? BirthDate { get; set; }
		public string Gender { get; set; }
		public string SSN { get; set; }
		public bool SpouseOfMilitary { get; set; }
		public bool AFIMember { get; set; }
		public string AFIMemberLength { get; set; }
		public string ServiceBranch { get; set; }
		public string ServiceRank { get; set; }
		public string ServiceStatus { get; set; }
		public string ServiceDischargeType { get; set; }
		public string CommissioningProgram { get; set; }
		public string EmploymentStatus { get; set; }
		public string MaritalStatus { get; set; }
		public string AFIExistingPolicyType { get; set; }
		public string ResidenceStatus { get; set; }
		public bool WantToReceiveInfo { get; set; }
		public string HowDidYouHearAboutUs { get; set; }
		public int? CNTCGroupID { get; set; }
		public string Suffix { get; set; }
		public string Prefix { get; set; }
		public string InsuredParent { get; set; }
		public string PropertyStreet { get; set; }
		public string PropertyCity { get; set; }
		public string PropertyState { get; set; }
		public string PropertyZipCode { get; set; }
		public string ServiceSpouseFirstName { get; set; }
		public string ServiceSpouseLastName { get; set; }
		public DateTime? SpouseBirthDate { get; set; }
		public string SpouseSSN { get; set; }
		public string SpouseFirstName { get; set; }
		public string SpouseLastName { get; set; }
		public string SpouseGender { get; set; }
		public string SpouseSuffix { get; set; }
		public bool CallForReview { get; set; }
		public string ReviewPhoneNum { get; set; }
		public decimal? CNTCLegacyNum { get; set; }
		public string CNTCLegacySuffix { get; set; }
		public string FirstCommandAdvisorName { get; set; }
		public bool UnderMoratorium { get; set; }
        public string ReviewEmail { get; set; }
        
	}
}