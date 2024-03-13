using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.SitecoreSend.Models
{
    public class MoosendResponseModel
    {
        public string Code { get; set; }
        public string Error { get; set; }
        public string Context { get; set; }

        public override string ToString()
        {
            // Customize the string representation as needed
            return $"Code: {Code}, Error: {Error}, Context: {Context}";
        }
    }

    public class ResponseModelSecurity
    {
        public string Code { get; set; }
        public string Token { get; set; }

        public string Message { get; set; }
        public override string ToString()
        {
            // Customize the string representation as needed
            return $"Code: {Code}, Message: {Message}";
        }
    }
    public class ResponseModel
    {

        public string Code { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            // Customize the string representation as needed
            return $"Code: {Code}, Message: {Message}";
        }
    }

    public class SingleApiResponse
    {
        public int Code { get; set; }
        public string Error { get; set; }
        public SubscriberData Context { get; set; }
    }
    public class ApiResponse
    {
        public int Code { get; set; }
        public string Error { get; set; }
        public List<SubscriberData> Context { get; set; }
    }

    public class SubscriberData
    {
        public string ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? UnsubscribedOn { get; set; }
        public string UnsubscribedFromID { get; set; }
        public int SubscribeType { get; set; }
        public int SubscribeMethod { get; set; }
        public List<CustomFieldResponse> CustomFields { get; set; }
        public DateTime? RemovedOn { get; set; }
        public List<string> Tags { get; set; }
    }

    public class CustomFieldResponse
    {
        public string CustomFieldID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

}