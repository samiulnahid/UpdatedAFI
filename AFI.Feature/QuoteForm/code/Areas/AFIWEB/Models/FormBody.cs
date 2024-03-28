using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{

    public class CustomFieldList
    {
        public string ListName { get; set; }
        public List<CustomField> Fields { get; set; }

    }
    public class CustomField
    {
        public string Name { get; set; }
        public string CustomFieldType { get; set; }
        public string Options { get; set; } = string.Empty;

    }
    public class EmailListData
    {
        public int Id { get; set; }
        public string ListId { get; set; }
        public string ListName { get; set; }
        public string SecurityKey { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    

    public class Status
    {
        public LogType LogType { get; set; }
    }

    public enum LogType
    {
        Subscriber,
        EmailList,
        SecurityKey
    }
}