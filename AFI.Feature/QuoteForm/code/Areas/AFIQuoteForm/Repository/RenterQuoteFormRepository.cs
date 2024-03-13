using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Repositories;
using AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.RenterForm;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Identifiers = AFI.Feature.Identifiers;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
    public interface IRenterQuoteFormRepository
    {
        RenterForm GetForm();
        string GetFormUrl();
        string GetFormCustomerServicePhone();
        CommonFormSaveResponseModel SubmitForm(RenterFormSaveModel form);
        CommonFormSaveResponseModel SaveForm(RenterFormSaveModel model);
        IEnumerable<KeyValuePair<string, string>> RetrieveFormByKey(string key);
    }
    public class RenterQuoteFormRepository : QuoteFormRepository, IRenterQuoteFormRepository
    {
		private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteContactRepository _quoteContactRepository;
        private readonly IQuoteRenterRepository _quoteRenterRepository;
        private readonly IQuoteRenterApplicationRepository _quoteRenterApplicationRepository;

        public RenterQuoteFormRepository(IAPIRepository apiRepository, ICommonRepository commonRepository, IQuoteRepository quoteRepository, IQuoteRenterRepository quoteRenterRepository, IQuoteContactRepository quoteContactRepository, IQuoteRenterApplicationRepository quoteRenterApplicationRepository) : base(apiRepository, commonRepository)
        {
            _quoteRepository = quoteRepository;
            _quoteRenterRepository = quoteRenterRepository;
            _quoteContactRepository = quoteContactRepository;
            _quoteRenterApplicationRepository = quoteRenterApplicationRepository;
        }

        public RenterForm GetForm()
        {
            Item renterFormItem = ApiRepository.GetRenterForm();
            if (renterFormItem == null) 
                return null;
            var formBlob = new RenterForm
            {
                wayfinder = new Wayfinder
                {
                    steps = CommonRepository.GetSteps(new MultilistField(renterFormItem.Fields[Templates.Renter_Form.FieldNames.Way_Finder_Steps]))
                },
                form = new RenterFormData
                {
                    id = renterFormItem[Templates.Renter_Form.FieldNames.ID],
                    backButtonText = renterFormItem[Templates.Renter_Form.FieldNames.Back_Button_Text],
                    preSaveButtonText = renterFormItem[Templates.Renter_Form.FieldNames.Pre_Save_Button_Text],
                    saveButtonText = renterFormItem[Templates.Renter_Form.FieldNames.Save_Button_Text],
                    nextButtonText = renterFormItem[Templates.Renter_Form.FieldNames.Next_Button_Text],
                    submitButtonText = renterFormItem[Templates.Renter_Form.FieldNames.Submit_Button_Text],
                    selectMenuDefaultText = renterFormItem[Templates.Renter_Form.FieldNames.Select_Menu_Default_Text],
                    saveSuccessRedirect = ((LinkField) renterFormItem.Fields[Templates.Renter_Form.FieldNames.SaveSuccessRedirect]).GetFriendlyUrl(),
                    submitSuccessRedirect = ((LinkField) renterFormItem.Fields[Templates.Renter_Form.FieldNames.SubmitSuccessRedirect]).GetFriendlyUrl(),
                    sections = new List<Section>(),
                    common = new CommonData(),
                    unique = GetUniqueValues(renterFormItem?.Axes.GetDescendants().FirstOrDefault(x => x.TemplateID == Identifiers.Templates.Unique_Renter_Form_Fields.TemplateId))
                }
            };
            IEnumerable<Item> sections = ApiRepository.GetCommonSections();
            formBlob.form.sections.AddRange(FillFormSections(sections, renterFormItem, CommonRepository));
            return formBlob;
        }

        public string GetFormUrl()
        {
            var renterForm = ApiRepository.GetRenterForm();
            return CommonRepository.GetFormUrl(renterForm);
        }

        public CommonFormSaveResponseModel SubmitForm(RenterFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public CommonFormSaveResponseModel SaveForm(RenterFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        private CommonFormSaveResponseModel Save(RenterFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            var quote = new Quote();
            quote.Map(CoverageTypes.Renters, form, isSubmitted, returningVisit);
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
            _quoteContactRepository.CreateOrUpdate(quoteContact);

            var quoteRenter = new QuoteRenter();
            quoteRenter.Map(form, quote.Key);
            _quoteRenterRepository.CreateOrUpdate(quoteRenter);

            return new CommonFormSaveResponseModel() { QuoteKey = quote.SaveForLaterKey, QuoteId = quote.Key, MemberNumber = form.MemberNumber?.ToString() ?? string.Empty };
        }

        public IEnumerable<KeyValuePair<string, string>> RetrieveFormByKey(string key)
        {
            Quote quote = _quoteRepository.GetBySaveForLaterKey(new Guid(key));
            if (quote == null) 
                return new List<KeyValuePair<string, string>>();
            QuoteContact contact = _quoteContactRepository.GetByKey(quote.Key);
            QuoteRenter renter = _quoteRenterRepository.GetByKey(quote.Key);
            RenterFormSaveModel model = new RenterFormSaveModel();
            model.ReverseMap(quote, contact, renter);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            return keyValueModel;
        }

        private static UniqueRenterFormData GetUniqueValues(Item uniqueFormFields)
        {
            if (uniqueFormFields == null) return null;
            var uniqueFormData = new UniqueRenterFormData();
            uniqueFormData.PropertyAgeOfRoofBeforeText = uniqueFormFields[Identifiers.Templates.Unique_Renter_Form_Fields.FieldNames.Property_Age_Of_Roof_Before_Text];
            uniqueFormData.PropertyAgeOfRoofEnd = uniqueFormFields[Identifiers.Templates.Unique_Renter_Form_Fields.FieldNames.Property_Age_Of_Roof_End];
            uniqueFormData.PropertyAgeOfRoofStart = uniqueFormFields[Identifiers.Templates.Unique_Renter_Form_Fields.FieldNames.Property_Age_Of_Roof_Start];
            uniqueFormData.PropertyYearBuiltBeforeText = uniqueFormFields[Identifiers.Templates.Unique_Renter_Form_Fields.FieldNames.Property_Year_Built_Before_Text];
            uniqueFormData.PropertyYearBuiltStartYear = uniqueFormFields[Identifiers.Templates.Unique_Renter_Form_Fields.FieldNames.Property_Year_Built_Start_Year];
            return uniqueFormData;
        }

        public string GetFormCustomerServicePhone()
        {
            return GetSimpleFieldFormValue(CoverageTypes.Renters, Identifiers.Templates.Renter_Form.FieldIds.Associated_Customer_Service_Phone);
        }
    }
}