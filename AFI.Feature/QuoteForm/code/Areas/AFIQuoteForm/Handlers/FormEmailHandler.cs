using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers.Strings;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Services;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using Identifiers =AFI.Feature.Identifiers;
//using Microsoft.Ajax.Utilities;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers
{
    public interface IFormEmailHandler
    {
        void HandleSave(List<KeyValuePair<string, string>> saveMapping, string emailRecipient, string coverageType);
        void HandleSubmit(List<KeyValuePair<string, string>> submitMapping, string emailRecipient, string coverageType);
        #region For Affinity Base Housing
        void HillHandleSubmit(List<KeyValuePair<string, string>> submitMapping, string emailRecipient, string coverageType);
        #endregion

        Email ParseEmail(KeyValuePair<string, string>[] mappingKeys, string commaSeparatedRecipientEmails, string commaSeparatedSenderEmails, string globalSettingSubjectItemId, string globalSettingBodyItemId);

        void QuoteleadformSubmit(List<KeyValuePair<string, string>> submitMapping, string emailRecipient, string coverageType);

    }

    public class FormEmailHandler : IFormEmailHandler
    {
        private readonly IGlobalSettingsRepository _globalSettingsRepository;
        private readonly IEmailService _emailService;

        public FormEmailHandler(IGlobalSettingsRepository globalSettingsRepository, IEmailService emailService)
        {
            _globalSettingsRepository = globalSettingsRepository;
            _emailService = emailService;
        }

        public void HandleSave(List<KeyValuePair<string, string>> saveMapping, string emailRecipient, string coverageType)
        {

            var email = ParseEmail(saveMapping, Identifiers.Templates.Global_Settings.FieldNames.Quote_Form_Saved_Email_From_List, Identifiers.Templates.Global_Settings.FieldNames.Quote_Form_Saved_Email_Subject, Identifiers.Templates.Global_Settings.FieldNames.Quote_Form_Saved_Email_Body, emailRecipient, coverageType);
            _emailService.Send(email);
        }

        public void HandleSubmit(List<KeyValuePair<string, string>> submitMapping, string emailRecipient, string coverageType)
        {
            var email = ParseEmail(submitMapping, Identifiers.Templates.Global_Settings.FieldNames.Quote_Form_Submitted_Email_From_List, Identifiers.Templates.Global_Settings.FieldNames.Quote_Form_Submitted_Email_Subject, Identifiers.Templates.Global_Settings.FieldNames.Quote_Form_Submitted_Email_Body, emailRecipient, coverageType);
            _emailService.Send(email);
        }
        #region For Affinity Base Housing
        public void HillHandleSubmit(List<KeyValuePair<string, string>> submitMapping, string emailRecipient, string coverageType)
        {  
            Sitecore.Data.Items.Item emailtemplate = Sitecore.Context.Database.GetItem(Constants.Email_ItemId);
            // EMAIL Quote Submitter
            string confirmation_Email_Subject = emailtemplate[Constants.Confirmation_Email_Subject];
            string confirmation_Email_Sender = emailtemplate[Constants.Confirmation_Email_Sender];
            string email_Recipients = emailtemplate[Constants.Email_Recipients];
            string confirmation_Email_Body = emailtemplate[Constants.Confirmation_Email_Body];

            var email = HillParseEmail(submitMapping, confirmation_Email_Sender, confirmation_Email_Subject, confirmation_Email_Body, emailRecipient, coverageType);
            _emailService.Send(email);

            // EMAIL SALES AFI
            //string email_Subject = emailtemplate[Constants.Email_Subject];            
            //string email_Recipients = emailtemplate[Constants.Email_Recipients];
            //string email_Sender = emailtemplate[Constants.Email_Sender];
            //string email_Body = emailtemplate[Constants.Email_Body];
            //var email_sales = HillParseEmail(submitMapping, email_Sender, email_Subject, email_Body, email_Recipients, coverageType);
            //_emailService.Send(email_sales);
        }
        private Email HillParseEmail(List<KeyValuePair<string, string>> mapping, string emailFromList, string emailSubject, string body, string emailRecipient, string coverageType)
        {
            Regex re = new Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled);
            IDictionary<string, string> mappingKeys = new Dictionary<string, string>();
            mapping.ForEach(c => mappingKeys.Add(c));
            mappingKeys.Add("coverageType", coverageType);
            string bodyParsed = re.ReplaceTokens(body, mappingKeys);
            string subjectParsed = re.ReplaceTokens(emailSubject, mappingKeys);
            return new Email
            {
                IsBodyHtml = true,
                To = new List<string> { emailRecipient },
                Subject = subjectParsed,
                Body = bodyParsed,
                FromEmail = emailFromList
            };
        }
        #endregion
        private Email ParseEmail(List<KeyValuePair<string, string>> mapping, string fromListItemID, string subjectItemID, string bodyItemID, string emailRecipient, string coverageType)
        {
            string emailFromList = _globalSettingsRepository.GetSetting(fromListItemID);
            string emailSubject = _globalSettingsRepository.GetSetting(subjectItemID);
            string body = _globalSettingsRepository.GetSetting(bodyItemID);
            Regex re = new Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled);
            IDictionary<string, string> mappingKeys = new Dictionary<string, string>();
            mapping.ForEach(c => mappingKeys.Add(c));
            mappingKeys.Add("coverageType", coverageType);
            string bodyParsed = re.ReplaceTokens(body, mappingKeys);
            string subjectParsed = re.ReplaceTokens(emailSubject, mappingKeys);
            return new Email
            {
                IsBodyHtml = true,
                To = new List<string> { emailRecipient },
                Subject = subjectParsed,
                Body = bodyParsed,
                FromEmail = emailFromList
            };
        }


        public Email ParseEmail(KeyValuePair<string, string>[] mappingKeys, string commaSeparatedRecipientEmails, string commaSeparatedSenderEmails, string globalSettingSubjectItemId, string globalSettingBodyItemId)
        {
            string emailFromList = commaSeparatedSenderEmails;
            string emailSubject = _globalSettingsRepository.GetSetting(globalSettingSubjectItemId);
            string body = _globalSettingsRepository.GetSetting(globalSettingBodyItemId);
            string emailTo = commaSeparatedRecipientEmails;
            Regex re = new Regex(@"(?i)\{\{(\w+)\}\}", RegexOptions.Compiled);
            IDictionary<string, string> mappings = new Dictionary<string, string>();
            mappingKeys.ForEach(c => mappings.Add(c));
            string bodyParsed = re.ReplaceTokens(body, mappings);
            return new Email
            {
                IsBodyHtml = true,
                To = emailTo?.Split(','),
                Subject = emailSubject,
                Body = bodyParsed,
                FromEmail = emailFromList
            };
        }

        private Email ParseEmail(KeyValuePair<string, string>[] mapping, string globalSettingFromEmailId, string globalSettingSubjectItemId, string globalSettingBodyItemId)
        {
            string emailFromList = _globalSettingsRepository.GetSetting(globalSettingFromEmailId);
            string emailSubject = _globalSettingsRepository.GetSetting(globalSettingSubjectItemId);
            string body = _globalSettingsRepository.GetSetting(globalSettingBodyItemId);
            string emailTo = mapping.FirstOrDefault(c => c.Key == "email").Value;
            return ParseEmail(mapping, emailTo, emailFromList, emailSubject, body);
        }

        public void QuoteleadformSubmit(List<KeyValuePair<string, string>> submitMapping, string emailRecipient, string coverageType)
        {
            Sitecore.Data.Items.Item emailtemplate = Sitecore.Context.Database.GetItem(QuoteleadformConstants.QuoteLeadFormEmail_Email_ItemId);
            // EMAIL Quote Submitter
            string confirmation_Email_Subject = emailtemplate[QuoteleadformConstants.Confirmation_Email_Subject];
            string confirmation_Email_Sender = emailtemplate[QuoteleadformConstants.Confirmation_Email_Sender];
            string confirmation_Email_Body = emailtemplate[QuoteleadformConstants.Confirmation_Email_Body];

            var email = QuoteleadformParseEmail(submitMapping, confirmation_Email_Sender, confirmation_Email_Subject, confirmation_Email_Body, emailRecipient);
            _emailService.Send(email);

            // EMAIL SALES AFI
            string email_Subject = emailtemplate[Constants.Email_Subject];
            string email_Recipients = emailtemplate[QuoteleadformConstants.Email_Recipients];
            string email_Sender = emailtemplate[Constants.Email_Sender];
            string email_Body = emailtemplate[Constants.Email_Body];
            if (!string.IsNullOrEmpty(email_Recipients))
            {
                var email_sales = HillParseEmail(submitMapping, email_Sender, email_Subject, email_Body, email_Recipients, coverageType);
                _emailService.Send(email_sales);
            }

        }
        private Email QuoteleadformParseEmail(List<KeyValuePair<string, string>> mapping, string emailFromList, string emailSubject, string body, string emailRecipient)
        {
            Regex re = new Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled);
            IDictionary<string, string> mappingKeys = new Dictionary<string, string>();
            mapping.ForEach(c => mappingKeys.Add(c));
            string bodyParsed = re.ReplaceTokens(body, mappingKeys);
            string subjectParsed = re.ReplaceTokens(emailSubject, mappingKeys);
            return new Email
            {
                IsBodyHtml = true,
                To = new List<string> { emailRecipient },
                Subject = subjectParsed,
                Body = bodyParsed,
                FromEmail = emailFromList
            };
        }
    }
}