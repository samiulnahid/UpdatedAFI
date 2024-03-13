using Sitecore.Data;
using Sitecore.Shell.Applications.ContentEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.Prospect.Constants
{
    public class Template
    {
        public struct MarketingProspectSetting
        {
            public static readonly ID TemplateId = new ID("{A16C4825-2921-4182-8F7D-BA87157F30A9}");
            public const string MarketingProspect_ItemId = "{4FF4B875-EE3C-4FDE-A93B-A963CF74894A}";
            public struct Fields
            {
                public static readonly ID EmailSubject = new ID("{DF22F425-B802-4DF2-8D83-0B74C6D8E6F3}");
                public static readonly ID EmailRecipients = new ID("{A3C780D9-8BE2-486E-8CC5-3E25E5ABCDC7}");
                public static readonly ID EmailSender = new ID("{0724F54A-1F1C-4FDF-87D5-774B0322254B}");
                public static readonly ID EmailBody = new ID("{F51D98FC-BBF4-46AD-AF8C-C14AEF471AED}");
            }
        }
        public static class MarketingProspectMail
        {
            public const string Email_ItemId = "{4FF4B875-EE3C-4FDE-A93B-A963CF74894A}";
            public const string Subject = "Email Subject";
            public const string Recipients = "Email Recipients";
            public const string Sender = "Email Sender";
            public const string Body = "Email Body";
        }

    }
}