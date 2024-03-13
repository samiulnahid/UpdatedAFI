using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Repositories;
using AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HillAFBForm;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Identifiers = AFI.Feature.Identifiers;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
    public interface IHillAFBQuoteFormRepository
    {
        HillAFBForm GetForm();
        string GetFormUrl();
        List<KeyValuePair<string, string>> RetrieveFormByKey(string key);
        CommonFormSaveResponseModel SubmitForm(HillAFBFormSaveModel form);
        CommonFormSaveResponseModel SaveForm(HillAFBFormSaveModel form);
        string GetFormCustomerServicePhone();
    }

	public class HillAFBQuoteFormRepository : QuoteFormRepository, IHillAFBQuoteFormRepository
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteContactRepository _contactRepository;
        private readonly IQuoteHillAFBRepository _hillAFBRepository;

        public HillAFBQuoteFormRepository(IAPIRepository apiRepository, ICommonRepository commonRepository, IQuoteRepository quoteRepository, IQuoteContactRepository contactRepository, IQuoteHillAFBRepository hillAFBRepository) : base(apiRepository, commonRepository)
        {
            _quoteRepository = quoteRepository;
            _contactRepository = contactRepository;
            _hillAFBRepository = hillAFBRepository;
        }

        public bool IsValid(string coverageType)
        {
            return string.Equals(coverageType, CoverageTypes.HillAFB, StringComparison.InvariantCultureIgnoreCase);
        }

        public HillAFBForm GetForm()
        {
            Item hillAFBFormItem = ApiRepository.GetHillAFBForm();
            if (hillAFBFormItem == null) return null;
            var formBlob = new HillAFBForm
            {
                wayfinder = new Wayfinder
                {
                    steps = CommonRepository.GetSteps(new MultilistField(hillAFBFormItem.Fields[Templates.Business_Form.FieldNames.Way_Finder_Steps]))
                },
                form = new HillAFBFormData
                {
                    id = hillAFBFormItem[Templates.Business_Form.FieldNames.ID],
                    backButtonText = hillAFBFormItem[Templates.Business_Form.FieldNames.Back_Button_Text],
                    preSaveButtonText = hillAFBFormItem[Templates.Business_Form.FieldNames.Pre_Save_Button_Text],
                    saveButtonText = hillAFBFormItem[Templates.Business_Form.FieldNames.Save_Button_Text],
                    nextButtonText = hillAFBFormItem[Templates.Business_Form.FieldNames.Next_Button_Text],
                    submitButtonText = hillAFBFormItem[Templates.Business_Form.FieldNames.Submit_Button_Text],
                    selectMenuDefaultText = hillAFBFormItem[Templates.Business_Form.FieldNames.Select_Menu_Default_Text],
                    saveSuccessRedirect = ((LinkField)hillAFBFormItem.Fields[Templates.Business_Form.FieldNames.SaveSuccessRedirect]).GetFriendlyUrl(),
                    submitSuccessRedirect = ((LinkField)hillAFBFormItem.Fields[Templates.Business_Form.FieldNames.SubmitSuccessRedirect]).GetFriendlyUrl(),
                    sections = new List<Section>(),
                    common = new CommonData(),
                    unique = new UniqueHillAFBFormData()
                }
            };
            IEnumerable<Item> sections = ApiRepository.GetCommonSections();
            formBlob.form.sections.AddRange(FillFormSections(sections, hillAFBFormItem, CommonRepository));
            return formBlob;
        }

        public string GetFormUrl()
        {
            var hillAFBForm = ApiRepository.GetHillAFBForm();
            return CommonRepository.GetFormUrl(hillAFBForm);
        }

        public List<KeyValuePair<string, string>> RetrieveFormByKey(string key)
        {
            Quote quote = _quoteRepository.GetBySaveForLaterKey(new Guid(key));
            if(quote == null) return new List<KeyValuePair<string, string>>();
            QuoteContact contact = _contactRepository.GetByKey(quote.Key);
            QuoteHillAFB commercial = _hillAFBRepository.GetByKey(quote.Key);
            HillAFBFormSaveModel model = new HillAFBFormSaveModel();
            model.ReverseMap(quote, contact, commercial);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            return keyValueModel;
        }

        private CommonFormSaveResponseModel Save(HillAFBFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            var quote = new Quote();
            form.EligibilityStatus = "military";
            quote.Map(CoverageTypes.HillAFB, form, isSubmitted, returningVisit);
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

            var quoteHillAFB = new QuoteHillAFB();
            quoteHillAFB.Map(form, quote.Key);
            _hillAFBRepository.CreateOrUpdate(quoteHillAFB);

            return new CommonFormSaveResponseModel() {QuoteId = quote.Key, QuoteKey = quote.SaveForLaterKey, MemberNumber = form.MemberNumber?.ToString() ?? string.Empty };
        }

        public CommonFormSaveResponseModel SubmitForm(HillAFBFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public CommonFormSaveResponseModel SaveForm(HillAFBFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        public string GetFormCustomerServicePhone()
        {
            return GetSimpleFieldFormValue(CoverageTypes.HillAFB, Identifiers.Templates.Business_Form.FieldIds.Associated_Customer_Service_Phone);
        }
    }
}