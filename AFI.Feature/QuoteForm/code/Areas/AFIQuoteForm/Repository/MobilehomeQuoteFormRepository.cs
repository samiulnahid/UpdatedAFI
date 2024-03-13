using System;
using System.Collections.Generic;
using System.Linq;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Repositories;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MobilehomeForm;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Identifiers = AFI.Feature.Identifiers;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
	public interface IMobilehomeQuoteFormRepository
	{
		MobilehomeForm GetForm();
        string GetFormUrl();
        string GetFormCustomerServicePhone();
        List<KeyValuePair<string, string>> RetrieveFormByKey(string key);
        CommonFormSaveResponseModel SubmitForm(MobilehomeFormSaveModel form);
        CommonFormSaveResponseModel SaveForm(MobilehomeFormSaveModel form);
    }


	public class MobilehomeQuoteFormRepository : QuoteFormRepository, IMobilehomeQuoteFormRepository
    {
        private readonly IQuoteMobilehomeRepository _quoteMobilehomeRepository;
        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteContactRepository _contactRepository;

        public MobilehomeQuoteFormRepository(IAPIRepository apiRepository, 
            ICommonRepository commonRepository, 
            IQuoteMobilehomeRepository quoteMobilehomeRepository,
            IQuoteRepository quoteRepository,
            IQuoteContactRepository contactRepository) : base(
            apiRepository, commonRepository)
        {
            _quoteMobilehomeRepository = quoteMobilehomeRepository;
            _quoteRepository = quoteRepository;
            _contactRepository = contactRepository;
        }

        public MobilehomeForm GetForm()
        {
            Item form = ApiRepository.GetMobilehomeForm();
            if (form == null) return null;
            var formBlob = new MobilehomeForm
            {
                wayfinder = new Wayfinder()
                {
                    steps = CommonRepository.GetSteps(new MultilistField(form.Fields[Identifiers.Templates.Homeowner_Form.FieldNames.Way_Finder_Steps]))
                },
                form = new MobilehomeFormData()
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
            var MobilehomeForm = ApiRepository.GetMobilehomeForm();
            return CommonRepository.GetFormUrl(MobilehomeForm);
        }

        public List<KeyValuePair<string, string>> RetrieveFormByKey(string key)
        {
            Quote quote = _quoteRepository.GetBySaveForLaterKey(new Guid(key));
            if (quote == null) 
                return new List<KeyValuePair<string, string>>();
            QuoteContact contact = _contactRepository.GetByKey(quote.Key);
            QuoteMobilehome mobilehome = _quoteMobilehomeRepository.GetByKey(quote.Key);
            MobilehomeFormSaveModel model = new MobilehomeFormSaveModel();
            model.ReverseMap(quote, contact, mobilehome);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            return keyValueModel;
        }

        private CommonFormSaveResponseModel Save(MobilehomeFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            var quote = new Quote();
            quote.Map("Mobilehome", form, isSubmitted, returningVisit);
            if (returningVisit)
            {
                _quoteRepository.Update(quote);
            }
            else
            {
                var quoteRecord = _quoteRepository.RecordExists(quote);
                if (quoteRecord == null)
                {
                    quote.Key = _quoteRepository.Create(quote);
                }
                else
                {
                    quote.Key = quoteRecord.Key;
                    quote.Remarks = quoteRecord.Remarks;
                    quote.ReadDisclaimer = quoteRecord.ReadDisclaimer;
                    quote.Finished = quoteRecord.Finished;
                    quote.ResponseType = quoteRecord.ResponseType;
                    quote.ResponseDescription = quoteRecord.ResponseDescription;
                    quote.Offer = quoteRecord.Offer;
                    quote.OfferDescription = quoteRecord.OfferDescription;
                    quote.IP_Address = quoteRecord.IP_Address;
                    quote.ExtraInfo = quoteRecord.ExtraInfo;
                    quote.IsSuspicious = quoteRecord.IsSuspicious;
                    quote.IsInterested = quoteRecord.IsInterested;
                    quote.SaveForLaterKey = quoteRecord.SaveForLaterKey;
                    quote.SaveForLaterCreateDate = quoteRecord.SaveForLaterCreateDate;

                    _quoteRepository.Update(quote);
                }
            }

            var quoteContact = new QuoteContact();
            quoteContact.Map(form, quote.Key);
            _contactRepository.CreateOrUpdate(quoteContact);

            var mobilehome = new QuoteMobilehome();
            mobilehome.Map(form, quote.Key);
            _quoteMobilehomeRepository.CreateOrUpdate(mobilehome);

            return new CommonFormSaveResponseModel() { QuoteKey = quote.SaveForLaterKey, QuoteId = quote.Key, MemberNumber = form.MemberNumber?.ToString() ?? string.Empty };
        }

        public CommonFormSaveResponseModel SubmitForm(MobilehomeFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public CommonFormSaveResponseModel SaveForm(MobilehomeFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        private UniqueMobilehomeFormData GetUniqueValues(Item uniqueFormValues)
        {
            UniqueMobilehomeFormData uniqueData = new UniqueMobilehomeFormData
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
            return GetSimpleFieldFormValue(CoverageTypes.Mobilehome, Identifiers.Templates.Homeowner_Form.FieldIds.Associated_Customer_Service_Phone);

        }
    }
}