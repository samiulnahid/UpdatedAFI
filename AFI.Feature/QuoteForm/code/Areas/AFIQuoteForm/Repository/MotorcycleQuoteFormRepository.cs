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
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MotorcycleForm;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Identifiers = AFI.Feature.Identifiers;
namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
    public interface IMotorcycleQuoteFormRepository
    {
        MotorcycleForm GetForm();
        string GetFormUrl();
        List<KeyValuePair<string, string>> RetrieveFormByKey(string key);
        CommonFormSaveResponseModel SubmitForm(MotorcycleFormSaveModel form);
        CommonFormSaveResponseModel SaveForm(MotorcycleFormSaveModel form);
        string GetFormCustomerServicePhone();
    }

    public class MotorcycleQuoteFormRepository : QuoteFormRepository, IMotorcycleQuoteFormRepository
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteContactRepository _contactRepository;
        private readonly IQuoteAutoDriverRepository _autoDriverRepository;
        private readonly IQuoteAutoIncidentRepository _incidentRepository;
        private readonly IQuoteMotorcycleVehicleRepository _motorcycleVehicleRepository;
        private readonly IQuoteAutoRepository _quoteAutoRepository;

        public MotorcycleQuoteFormRepository(IAPIRepository apiRepository, ICommonRepository commonRepository, IQuoteRepository quoteRepository, IQuoteContactRepository contactRepository, IQuoteAutoDriverRepository autoDriverRepository, IQuoteAutoIncidentRepository incidentRepository, IQuoteMotorcycleVehicleRepository motorcycleVehicleRepository, IQuoteAutoRepository quoteAutoRepository) : base(apiRepository, commonRepository)
        {
            _quoteRepository = quoteRepository;
            _contactRepository = contactRepository;
            _autoDriverRepository = autoDriverRepository;
            _incidentRepository = incidentRepository;
            _motorcycleVehicleRepository = motorcycleVehicleRepository;
            _quoteAutoRepository = quoteAutoRepository;
        }

        public MotorcycleForm GetForm()
        {
            Item motorcycleForm = ApiRepository.GetMotorcycleForm();
            if (motorcycleForm == null) return null;
            var formBlob = new MotorcycleForm()
            {
                wayfinder = new Wayfinder()
                {
                    steps = CommonRepository.GetSteps(new MultilistField(motorcycleForm.Fields[Identifiers.Templates.Motorcycle_Form.FieldNames.Way_Finder_Steps]))
                },
                form = new MotorcycleFormData()
                {
                    id = motorcycleForm[Identifiers.Templates.Motorcycle_Form.FieldNames.ID],
                    backButtonText = motorcycleForm[Identifiers.Templates.Motorcycle_Form.FieldNames.Back_Button_Text],
                    preSaveButtonText = motorcycleForm[Identifiers.Templates.Motorcycle_Form.FieldNames.Pre_Save_Button_Text],
                    saveButtonText = motorcycleForm[Identifiers.Templates.Motorcycle_Form.FieldNames.Save_Button_Text],
                    nextButtonText = motorcycleForm[Identifiers.Templates.Motorcycle_Form.FieldNames.Next_Button_Text],
                    submitButtonText = motorcycleForm[Identifiers.Templates.Motorcycle_Form.FieldNames.Submit_Button_Text],
                    selectMenuDefaultText = motorcycleForm[Identifiers.Templates.Motorcycle_Form.FieldNames.Select_Menu_Default_Text],
                    saveSuccessRedirect = ((LinkField)motorcycleForm.Fields[Identifiers.Templates.Motorcycle_Form.FieldNames.SaveSuccessRedirect]).GetFriendlyUrl(),
                    submitSuccessRedirect = ((LinkField)motorcycleForm.Fields[Identifiers.Templates.Motorcycle_Form.FieldNames.SubmitSuccessRedirect]).GetFriendlyUrl(),
                    sections = new List<Section>(),
                    common = new CommonData(),
                    unique = GetUniqueValues(motorcycleForm?.Axes.GetDescendants().FirstOrDefault(x => x.TemplateID == Identifiers.Templates.Unique_Motorcycle_Form_Fields.TemplateId))
                }
            };
            IEnumerable<Item> sections = ApiRepository.GetCommonSections();
            formBlob.form.sections.AddRange(FillFormSections(sections, motorcycleForm, CommonRepository));
            return formBlob;
        }

        public string GetFormUrl()
        {
            var motorcycleForm = ApiRepository.GetMotorcycleForm();
            return CommonRepository.GetFormUrl(motorcycleForm);
        }

        private UniqueMotorcycleData GetUniqueValues(Item uniqueItem)
        {
            if (uniqueItem == null) return null;
            UniqueMotorcycleData uniqueMotorcycleData = new UniqueMotorcycleData()
            {
                addDriverButtonText = uniqueItem[Identifiers.Templates.Unique_Motorcycle_Form_Fields.FieldNames.Add_Driver_Button],
                addVehicleButtonText = uniqueItem[Identifiers.Templates.Unique_Motorcycle_Form_Fields.FieldNames.Add_Vehicle_Button],
                addViolationButtonText = uniqueItem[Identifiers.Templates.Unique_Motorcycle_Form_Fields.FieldNames.Add_Violation_Button],
                bodilyInjuryLiabilitySublabel = uniqueItem[Identifiers.Templates.Unique_Motorcycle_Form_Fields.FieldNames.Bodily_Injury_Liability_Sub_Label],
                personalInjuryProtectionSublabel = uniqueItem[Identifiers.Templates.Unique_Motorcycle_Form_Fields.FieldNames.Personal_Injury_Protection_Sub_Label],
                propertyDamageLiabilitySublabel = uniqueItem[Identifiers.Templates.Unique_Motorcycle_Form_Fields.FieldNames.Property_Damage_Liability_Sub_Label],
                medicalPaymentSublabel = uniqueItem[Identifiers.Templates.Unique_Motorcycle_Form_Fields.FieldNames.Medical_Payment_Sub_Label],
                uninsuredMotoristBodilyInjurySublabel = uniqueItem[Identifiers.Templates.Unique_Motorcycle_Form_Fields.FieldNames.Uninsured_Motorist_Bodily_Injury_Sub_Label],
                vehicleTypes = CommonRepository.GetOptions(uniqueItem.Axes.GetDescendants().SingleOrDefault(x => x.TemplateID == Identifiers.Templates.Motorcycle_Types.TemplateId)?.Axes.GetDescendants()),
                overallCoveragesSidebars = GetoverallCoverages(uniqueItem),
                addDriverHeadings = new Heading
                {
                    married = uniqueItem[Identifiers.Templates.Unique_Motorcycle_Form_Fields.FieldNames.Married],
                    cohabitant = uniqueItem[Identifiers.Templates.Unique_Motorcycle_Form_Fields.FieldNames.Cohabitant],
                    additionalDriver = uniqueItem[Identifiers.Templates.Unique_Motorcycle_Form_Fields.FieldNames.Additional_Driver],
                    civilunionOrdomesticpartner = "Spouse Information"
                },
                householdViolationTypes = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Identifiers.Templates.Household_Violations_Types.TemplateId)),
                physicalDamageDeductibles = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Identifiers.Templates.Physical_Damage_Deductibles.TemplateId)),
                physicalDamageComprehensiveDeductibles = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Templates.Physical_Damage_Comprehensive_Deductibles.TemplateId)),
                pipAmounts = CommonRepository.GetPipAmounts(GetCommonFormFieldsItemChildren(Identifiers.Templates.PipAmounts.TemplateId)),
            };
            return uniqueMotorcycleData;
        }
        private OverallCoveragesSidebars GetoverallCoverages(Item item)
        {
            return new OverallCoveragesSidebars()
            {
                DefaultSidebar = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Templates.Unique_Motorcycle_Form_Fields.FieldNames.Default_Sidebar])),
                BodilyInjuryLiability = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Templates.Unique_Motorcycle_Form_Fields.FieldNames.Bodily_Injury_Liability_Sidebar])),
                PropertyDamageLiability = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Templates.Unique_Motorcycle_Form_Fields.FieldNames.Property_Damage_Liability_Sidebar])),
                MedicalPayment = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Templates.Unique_Motorcycle_Form_Fields.FieldNames.Medical_Payment_Sidebar])),
                UninsuredMotoristBodilyInjury = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Templates.Unique_Motorcycle_Form_Fields.FieldNames.Uninsured_Motorist_Bodily_Injury_Sidebar])),
                PersonalInjuryProtection = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Templates.Unique_Motorcycle_Form_Fields.FieldNames.Personal_Injury_Protection_Sidebar])),
            };
        }

        public List<KeyValuePair<string, string>> RetrieveFormByKey(string key)
        {
            Quote quote = _quoteRepository.GetBySaveForLaterKey(new Guid(key));
            if (quote == null) return new List<KeyValuePair<string, string>>();
            QuoteContact contact = _contactRepository.GetByKey(quote.Key);
            List<QuoteMotorcycleVehicle> motorcycles = _motorcycleVehicleRepository.GetAllForQuote(quote.Key).ToList();
            List<QuoteAutoDriver> drivers = _autoDriverRepository.GetAllForQuote(quote.Key).ToList();
            List<QuoteAutoIncident> incidents = _incidentRepository.GetAllForQuote(quote.Key).ToList();
            QuoteAuto auto = _quoteAutoRepository.GetByKey(quote.Key);
            MotorcycleFormSaveModel model = new MotorcycleFormSaveModel();
            model.ReverseMap(quote, contact, motorcycles, drivers, incidents, auto);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            return keyValueModel;
        }

        private CommonFormSaveResponseModel Save(MotorcycleFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            var quote = new Quote();
            quote.Map(CoverageTypes.Motorcycle, form, isSubmitted, returningVisit);
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

            quoteContact.PropertyCity = form.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>("Vehicle", "City", 0) ?? form.PolicyHolderCity;
            quoteContact.PropertyState = form.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>("Vehicle", "State", 0) ?? form.PolicyHolderState;
            quoteContact.PropertyStreet = form.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>("Vehicle", "Address", 0) ?? form.PolicyHolderMailingAddress;
            quoteContact.PropertyZipCode = form.GetPropertyValueByIndex<MotorcycleFormSaveModel, string>("Vehicle", "Zip", 0) ?? form.PolicyHolderZip;

            _contactRepository.CreateOrUpdate(quoteContact);

            var quoteAuto = new QuoteAuto();
            quoteAuto.Map(form, quote.Key);
            _quoteAutoRepository.CreateOrUpdate(quoteAuto);

            List<int> vehicleIndexes = form.FormObjectIndexExtractor("Vehicle", "Year");
            List<QuoteMotorcycleVehicle> vehicles = new List<QuoteMotorcycleVehicle>();
            vehicleIndexes.ForEach(v =>
            {
                var vehicle = new QuoteMotorcycleVehicle();
                vehicle.Map(form, quote.Key, v);
                vehicles.Add(vehicle);
            });
            if (vehicles.Any())
            {
                _motorcycleVehicleRepository.CreateOrUpdate(vehicles);
            }
            List<QuoteAutoDriver> drivers = new List<QuoteAutoDriver>();
            var quotePolicyHolderDriver = new QuoteAutoDriver();
            quotePolicyHolderDriver.MapPolicyHolder(form, quote.Key);
            drivers.Add(quotePolicyHolderDriver);
            bool availableSpouseDriver = string.Equals(quoteContact.MaritalStatus, "Married", StringComparison.InvariantCultureIgnoreCase) || string.Equals(quoteContact.MaritalStatus, "Cohabitant", StringComparison.InvariantCultureIgnoreCase) || string.Equals(quoteContact.MaritalStatus, "Civil Union/Domestic Partner", StringComparison.InvariantCultureIgnoreCase);
            if (availableSpouseDriver && form.CohabitantIsOperator)
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

        public CommonFormSaveResponseModel SubmitForm(MotorcycleFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public CommonFormSaveResponseModel SaveForm(MotorcycleFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        public string GetFormCustomerServicePhone()
        {
            return GetSimpleFieldFormValue(CoverageTypes.Motorcycle, Identifiers.Templates.Motorcycle_Form.FieldIds.Associated_Customer_Service_Phone);
        }

        private static string GetAutoIncidentDriverName(MotorcycleFormSaveModel form, string driverNameKey)
        {
            if (driverNameKey.ToLower().StartsWith("additionaldriver"))
            {
                if (driverNameKey[driverNameKey.Length - 1].ToString() == "0")
                    return form.AdditionalDriver0FirstName + " " + form.AdditionalDriver0LastName;
                if (driverNameKey[driverNameKey.Length - 1].ToString() == "1")
                    return form.AdditionalDriver1FirstName + " " + form.AdditionalDriver1LastName;
                if (driverNameKey[driverNameKey.Length - 1].ToString() == "2")
                    return form.AdditionalDriver2FirstName + " " + form.AdditionalDriver2LastName;
            }

            else if (driverNameKey.ToLower().StartsWith("cohabitant"))
                return form.CohabitantFirstName + " " + form.CohabitantLastName;
            else if (driverNameKey.ToLower().StartsWith("policyholder"))
                return form.PolicyHolderFirstName + " " + form.PolicyHolderLastName;

            return null;

        }
    }
}