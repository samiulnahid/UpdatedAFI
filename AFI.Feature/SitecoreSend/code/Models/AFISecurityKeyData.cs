using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.SitecoreSend.Models
{
    public class AFISecurityKeyData
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string SecurityKey { get; set; }
        public DateTime StateTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}