using System;
using System.Collections.Generic;
using System.Linq;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Repositories;
using AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.UmbrellaForm;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
	public interface IUmbrellaQuoteFormRepository
	{
		UmbrellaForm GetForm();
        string GetFormUrl();
        List<KeyValuePair<string, string>> RetrieveFormByKey(string key);
        CommonFormSaveResponseModel SubmitForm(UmbrellaFormSaveModel form);
        CommonFormSaveResponseModel SaveForm(UmbrellaFormSaveModel form);
        string GetFormCustomerServicePhone();
    }

	public class UmbrellaQuoteFormRepository : QuoteFormRepository, IUmbrellaQuoteFormRepository
	{
        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteContactRepository _contactRepository;
        private readonly IQuoteUmbrellaRepository _quoteUmbrellaRepository;
        private readonly IQuoteUmbrellaVehicleRepository _quoteUmbrellaVehicleRepository;
        private readonly IQuoteUmbrellaWatercraftRepository _quoteUmbrellaWatercraftRepository;
        private readonly IQuoteAutoIncidentRepository _quoteAutoIncidentRepository;

        public UmbrellaQuoteFormRepository(IAPIRepository apiRepository, ICommonRepository commonRepository, IQuoteRepository quoteRepository, IQuoteContactRepository contactRepository, IQuoteUmbrellaRepository quoteUmbrellaRepository, IQuoteUmbrellaVehicleRepository quoteUmbrellaVehicleRepository, IQuoteUmbrellaWatercraftRepository quoteUmbrellaWatercraftRepository, IQuoteAutoIncidentRepository quoteAutoIncidentRepository) : base(apiRepository, commonRepository)
        {
            _quoteRepository = quoteRepository;
            _contactRepository = contactRepository;
            _quoteUmbrellaRepository = quoteUmbrellaRepository;
            _quoteUmbrellaVehicleRepository = quoteUmbrellaVehicleRepository;
            _quoteUmbrellaWatercraftRepository = quoteUmbrellaWatercraftRepository;
            _quoteAutoIncidentRepository = quoteAutoIncidentRepository;
        }

        public UmbrellaForm GetForm()
        {
            Item umbrellaForm = ApiRepository.GetUmbrellaForm();
            if (umbrellaForm == null) return null;
            var formBlob = new UmbrellaForm
            {
                wayfinder = new Wayfinder
                {
                    steps = CommonRepository.GetSteps(new MultilistField(umbrellaForm.Fields[Templates.Umbrella_Form.FieldNames.Way_Finder_Steps]))
                },
                form = new UmbrellaFormData
                {
                    id = umbrellaForm[Templates.Umbrella_Form.FieldNames.ID],
                    backButtonText = umbrellaForm[Templates.Umbrella_Form.FieldNames.Back_Button_Text],
                    preSaveButtonText = umbrellaForm[Templates.Umbrella_Form.FieldNames.Pre_Save_Button_Text],
                    saveButtonText = umbrellaForm[Templates.Umbrella_Form.FieldNames.Save_Button_Text],
                    nextButtonText = umbrellaForm[Templates.Umbrella_Form.FieldNames.Next_Button_Text],
                    submitButtonText = umbrellaForm[Templates.Umbrella_Form.FieldNames.Submit_Button_Text],
                    selectMenuDefaultText = umbrellaForm[Templates.Umbrella_Form.FieldNames.Select_Menu_Default_Text],
                    saveSuccessRedirect = ((LinkField)umbrellaForm.Fields[Templates.Umbrella_Form.FieldNames.SaveSuccessRedirect]).GetFriendlyUrl(),
                    submitSuccessRedirect = ((LinkField)umbrellaForm.Fields[Templates.Umbrella_Form.FieldNames.SubmitSuccessRedirect]).GetFriendlyUrl(),
                    sections = new List<Section>(),
                    common = new CommonData(),
                    unique = GetUniqueFormFields(umbrellaForm?.Axes.GetDescendants().FirstOrDefault(x => x.TemplateID == Templates.Unique_Umbrella_Form_Fields.TemplateId))
                }
            };
            IEnumerable<Item> sections = ApiRepository.GetCommonSections();
            formBlob.form.sections.AddRange(FillFormSections(sections, umbrellaForm, CommonRepository));
            return formBlob;
        }

        public string GetFormUrl()
        {
            var umbrellaForm = ApiRepository.GetUmbrellaForm();
            return CommonRepository.GetFormUrl(umbrellaForm);
        }

        private UniqueUmbrellaData GetUniqueFormFields(Item umbrellaForm)
        {
            if (umbrellaForm == null) return null;
            var uniqueFormFields = new UniqueUmbrellaData
            {
                AddViolationText = umbrellaForm[Templates.Unique_Umbrella_Form_Fields.FieldNames.Add_Violation_Text],
                AddAnotherRecreationalVehicleText = umbrellaForm[Templates.Unique_Umbrella_Form_Fields.FieldNames.Add_Another_Recreational_Vehicle_Text],
                AddAnotherWatercraftText = umbrellaForm[Templates.Unique_Umbrella_Form_Fields.FieldNames.Add_Another_Watercraft_Text],
                HouseholdViolationTypes = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Templates.Household_Violations_Types.TemplateId)),
            };
            return uniqueFormFields;
        }

        public List<KeyValuePair<string, string>> RetrieveFormByKey(string key)
        {
            Quote quote = _quoteRepository.GetBySaveForLaterKey(new Guid(key));
            if (quote == null) return new List<KeyValuePair<string, string>>();
            QuoteContact contact = _contactRepository.GetByKey(quote.Key);
            QuoteUmbrella umbrella = _quoteUmbrellaRepository.GetByKey(quote.Key);
            List<QuoteUmbrellaVehicle> umbrellaVehicles = _quoteUmbrellaVehicleRepository.GetAllForQuote(quote.Key).ToList(); //TODO: CHECK THIS!!
            List<QuoteUmbrellaWatercraft> umbrellaWatercrafts = _quoteUmbrellaWatercraftRepository.GetAllForQuote(quote.Key).ToList(); //TODO: CHECK THIS!!
            List<QuoteAutoIncident> autoIncidents = _quoteAutoIncidentRepository.GetAllForQuote(quote.Key).ToList();

            UmbrellaFormSaveModel model = new UmbrellaFormSaveModel();
            model.ReverseMap(quote, contact, umbrella, umbrellaVehicles, umbrellaWatercrafts, autoIncidents);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            return keyValueModel;
        }

        private CommonFormSaveResponseModel Save(UmbrellaFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            var quote = new Quote();
            quote.Map(CoverageTypes.Umbrella, form, isSubmitted, returningVisit);
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

            var quoteUmbrella = new QuoteUmbrella();
            quoteUmbrella.Map(form, quote.Key);
            _quoteUmbrellaRepository.CreateOrUpdate(quoteUmbrella);

            if (form.DoYouOwnAnyRecreationalVehicles)
            {
                List<QuoteUmbrellaVehicle> quoteUmbrellaVehicles = new List<QuoteUmbrellaVehicle>();
                if (!string.IsNullOrEmpty(form.Type0OfRecreationalVehicle))
                {
                    QuoteUmbrellaVehicle quv = new QuoteUmbrellaVehicle { VehicleType=form.Type0OfRecreationalVehicle, VehicleUnderlyingInsurance=form.Recreational0VehicleLiability, QuoteKey= quote.Key, Key=0 };
                    quoteUmbrellaVehicles.Add(quv);
                }
                if (!string.IsNullOrEmpty(form.Type1OfRecreationalVehicle))
                {
                    QuoteUmbrellaVehicle quv = new QuoteUmbrellaVehicle { VehicleType = form.Type1OfRecreationalVehicle, VehicleUnderlyingInsurance = form.Recreational1VehicleLiability, QuoteKey = quote.Key, Key = 1 };
                    quoteUmbrellaVehicles.Add(quv);
                }
                if (!string.IsNullOrEmpty(form.Type2OfRecreationalVehicle))
                {
                    QuoteUmbrellaVehicle quv = new QuoteUmbrellaVehicle { VehicleType = form.Type2OfRecreationalVehicle, VehicleUnderlyingInsurance = form.Recreational2VehicleLiability, QuoteKey = quote.Key, Key = 2 };
                    quoteUmbrellaVehicles.Add(quv);
                }
                if (!string.IsNullOrEmpty(form.Type3OfRecreationalVehicle))
                {
                    QuoteUmbrellaVehicle quv = new QuoteUmbrellaVehicle { VehicleType = form.Type3OfRecreationalVehicle, VehicleUnderlyingInsurance = form.Recreational3VehicleLiability, QuoteKey = quote.Key, Key = 3 };
                    quoteUmbrellaVehicles.Add(quv);
                }
                if (!string.IsNullOrEmpty(form.Type4OfRecreationalVehicle))
                {
                    QuoteUmbrellaVehicle quv = new QuoteUmbrellaVehicle { VehicleType = form.Type4OfRecreationalVehicle, VehicleUnderlyingInsurance = form.Recreational4VehicleLiability, QuoteKey = quote.Key, Key = 4 };
                    quoteUmbrellaVehicles.Add(quv);
                }
                if (!string.IsNullOrEmpty(form.Type5OfRecreationalVehicle))
                {
                    QuoteUmbrellaVehicle quv = new QuoteUmbrellaVehicle { VehicleType = form.Type5OfRecreationalVehicle, VehicleUnderlyingInsurance = form.Recreational5VehicleLiability, QuoteKey = quote.Key, Key = 5};
                    quoteUmbrellaVehicles.Add(quv);
                }

                _quoteUmbrellaVehicleRepository.CreateOrUpdate(quoteUmbrellaVehicles);
            }

            if (form.DoYouOwnAnyWatercraft)
            {
                List<QuoteUmbrellaWatercraft> quoteUmbrellaVehicles = new List<QuoteUmbrellaWatercraft>();

                if (!string.IsNullOrEmpty(form.Type0OfWatercraft))
                {
                    QuoteUmbrellaWatercraft quv = new QuoteUmbrellaWatercraft { WatercraftType=form.Type0OfWatercraft, WatercraftLength=form.Length0OfWatercraft, WatercraftHorsepower=form.Horsepower0OfWatercraft, WatercraftUnderlyingInsurance=form.Watercraft0Liability, QuoteKey = quote.Key, Key = 0 };
                    quoteUmbrellaVehicles.Add(quv);
                }
                if (!string.IsNullOrEmpty(form.Type1OfWatercraft))
                {
                    QuoteUmbrellaWatercraft quv = new QuoteUmbrellaWatercraft { WatercraftType = form.Type1OfWatercraft, WatercraftLength = form.Length1OfWatercraft, WatercraftHorsepower = form.Horsepower1OfWatercraft, WatercraftUnderlyingInsurance = form.Watercraft1Liability, QuoteKey = quote.Key, Key = 1 };
                    quoteUmbrellaVehicles.Add(quv);
                }
                if (!string.IsNullOrEmpty(form.Type2OfWatercraft))
                {
                    QuoteUmbrellaWatercraft quv = new QuoteUmbrellaWatercraft { WatercraftType = form.Type2OfWatercraft, WatercraftLength = form.Length2OfWatercraft, WatercraftHorsepower = form.Horsepower2OfWatercraft, WatercraftUnderlyingInsurance = form.Watercraft2Liability, QuoteKey = quote.Key, Key = 2 };
                    quoteUmbrellaVehicles.Add(quv);
                }
                if (!string.IsNullOrEmpty(form.Type3OfWatercraft))
                {
                    QuoteUmbrellaWatercraft quv = new QuoteUmbrellaWatercraft { WatercraftType = form.Type3OfWatercraft, WatercraftLength = form.Length3OfWatercraft, WatercraftHorsepower = form.Horsepower3OfWatercraft, WatercraftUnderlyingInsurance = form.Watercraft3Liability, QuoteKey = quote.Key, Key = 3 };
                    quoteUmbrellaVehicles.Add(quv);
                }
                if (!string.IsNullOrEmpty(form.Type4OfWatercraft))
                {
                    QuoteUmbrellaWatercraft quv = new QuoteUmbrellaWatercraft { WatercraftType = form.Type4OfWatercraft, WatercraftLength = form.Length4OfWatercraft, WatercraftHorsepower = form.Horsepower4OfWatercraft, WatercraftUnderlyingInsurance = form.Watercraft4Liability, QuoteKey = quote.Key, Key = 4 };
                    quoteUmbrellaVehicles.Add(quv);
                }
                if (!string.IsNullOrEmpty(form.Type5OfWatercraft))
                {
                    QuoteUmbrellaWatercraft quv = new QuoteUmbrellaWatercraft { WatercraftType = form.Type5OfWatercraft, WatercraftLength = form.Length5OfWatercraft, WatercraftHorsepower = form.Horsepower5OfWatercraft, WatercraftUnderlyingInsurance = form.Watercraft5Liability, QuoteKey = quote.Key, Key =5 };
                    quoteUmbrellaVehicles.Add(quv);
                } 
                _quoteUmbrellaWatercraftRepository.CreateOrUpdate(quoteUmbrellaVehicles);
            }

            if (form.HouseholdViolationsPreviousClaims)
            {
                List<int> incidentIndexes = form.FormObjectIndexExtractor("HouseholdViolations", "Type");
                List<QuoteAutoIncident> incidents = new List<QuoteAutoIncident>();
                incidentIndexes.ForEach(i =>
                {
                    var incident = new QuoteAutoIncident();                                         
                    incident.Map(form, quote.Key, i);
                    incidents.Add(incident);
                });
                if (incidents.Any())
                {
                    _quoteAutoIncidentRepository.CreateOrUpdate(incidents);
                }
            }

            return new CommonFormSaveResponseModel() {QuoteKey = quote.SaveForLaterKey, QuoteId = quote.Key, MemberNumber = form.MemberNumber?.ToString() ?? string.Empty };
        }

        public CommonFormSaveResponseModel SubmitForm(UmbrellaFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public CommonFormSaveResponseModel SaveForm(UmbrellaFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        public string GetFormCustomerServicePhone()
        {
            return GetSimpleFieldFormValue(CoverageTypes.Umbrella, Templates.Umbrella_Form.FieldIds.Associated_Customer_Service_Phone);
        }
    }
}