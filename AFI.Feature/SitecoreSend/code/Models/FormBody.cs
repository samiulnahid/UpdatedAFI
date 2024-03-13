using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.SitecoreSend.Models
{

    public class CreateSubscribersRequest
    {
        public string ListName { get; set; }
        public string Source { get; set; }
        public List<Subscriber> Subscribers { get; set; }
    }

    public class Subscriber
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public Dictionary<string, string> CustomFields { get; set; }
    }

    public class SubscriberCustomField
    {
        public string Salutation { get; set; }
        public string Suffix { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MailAddress1 { get; set; }
    }
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
    public class MoosendListSubscriber
    {
        public int Id { get; set; }
        public int EmailListId { get; set; }
        public string SendListId { get; set; }
        public string ListName { get; set; }
        public string SubscriberId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string JsonBody { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsSynced { get; set; } = false;
        public DateTime? SyncedTime { get; set; }
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