using System;
using System.Collections.Generic;
using System.Linq;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Repositories;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HomenonownerForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HomeownerForm;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Identifiers = AFI.Feature.Identifiers;
namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
	public interface IHomenonownerQuoteFormRepository
    {
		HomenonownerForm GetForm();
        string GetFormUrl();
        string GetFormCustomerServicePhone();
        List<KeyValuePair<string, string>> RetrieveFormByKey(string key);
        CommonFormSaveResponseModel SubmitForm(HomenonownerFormSaveModel form);
        CommonFormSaveResponseModel SaveForm(HomenonownerFormSaveModel form);
    }


	public class HomenonownerQuoteFormRepository : QuoteFormRepository, IHomenonownerQuoteFormRepository
    {
        private readonly IQuoteHomenonownerRepository _quoteHomenonownerRepository;
        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteContactRepository _contactRepository;

        public HomenonownerQuoteFormRepository(IAPIRepository apiRepository, 
            ICommonRepository commonRepository, 
            IQuoteHomenonownerRepository quoteHomenonownerRepository,
            IQuoteRepository quoteRepository,
            IQuoteContactRepository contactRepository) : base(
            apiRepository, commonRepository)
        {
            _quoteHomenonownerRepository = quoteHomenonownerRepository;
            _quoteRepository = quoteRepository;
            _contactRepository = contactRepository;
        }

        public HomenonownerForm GetForm()
        {
            Item form = ApiRepository.GetHomenonownerForm();
            if (form == null) return null;
            var formBlob = new HomenonownerForm
            {
                wayfinder = new Wayfinder()
                {
                    steps = CommonRepository.GetSteps(new MultilistField(form.Fields[Identifiers.Templates.Homenonowner_Form.FieldNames.Way_Finder_Steps]))
                },
                form = new HomenonownerFormData()
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
                    unique = GetUniqueValues(form?.Axes.GetDescendants().FirstOrDefault(x => x.TemplateID == Identifiers.Templates.Unique_Homenonowner_Form_Fields.TemplateId))
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
            QuoteHomenonowner homeowner = _quoteHomenonownerRepository.GetByKey(quote.Key);
            HomenonownerFormSaveModel model = new HomenonownerFormSaveModel();
            model.ReverseMap(quote, contact, homeowner);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            return keyValueModel;
        }

        private CommonFormSaveResponseModel Save(HomenonownerFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            var quote = new Quote();
            quote.Map(CoverageTypes.DwellingFire, form, isSubmitted, returningVisit);
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

            var homenonowner = new QuoteHomenonowner();
            homenonowner.Map(form, quote.Key);
            _quoteHomenonownerRepository.CreateOrUpdate(homenonowner);

            return new CommonFormSaveResponseModel() { QuoteKey = quote.SaveForLaterKey, QuoteId = quote.Key, MemberNumber = form.MemberNumber?.ToString() ?? string.Empty };
        }

        public CommonFormSaveResponseModel SubmitForm(HomenonownerFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public CommonFormSaveResponseModel SaveForm(HomenonownerFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        private UniqueHomenonownerFormData GetUniqueValues(Item uniqueFormValues)
        {
            UniqueHomenonownerFormData uniqueData = new UniqueHomenonownerFormData
            {
                PropertyAgeOfRoofBeforeText = uniqueFormValues[Identifiers.Templates.Unique_Homenonowner_Form_Fields.FieldNames.Property_Age_Of_Roof_Before_Text],
                PropertyAgeOfRoofEnd = uniqueFormValues[Identifiers.Templates.Unique_Homenonowner_Form_Fields.FieldNames.Property_Age_Of_Roof_End],
                PropertyAgeOfRoofStart = uniqueFormValues[Identifiers.Templates.Unique_Homenonowner_Form_Fields.FieldNames.Property_Age_Of_Roof_Start],
                PropertyYearBuiltBeforeText = uniqueFormValues[Identifiers.Templates.Unique_Homenonowner_Form_Fields.FieldNames.Property_Year_Built_Before_Text],
                PropertyYearBuiltStartYear = uniqueFormValues[Identifiers.Templates.Unique_Homenonowner_Form_Fields.FieldNames.Property_Year_Built_Start_Year]
            };
            return uniqueData;
        }

        public string GetFormCustomerServicePhone()
        {
            return GetSimpleFieldFormValue(CoverageTypes.DwellingFire, Identifiers.Templates.Homenonowner_Form.FieldIds.Associated_Customer_Service_Phone);

        }
    }
}