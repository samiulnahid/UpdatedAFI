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
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.WatercraftForm;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Identifiers = AFI.Feature.Identifiers;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
    public interface IWatercraftQuoteFormRepository
    {
        WatercraftForm GetForm();
        string GetFormUrl();
        List<KeyValuePair<string, string>> RetrieveFormByKey(string key);
        CommonFormSaveResponseModel SubmitForm(WatercraftFormSaveModel form);
        CommonFormSaveResponseModel SaveForm(WatercraftFormSaveModel form);
        string GetFormCustomerServicePhone();
    }

    public class WatercraftQuoteFormRepository : QuoteFormRepository, IWatercraftQuoteFormRepository
    {

        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteContactRepository _contactRepository;
        private readonly IQuoteWatercraftVehicleRepository _watercraftVehicleRepository;
        private readonly IQuoteAutoDriverRepository _autoDriverRepository;
        private readonly IQuoteAutoIncidentRepository _incidentRepository;
        private readonly IQuoteAutoRepository _autoRepository;


        public WatercraftQuoteFormRepository(IAPIRepository apiRepository, ICommonRepository commonRepository, IQuoteRepository quoteRepository, IQuoteContactRepository contactRepository, IQuoteWatercraftVehicleRepository watercraftVehicleRepository, IQuoteAutoDriverRepository autoDriverRepository, IQuoteAutoIncidentRepository incidentRepository, IQuoteAutoRepository autoRepository) : base(apiRepository, commonRepository)
        {
            _quoteRepository = quoteRepository;
            _contactRepository = contactRepository;
            _watercraftVehicleRepository = watercraftVehicleRepository;
            _autoDriverRepository = autoDriverRepository;
            _incidentRepository = incidentRepository;
            _autoRepository = autoRepository;
        }

        public WatercraftForm GetForm()
        {
            Item watercraftForm = ApiRepository.GetWatercraftForm();
            if (watercraftForm == null) return null;
            var formBlob = new WatercraftForm()
            {
                wayfinder = new Wayfinder()
                {
                    steps = CommonRepository.GetSteps(new MultilistField(watercraftForm.Fields[Identifiers.Templates.Watercraft.FieldNames.Way_Finder_Steps]))
                },
                form = new WatercraftFormData()
                {
                    id = watercraftForm[Identifiers.Templates.Watercraft.FieldNames.ID],
                    backButtonText = watercraftForm[Identifiers.Templates.Watercraft.FieldNames.Back_Button_Text],
                    preSaveButtonText = watercraftForm[Identifiers.Templates.Watercraft.FieldNames.Pre_Save_Button_Text],
                    saveButtonText = watercraftForm[Identifiers.Templates.Watercraft.FieldNames.Save_Button_Text],
                    nextButtonText = watercraftForm[Identifiers.Templates.Watercraft.FieldNames.Next_Button_Text],
                    submitButtonText = watercraftForm[Identifiers.Templates.Watercraft.FieldNames.Submit_Button_Text],
                    selectMenuDefaultText = watercraftForm[Identifiers.Templates.Watercraft.FieldNames.Select_Menu_Default_Text],
                    saveSuccessRedirect = ((LinkField)watercraftForm.Fields[Identifiers.Templates.Watercraft.FieldNames.SaveSuccessRedirect]).GetFriendlyUrl(),
                    submitSuccessRedirect = ((LinkField)watercraftForm.Fields[Identifiers.Templates.Watercraft.FieldNames.SubmitSuccessRedirect]).GetFriendlyUrl(),
                    sections = new List<Section>(),
                    common = new CommonData(),
                    unique = GetUniqueValues(watercraftForm?.Axes.GetDescendants().FirstOrDefault(x => x.TemplateID == Identifiers.Templates.Unique_Watercraft_Form_Fields.TemplateId))
                }
            };
            IEnumerable<Item> sections = ApiRepository.GetCommonSections();
            formBlob.form.sections.AddRange(FillFormSections(sections, watercraftForm, CommonRepository));
            return formBlob;
        }

        public string GetFormUrl()
        {
            var watercraftForm = ApiRepository.GetWatercraftForm();
            return CommonRepository.GetFormUrl(watercraftForm);
        }

        private UniqueWatercraftData GetUniqueValues(Item uniqueItem)
        {
            if (uniqueItem == null) return null;
            UniqueWatercraftData uniqueWatercraftData = new UniqueWatercraftData()
            {
                addDriverHeadings = new Heading
                {
                    married = uniqueItem[Identifiers.Templates.Unique_Watercraft_Form_Fields.FieldNames.Married],
                    cohabitant = uniqueItem[Identifiers.Templates.Unique_Watercraft_Form_Fields.FieldNames.Cohabitant],
                    additionalDriver = uniqueItem[Identifiers.Templates.Unique_Watercraft_Form_Fields.FieldNames.Additional_Driver],
                    civilunionOrdomesticpartner = "Spouse Information"
                },
                vehicleStartYear = uniqueItem[Identifiers.Templates.Unique_Watercraft_Form_Fields.FieldNames.Vehicle_Start_Year],
                addDriverButtonText = uniqueItem[Identifiers.Templates.Unique_Watercraft_Form_Fields.FieldNames.Add_Driver_Button],
                addVehicleButtonText = uniqueItem[Identifiers.Templates.Unique_Watercraft_Form_Fields.FieldNames.Add_Vehicle_Button],
                addViolationButtonText = uniqueItem[Identifiers.Templates.Unique_Watercraft_Form_Fields.FieldNames.Add_Violation_Button],
                pipAmounts = CommonRepository.GetPipAmounts(GetCommonFormFieldsItemChildren(Identifiers.Templates.PipAmounts.TemplateId)),
                bodilyInjuryPropertyDamageSublabel = uniqueItem[Identifiers.Templates.Unique_Watercraft_Form_Fields.FieldNames.Bodily_Injury_Property_Damage_Sub_Label],
                medicalCoverageSublabel = uniqueItem[Identifiers.Templates.Unique_Watercraft_Form_Fields.FieldNames.Medical_Coverage_Sub_Label],
                uninsuredBoatersCoverageSublabel = uniqueItem[Identifiers.Templates.Unique_Watercraft_Form_Fields.FieldNames.Uninsured_Boaters_Coverage_Sub_Label],
                hullMaterials = CommonRepository.GetOptions(uniqueItem.Axes.GetDescendants().SingleOrDefault(x => x.TemplateID == Identifiers.Templates.Hull_Materials.TemplateId)?.Axes.GetDescendants()),
                propulsionTypes = CommonRepository.GetOptions(uniqueItem.Axes.GetDescendants().SingleOrDefault(x => x.TemplateID == Identifiers.Templates.Propulsion_Types.TemplateId)?.Axes.GetDescendants()),
                overallCoveragesSidebars = GetOverallCoverages(uniqueItem),
                householdViolationTypes = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Identifiers.Templates.Household_Violations_Types.TemplateId)),
                physicalDamageComprehensiveDeductibles = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Templates.Physical_Damage_Comprehensive_Deductibles.TemplateId)),
                physicalDamageDeductibles = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Identifiers.Templates.Physical_Damage_Deductibles.TemplateId))

            };
            return uniqueWatercraftData;
        }
        private OverallCoveragesWatercraftSidebars GetOverallCoverages(Item item)
        {
            return new OverallCoveragesWatercraftSidebars()
            {
                DefaultSidebar = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Identifiers.Templates.Unique_Watercraft_Form_Fields.FieldNames.Default_Sidebar])),
                BodilyInjuryPropertyDamage = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Identifiers.Templates.Unique_Watercraft_Form_Fields.FieldNames.Bodily_Injury_Property_Damage])),
                MedicalCoverage = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Identifiers.Templates.Unique_Watercraft_Form_Fields.FieldNames.Medical_Coverage])),
                UninsuredBoatersCoverage = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Identifiers.Templates.Unique_Watercraft_Form_Fields.FieldNames.Uninsured_Boaters_Coverage]))
            };
        }


        public List<KeyValuePair<string, string>> RetrieveFormByKey(string key)
        {
            Quote quote = _quoteRepository.GetBySaveForLaterKey(new Guid(key));
            if (quote == null) return new List<KeyValuePair<string, string>>();
            QuoteContact contact = _contactRepository.GetByKey(quote.Key);
            List<QuoteWatercraftVehicle> vehicles = _watercraftVehicleRepository.GetAllForQuote(quote.Key).ToList();
            List<QuoteAutoDriver> drivers = _autoDriverRepository.GetAllForQuote(quote.Key).ToList();
            List<QuoteAutoIncident> incidents = _incidentRepository.GetAllForQuote(quote.Key).ToList();
            QuoteAuto auto = _autoRepository.GetByKey(quote.Key);
            WatercraftFormSaveModel model = new WatercraftFormSaveModel();
            model.ReverseMap(quote, contact, vehicles, drivers, incidents, auto);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            return keyValueModel;
        }

        private CommonFormSaveResponseModel Save(WatercraftFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            var quote = new Quote();
            quote.Map(CoverageTypes.Watercraft, form, isSubmitted, returningVisit);
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

            quoteContact.PropertyCity = form.GetPropertyValueByIndex<WatercraftFormSaveModel, string>("Vehicle", "City", 0) ?? form.PolicyHolderCity;
            quoteContact.PropertyState = form.GetPropertyValueByIndex<WatercraftFormSaveModel, string>("Vehicle", "State", 0) ?? form.PolicyHolderState;
            quoteContact.PropertyStreet = form.GetPropertyValueByIndex<WatercraftFormSaveModel, string>("Vehicle", "Address", 0) ?? form.PolicyHolderMailingAddress;
            quoteContact.PropertyZipCode = form.GetPropertyValueByIndex<WatercraftFormSaveModel, string>("Vehicle", "Zip", 0) ?? form.PolicyHolderZip;
            
            _contactRepository.CreateOrUpdate(quoteContact);

            List<int> vehicleIndexes = form.FormObjectIndexExtractor("Vehicle", "Year");
            List<QuoteWatercraftVehicle> vehicles = new List<QuoteWatercraftVehicle>();
            vehicleIndexes.ForEach(v =>
            {
                var vehicle = new QuoteWatercraftVehicle();
                vehicle.Map(form, quote.Key, v);
                vehicles.Add(vehicle);
            });
            if (vehicles.Any())
            {
                _watercraftVehicleRepository.CreateOrUpdate(vehicles);
            }

            var auto = new QuoteAuto();
            auto.Map(form, quote.Key);
            _autoRepository.CreateOrUpdate(auto);

            List<QuoteAutoDriver> drivers = new List<QuoteAutoDriver>();
            var quotePolicyHolderDriver = new QuoteAutoDriver();
            quotePolicyHolderDriver.MapPolicyHolder(form, quote.Key);
            drivers.Add(quotePolicyHolderDriver);
            
            bool availableSpouseDriver = string.Equals(quoteContact.MaritalStatus, "Married", StringComparison.InvariantCultureIgnoreCase) || string.Equals(quoteContact.MaritalStatus, "Cohabitant", StringComparison.InvariantCultureIgnoreCase);
            if (availableSpouseDriver && (form.CohabitantIsOperator || form.CohabitantWatercraftOperator))
            {
                var quoteSpouseDriver = new QuoteAutoDriver();
                quoteSpouseDriver.MapCohabitant(form, quote.Key);
                drivers.Add(quoteSpouseDriver);
            }

            List<int> additionalDriverIndexes = form.FormObjectIndexExtractor("AdditionalDriver", "FirstName");
            foreach (var driverIndex in additionalDriverIndexes)
            {
                var additionalDriver = new QuoteAutoDriver();
                additionalDriver.MapDriver(form, quote.Key, driverIndex);
                drivers.Add(additionalDriver);
            }

            _autoDriverRepository.CreateOrUpdate(drivers);

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

                    incident.DriverName = GetAutoIncidentDriverName(form, driverNameKey);
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

        public CommonFormSaveResponseModel SubmitForm(WatercraftFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public CommonFormSaveResponseModel SaveForm(WatercraftFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        public string GetFormCustomerServicePhone()
        {
            return GetSimpleFieldFormValue(CoverageTypes.Watercraft, Identifiers.Templates.Watercraft.FieldIds.Associated_Customer_Service_Phone);
        }
        private static string GetAutoIncidentDriverName(WatercraftFormSaveModel form, string driverNameKey)
        {
            if (driverNameKey.ToLower().StartsWith("additionaldriver"))
            {
                if (driverNameKey[driverNameKey.Length - 1].ToString() == "0")
                    return form.AdditionalDriver0FirstName + " " + form.AdditionalDriver0LastName;
                if (driverNameKey[driverNameKey.Length - 1].ToString() == "1")
                    return form.AdditionalDriver1FirstName + " " + form.AdditionalDriver1LastName;
                if (driverNameKey[driverNameKey.Length - 1].ToString() == "2")
                    return form.AdditionalDriver2FirstName + " " + form.AdditionalDriver2LastName;
                if (driverNameKey[driverNameKey.Length - 1].ToString() == "3")
                    return form.AdditionalDriver3FirstName + " " + form.AdditionalDriver3LastName;
                if (driverNameKey[driverNameKey.Length - 1].ToString() == "4")
                    return form.AdditionalDriver4FirstName + " " + form.AdditionalDriver4LastName;
            }
            else if (driverNameKey.ToLower().StartsWith("cohabitant"))
                return form.CohabitantFirstName + " " + form.CohabitantLastName;
            else if (driverNameKey.ToLower().StartsWith("policyholder"))
                return form.PolicyHolderFirstName + " " + form.PolicyHolderLastName;

            return null;
        }
    }
}