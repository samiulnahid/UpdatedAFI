using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteAutoDriver
	{
		public int Key { get; set; }
		public int QuoteKey { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MaritalStatus { get; set; }
		public DateTime? BirthDate { get; set; }
		public string Gender { get; set; }
		public int? AgeLicensed { get; set; }
		public byte[] changeset { get; set; }
		public string SSN { get; set; }
		public bool SafetyCourse { get; set; }
		public bool HouseholdMovingViolation { get; set; }
		public int? ExperienceYears { get; set; }
		public bool GoodStudentDiscount { get; set; }
        public string Occupation { get; set; }
        public string Education { get; set; }
	}
}