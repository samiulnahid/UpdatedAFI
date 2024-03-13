using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class ProxyVoteVotingPeriod
    {
		public int VotingPeriodId { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public byte[] changeset { get; set; }
	}
}