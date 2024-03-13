using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Identifiers = AFI.Feature.Identifiers;
using Sitecore.Data.Fields;
using System;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.QuoteLeadForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.Data.Repositories;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.BusinessForm;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
    
    public interface IQuoteLeadFormRepository
    {
        QuoteLeadForm GetForm();
        string GetFormUrl();
        List<KeyValuePair<string, string>> RetrieveFormByKey(string key);
        CommonFormSaveResponseModel SubmitForm(QuoteLeadFormSaveModel form);
        CommonFormSaveResponseModel SaveForm(QuoteLeadFormSaveModel form);
        string GetFormCustomerServicePhone();
    }

    public class QuoteLeadFormRepository : QuoteFormRepository, IQuoteLeadFormRepository
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteContactRepository _contactRepository;
        private readonly IQuoteLeadRepository _quoteLeadRepository;

        public QuoteLeadFormRepository(IAPIRepository apiRepository, ICommonRepository commonRepository, IQuoteRepository quoteRepository, IQuoteContactRepository contactRepository, IQuoteLeadRepository quoteLeadRepository) : base(apiRepository, commonRepository)
        {
            _quoteRepository = quoteRepository;
            _contactRepository = contactRepository;
            _quoteLeadRepository = quoteLeadRepository;
        }

        public bool IsValid(string coverageType)
        {
            return string.Equals(coverageType, CoverageTypes.Quote_Lead, StringComparison.InvariantCultureIgnoreCase);
        }

        public QuoteLeadForm GetForm()
        {
            Item quoteLeadFormtem = ApiRepository.GetQuoteLeadForm();
            if (quoteLeadFormtem == null) return null;
            var formBlob = new QuoteLeadForm
            {
                wayfinder = new Wayfinder
                {
                    steps = CommonRepository.GetSteps(new MultilistField(quoteLeadFormtem.Fields[Templates.Business_Form.FieldNames.Way_Finder_Steps]))
                },
                form = new QuoteLeadFormData
                {
                    id = quoteLeadFormtem[Templates.Business_Form.FieldNames.ID],
                    backButtonText = quoteLeadFormtem[Templates.Business_Form.FieldNames.Back_Button_Text],
                    preSaveButtonText = quoteLeadFormtem[Templates.Business_Form.FieldNames.Pre_Save_Button_Text],
                    saveButtonText = quoteLeadFormtem[Templates.Business_Form.FieldNames.Save_Button_Text],
                    nextButtonText = quoteLeadFormtem[Templates.Business_Form.FieldNames.Next_Button_Text],
                    submitButtonText = quoteLeadFormtem[Templates.Business_Form.FieldNames.Submit_Button_Text],
                    selectMenuDefaultText = quoteLeadFormtem[Templates.Business_Form.FieldNames.Select_Menu_Default_Text],
                    saveSuccessRedirect = ((LinkField)quoteLeadFormtem.Fields[Templates.Business_Form.FieldNames.SaveSuccessRedirect]).GetFriendlyUrl(),
                    submitSuccessRedirect = ((LinkField)quoteLeadFormtem.Fields[Templates.Business_Form.FieldNames.SubmitSuccessRedirect]).GetFriendlyUrl(),
                    sections = new List<Section>(),
                    common = new CommonData(),
                    unique = new UniqueQuoteLeadFormData()
                }
            };
            IEnumerable<Item> sections = ApiRepository.GetCommonSections();
            formBlob.form.sections.AddRange(FillFormSections(sections, quoteLeadFormtem, CommonRepository));
            return formBlob;
        }

        public string GetFormUrl()
        {
            var quoteLeadForm = ApiRepository.GetQuoteLeadForm();
            return CommonRepository.GetFormUrl(quoteLeadForm);
        }

        public List<KeyValuePair<string, string>> RetrieveFormByKey(string key)
        {
            Quote quote = _quoteRepository.GetBySaveForLaterKey(new Guid(key));
            if (quote == null) return new List<KeyValuePair<string, string>>();
            QuoteContact contact = _contactRepository.GetByKey(quote.Key);
            QuoteLead commercial = _quoteLeadRepository.GetByKey(quote.Key);
            QuoteLeadFormSaveModel model = new QuoteLeadFormSaveModel();
            model.ReverseMap(quote, contact, commercial);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            return keyValueModel;
        }

        private CommonFormSaveResponseModel Save(QuoteLeadFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            var quote = new Quote();
            form.EligibilityStatus = "military";
            form.ResponseDescription = String.IsNullOrEmpty(form.ResponseType) && String.IsNullOrEmpty(form.ResponseDescription) ? "8942- MVP/VAREP" : form.ResponseDescription;
            isSubmitted = true;
            quote.Map(CoverageTypes.Quote_Lead, form, isSubmitted, returningVisit);
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

            var quoteLead = new QuoteLead();
            quoteLead.Map(form, quote.Key);
            _quoteLeadRepository.CreateOrUpdate(quoteLead);

            return new CommonFormSaveResponseModel() { QuoteId = quote.Key, QuoteKey = quote.SaveForLaterKey, MemberNumber = form.MemberNumber?.ToString() ?? string.Empty };
        }

        public CommonFormSaveResponseModel SubmitForm(QuoteLeadFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public CommonFormSaveResponseModel SaveForm(QuoteLeadFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        public string GetFormCustomerServicePhone()
        {
            return GetSimpleFieldFormValue(CoverageTypes.Quote_Lead, Identifiers.Templates.Business_Form.FieldIds.Associated_Customer_Service_Phone);
        }
    }
}