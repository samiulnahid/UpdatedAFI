using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class ProxyVoteCandidate
    {
        public int CandidateId { get; set; }
        public int VotingPeriodId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public byte[] changeset { get; set; }
    }
}