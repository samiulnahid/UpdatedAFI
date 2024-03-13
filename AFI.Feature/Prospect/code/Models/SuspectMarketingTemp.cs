using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.Prospect.Models
{
    public class SuspectMarketingTemp
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Occupation { get; set; }
        public string PreferredCoverage { get; set; }
        public string LeadSource { get; set; }
        public string LeadStatus { get; set; }
        public string LeadOwner { get; set; }
        public int LeadScore { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string EntityType { get; set; }
        public int EntityID { get; set; }
        public bool IsSynced { get; set; } = false;
        public bool IsValid { get; set; } = true;
        public bool IsBlockCountry { get; set; } = false;
        public int TotalCount { get; set; } 

    }
}