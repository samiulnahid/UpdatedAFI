﻿using System;
using System.Collections.Generic;
using System.Linq;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers; 
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using AFI.Feature.ZipLookup;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.QuoteLeadForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers
{
    public class QuoteLeadFormSaveHandler : IFormSaveHandler<QuoteLeadForm>
    {
        private readonly IQuoteLeadFormRepository _quoteLeadRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IFormEmailHandler _emailHandler;
        private readonly ICommonRepository _commonRepository;
        private readonly IAPIRepository _apiRepository;
        private readonly IZipLookupService _zipLookupService;
        private readonly IAFIWebServicesSaveHandler _afiWebServicesSaveHandler;

        public QuoteLeadFormSaveHandler(ISessionRepository sessionRepository, IQuoteLeadFormRepository quoteLeadRepository, IFormEmailHandler emailHandler, ICommonRepository commonRepository, IAPIRepository apiRepository, IZipLookupService zipLookupService, IAFIWebServicesSaveHandler afiWebServicesSaveHandler)
        {
            _quoteLeadRepository = quoteLeadRepository;
            _sessionRepository = sessionRepository;
            _emailHandler = emailHandler;
            _commonRepository = commonRepository;
            _apiRepository = apiRepository;
            _zipLookupService = zipLookupService;
            _afiWebServicesSaveHandler = afiWebServicesSaveHandler;
        }

        public void ApplySessionFieldChanges(QuoteLeadForm form, string key)
        {
            IEnumerable<KeyValuePair<string, string>> mapper = _quoteLeadRepository.RetrieveFormByKey(key);
            Item formItem = _apiRepository.GetQuoteLeadForm();
            if (mapper.Any())
            {
                form.form.quoteKey = key;
                if (mapper.Any(c => string.Equals("quoteId", c.Key, StringComparison.InvariantCultureIgnoreCase)))
                {
                    string quoteIdValue = mapper.First(c => c.Key.Equals("quoteId", StringComparison.InvariantCultureIgnoreCase)).Value;
                    int quoteId = 0;
                    int.TryParse(quoteIdValue, out quoteId);
                    form.form.quoteId = quoteId;
                }
                form.form.returningMessage = _commonRepository.GetSuccessReturnFormMessage(formItem);
            }
            else
            {
                form.form.returningMessage = _commonRepository.GetExpiredReturnFormMessage(formItem);
            }

            form.ApplyFieldChanges(mapper);
        }

        public void ApplySessionFieldChanges(QuoteLeadForm form)
        {
            var mapper = FormExtensions.ApplyZipCodeLookup(_sessionRepository, _zipLookupService, CoverageTypes.Quote_Lead);
            form.ApplyFieldChanges(mapper);
        }

        public CommonFormSaveResponseModel HandlePost(string json, string sessionId)
        {
            var quoteKey = Guid.Empty;
            var saveResponse = new CommonFormSaveResponseModel();
            saveResponse.QuoteKey = quoteKey;
            var obj = JObject.Parse(json);
            var action = (string) obj["action"];
            var values = (JObject) obj["values"];
            List<KeyValuePair<string, string>> toSave = values.ToKeyValuePair().ToList();
            string emailRecipient = string.Empty;
            if (new[] {FormActions.SaveAndExit, FormActions.Submit}.Contains(action.ToLower()))
            {
                emailRecipient = action.ToLower() == FormActions.Submit
                    ? toSave.First(x => x.Key == "policyHolderEmail").Value.ToLower()
                    : toSave.First(x => x.Key == "saveQuoteEmailAddress").Value.ToLower();
            }
            QuoteLeadFormSaveModel form = FormExtensions.MapJson<QuoteLeadFormSaveModel>(values.ToString());
            switch (action.ToLower())
            {
                case FormActions.Send:
                    saveResponse = HandleSubmit(form, toSave, emailRecipient);
                    break;
                case FormActions.Abandon:
                    HandleAbandon(form);
                    break;                    
                case FormActions.SaveAndExit:
                    HandleSaveAndExit(form, toSave, emailRecipient);
                    break;
                case FormActions.Submit:
                    saveResponse = HandleSubmit(form, toSave, emailRecipient);
                    break;
                case FormActions.Cancel:
                    _sessionRepository.RemoveValues(sessionId);
                    break;
                default:
                    throw new InvalidOperationException($"Quote Forms: action with name '{action.ToLower()}' doesn't exist");
            }

            return saveResponse;
        }

       
        private CommonFormSaveResponseModel HandleSubmit(QuoteLeadFormSaveModel form, List<KeyValuePair<string, string>> toSave, string emailRecipient)
        {
            CommonFormSaveResponseModel response = _quoteLeadRepository.SaveForm(form); 
            //INFO: Calling AFI WS
            if (response.QuoteId > 0)
            {
                form.QuoteId = response.QuoteId;
                try
                {
                    Sitecore.Diagnostics.Log.Info($"Quote Forms WS Endpoint: {_afiWebServicesSaveHandler.GetCurrentEndpoint()}", Constants.QUOTE_FORMS_LOGGER_NAME);
                    _afiWebServicesSaveHandler.ProcessSubmitInformationToAFI(CoverageTypes.Quote_Lead, form);
                        string customerServicePhone = _quoteLeadRepository.GetFormCustomerServicePhone();
                    toSave.Add(new KeyValuePair<string, string>("customerServicePhone", customerServicePhone));
                 
                    _emailHandler.QuoteleadformSubmit(toSave, emailRecipient, CoverageTypes.Quote_Lead); 
                   
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Quote Forms error - WS Call Error", ex, Constants.QUOTE_FORMS_LOGGER_NAME);
                }
                
            }
            return response;
        }
       

        private void HandleAbandon(QuoteLeadFormSaveModel model)
        {
            _quoteLeadRepository.SaveForm(model);
        }

        private CommonFormSaveResponseModel HandleSave(QuoteLeadFormSaveModel model)
        {
            CommonFormSaveResponseModel saveResponse = _quoteLeadRepository.SaveForm(model);
            return saveResponse;
        }

        private void HandleSaveAndExit(QuoteLeadFormSaveModel form, List<KeyValuePair<string, string>> toSave, string emailRecipient)
        {
            CommonFormSaveResponseModel saveResponse = HandleSave(form);
            Guid? saveForLaterKey = saveResponse.QuoteKey;
            toSave.Add(new KeyValuePair<string, string>("saveForLaterKey", saveForLaterKey.ToString()));
            string formUrl = _quoteLeadRepository.GetFormUrl();
            toSave.Add(new KeyValuePair<string, string>("formurl", formUrl));
            _emailHandler.HandleSave(toSave, emailRecipient, CoverageTypes.Quote_Lead);
        }


    }
}