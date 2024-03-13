using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Services;
using AFI.Feature.QuoteForm.Areas.ShortForms.Models;

namespace AFI.Feature.QuoteForm.Areas.ShortForms.Repository
{
    public interface IShortFormRepository
    {
        void SubmitForm(ShortFormModel model);
    }

    public class ShortFormRepository : IShortFormRepository
    {
        private readonly IFormEmailHandler _emailHandler;
        private readonly IEmailService _emailService;
        private readonly IGlobalSettingsRepository _globalSettings;

        public ShortFormRepository(IFormEmailHandler emailHandler, IEmailService emailService, IGlobalSettingsRepository globalSettings)
        {
            _emailHandler = emailHandler;
            _emailService = emailService;
            _globalSettings = globalSettings;
        }

        public void SubmitForm(ShortFormModel model)
        {
            SendCustomerServiceEmail(model);
            SendConfirmationEmail(model);
        }

        private void SendCustomerServiceEmail(ShortFormModel model)
        {
            List<KeyValuePair<string, string>> mappedModel = model.ToKeyValuePairs();
            string recipientEmails = _globalSettings.GetSetting(Templates.Global_Settings.FieldNames.Short_Form_Customer_Service_Email_Recipients);
            string senderEmail = _globalSettings.GetSetting(Templates.Global_Settings.FieldNames.Short_Form_Customer_Service_Email_Sender);
            Email customerServiceEmail = _emailHandler.ParseEmail(mappedModel.ToArray(), recipientEmails, senderEmail, Templates.Global_Settings.FieldNames.Short_Form_Customer_Service_Email_Subject, Templates.Global_Settings.FieldNames.Short_Form_Customer_Service_Email_Body);
            _emailService.Send(customerServiceEmail);
        }

        private void SendConfirmationEmail(ShortFormModel model)
        {
            List<KeyValuePair<string, string>> mappedModel = model.ToKeyValuePairs();
            string emailSender = _globalSettings.GetSetting(Templates.Global_Settings.FieldNames.Short_Form_Confirmation_Email_Sender);
            Email confirmationEmail = _emailHandler.ParseEmail(mappedModel.ToArray(), model.EmailAddress, emailSender, Templates.Global_Settings.FieldNames.Short_Form_Confirmation_Email_Subject, Templates.Global_Settings.FieldNames.Short_Form_Confirmation_Email_Body);
            _emailService.Send(confirmationEmail);
        }
    }
}   