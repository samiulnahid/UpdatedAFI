﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Models
{
    public class VoteReport
    {
        public string MemberId { get; set; }
        public string CandidateId { get; set; }
        public string CandidateName { get; set; }
        public string VoteFor { get; set; }
        public string VoteAgainst { get; set; }
    }
}