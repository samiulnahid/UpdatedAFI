using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;


namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class Email
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> To { get; set; } = new List<string>();
        public string ReplyTo { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public IEnumerable<string> Cc { get; set; } = new List<string>();
        public IEnumerable<string> Bcc { get; set; } = new List<string>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; } = new Attachment[0];
    }
}