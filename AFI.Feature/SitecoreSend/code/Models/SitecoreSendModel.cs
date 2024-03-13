using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.SitecoreSend.Models
{
    public class SitecoreSendModel
    {

        public string Email { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public List<string> CustomFields { get; set; }


    }
    public class CustomFieldMarketing
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MemberNumber { get; set; }
    }
}