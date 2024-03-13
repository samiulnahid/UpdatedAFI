using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers.Configurations
{
    public static class GlobalConfigurations
    {
        public const string DefaultDateformat = "dd MMMM yyyy";
    }
    public static class ArticleRoot
    {
        public const string RootID = "{0CA10DE5-24EC-4EE0-93B9-5F005782EB8A}";
        public const string ArticleTemplateID = "{8AF6F82A-CB36-4F03-9A04-60C519503C7A}";
        public const string CategoryTemplateID = "{4534CF7B-F692-487E-8987-8430AD2806ED}";
    }

    public static class InternalUrl
    {
        public const string SAC_Vote_ID = "{CDA8AFBB-E4E9-443B-A738-5BE1FB2AB353}";
        //public const string Vote_step_one_ID = "{CDA8AFBB-E4E9-443B-A738-5BE1FB2AB353}";
        public const string Vote_step_two_ID = "{4EF65690-22F9-46DC-A73C-C20CAC5B4955}";
        public const string Vote_Thanks_ID = "{B58F18F9-1A33-40A2-85B0-FB6250099625}";
        public const string Vote_Message_ID = "{93DBD984-3C2A-48FE-AD8E-F917302649B7}";
    }
    public static class ProxyVoteMail
    {
        public const string Email_TemplateId = "{42F53626-FD46-4C75-AEC0-479DCF076551}";
        public const string Email_ItemId = "{4472D71A-7E21-4441-B6B8-4B3ADA44176E}";
        public const string Email_Subject = "Email Subject";
        public const string Email_Recipients = "Email Recipients";
        public const string Email_Sender = "Email Sender";
        public const string Email_Body = "Email Body";
    }
    public static class SC
    {
        public const string Request_Info_Email_ItemId = "{94C04C36-FF9A-4E42-8F8A-10F5988453DA}";
        public const string Email_Subject = "Email Subject";
        public const string Email_Recipients = "Email Recipients";
        public const string Email_Sender = "Email Sender";
        public const string Email_Body = "Email Body";
    }
    public static class LeadGenMail
    {
        public const string Email_ItemId = "{B70BF58B-ED0C-4FBF-AD6F-73585FE21679}";
        public const string Admin_Email_Subject = "Admin Email Subject";
        public const string Admin_Email_Recipients = "Admin Email Recipients";
        public const string Admin_Email_Sender = "Admin Email Sender";
        public const string Admin_Email_Body = "Admin Email Body";
        public const string Client_Confirmation_Email_Subject = "Client Confirmation Email Subject";
        public const string Client_Confirmation_Email_Sender = "Client Confirmation Email Sender";
        public const string Client_Confirmation_Email_Body = "Client Confirmation Email Body";
    }

    public static class CorviasMail
    {
        public const string Email_ItemId = "{DE317619-5E56-4BAB-995E-6A897BEFC0A3}";
        public const string Admin_Email_Subject = "Admin Email Subject";
        public const string Admin_Email_Recipients = "Admin Email Recipients";
        public const string Admin_Email_Sender = "Admin Email Sender";
        public const string Admin_Email_Body = "Admin Email Body";
        public const string Client_Confirmation_Email_Subject = "Client Confirmation Email Subject";
        public const string Client_Confirmation_Email_Sender = "Client Confirmation Email Sender";
        public const string Client_Confirmation_Email_Body = "Client Confirmation Email Body";
    }
    public static class UCMail
    {
        public const string Email_ItemId = "{4F2E684A-BFB5-4505-A114-183A33DB5595}";
        public const string Admin_Email_Subject = "Admin Email Subject";
        public const string Admin_Email_Recipients = "Admin Email Recipients";
        public const string Admin_Email_Sender = "Admin Email Sender";
        public const string Admin_Email_Body = "Admin Email Body";
        public const string Client_Confirmation_Email_Subject = "Client Confirmation Email Subject";
        public const string Client_Confirmation_Email_Sender = "Client Confirmation Email Sender";
        public const string Client_Confirmation_Email_Body = "Client Confirmation Email Body";
    }
    public static class HillFormMail
    {
        public const string Email_ItemId = "{05BE2B75-DBFE-4233-8398-C2A263003EE1}";
        public const string Admin_Email_Subject = "Admin Email Subject";
        public const string Admin_Email_Recipients = "Admin Email Recipients";
        public const string Admin_Email_Sender = "Admin Email Sender";
        public const string Admin_Email_Body = "Admin Email Body";
        public const string Client_Confirmation_Email_Subject = "Client Confirmation Email Subject";
        public const string Client_Confirmation_Email_Sender = "Client Confirmation Email Sender";
        public const string Client_Confirmation_Email_Body = "Client Confirmation Email Body";
    }
    public static class ClaimSurveyMail
    {
        public const string Email_ItemId = "{C81CEEEE-A560-445A-9E66-CF394F299CD1}";
        public const string Subject = "Email Subject";
        public const string Recipients = "Email Recipients";
        public const string Sender = "Email Sender";
        public const string Body = "Email Body";
    }
    public static class ClaimSurveyForm
    {
        public const string ID = "{9D9BFAC4-4384-4A78-A022-DD789F154CDC}";
    }
    public static class Prefix_List_Location
    {
        public static readonly ID ID = new ID("{1F44539A-ABD7-46F5-80BF-5FAD9F35E3A8}");
    }
    public static class Referral_Form_Thanks_Location
    {
        public static readonly ID ID = new ID("{DD5B7A64-3AF9-4F0B-B03A-C57EC3163D72}");
        public const string ItemPath = "/referral-form-submission-page";
    }
    public static class Site_Seeting
    {
        public static readonly ID Thanks_Page = new ID("{2915B07F-7E2A-4C8C-A2F3-083BF1BCBBDD}");
        public static readonly ID Error_Page = new ID("{D144ECC1-9271-4FB2-84C8-7D22D4BA17C9}");

        public static readonly ID Global_Setting_Location = new ID("{B848F1F8-1014-46A3-A79F-85D4376C7553}");
        public static readonly ID Global_Setting_TemplateID = new ID("{D476BFED-304C-45C0-AA54-D28524B2129B}");
    }
}