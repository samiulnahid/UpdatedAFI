using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    [DataContract]
    public class ReCaptchaVerifyResponse
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "challenge_ts")]
        public string Timestamp { get; set; }

        [DataMember(Name = "hostname")]
        public string Hostname { get; set; }

        [DataMember(Name = "error-codes")]
        public IEnumerable<string> ErrorCodes { get; set; } = new string[0];
    }
}