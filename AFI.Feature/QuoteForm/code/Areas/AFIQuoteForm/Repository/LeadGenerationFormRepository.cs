using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Repositories;
using AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.LeadGenerationForm;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
    public interface ILeadGenerationFormRepository
    {
        LeadGenerationForm GetForm();
        string GetFormUrl();
        List<KeyValuePair<string, string>> RetrieveFormByKey(string key);
        CommonFormSaveResponseModel SubmitForm(LeadGenerationFormSaveModel form);
        CommonFormSaveResponseModel SaveForm(LeadGenerationFormSaveModel form);
        string GetFormCustomerServicePhone();
    }

	public class LeadGenerationFormRepository : QuoteFormRepository, ILeadGenerationFormRepository
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteContactRepository _contactRepository;
        private readonly IQuoteLeadGenerationRepository _leadGenerationRepository;

        public LeadGenerationFormRepository(IAPIRepository apiRepository, ICommonRepository commonRepository, IQuoteRepository quoteRepository, IQuoteContactRepository contactRepository, IQuoteLeadGenerationRepository leadGenerationRepository) : base(apiRepository, commonRepository)
        {
            _quoteRepository = quoteRepository;
            _contactRepository = contactRepository;
            _leadGenerationRepository = leadGenerationRepository;
        }

        public bool IsValid(string coverageType)
        {
            return string.Equals(coverageType, CoverageTypes.HillAFB, StringComparison.InvariantCultureIgnoreCase);
        }

        public LeadGenerationForm GetForm()
        {
            Item leadGenerationFormItem = ApiRepository.GetLeadGenerationForm();
            if (leadGenerationFormItem == null) return null;
            var formBlob = new LeadGenerationForm
            {
                wayfinder = new Wayfinder
                {
                    steps = CommonRepository.GetSteps(new MultilistField(leadGenerationFormItem.Fields[Templates.Business_Form.FieldNames.Way_Finder_Steps]))
                },
                form = new LeadGenerationFormData
                {
                    id = leadGenerationFormItem[Templates.Business_Form.FieldNames.ID],
                    backButtonText = leadGenerationFormItem[Templates.Business_Form.FieldNames.Back_Button_Text],
                    preSaveButtonText = leadGenerationFormItem[Templates.Business_Form.FieldNames.Pre_Save_Button_Text],
                    saveButtonText = leadGenerationFormItem[Templates.Business_Form.FieldNames.Save_Button_Text],
                    nextButtonText = leadGenerationFormItem[Templates.Business_Form.FieldNames.Next_Button_Text],
                    submitButtonText = leadGenerationFormItem[Templates.Business_Form.FieldNames.Submit_Button_Text],
                    selectMenuDefaultText = leadGenerationFormItem[Templates.Business_Form.FieldNames.Select_Menu_Default_Text],
                    saveSuccessRedirect = ((LinkField)leadGenerationFormItem.Fields[Templates.Business_Form.FieldNames.SaveSuccessRedirect]).GetFriendlyUrl(),
                    submitSuccessRedirect = ((LinkField)leadGenerationFormItem.Fields[Templates.Business_Form.FieldNames.SubmitSuccessRedirect]).GetFriendlyUrl(),
                    sections = new List<Section>(),
                    common = new CommonData(),
                    unique = new UniqueLeadGenerationFormData()
                }
            };
            IEnumerable<Item> sections = ApiRepository.GetCommonSections();
            formBlob.form.sections.AddRange(FillFormSections(sections, leadGenerationFormItem, CommonRepository));
            return formBlob;
        }

        public string GetFormUrl()
        {
            var leadGenerationForm = ApiRepository.GetLeadGenerationForm();
            return CommonRepository.GetFormUrl(leadGenerationForm);
        }

        public List<KeyValuePair<string, string>> RetrieveFormByKey(string key)
        {
            Quote quote = _quoteRepository.GetBySaveForLaterKey(new Guid(key));
            if(quote == null) return new List<KeyValuePair<string, string>>();
            QuoteContact contact = _contactRepository.GetByKey(quote.Key);
            QuoteLeadGeneration commercial = _leadGenerationRepository.GetByKey(quote.Key);
            LeadGenerationFormSaveModel model = new LeadGenerationFormSaveModel();
            model.ReverseMap(quote, contact, commercial);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            return keyValueModel;
        }

        private CommonFormSaveResponseModel Save(LeadGenerationFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            var quote = new Quote();
            form.EligibilityStatus = "military";
            quote.Map(CoverageTypes.LeadGeneration, form, isSubmitted, returningVisit);
            if (returningVisit)
            {
                _quoteRepository.Update(quote);
            }
            else
            {
                quote.Key = _quoteRepository.Create(quote);
            }
            var quoteContact = new QuoteContact();
            if (!string.IsNullOrEmpty(form.SpouseFirstName))
                form.PolicyHolderMaritalStatus = "married";
            else form.PolicyHolderMaritalStatus = "";

            quoteContact.Map(form, quote.Key);
            _contactRepository.CreateOrUpdate(quoteContact);

            var quoteLeadGeneration = new QuoteLeadGeneration();
            quoteLeadGeneration.Map(form, quote.Key);
            _leadGenerationRepository.CreateOrUpdate(quoteLeadGeneration);

            return new CommonFormSaveResponseModel() {QuoteId = quote.Key, QuoteKey = quote.SaveForLaterKey, MemberNumber = form.MemberNumber?.ToString() ?? string.Empty };
        }

        public CommonFormSaveResponseModel SubmitForm(LeadGenerationFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public CommonFormSaveResponseModel SaveForm(LeadGenerationFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        public string GetFormCustomerServicePhone()
        {
            return GetSimpleFieldFormValue(CoverageTypes.LeadGeneration, Templates.Business_Form.FieldIds.Associated_Customer_Service_Phone);
        }
    }
}