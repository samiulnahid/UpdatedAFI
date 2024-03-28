using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class VotingPeriod
    {
        public int VotingPeriodId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }

    }
}
