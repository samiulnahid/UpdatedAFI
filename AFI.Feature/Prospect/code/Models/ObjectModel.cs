using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.Prospect.Models
{
    public class ObjectModel
    {
        public string DataSource { get; set; }
        public string DbName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Query { get; set; }
        public string Email { get; set; }

    }
}