using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AFI.Feature.ZipLookup;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MotorcycleForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers
{
    public class MotorcycleFormSaveHandler : IFormSaveHandler<MotorcycleForm>
    {
        private readonly IMotorcycleQuoteFormRepository _motorcycleRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IFormEmailHandler _emailHandler;
        private readonly ICommonRepository _commonRepository;
        private readonly IAPIRepository _apiRepository;
        private readonly IZipLookupService _zipLookupService;
        private readonly IAFIWebServicesSaveHandler _afiWebServicesSaveHandler;


        public MotorcycleFormSaveHandler(IMotorcycleQuoteFormRepository motorcycleRepository, ISessionRepository sessionRepository, IFormEmailHandler emailHandler, ICommonRepository commonRepository, IAPIRepository apiRepository, IZipLookupService zipLookupService, IAFIWebServicesSaveHandler afiWebServicesSaveHandler)
        {
            _motorcycleRepository = motorcycleRepository;
            _sessionRepository = sessionRepository;
            _emailHandler = emailHandler;
            _commonRepository = commonRepository;
            _apiRepository = apiRepository;
            _zipLookupService = zipLookupService;
            _afiWebServicesSaveHandler = afiWebServicesSaveHandler;
        }


        public CommonFormSaveResponseModel HandlePost(string json, string sessionId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
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
            MotorcycleFormSaveModel form = FormExtensions.MapJson<MotorcycleFormSaveModel>(values.ToString());
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
                    saveResponse = HandleSubmit(form, toSave, emailRecipient);
                    break;
                case FormActions.Cancel:
                    _sessionRepository.RemoveValues(sessionId);
                    break;
                default:
                    throw new InvalidOperationException($"Quote Forms: action with name '{action.ToLower()}' doesn't exist");
            }
            sw.Stop();
            Sitecore.Diagnostics.Log.Info("STOPWATCH: end handle post motorcycle" + sw.Elapsed, "stpowatch");
            return saveResponse;
        }

        private CommonFormSaveResponseModel HandleSubmit(MotorcycleFormSaveModel form, List<KeyValuePair<string, string>> toSave, string emailRecipient)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            CommonFormSaveResponseModel response = _motorcycleRepository.SubmitForm(form);
            //INFO: Calling AFI WS
            if (response.QuoteId > 0)
            {
                form.QuoteId = response.QuoteId;
                try
                {
                    Sitecore.Diagnostics.Log.Info($"Quote Forms WS Endpoint: {_afiWebServicesSaveHandler.GetCurrentEndpoint()}", Constants.QUOTE_FORMS_LOGGER_NAME);
                    _afiWebServicesSaveHandler.ProcessSubmitInformationToAFI(CoverageTypes.Motorcycle, form);
                    string customerServicePhone = _motorcycleRepository.GetFormCustomerServicePhone();
                    toSave.Add(new KeyValuePair<string, string>("customerServicePhone", customerServicePhone));
                    _emailHandler.HandleSubmit(toSave, emailRecipient, CoverageTypes.Motorcycle);
                    sw.Stop();
                    Sitecore.Diagnostics.Log.Info("STOPWATCH: end handle submit motorcycle" + sw.Elapsed, "stpowatch");
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    Sitecore.Diagnostics.Log.Info("STOPWATCH: end handle submit motorcycle error" + sw.Elapsed, "stpowatch");
                    Sitecore.Diagnostics.Log.Error("Quote Forms error - WS Call Error", ex, Constants.QUOTE_FORMS_LOGGER_NAME);
                }
            }
            return response;
        }

        private void HandleAbandon(MotorcycleFormSaveModel model)
        {
            _motorcycleRepository.SaveForm(model);
        }

        private CommonFormSaveResponseModel HandleSave(MotorcycleFormSaveModel model)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            CommonFormSaveResponseModel saveResponse = _motorcycleRepository.SaveForm(model);
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
            sw.Start();
            Sitecore.Diagnostics.Log.Info("STOPWATCH: end handle save motorcycle" + sw.Elapsed, "stopwatch");
            return saveResponse;
        }

        private void HandleSaveAndExit(MotorcycleFormSaveModel form, List<KeyValuePair<string, string>> toSave, string emailRecipient)
        {
            CommonFormSaveResponseModel saveResponse = HandleSave(form);
            Guid? saveForLaterKey = saveResponse.QuoteKey;
            toSave.Add(new KeyValuePair<string, string>("saveForLaterKey", saveForLaterKey.ToString()));
            string formUrl = _motorcycleRepository.GetFormUrl();
            toSave.Add(new KeyValuePair<string, string>("formurl", formUrl));
            _emailHandler.HandleSave(toSave, emailRecipient, CoverageTypes.Motorcycle);
        }

        public void ApplySessionFieldChanges(MotorcycleForm form, string key)
        {
            IEnumerable<KeyValuePair<string, string>> mapper = _motorcycleRepository.RetrieveFormByKey(key);
            Item formItem = _apiRepository.GetMotorcycleForm();
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

        public void ApplySessionFieldChanges(MotorcycleForm form)
        {
            var mapper = FormExtensions.ApplyZipCodeLookup(_sessionRepository, _zipLookupService, CoverageTypes.Motorcycle);
            form.ApplyFieldChanges(mapper);
        }


        
    }
}