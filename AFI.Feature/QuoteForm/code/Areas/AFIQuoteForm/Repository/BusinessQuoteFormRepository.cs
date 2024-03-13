using System;
using System.Collections.Generic;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Repositories;
using AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.BusinessForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Identifiers = AFI.Feature.Identifiers;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
    public interface IBusinessQuoteFormRepository
    {
		BusinessForm GetForm();
        string GetFormUrl();
        List<KeyValuePair<string, string>> RetrieveFormByKey(string key);
        CommonFormSaveResponseModel SubmitForm(BusinessFormSaveModel form);
        CommonFormSaveResponseModel SaveForm(BusinessFormSaveModel form);
        string GetFormCustomerServicePhone();
    }

	public class BusinessQuoteFormRepository : QuoteFormRepository, IBusinessQuoteFormRepository
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteContactRepository _contactRepository;
        private readonly IQuoteCommercialRepository _commercialRepository;

        public BusinessQuoteFormRepository(IAPIRepository apiRepository, ICommonRepository commonRepository, IQuoteRepository quoteRepository, IQuoteContactRepository contactRepository, IQuoteCommercialRepository commercialRepository) : base(apiRepository, commonRepository)
        {
            _quoteRepository = quoteRepository;
            _contactRepository = contactRepository;
            _commercialRepository = commercialRepository;
        }

        public bool IsValid(string coverageType)
        {
            return string.Equals(coverageType, CoverageTypes.Business, StringComparison.InvariantCultureIgnoreCase);
        }

        public BusinessForm GetForm()
        {
            Item businessFormItem = ApiRepository.GetBusinessForm();
            if (businessFormItem == null) return null;
            var formBlob = new BusinessForm
            {
                wayfinder = new Wayfinder
                {
                    steps = CommonRepository.GetSteps(new MultilistField(businessFormItem.Fields[Templates.Business_Form.FieldNames.Way_Finder_Steps]))
                },
                form = new BusinessFormData
                {
                    id = businessFormItem[Templates.Business_Form.FieldNames.ID],
                    backButtonText = businessFormItem[Templates.Business_Form.FieldNames.Back_Button_Text],
                    preSaveButtonText = businessFormItem[Templates.Business_Form.FieldNames.Pre_Save_Button_Text],
                    saveButtonText = businessFormItem[Templates.Business_Form.FieldNames.Save_Button_Text],
                    nextButtonText = businessFormItem[Templates.Business_Form.FieldNames.Next_Button_Text],
                    submitButtonText = businessFormItem[Templates.Business_Form.FieldNames.Submit_Button_Text],
                    selectMenuDefaultText = businessFormItem[Templates.Business_Form.FieldNames.Select_Menu_Default_Text],
                    saveSuccessRedirect = ((LinkField) businessFormItem.Fields[Templates.Business_Form.FieldNames.SaveSuccessRedirect]).GetFriendlyUrl(),
                    submitSuccessRedirect = ((LinkField) businessFormItem.Fields[Templates.Business_Form.FieldNames.SubmitSuccessRedirect]).GetFriendlyUrl(),
                    sections = new List<Section>(),
                    common = new CommonData(),
                    unique = new UniqueBusinessFormData()
                }
            };
            IEnumerable<Item> sections = ApiRepository.GetCommonSections();
            formBlob.form.sections.AddRange(FillFormSections(sections, businessFormItem, CommonRepository));
            return formBlob;
        }

        public string GetFormUrl()
        {
            var businessForm = ApiRepository.GetBusinessForm();
            return CommonRepository.GetFormUrl(businessForm);
        }

        public List<KeyValuePair<string, string>> RetrieveFormByKey(string key)
        {
            Quote quote = _quoteRepository.GetBySaveForLaterKey(new Guid(key));
            if(quote == null) return new List<KeyValuePair<string, string>>();
            QuoteContact contact = _contactRepository.GetByKey(quote.Key);
            QuoteCommercial commercial = _commercialRepository.GetByKey(quote.Key);
            BusinessFormSaveModel model = new BusinessFormSaveModel();
            model.ReverseMap(quote, contact, commercial);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            return keyValueModel;
        }

        private CommonFormSaveResponseModel Save(BusinessFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            var quote = new Quote();
            quote.Map(CoverageTypes.Business, form, isSubmitted, returningVisit);
            if (returningVisit)
            {
                _quoteRepository.Update(quote);
            }
            else
            {
                quote.Key = _quoteRepository.Create(quote);
            }
            var quoteContact = new QuoteContact();
            quoteContact.Map(form, quote.Key);
            _contactRepository.CreateOrUpdate(quoteContact);

            var quoteCommercial = new QuoteCommercial();
            quoteCommercial.Map(form, quote.Key);
            _commercialRepository.CreateOrUpdate(quoteCommercial);

            return new CommonFormSaveResponseModel() {QuoteId = quote.Key, QuoteKey = quote.SaveForLaterKey, MemberNumber = form.MemberNumber?.ToString() ?? string.Empty };
        }

        public CommonFormSaveResponseModel SubmitForm(BusinessFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public CommonFormSaveResponseModel SaveForm(BusinessFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        public string GetFormCustomerServicePhone()
        {
            return GetSimpleFieldFormValue(CoverageTypes.Business, Identifiers.Templates.Business_Form.FieldIds.Associated_Customer_Service_Phone);
        }
    }
}