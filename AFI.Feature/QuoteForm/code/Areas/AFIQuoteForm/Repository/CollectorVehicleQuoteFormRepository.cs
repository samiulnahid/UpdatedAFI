using System;
using System.Collections.Generic;
using System.Linq;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Repositories;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.CollectorForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.CollectorVehicleForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Identifiers = AFI.Feature.Identifiers;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
    public interface ICollectorVehicleQuoteFormRepository
    {
        CollectorVehicleForm GetForm();
        string GetFormUrl();
        List<KeyValuePair<string, string>> RetrieveFormByKey(string key);
        CommonFormSaveResponseModel SubmitForm(CollectorVehicleFormSaveModel form);
        CommonFormSaveResponseModel SaveForm(CollectorVehicleFormSaveModel form);
        string GetFormCustomerServicePhone();
    }


    public class CollectorVehicleQuoteFormRepository : QuoteFormRepository, ICollectorVehicleQuoteFormRepository
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteContactRepository _contactRepository;
        private readonly IQuoteCollectorVehicleRepository _vehicleRepository;
        private readonly IQuoteAutoIncidentRepository _incidentRepository;
        private readonly IQuoteAutoRepository _autoRepository;

        public CollectorVehicleQuoteFormRepository(IAPIRepository apiRepository, ICommonRepository commonRepository, IQuoteRepository quoteRepository, IQuoteContactRepository contactRepository, IQuoteCollectorVehicleRepository vehicleRepository, IQuoteAutoIncidentRepository incidentRepository, IQuoteAutoRepository autoRepository) : base(apiRepository, commonRepository)
        {
            _quoteRepository = quoteRepository;
            _contactRepository = contactRepository;
            _vehicleRepository = vehicleRepository;
            _incidentRepository = incidentRepository;
            _autoRepository = autoRepository;
        }

        public CollectorVehicleForm GetForm()
        {
            Item collectorForm = ApiRepository.GetCollectorVehicleForm();
            if (collectorForm == null) return null;
            var formBlob = new CollectorVehicleForm();
            formBlob.wayfinder = new Wayfinder
            {
                steps = CommonRepository.GetSteps(new MultilistField(collectorForm.Fields[Identifiers.Templates.Collector_Vehicle_Form.FieldNames.Way_Finder_Steps]))
            };
            formBlob.form = new CollectorVehicleFormData
            {
                id = collectorForm[Identifiers.Templates.Collector_Vehicle_Form.FieldNames.ID],
                backButtonText = collectorForm[Identifiers.Templates.Collector_Vehicle_Form.FieldNames.Back_Button_Text],
                preSaveButtonText = collectorForm[Identifiers.Templates.Collector_Vehicle_Form.FieldNames.Pre_Save_Button_Text],
                saveButtonText = collectorForm[Identifiers.Templates.Collector_Vehicle_Form.FieldNames.Save_Button_Text],
                nextButtonText = collectorForm[Identifiers.Templates.Collector_Vehicle_Form.FieldNames.Next_Button_Text],
                submitButtonText = collectorForm[Identifiers.Templates.Collector_Vehicle_Form.FieldNames.Submit_Button_Text],
                selectMenuDefaultText = collectorForm[Identifiers.Templates.Collector_Vehicle_Form.FieldNames.Select_Menu_Default_Text],
                saveSuccessRedirect = ((LinkField)collectorForm.Fields[Identifiers.Templates.Collector_Vehicle_Form.FieldNames.SaveSuccessRedirect]).GetFriendlyUrl(),
                submitSuccessRedirect = ((LinkField)collectorForm.Fields[Identifiers.Templates.Collector_Vehicle_Form.FieldNames.SubmitSuccessRedirect]).GetFriendlyUrl(),
                sections = new List<Section>(),
                common = new CommonData(),
                unique = GetUniqueValues(collectorForm?.Axes.GetDescendants().FirstOrDefault(x => x.TemplateID == Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.TemplateId))
            };
            IEnumerable<Item> sections = ApiRepository.GetCommonSections();
            formBlob.form.sections.AddRange(FillFormSections(sections, collectorForm, CommonRepository));
            return formBlob;
        }

        public string GetFormUrl()
        {
            var collectorForm = ApiRepository.GetCollectorVehicleForm();
            return CommonRepository.GetFormUrl(collectorForm);
        }

        private UniqueCollectorVehicleData GetUniqueValues(Item uniqueFormFields)
        {
            if (uniqueFormFields == null) return null;
            var uniqueFormData = new UniqueCollectorVehicleData
            {
                AddVehicleButtonText = uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Add_Vehicle_Button_Text],
                AddViolationButtonText = uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Add_Violation_Button_Text],
                AddDriverHeadings = new Heading
                {
                    married = uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Married],
                    cohabitant = uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Cohabitant],
                    additionalDriver = uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Additional_Driver],
                    civilunionOrdomesticpartner = "Spouse Information"
                },
                OverallCoveragesSidebars = new OverallCoveragesSidebars
                {
                    DefaultSidebar = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Default_Sidebar])),
                    PersonalInjuryProtection = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Personal_Injury_Protection_Sidebar])),
                    UninsuredMotoristBodilyInjury = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Uninsured_Motorist_Bodily_Injury_Sidebar])),
                    PropertyDamageLiability = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Property_Damage_Liability_Sidebar])),
                    MedicalPayment = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Medical_Payment_Sidebar])),
                    BodilyInjuryLiability = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Bodily_Injury_Liability_Sidebar]))
                },
                HouseholdViolationTypes = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Identifiers.Templates.Household_Violations_Types.TemplateId)),
                PhysicalDamageDeductibles = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Identifiers.Templates.Physical_Damage_Deductibles.TemplateId)),
                PhysicalDamageComprensiveDeductibles = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Identifiers.Templates.Physical_Damage_Comprehensive_Deductibles.TemplateId)),
                PipAmounts = CommonRepository.GetPipAmounts(GetCommonFormFieldsItemChildren(Identifiers.Templates.PipAmounts.TemplateId)),
                BodilyInjuryLiabilitySublabel = uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Bodily_Injury_Liability_Sub_Label],
                PropertyDamageLiabilitySublabel = uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Property_Damage_Liability_Sub_Label],
                MedicalPaymentSublabel = uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Medical_Payment_Sub_Label],
                UninsuredMotoristBodilyInjurySublabel = uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Uninsured_Motorist_Bodily_Injury_Sub_Label],
                PersonalInjuryProtectionSublabel = uniqueFormFields[Identifiers.Templates.Unique_Collector_Vehicle_Form_Fields.FieldNames.Personal_Injury_Protection_Sub_Label]
            };
            return uniqueFormData;
        }

        public List<KeyValuePair<string, string>> RetrieveFormByKey(string key)
        {
            Quote quote = _quoteRepository.GetBySaveForLaterKey(new Guid(key));
            if (quote == null) return new List<KeyValuePair<string, string>>();
            QuoteContact contact = _contactRepository.GetByKey(quote.Key);
            QuoteAuto auto = _autoRepository.GetByKey(quote.Key);
            List<QuoteCollectorVehicle> vehicles = _vehicleRepository.GetAllForQuote(quote.Key).ToList();
            List<QuoteAutoIncident> incidents = _incidentRepository.GetAllForQuote(quote.Key).ToList();
            CollectorVehicleFormSaveModel model = new CollectorVehicleFormSaveModel();
            model.ReverseMap(quote, contact, auto, vehicles, incidents);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            return keyValueModel;
        }

        private CommonFormSaveResponseModel Save(CollectorVehicleFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            var quote = new Quote();
            quote.Map(CoverageTypes.CollectorVehicle, form, isSubmitted, returningVisit);
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

            var quoteAuto = new QuoteAuto();
            quoteAuto.Map(form, quote.Key);
            _autoRepository.CreateOrUpdate(quoteAuto);

            List<int> vehicleIndexes = form.FormObjectIndexExtractor("Vehicle", "Year");
            List<QuoteCollectorVehicle> vehicles = new List<QuoteCollectorVehicle>();
            vehicleIndexes.ForEach(v =>
            {
                var vehicle = new QuoteCollectorVehicle();
                vehicle.Map(form, quote.Key, v);
                vehicles.Add(vehicle);
            });
            if (vehicles.Any())
            {
                _vehicleRepository.CreateOrUpdate(vehicles);
            }

            if (form.HouseholdViolationsPreviousClaims)
            {
                List<int> incidentIndexes = form.FormObjectIndexExtractor("HouseholdViolations", "Type");
                List<QuoteAutoIncident> incidents = new List<QuoteAutoIncident>();
                incidentIndexes.ForEach(i =>
                {
                    var incident = new QuoteAutoIncident();

                    incident.QuoteKey = quote.Key;
                    incident.Key = i;
                    string householdPrefix = "HouseholdViolations";

                    incident.DriverKey = form.GetPropertyValueByIndex<CommonFormSaveModel, int?>(householdPrefix, "DriverKey", i);
                    var driverNameKey = form.GetPropertyValueByIndex<CommonFormSaveModel, string>(householdPrefix, "Driver", i) ?? string.Empty;

                    //incident.DriverName = GetAutoIncidentDriverName(form, driverNameKey);
                    incident.DriverName = driverNameKey;
                    incident.Incident = form.GetPropertyValueByIndex<CommonFormSaveModel, string>(householdPrefix, "Type", i);
                    incident.Date = form.GetPropertyValueByIndex<CommonFormSaveModel, DateTime?>(householdPrefix, "Date", i);

                    incidents.Add(incident);
                }); 
                if (incidents.Any())
                {
                    _incidentRepository.CreateOrUpdate(incidents);
                }
            }

            return new CommonFormSaveResponseModel() { QuoteKey = quote.SaveForLaterKey, QuoteId = quote.Key, MemberNumber = form.MemberNumber?.ToString() ?? string.Empty };
        }

        public CommonFormSaveResponseModel SubmitForm(CollectorVehicleFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public CommonFormSaveResponseModel SaveForm(CollectorVehicleFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        public string GetFormCustomerServicePhone()
        {
            return GetSimpleFieldFormValue(CoverageTypes.CollectorVehicle, Identifiers.Templates.Collector_Vehicle_Form.FieldIds.Associated_Customer_Service_Phone);
        }

        private static string GetAutoIncidentDriverName(CollectorVehicleFormSaveModel form, string driverNameKey)
        {
            if (driverNameKey.ToLower().StartsWith("cohabitant"))
                return form.CohabitantFirstName + " " + form.CohabitantLastName;
            if (driverNameKey.ToLower().StartsWith("policyholder"))
                return form.PolicyHolderFirstName + " " + form.PolicyHolderLastName;

            return null;
        }
    }
}