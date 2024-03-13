using System;
using System.Collections.Generic;
using System.Linq;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Repositories;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.FloodForm;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Identifiers = AFI.Feature.Identifiers;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
	public interface IFloodQuoteFormRepository
	{
		FloodForm GetForm();
        string GetFormUrl();
        List<KeyValuePair<string, string>> RetrieveFormByKey(string key);
        CommonFormSaveResponseModel SaveForm(FloodFormSaveModel form);
        CommonFormSaveResponseModel SubmitForm(FloodFormSaveModel form);
        string GetFormCustomerServicePhone();
    }

    public class FloodQuoteFormRepository : QuoteFormRepository, IFloodQuoteFormRepository
    {
		private readonly IQuoteRepository _quoteRepository;
		private readonly IQuoteContactRepository _quoteContactRepository;
		private readonly IQuoteFloodRepository _quoteFloodRepository;

        public FloodQuoteFormRepository(IAPIRepository apiRepository, ICommonRepository commonRepository, IQuoteRepository quoteRepository, IQuoteContactRepository quoteContactRepository, IQuoteFloodRepository quoteFloodRepository) : base(apiRepository, commonRepository)
		{
			_quoteContactRepository = quoteContactRepository;
			_quoteFloodRepository = quoteFloodRepository;
			_quoteRepository = quoteRepository;
		}

        public FloodForm GetForm()
        {
            Item floodForm = ApiRepository.GetFloodForm();
            if (floodForm == null) return null;
            var formBlob = new FloodForm
            {
                wayfinder = new Wayfinder()
                {
                    steps = CommonRepository.GetSteps(new MultilistField(floodForm.Fields[Identifiers.Templates.Flood_Form.FieldNames.Way_Finder_Steps]))
                },
                form = new FloodFormData
                {
                    id = floodForm[Identifiers.Templates.Flood_Form.FieldNames.ID],
                    backButtonText = floodForm[Identifiers.Templates.Flood_Form.FieldNames.Back_Button_Text],
                    preSaveButtonText = floodForm[Identifiers.Templates.Flood_Form.FieldNames.Pre_Save_Button_Text],
                    saveButtonText = floodForm[Identifiers.Templates.Flood_Form.FieldNames.Save_Button_Text],
                    nextButtonText = floodForm[Identifiers.Templates.Flood_Form.FieldNames.Next_Button_Text],
                    submitButtonText = floodForm[Identifiers.Templates.Flood_Form.FieldNames.Submit_Button_Text],
                    selectMenuDefaultText = floodForm[Identifiers.Templates.Flood_Form.FieldNames.Select_Menu_Default_Text],
                    saveSuccessRedirect = ((LinkField)floodForm.Fields[Identifiers.Templates.Flood_Form.FieldNames.SaveSuccessRedirect]).GetFriendlyUrl(),
                    submitSuccessRedirect = ((LinkField)floodForm.Fields[Identifiers.Templates.Flood_Form.FieldNames.SubmitSuccessRedirect]).GetFriendlyUrl(),
                    sections = new List<Section>(),
                    common = new CommonData(),
                    unique = GetUniqueValues(floodForm?.Axes.GetDescendants().FirstOrDefault(x => x.TemplateID == Identifiers.Templates.Unique_Flood_Form_Fields.TemplateId))
                }
            };
            IEnumerable<Item> sections = ApiRepository.GetCommonSections();
            formBlob.form.sections.AddRange(FillFormSections(sections, floodForm, CommonRepository));
            return formBlob;
        }

        public string GetFormUrl()
        {
            var floodForm = ApiRepository.GetFloodForm();
            return CommonRepository.GetFormUrl(floodForm);
        }

        public List<KeyValuePair<string, string>> RetrieveFormByKey(string key)
        {
            Quote quote = _quoteRepository.GetBySaveForLaterKey(new Guid(key));
            if (quote == null) return new List<KeyValuePair<string, string>>();
            QuoteContact contact = _quoteContactRepository.GetByKey(quote.Key);
            QuoteFlood flood = _quoteFloodRepository.GetByKey(quote.Key);
            FloodFormSaveModel model = new FloodFormSaveModel();
            model.ReverseMap(quote, contact, flood);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            return keyValueModel;
        }

        private CommonFormSaveResponseModel Save(FloodFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            var quote = new Quote();
            quote.Map(CoverageTypes.Flood, form, isSubmitted, returningVisit);
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

            var quoteFlood = new QuoteFlood();
            quoteFlood.Map(form, quote.Key);
            _quoteFloodRepository.CreateOrUpdate(quoteFlood);

            return new CommonFormSaveResponseModel() {QuoteKey = quote.SaveForLaterKey, QuoteId = quote.Key, MemberNumber = form.MemberNumber?.ToString() ?? string.Empty };
        }

        public CommonFormSaveResponseModel SubmitForm(FloodFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public string GetFormCustomerServicePhone()
        {
            return GetSimpleFieldFormValue(CoverageTypes.Flood, Identifiers.Templates.Flood_Form.FieldIds.Associated_Customer_Service_Phone);
        }

        public CommonFormSaveResponseModel SaveForm(FloodFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        private UniqueFloodData GetUniqueValues(Item uniqueFormFields)
        {
            if (uniqueFormFields == null) return null;
            var uniqueFormData = new UniqueFloodData();
            uniqueFormData.PropertyBuiltYear = uniqueFormFields[Identifiers.Templates.Unique_Flood_Form_Fields.FieldNames.Property_Built_Year];
            uniqueFormData.PropertyOldestYearText = uniqueFormFields[Identifiers.Templates.Unique_Flood_Form_Fields.FieldNames.Property_Oldest_Year_Text];
            return uniqueFormData;
        }
        
    }
}