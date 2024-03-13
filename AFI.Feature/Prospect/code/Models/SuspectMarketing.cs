using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.Prospect.Models
{
    public class SuspectMarketing
    {
        public int SuspectID { get; set; }
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        public string Email { get; set; } = null;
        public string Phone { get; set; } = null;
        public string Address { get; set; } = null;
        public string City { get; set; } = null;
        public string State { get; set; } = null;
        public string ZipCode { get; set; } = null;
        public string Country { get; set; } = null;
        public DateTime? DateOfBirth { get; set; }
        public string Occupation { get; set; } = null;
        public string PreferredCoverage { get; set; } = null;
        public string LeadSource { get; set; } = null;
        public string LeadStatus { get; set; } = null;
        public string LeadOwner { get; set; } = null;
        public int LeadScore { get; set; } = 0;
        public DateTime? DateCreated { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsSynced { get; set; } = false;
        public int TotalCount { get; set; }

    }
}