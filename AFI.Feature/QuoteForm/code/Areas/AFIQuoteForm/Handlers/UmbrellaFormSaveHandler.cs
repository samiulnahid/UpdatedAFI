using System;
using System.Collections.Generic;
using System.Linq;
using AFI.Feature.ZipLookup;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.UmbrellaForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers
{
    public class UmbrellaFormSaveHandler : IFormSaveHandler<UmbrellaForm>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IFormEmailHandler _emailHandler;
        private readonly IUmbrellaQuoteFormRepository _umbrellaQuoteFormRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly IAPIRepository _apiRepository;
        private readonly IZipLookupService _zipLookupService;
        private readonly IAFIWebServicesSaveHandler _afiWebServicesSaveHandler;


        public UmbrellaFormSaveHandler(ISessionRepository sessionRepository, IFormEmailHandler emailHandler, IUmbrellaQuoteFormRepository umbrellaQuoteFormRepository, ICommonRepository commonRepository, IAPIRepository apiRepository, IZipLookupService zipLookupService, IAFIWebServicesSaveHandler afiWebServicesSaveHandler)
        {
            _sessionRepository = sessionRepository;
            _emailHandler = emailHandler;
            _umbrellaQuoteFormRepository = umbrellaQuoteFormRepository;
            _commonRepository = commonRepository;
            _apiRepository = apiRepository;
            _zipLookupService = zipLookupService;
            _afiWebServicesSaveHandler = afiWebServicesSaveHandler;
        }


        public CommonFormSaveResponseModel HandlePost(string json, string sessionId)
        {
            var quoteKey = Guid.Empty;
            var saveResponse = new CommonFormSaveResponseModel();
            saveResponse.QuoteKey = quoteKey;
            var obj = JObject.Parse(json);
            var action = (string)obj["action"];
            var values = (JObject)obj["values"];
            List<KeyValuePair<string, string>> toSave = values.ToKeyValuePair().ToList();
            string emailRecipient = string.Empty;
            if (new[] { FormActions.SaveAndExit, FormActions.Submit }.Contains(action.ToLower()))
            {
                emailRecipient = action.ToLower() == FormActions.Submit
                    ? toSave.First(x => x.Key == "policyHolderEmail").Value.ToLower()
                    : toSave.First(x => x.Key == "saveQuoteEmailAddress").Value.ToLower();
            }
            UmbrellaFormSaveModel form = FormExtensions.MapJson<UmbrellaFormSaveModel>(values.ToString());
            switch (action.ToLower())
            {
                case FormActions.Send:
                    saveResponse = HandleSave(form);
                    break;
                case FormActions.Abandon:
                    HandleAbandon(form);
                    break;
                case FormActions.SaveAndExit:
                    HandleSaveAndExit(form, toSave, emailRecipient);
                    break;
                case FormActions.Submit:
                    HandleSubmit(form, toSave, emailRecipient);
                    break;
                case FormActions.Cancel:
                    _sessionRepository.RemoveValues(sessionId);
                    break;
                default:
                    throw new InvalidOperationException($"Quote Forms: action with name '{action.ToLower()}' doesn't exist");
            }

            return saveResponse;
        }

        private void HandleSubmit(UmbrellaFormSaveModel form, List<KeyValuePair<string, string>> toSave, string emailRecipient)
        {
            CommonFormSaveResponseModel response = _umbrellaQuoteFormRepository.SubmitForm(form);
            //INFO: Calling AFI WS
            if (response.QuoteId > 0)
            {
                form.QuoteId = response.QuoteId;
                try
                {
                    Sitecore.Diagnostics.Log.Info($"Quote Forms WS Endpoint: {_afiWebServicesSaveHandler.GetCurrentEndpoint()}", Constants.QUOTE_FORMS_LOGGER_NAME);
                    _afiWebServicesSaveHandler.ProcessSubmitInformationToAFI(CoverageTypes.Umbrella, form);
                    string customerServicePhone = _umbrellaQuoteFormRepository.GetFormCustomerServicePhone();
                    toSave.Add(new KeyValuePair<string, string>("customerServicePhone", customerServicePhone));
                    _emailHandler.HandleSubmit(toSave, emailRecipient, CoverageTypes.Umbrella);
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Quote Forms error - WS Call Error", ex, Constants.QUOTE_FORMS_LOGGER_NAME);
                }
            }

        }

        private void HandleAbandon(UmbrellaFormSaveModel model)
        {
            _umbrellaQuoteFormRepository.SaveForm(model);
        }

        private CommonFormSaveResponseModel HandleSave(UmbrellaFormSaveModel model)
        {
            CommonFormSaveResponseModel saveResponse = _umbrellaQuoteFormRepository.SaveForm(model);
            if (model.PolicyHolderMaritalStatus == "Civil Union/Domestic Partner")
                model.IsStepOne = true;
            if (!model.IsStepOne) return saveResponse;
            if (model.MemberNumber.HasValue) return saveResponse;
            model.QuoteId = saveResponse.QuoteId;
            try
            {
                Sitecore.Diagnostics.Log.Info($"Quote Forms WS Endpoint: {_afiWebServicesSaveHandler.GetCurrentEndpoint()}", Constants.QUOTE_FORMS_LOGGER_NAME);

                saveResponse.MemberNumber = _afiWebServicesSaveHandler.SendQuoteContact(model);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Quote Forms error - WS Call Error", ex, Constants.QUOTE_FORMS_LOGGER_NAME);
            }

            return saveResponse;
        }

        private void HandleSaveAndExit(UmbrellaFormSaveModel form, List<KeyValuePair<string, string>> toSave, string emailRecipient)
        {
            CommonFormSaveResponseModel saveResponse = HandleSave(form);
            Guid? saveForLaterKey = saveResponse.QuoteKey;
            toSave.Add(new KeyValuePair<string, string>("saveForLaterKey", saveForLaterKey.ToString()));
            string formUrl = _umbrellaQuoteFormRepository.GetFormUrl();
            toSave.Add(new KeyValuePair<string, string>("formurl", formUrl));
            _emailHandler.HandleSave(toSave, emailRecipient, CoverageTypes.Umbrella);
        }

        public void ApplySessionFieldChanges(UmbrellaForm form, string key)
        {
            IEnumerable<KeyValuePair<string, string>> mapper = _umbrellaQuoteFormRepository.RetrieveFormByKey(key);
            Item formItem = _apiRepository.GetUmbrellaForm();
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

        public void ApplySessionFieldChanges(UmbrellaForm form)
        {
            var mapper = FormExtensions.ApplyZipCodeLookup(_sessionRepository, _zipLookupService, CoverageTypes.Umbrella);
            form.ApplyFieldChanges(mapper);
        }
    }
}