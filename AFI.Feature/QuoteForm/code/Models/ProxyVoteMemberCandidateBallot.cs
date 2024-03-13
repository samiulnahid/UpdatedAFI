using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Models
{
    public class ProxyVoteMemberCandidateBallot
    {
		public int MemberId { get; set; }
		public int CandidateId { get; set; }
		public int VotingPeriodId { get; set; }
		public string Ballot { get; set; }
		public DateTime DateTimeCast { get; set; }
	}
}