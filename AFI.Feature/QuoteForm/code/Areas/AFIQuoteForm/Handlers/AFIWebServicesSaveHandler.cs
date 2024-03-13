using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Repositories;
using AFI.Feature.WebQuoteService.Models;
using AFI.Feature.WebQuoteService.Repositories;

using AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.WebQuoteMappers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers
{
    public interface IAFIWebServicesSaveHandler
    {
        string SendQuoteContact(CommonFormSaveModel model);
        void ProcessSubmitInformationToAFI(string coverageType, CommonFormSaveModel model);
        string GetCurrentEndpoint();
    }

    public class AFIWebServicesSaveHandler : IAFIWebServicesSaveHandler
    {
        private readonly IPartnerAdvisorRepository _partnerAdvisorRepository;
        private readonly IAFIFormsMapRepository _afiFormsMapRepository;
        private readonly IAFIFormsSentToRepository _afiFormsSentToRepository;
        private readonly IGlobalSettingsRepository _globalSettingsRepository;

        public AFIWebServicesSaveHandler(IPartnerAdvisorRepository partnerAdvisorRepository, IAFIFormsMapRepository afiFormsMapRepository, IAFIFormsSentToRepository afiFormsSentToRepository, IGlobalSettingsRepository globalSettingsRepository)
        {
            _partnerAdvisorRepository = partnerAdvisorRepository;
            _afiFormsMapRepository = afiFormsMapRepository;
            _afiFormsSentToRepository = afiFormsSentToRepository;
            _globalSettingsRepository = globalSettingsRepository;
        }

        private bool IsTestIPAddress()
        {
            string afiTestingIPAddress = _globalSettingsRepository.GetSetting(Templates.Global_Settings.FieldNames.AFI_Testing_IP_Address);
            afiTestingIPAddress = afiTestingIPAddress.TrimEnd(',', ' ');
            string[] validationIpAddresses = afiTestingIPAddress.Split(',');
            string currentIpAddress = string.Empty;
            try
            {
                currentIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                Sitecore.Diagnostics.Log.Info($"AFI Form current IP Address {currentIpAddress}", Constants.QUOTE_FORMS_LOGGER_NAME);
                Sitecore.Diagnostics.Log.Info($"AFI Form IP Addresses to test with {Newtonsoft.Json.JsonConvert.SerializeObject(validationIpAddresses)}", Constants.QUOTE_FORMS_LOGGER_NAME);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("AFI Form current IP Address cannot be aquired", ex, Constants.QUOTE_FORMS_LOGGER_NAME);
            }
            return !string.IsNullOrWhiteSpace(currentIpAddress) && validationIpAddresses.Any() && validationIpAddresses.Contains(currentIpAddress.Trim());
        }

        public string SendQuoteContact(CommonFormSaveModel model)
        {
            Sitecore.Diagnostics.Log.Info("Invoked SendQuoteContact AFIWebServiceSaveHandler", Constants.QUOTE_FORMS_LOGGER_NAME);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            ContactInformation contactInformation = new ContactInformation();
            contactInformation.Map(model);
            
            sw.Stop();
            Sitecore.Diagnostics.Log.Info("WS SendQuoteContact Mapping time elapsed" + sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);
            try
            {
                Sitecore.Diagnostics.Log.Info($"AFI Contact to be sent for QuoteId {model.QuoteId}: {Newtonsoft.Json.JsonConvert.SerializeObject(contactInformation)}", Constants.QUOTE_FORMS_LOGGER_NAME);
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("AFI Contact couldn't be logged", e, Constants.QUOTE_FORMS_LOGGER_NAME);
            }
            if (model.QuoteId == null)
                return string.Empty;
            sw.Restart();
            var quoteContact=_partnerAdvisorRepository.SendQuoteContactToClientAndMF(contactInformation, model.QuoteId.Value, IsTestIPAddress());
            sw.Stop();
            Sitecore.Diagnostics.Log.Info("WS SendQuoteContact call time elapsed" + sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);
            Sitecore.Diagnostics.Log.Info("Exited SendQuoteContact AFIWebServiceSaveHandler", Constants.QUOTE_FORMS_LOGGER_NAME);
            return quoteContact;
        }

        private bool ProcessFirstSubmitInformationToAFI(string coverageType, CommonFormSaveModel model)
        {
            Sitecore.Diagnostics.Log.Info("Invoked ProcessFirstSubmitInformationToAFI AFIWebServiceSaveHandler", Constants.QUOTE_FORMS_LOGGER_NAME);
            Stopwatch sw = new Stopwatch();
            sw.Start(); 
            
            bool quoteIsDuplicate = false;
            if (string.Equals(coverageType, CoverageTypes.Business, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            try
            {
                quoteIsDuplicate = model.QuoteId.HasValue && _partnerAdvisorRepository.QuoteIsDuplicate(model.QuoteId.Value);
                sw.Stop();
                Sitecore.Diagnostics.Log.Info("WS QuoteIsDuplicate call time elapsed" + sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Quote Forms error - WS QuoteIsDuplicate", ex, Constants.QUOTE_FORMS_LOGGER_NAME);
            }

            if (model.MemberNumber <= 0) return quoteIsDuplicate;
            if (!model.MemberNumber.HasValue) return quoteIsDuplicate;
            try
            {
                sw.Restart();
                Sitecore.Diagnostics.Log.Info("WS SendActivityToEpic making call" + sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);
                _partnerAdvisorRepository.SendActivityToEpic(model.MemberNumber.Value, model.PolicyHolderState, coverageType, model.StartDate, DateTime.Now, quoteIsDuplicate);
                sw.Stop();
                Sitecore.Diagnostics.Log.Info("WS SendActivityToEpic call time elapsed" + sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Quote Forms error - WS SendActivityToEpic", ex, Constants.QUOTE_FORMS_LOGGER_NAME);
            }

            try
            {
                if (model.QuoteId.HasValue)
                {
                    sw.Restart();
                    Sitecore.Diagnostics.Log.Info("WS UpdateMarketingSource making call" + sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

                    _partnerAdvisorRepository.UpdateMarketingSource(model.MemberNumber.Value, model.ResponseDescription,
                        model.QuoteId.Value);

                    sw.Stop();
                    Sitecore.Diagnostics.Log.Info("WS UpdateMarketingSource call time elapsed" + sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Quote Forms error - WS UpdateMarketingSource", ex, Constants.QUOTE_FORMS_LOGGER_NAME);
            }

            Sitecore.Diagnostics.Log.Info("Exited ProcessFirstSubmitInformationToAFI AFIWebServiceSaveHandler", Constants.QUOTE_FORMS_LOGGER_NAME);
            return quoteIsDuplicate;
        }

        private AFIForm FillCommonAFIFormValues(string coverageType, CommonFormSaveModel model, bool quoteIsDuplicate)
        {
            Sitecore.Diagnostics.Log.Info("Invoked FillCommonAFIFormValues AFIWebServiceSaveHandler", Constants.QUOTE_FORMS_LOGGER_NAME);
            Stopwatch sw = new Stopwatch();
            sw.Start();

            AFIFormsMapID form = _afiFormsMapRepository.GetByCoverageType(string.Equals(coverageType, CoverageTypes.Business, StringComparison.InvariantCultureIgnoreCase) ? "Business" : coverageType);
            sw.Stop();
            Sitecore.Diagnostics.Log.Info("WS FillCommonAFIFormValues GetByCoverageType afiFormsMapRepository time elapsed" + sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            sw.Restart();
            AFIForm afiForm = new AFIForm();
            string afiTestingIPAddress = _globalSettingsRepository.GetSetting(Templates.Global_Settings.FieldNames.AFI_Testing_IP_Address);
            afiTestingIPAddress = afiTestingIPAddress.TrimEnd(',', ' ');
            string[] validationIpAddresses = afiTestingIPAddress.Split(',');
            afiForm.Map(model, form, quoteIsDuplicate, coverageType, validationIpAddresses);
            
            sw.Stop();
            Sitecore.Diagnostics.Log.Info("WS FillCommonAFIFormValues GetSetting & Map time elapsed" + sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            Sitecore.Diagnostics.Log.Info("Exited FillCommonAFIFormValues AFIWebServiceSaveHandler", Constants.QUOTE_FORMS_LOGGER_NAME);
            return afiForm;
        }

        public void ProcessSubmitInformationToAFI(string coverageType, CommonFormSaveModel model)
        {
            Sitecore.Diagnostics.Log.Info("Invoked ProcessSubmitInformationToAFI AFIWebServiceSaveHandler", Constants.QUOTE_FORMS_LOGGER_NAME);
            
            bool quoteIsDuplicate = ProcessFirstSubmitInformationToAFI(coverageType, model);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            AFIFormsSentTo sentTo = _afiFormsSentToRepository.GetByQuoteID(model.QuoteId.ToString());
            sw.Stop();
            Sitecore.Diagnostics.Log.Info("WS ProcessSubmitInformationToAFI GetByQuoteID afiFormsSentToRepository time elapsed" + sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            if (sentTo != null) return;
            AFIForm afiForm = FillCommonAFIFormValues(coverageType, model, quoteIsDuplicate);
            try
            {
                Sitecore.Diagnostics.Log.Info($"AFI Form to be sent for QuoteId {model.QuoteId}: {Newtonsoft.Json.JsonConvert.SerializeObject(afiForm)}", Constants.QUOTE_FORMS_LOGGER_NAME);
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("AFI Form couldn't be logged", e, Constants.QUOTE_FORMS_LOGGER_NAME);
            }

            sw.Restart();
            sentTo = new AFIFormsSentTo();
            sentTo.CreateDate = model.StartDate;
            sentTo.QuoteID = model.QuoteId.ToString();
            sentTo.QuoteType = coverageType;
            _afiFormsSentToRepository.Create(sentTo);
            sw.Stop();
            Sitecore.Diagnostics.Log.Info("WS ProcessSubmitInformationToAFI Create afiFormsSentToRepository time elapsed" + sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            sw.Restart();
            _partnerAdvisorRepository.InsertAFIForm(afiForm);
            sw.Stop();
            Sitecore.Diagnostics.Log.Info("WS ProcessSubmitInformationToAFI InsertAFIForm partnerAdvisorRepository time elapsed" + sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);
            
            Sitecore.Diagnostics.Log.Info("Exited ProcessSubmitInformationToAFI AFIWebServiceSaveHandler", Constants.QUOTE_FORMS_LOGGER_NAME);
        }

        public string GetCurrentEndpoint()
        {
            return _partnerAdvisorRepository.GetCurrentEndpoint();
        }
    }
}