using System;
using System.Collections.Generic;
using System.Linq;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Repositories;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HomeownerForm;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Identifiers = AFI.Feature.Identifiers;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
	public interface IHomeownerQuoteFormRepository
	{
		HomeownerForm GetForm();
        string GetFormUrl();
        string GetFormCustomerServicePhone();
        List<KeyValuePair<string, string>> RetrieveFormByKey(string key);
        CommonFormSaveResponseModel SubmitForm(HomeownerFormSaveModel form);
        CommonFormSaveResponseModel SaveForm(HomeownerFormSaveModel form);
    }


	public class HomeownerQuoteFormRepository : QuoteFormRepository, IHomeownerQuoteFormRepository
	{
        private readonly IQuoteHomeownerRepository _quoteHomeownerRepository;
        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteContactRepository _contactRepository;

        public HomeownerQuoteFormRepository(IAPIRepository apiRepository, 
            ICommonRepository commonRepository, 
            IQuoteHomeownerRepository quoteHomeownerRepository,
            IQuoteRepository quoteRepository,
            IQuoteContactRepository contactRepository) : base(
            apiRepository, commonRepository)
        {
            _quoteHomeownerRepository = quoteHomeownerRepository;
            _quoteRepository = quoteRepository;
            _contactRepository = contactRepository;
        }

        public HomeownerForm GetForm()
        {
            Item form = ApiRepository.GetHomeownerForm();
            if (form == null) return null;
            var formBlob = new HomeownerForm
            {
                wayfinder = new Wayfinder()
                {
                    steps = CommonRepository.GetSteps(new MultilistField(form.Fields[Identifiers.Templates.Homeowner_Form.FieldNames.Way_Finder_Steps]))
                },
                form = new HomeownerFormData()
                {
                    id = form[Identifiers.Templates.Motorcycle_Form.FieldNames.ID],
                    backButtonText = form[Identifiers.Templates.Motorcycle_Form.FieldNames.Back_Button_Text],
                    preSaveButtonText = form[Identifiers.Templates.Motorcycle_Form.FieldNames.Pre_Save_Button_Text],
                    saveButtonText = form[Identifiers.Templates.Motorcycle_Form.FieldNames.Save_Button_Text],
                    nextButtonText = form[Identifiers.Templates.Motorcycle_Form.FieldNames.Next_Button_Text],
                    submitButtonText = form[Identifiers.Templates.Motorcycle_Form.FieldNames.Submit_Button_Text],
                    selectMenuDefaultText = form[Identifiers.Templates.Motorcycle_Form.FieldNames.Select_Menu_Default_Text],
                    saveSuccessRedirect = ((LinkField)form.Fields[Identifiers.Templates.Motorcycle_Form.FieldNames.SaveSuccessRedirect]).GetFriendlyUrl(),
                    submitSuccessRedirect = ((LinkField)form.Fields[Identifiers.Templates.Motorcycle_Form.FieldNames.SubmitSuccessRedirect]).GetFriendlyUrl(),
                    sections = new List<Section>(),
                    common = new CommonData(),
                    unique = GetUniqueValues(form?.Axes.GetDescendants().FirstOrDefault(x => x.TemplateID == Identifiers.Templates.Unique_Homeowner_Form_Fields.TemplateId))
                }
            };
            IEnumerable<Item> sections = ApiRepository.GetCommonSections();
            formBlob.form.sections.AddRange(FillFormSections(sections, form, CommonRepository));
            return formBlob;
        }

        public string GetFormUrl()
        {
            var homeownerForm = ApiRepository.GetHomeownerForm();
            return CommonRepository.GetFormUrl(homeownerForm);
        }

        public List<KeyValuePair<string, string>> RetrieveFormByKey(string key)
        {
            Quote quote = _quoteRepository.GetBySaveForLaterKey(new Guid(key));
            if (quote == null) 
                return new List<KeyValuePair<string, string>>();
            QuoteContact contact = _contactRepository.GetByKey(quote.Key);
            QuoteHomeowner homeowner = _quoteHomeownerRepository.GetByKey(quote.Key);
            HomeownerFormSaveModel model = new HomeownerFormSaveModel();
            model.ReverseMap(quote, contact, homeowner);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            return keyValueModel;
        }

        private CommonFormSaveResponseModel Save(HomeownerFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            var quote = new Quote();
            quote.Map("Homeowners", form, isSubmitted, returningVisit);
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

            var homeowner = new QuoteHomeowner();
            homeowner.Map(form, quote.Key);
            _quoteHomeownerRepository.CreateOrUpdate(homeowner);

            return new CommonFormSaveResponseModel() { QuoteKey = quote.SaveForLaterKey, QuoteId = quote.Key, MemberNumber = form.MemberNumber?.ToString() ?? string.Empty };
        }

        public CommonFormSaveResponseModel SubmitForm(HomeownerFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public CommonFormSaveResponseModel SaveForm(HomeownerFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        private UniqueHomeownerFormData GetUniqueValues(Item uniqueFormValues)
        {
            UniqueHomeownerFormData uniqueData = new UniqueHomeownerFormData
            {
                PropertyAgeOfRoofBeforeText = uniqueFormValues[Identifiers.Templates.Unique_Homeowner_Form_Fields.FieldNames.Property_Age_Of_Roof_Before_Text],
                PropertyAgeOfRoofEnd = uniqueFormValues[Identifiers.Templates.Unique_Homeowner_Form_Fields.FieldNames.Property_Age_Of_Roof_End],
                PropertyAgeOfRoofStart = uniqueFormValues[Identifiers.Templates.Unique_Homeowner_Form_Fields.FieldNames.Property_Age_Of_Roof_Start],
                PropertyYearBuiltBeforeText = uniqueFormValues[Identifiers.Templates.Unique_Homeowner_Form_Fields.FieldNames.Property_Year_Built_Before_Text],
                PropertyYearBuiltStartYear = uniqueFormValues[Identifiers.Templates.Unique_Homeowner_Form_Fields.FieldNames.Property_Year_Built_Start_Year]
            };
            return uniqueData;
        }

        public string GetFormCustomerServicePhone()
        {
            return GetSimpleFieldFormValue(CoverageTypes.Homeowner, Identifiers.Templates.Homeowner_Form.FieldIds.Associated_Customer_Service_Phone);

        }
    }
}