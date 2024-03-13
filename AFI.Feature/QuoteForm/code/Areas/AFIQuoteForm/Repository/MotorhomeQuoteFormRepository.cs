using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Repositories;
using AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Motorhome;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Identifiers = AFI.Feature.Identifiers;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
    public interface IMotorhomeQuoteFormRepository
    {
        MotorhomeForm GetForm();
        string GetFormUrl();
        List<KeyValuePair<string, string>> RetrieveFormByKey(string key);
        CommonFormSaveResponseModel SubmitForm(MotorhomeFormSaveModel form);
        CommonFormSaveResponseModel SaveForm(MotorhomeFormSaveModel form);
        string GetFormCustomerServicePhone();
    }

    public class MotorhomeQuoteFormRepository : QuoteFormRepository, IMotorhomeQuoteFormRepository
    {

        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteContactRepository _contactRepository;
        private readonly IQuoteMotorhomeVehicleRepository _vehicleRepository;
        private readonly IQuoteAutoDriverRepository _driverRepository;
        private readonly IQuoteAutoRepository _autoRepository;
        private readonly IQuoteAutoIncidentRepository _incidentRepository;
        private readonly IQuoteAutoVehicleRepository _autoVehicleRepository;
        private Stopwatch _sw;

        public MotorhomeQuoteFormRepository(IAPIRepository apiRepository, ICommonRepository commonRepository, IQuoteRepository quoteRepository, IQuoteContactRepository contactRepository, IQuoteMotorhomeVehicleRepository vehicleRepository, IQuoteAutoDriverRepository driverRepository, IQuoteAutoRepository autoRepository, IQuoteAutoIncidentRepository incidentRepository, IQuoteAutoVehicleRepository autoVehicleRepository) : base(apiRepository, commonRepository)
        {
            _quoteRepository = quoteRepository;
            _contactRepository = contactRepository;
            _vehicleRepository = vehicleRepository;
            _driverRepository = driverRepository;
            _autoRepository = autoRepository;
            _incidentRepository = incidentRepository;
            _autoVehicleRepository = autoVehicleRepository;
            _sw = new Stopwatch();
        }

        public MotorhomeForm GetForm()
        {
            if(_sw == null)
                _sw = new Stopwatch();

            Sitecore.Diagnostics.Log.Info("Invoke MotorhomeForm GetForm", Constants.QUOTE_FORMS_LOGGER_NAME);

            _sw.Start();

            Item motorhomeForm = ApiRepository.GetMotorhomeForm();
            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("GetForm GetMotorhomeForm time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            if (motorhomeForm == null) 
                return null;
            
            _sw.Restart();
            var formBlob = new MotorhomeForm
            {
                wayfinder = new Wayfinder()
                {
                    steps = CommonRepository.GetSteps(new MultilistField(motorhomeForm.Fields[Identifiers.Templates.Motorhome_Form.FieldNames.Way_Finder_Steps]))
                },
                form = new MotorhomeFormData()
                {
                    id = motorhomeForm[Identifiers.Templates.Motorhome_Form.FieldNames.ID],
                    backButtonText = motorhomeForm[Identifiers.Templates.Motorhome_Form.FieldNames.Back_Button_Text],
                    preSaveButtonText = motorhomeForm[Identifiers.Templates.Motorhome_Form.FieldNames.Pre_Save_Button_Text],
                    saveButtonText = motorhomeForm[Identifiers.Templates.Motorhome_Form.FieldNames.Save_Button_Text],
                    nextButtonText = motorhomeForm[Identifiers.Templates.Motorhome_Form.FieldNames.Next_Button_Text],
                    submitButtonText = motorhomeForm[Identifiers.Templates.Motorhome_Form.FieldNames.Submit_Button_Text],
                    selectMenuDefaultText = motorhomeForm[Identifiers.Templates.Motorhome_Form.FieldNames.Select_Menu_Default_Text],
                    saveSuccessRedirect = ((LinkField)motorhomeForm.Fields[Identifiers.Templates.Motorhome_Form.FieldNames.SaveSuccessRedirect]).GetFriendlyUrl(),
                    submitSuccessRedirect = ((LinkField)motorhomeForm.Fields[Identifiers.Templates.Motorhome_Form.FieldNames.SubmitSuccessRedirect]).GetFriendlyUrl(),
                    sections = new List<Section>(),
                    common = new CommonData(),
                    unique = GetUniqueValues(motorhomeForm?.Axes.GetDescendants().FirstOrDefault(x => x.TemplateID == Identifiers.Templates.Unique_Motorhome_Form_Fields.TemplateId))
                }
            };

            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("GetForm Motorhome templates time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            _sw.Restart();
            IEnumerable<Item> sections = ApiRepository.GetCommonSections();
            formBlob.form.sections.AddRange(FillFormSections(sections, motorhomeForm, CommonRepository));

            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("GetForm Motorhome FormSections time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            Sitecore.Diagnostics.Log.Info("Exited MotorhomeForm GetForm", Constants.QUOTE_FORMS_LOGGER_NAME);
            return formBlob;
        }

        public string GetFormUrl()
        {
            if (_sw == null)
                _sw = new Stopwatch();

            Sitecore.Diagnostics.Log.Info("Invoke MotorhomeForm GetFormUrl", Constants.QUOTE_FORMS_LOGGER_NAME);

            _sw.Start();
            var motorhomeForm = ApiRepository.GetMotorhomeForm();
            var url = CommonRepository.GetFormUrl(motorhomeForm);

            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm GetFormUrl time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            Sitecore.Diagnostics.Log.Info("Exited MotorhomeForm GetFormUrl", Constants.QUOTE_FORMS_LOGGER_NAME);
            return url;
        }

        private UniqueMotorhomeFormData GetUniqueValues(Item uniqueFormValues)
        {
            if (_sw == null)
                _sw = new Stopwatch();

            Sitecore.Diagnostics.Log.Info("Invoke MotorhomeForm GetUniqueValues", Constants.QUOTE_FORMS_LOGGER_NAME);

            if (uniqueFormValues == null) return null;

            _sw.Start();
            var uniqueFormData = new UniqueMotorhomeFormData
            {
                overallCoveragesSidebars = GetOverallCoverages(uniqueFormValues),
                physicalDamageDeductibles = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Templates.Physical_Damage_Deductibles.TemplateId)),
                physicalDamageComprehensiveDeductibles = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Templates.Physical_Damage_Comprehensive_Deductibles.TemplateId)),
                personalInjuryProtectionSublabel = uniqueFormValues[Templates.Unique_Motorhome_Form_Fields.FieldNames.Personal_Injury_Protection_Sub_Label],
                pipAmounts = CommonRepository.GetPipAmounts(GetCommonFormFieldsItemChildren(Templates.PipAmounts.TemplateId)),
                vehicleTypes = CommonRepository.GetOptions(GetDescendantFieldOptions(uniqueFormValues, Templates.Vehicle_Types.TemplateId)),
                vehicleUses = CommonRepository.GetOptions(GetDescendantFieldOptions(uniqueFormValues, Templates.Vehicle_Uses.TemplateId)),
                addDriverButtonText = uniqueFormValues[Templates.Unique_Motorhome_Form_Fields.FieldNames.Add_Driver_Button_Text],
                addVehicleButtonText = uniqueFormValues[Templates.Unique_Motorhome_Form_Fields.FieldNames.Add_Vehicle_Button_Text],
                addViolationButtonText = uniqueFormValues[Templates.Unique_Motorhome_Form_Fields.FieldNames.Add_Violation_Button_Text],
                bodilyInjuryLiabilitySublabel = uniqueFormValues[Templates.Unique_Motorhome_Form_Fields.FieldNames.Bodily_Injury_Liability_Sub_Label],
                householdViolationTypes = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Templates.Household_Violations_Types.TemplateId)),
                medicalPaymentSublabel = uniqueFormValues[Templates.Unique_Motorhome_Form_Fields.FieldNames.Medical_Payment_Sub_Label],
                propertyDamageLiabilitySublabel = uniqueFormValues[Templates.Unique_Motorhome_Form_Fields.FieldNames.Property_Damage_Liability_Sub_Label],
                uninsuredMotoristBodilyInjurySublabel = uniqueFormValues[Templates.Unique_Motorhome_Form_Fields.FieldNames.Uninsured_Motorist_Bodily_Injury_Sub_Label],
                addDriverHeadings = new Heading
                {
                    married = uniqueFormValues[Identifiers.Templates.Unique_Motorhome_Form_Fields.FieldNames.Married],
                    cohabitant = uniqueFormValues[Identifiers.Templates.Unique_Motorhome_Form_Fields.FieldNames.Cohabitant],
                    additionalDriver = uniqueFormValues[Identifiers.Templates.Unique_Motorhome_Form_Fields.FieldNames.Additional_Driver]
                }
            };

            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm GetUniqueValues time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            Sitecore.Diagnostics.Log.Info("Exited MotorhomeForm GetUniqueValues", Constants.QUOTE_FORMS_LOGGER_NAME);
            return uniqueFormData;
        }

        private OverallCoveragesSidebars GetOverallCoverages(Item item)
        {
            if (_sw == null)
                _sw = new Stopwatch();

            Sitecore.Diagnostics.Log.Info("Invoke MotorhomeForm GetOverallCoverages", Constants.QUOTE_FORMS_LOGGER_NAME);
            _sw.Start();

            var sidebars = new OverallCoveragesSidebars()
            {
                DefaultSidebar = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Templates.Unique_Motorhome_Form_Fields.FieldNames.Default_Sidebar])),
                BodilyInjuryLiability = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Templates.Unique_Motorhome_Form_Fields.FieldNames.Bodily_Injury_Liability_Sidebar])),
                PropertyDamageLiability = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Templates.Unique_Motorhome_Form_Fields.FieldNames.Property_Damage_Liability_Sidebar])),
                MedicalPayment = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Templates.Unique_Motorhome_Form_Fields.FieldNames.Medical_Payment_Sidebar])),
                UninsuredMotoristBodilyInjury = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Templates.Unique_Motorhome_Form_Fields.FieldNames.Uninsured_Motorist_Bodily_Injury_Sidebar])),
                PersonalInjuryProtection = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Templates.Unique_Motorhome_Form_Fields.FieldNames.Personal_Injury_Protection_Sidebar])),
            };

            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm GetOverallCoverages time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            Sitecore.Diagnostics.Log.Info("Exited MotorhomeForm GetOverallCoverages", Constants.QUOTE_FORMS_LOGGER_NAME);
            return sidebars;
        }

        public List<KeyValuePair<string, string>> RetrieveFormByKey(string key)
        {
            if (_sw == null)
                _sw = new Stopwatch();

            Sitecore.Diagnostics.Log.Info("Invoke MotorhomeForm RetrieveFormByKey", Constants.QUOTE_FORMS_LOGGER_NAME);

            _sw.Start();

            Quote quote = _quoteRepository.GetBySaveForLaterKey(new Guid(key));
            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm RetrieveFormByKey GetBySaveForLaterKey time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);


            if (quote == null) return new List<KeyValuePair<string, string>>();
            _sw.Restart();
            
            QuoteContact contact = _contactRepository.GetByKey(quote.Key);
            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm RetrieveFormByKey QuoteContact GetByKey time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            _sw.Restart();
            QuoteMotorhomeVehicle vehicle = _vehicleRepository.GetByKey(quote.Key);
            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm RetrieveFormByKey VehicleMotorhomeVehicle GetByKey time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            _sw.Restart();
            List<QuoteAutoDriver> drivers = _driverRepository.GetAllForQuote(quote.Key).ToList();
            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm RetrieveFormByKey QuoteAutoDriver GetAllForQuote time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            _sw.Restart();
            List<QuoteAutoIncident> autoIncidents = _incidentRepository.GetAllForQuote(quote.Key).ToList();
            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm RetrieveFormByKey QuoteAutoIncident GetAllForQuote time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            _sw.Restart();
            QuoteAuto auto = _autoRepository.GetByKey(quote.Key);
            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm RetrieveFormByKey QuoteAuto GetByKey time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            _sw.Restart();
            QuoteAutoVehicle autoVehicle = _autoVehicleRepository.GetAllForQuote(quote.Key)?.FirstOrDefault();
            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm RetrieveFormByKey GetAllForQuote GetAllForQuote time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            _sw.Restart();
            MotorhomeFormSaveModel model = new MotorhomeFormSaveModel();
            model.ReverseMap(quote, contact, vehicle, drivers, autoIncidents, auto, autoVehicle);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            
            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm RetrieveFormByKey ReverseMap time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            Sitecore.Diagnostics.Log.Info("Exited MotorhomeForm RetrieveFormByKey", Constants.QUOTE_FORMS_LOGGER_NAME);

            return keyValueModel;
        }

        private CommonFormSaveResponseModel Save(MotorhomeFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            if (_sw == null)
                _sw = new Stopwatch();

            Sitecore.Diagnostics.Log.Info("Invoke MotorhomeForm Save", Constants.QUOTE_FORMS_LOGGER_NAME);
            _sw.Start();

            var quote = new Quote();
            quote.Map(CoverageTypes.Motorhome, form, isSubmitted, returningVisit);
            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm Save Quote Map time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            if (returningVisit)
            {
                _sw.Restart();
                _quoteRepository.Update(quote);
                
                _sw.Stop();
                Sitecore.Diagnostics.Log.Info("MotorhomeForm Save Quote Update time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            }
            else
            {
                _sw.Restart();
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

                _sw.Stop();
                Sitecore.Diagnostics.Log.Info("MotorhomeForm Save Quote Create time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);
            }

            _sw.Restart();
            var quoteContact = new QuoteContact();
            quoteContact.Map(form, quote.Key);

            _contactRepository.CreateOrUpdate(quoteContact);
            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm Save QuoteContact Map and CreateOrUpdate time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            _sw.Restart();
            var quoteAuto = new QuoteAuto();
            quoteAuto.Map(form, quote.Key);
            _autoRepository.CreateOrUpdate(quoteAuto);
            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm Save QuoteAuto Map and CreateOrUpdate time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            _sw.Restart();
            var vehicle = new QuoteMotorhomeVehicle();
            vehicle.Map(form, quote.Key);
            _vehicleRepository.CreateOrUpdate(new List<QuoteMotorhomeVehicle>() { vehicle });
            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm Save QuoteVehicle Map and CreateOrUpdate time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            _sw.Restart();
            List<QuoteAutoDriver> drivers = new List<QuoteAutoDriver>();
            var quotePolicyHolderDriver = new QuoteAutoDriver();
            quotePolicyHolderDriver.MapPolicyHolder(form, quote.Key);
            drivers.Add(quotePolicyHolderDriver);

            
            var primaryDriver = new QuoteAutoDriver();
            primaryDriver.MapPrimaryDriver(form, quote.Key);
            drivers.Add(primaryDriver);

            bool isMarriedOrSpouse = string.Equals(quoteContact.MaritalStatus, "Married", StringComparison.InvariantCultureIgnoreCase) ||
                                     string.Equals(quoteContact.MaritalStatus, "Cohabitant", StringComparison.InvariantCultureIgnoreCase);

            if (isMarriedOrSpouse && form.SpouseMotorHomeOperator)
            {
                var spouseDriver = new QuoteAutoDriver();
                spouseDriver.MapSpouseDriver(form, quote.Key);
                drivers.Add(spouseDriver);
            }

            List<int> additionalDriverIndexes = form.FormObjectIndexExtractor("AdditionalDriver", "FirstName");
            int counter = 0;
            foreach (var driverKey in additionalDriverIndexes)
            {
                var additionalDriver = new QuoteAutoDriver();
                additionalDriver.MapDriver(form, quote.Key, driverKey, counter);
                drivers.Add(additionalDriver);
                counter++;
            }

            _driverRepository.CreateOrUpdate(drivers);

            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm Save QuoteAutoDriver Map and CreateOrUpdate time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            _sw.Restart();
            var autoVehicle = new QuoteAutoVehicle();
            autoVehicle.MapVehicle(form, quote.Key);
            _autoVehicleRepository.CreateOrUpdate(new List<QuoteAutoVehicle>() { autoVehicle });
            _sw.Stop();
            Sitecore.Diagnostics.Log.Info("MotorhomeForm Save QuoteAutoVehicle Map and CreateOrUpdate time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

            if (form.HouseholdViolationsPreviousClaims)
            {
                _sw.Restart();
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
                    
                    _sw.Stop();
                    Sitecore.Diagnostics.Log.Info("MotorhomeForm Save QuoteAutoIncidents Map and CreateOrUpdate time elapsed: " + _sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);
                }
            }

            Sitecore.Diagnostics.Log.Info("Exited MotorhomeForm Save", Constants.QUOTE_FORMS_LOGGER_NAME);
            return new CommonFormSaveResponseModel() { QuoteKey = quote.SaveForLaterKey, QuoteId = quote.Key, MemberNumber = form.MemberNumber?.ToString() ?? string.Empty };
        }

        public CommonFormSaveResponseModel SubmitForm(MotorhomeFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public CommonFormSaveResponseModel SaveForm(MotorhomeFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        public string GetFormCustomerServicePhone()
        {
            return GetSimpleFieldFormValue(CoverageTypes.Motorhome, Identifiers.Templates.Motorhome_Form.FieldIds.Associated_Customer_Service_Phone);
        }

        private static string GetAutoIncidentDriverName(MotorhomeFormSaveModel form, string driverNameKey)
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
            }

            else if (driverNameKey.ToLower().StartsWith("cohabitant"))
                return form.CohabitantFirstName + " " + form.CohabitantLastName;
            else if (driverNameKey.ToLower().StartsWith("spouse"))
                return form.SpouseFirstName + " " + form.SpouseLastName;
            else if (driverNameKey.ToLower().StartsWith("policyholder"))
                return form.PolicyHolderFirstName + " " + form.PolicyHolderLastName;
            else if (driverNameKey.ToLower().StartsWith("primary"))
                return form.PrimaryDriverFirstName + " " + form.PrimaryDriverLastName;
            return null;
        }
    }
}