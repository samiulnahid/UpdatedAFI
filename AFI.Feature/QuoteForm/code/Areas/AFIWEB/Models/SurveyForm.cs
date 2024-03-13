using System;
using System.Collections.Generic;
namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class SurveyForm
    {
        public string id { get; set; }
        public string MemberNumber { get; set; }
        public string ClaimNumber { get; set; }
        public string ClientTypeDesc { get; set; }
        public string Salutation { get; set; }
        public string Suffix { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ClaimClosedDate { get; set; }
        public string ClaimRunDate { get; set; }
        public int MaxSelectionScale { get; set; } = 0;
        public string Q_1 { get; set; }
        public string Q_2 { get; set; }
        public string Q_3 { get; set; }
        public string Q_4 { get; set; }
        public string Q_1_Txt { get; set; }
        public string Q_2_Txt { get; set; }
        public string Q_3_Txt { get; set; }
        public string Q_4_Txt { get; set; }
        public string CommentsLabel { get; set; }
        public string Comments { get; set; }
        public string DateSurveySent { get; set; }
        public string DateResponseReceived { get; set; }
        public bool IsSubmitted { get; set; } = false;
        public string CreateDate { get; set; }
        public List<Satisfaction> Satisfactions { get; set; }
    }
    public class Satisfaction
    {
        public string Name { get; set; }
        public int ScaleForm { get; set; } = 0;
        public int ScaleTo { get; set; } = 0;
    }
    public class ClaimSent
    {
        public string ClaimNumber { get; set; }
        public DateTime Datesurveysent { get; set; } = DateTime.Now;
    }
    public class LogMail
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string ClaimNumber { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From_Email { get; set; }
        public string To_Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Source { get; set; }
    }
}