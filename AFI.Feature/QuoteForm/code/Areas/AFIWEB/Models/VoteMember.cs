using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class VoteMember
    {
        public int MemberId { get; set; }
        public string MemberNumber { get; set; }
        public string PIN { get; set; }
        public int VotingPeriodId { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public int TotalCount { get; set; }
        public string VotingPeriod { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailUpdated {  get; set; }
    }
    public class MemberPagination
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItem { get; set; }
        public bool Success { get; set; } = true;
        public IEnumerable<VoteMember> MemberList { get; set; }
    }
}