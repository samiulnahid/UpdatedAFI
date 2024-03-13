using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteSpouse
	{
		public int Key { get; set; }
		public byte[] changeset { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool Officer { get; set; }
		public string ServiceBranch { get; set; }
		public string ServiceRank { get; set; }
		public string ServiceStatus { get; set; }
		public string ServiceDischargeType { get; set; }
		public string CommissioningProgram { get; set; }
		public string Suffix { get; set; }

	}
}