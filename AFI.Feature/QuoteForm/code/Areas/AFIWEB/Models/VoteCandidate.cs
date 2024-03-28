using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class VoteCandidate
    {
        public int CandidateId { get; set; }
        public int VotingPeriodId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        [NotMapped]
        public string VotingPeriodName
        {
            get; set;
        }
        public bool IsActive { get; set; }
    }
}
