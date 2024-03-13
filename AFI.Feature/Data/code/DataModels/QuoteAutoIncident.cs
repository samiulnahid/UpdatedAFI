using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteAutoIncident
	{
		public int Key { get; set; }
		public int QuoteKey { get; set; }
		public int? DriverKey { get; set; }
		public string Incident { get; set; }
		public DateTime? Date { get; set; }
		public byte[] changeset { get; set; }
		public string DriverName { get; set; }

	}
}