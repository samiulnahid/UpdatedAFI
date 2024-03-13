using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AFI.Feature.Data.DataModels;
using FTData = AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Repositories;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Identifiers = AFI.Feature.Identifiers;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
    public interface IAutoQuoteFormRepository
    {
        AutoForm GetForm();
        string GetFormUrl();
        string GetFormCustomerServicePhone();
        List<KeyValuePair<string, string>> RetrieveFormByKey(string key);
        CommonFormSaveResponseModel SubmitForm(AutoFormSaveModel form);
        CommonFormSaveResponseModel SaveForm(AutoFormSaveModel form);
    }

    public class AutoQuoteFormRepository : QuoteFormRepository, IAutoQuoteFormRepository
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteContactRepository _contactRepository;
        private readonly IQuoteAutoVehicleRepository _autoVehicleRepository;
        private readonly IQuoteAutoDriverRepository _autoDriverRepository;
        private readonly IQuoteAutoIncidentRepository _autoIncidentRepository;
        private readonly IQuoteAutoRepository _autoRepository;

        public AutoQuoteFormRepository(IAPIRepository apiRepository,
                                       ICommonRepository commonRepository,
                                       IQuoteRepository quoteRepository,
                                       IQuoteContactRepository contactRepository,
                                       IQuoteAutoVehicleRepository autoVehicleRepository,
                                       IQuoteAutoDriverRepository autoDriverRepository,
                                       IQuoteAutoIncidentRepository autoIncidentRepository,
                                       IQuoteAutoRepository autoRepository) : base(apiRepository, commonRepository)
        {
            _quoteRepository = quoteRepository;
            _contactRepository = contactRepository;
            _autoVehicleRepository = autoVehicleRepository;
            _autoDriverRepository = autoDriverRepository;
            _autoIncidentRepository = autoIncidentRepository;
            _autoRepository = autoRepository;
        }

        public AutoForm GetForm()
        {
            var autoFormItem = ApiRepository.GetAutoForm();
            if (autoFormItem != null)
            {
                // make the top level object and grab its fields
                var formBlob = new AutoForm()
                {
                    wayfinder = new Wayfinder
                    {
                        steps = CommonRepository.GetSteps(new MultilistField(autoFormItem.Fields[Identifiers.Templates.Auto_Form.FieldNames.Way_Finder_Steps]))
                    },
                    form = new AutoFormData
                    {
                        id = autoFormItem[Identifiers.Templates.Auto_Form.FieldNames.ID],
                        backButtonText = autoFormItem[Identifiers.Templates.Auto_Form.FieldNames.Back_Button_Text],
                        preSaveButtonText = autoFormItem[Identifiers.Templates.Auto_Form.FieldNames.Pre_Save_Button_Text],
                        saveButtonText = autoFormItem[Identifiers.Templates.Auto_Form.FieldNames.Save_Button_Text],
                        nextButtonText = autoFormItem[Identifiers.Templates.Auto_Form.FieldNames.Next_Button_Text],
                        submitButtonText = autoFormItem[Identifiers.Templates.Auto_Form.FieldNames.Submit_Button_Text],
                        selectMenuDefaultText = autoFormItem[Identifiers.Templates.Auto_Form.FieldNames.Select_Menu_Default_Text],
                        saveSuccessRedirect = ((LinkField)autoFormItem.Fields[Identifiers.Templates.Auto_Form.FieldNames.SaveSuccessRedirect]).GetFriendlyUrl(),
                        submitSuccessRedirect = ((LinkField)autoFormItem.Fields[Identifiers.Templates.Auto_Form.FieldNames.SubmitSuccessRedirect]).GetFriendlyUrl(),
                        sections = new List<Section>(),
                        common = new CommonData(),
                        unique = GetUniqueValues(autoFormItem?.Axes.GetDescendants().FirstOrDefault(x => x.TemplateID == Identifiers.Templates.Unique_Auto_Form_Fields.TemplateId))
                    }
                };
                // start to parse the children/sections
                IEnumerable<Item> sections = ApiRepository.GetCommonSections();
                formBlob.form.sections.AddRange(FillFormSections(sections, autoFormItem, CommonRepository));
                return formBlob;
            }
            return null;
        }

        public string GetFormCustomerServicePhone()
        {
            return GetSimpleFieldFormValue(CoverageTypes.Auto, Identifiers.Templates.Auto_Form.FieldIds.Associated_Customer_Service_Phone);
        }

        public string GetFormUrl()
        {
            var autoFormItem = ApiRepository.GetAutoForm();
            return CommonRepository.GetFormUrl(autoFormItem);
        }

        public List<KeyValuePair<string, string>> RetrieveFormByKey(string key)
        {
            Quote quote = _quoteRepository.GetBySaveForLaterKey(new Guid(key));
            if (quote == null) return new List<KeyValuePair<string, string>>();
            QuoteContact contact = _contactRepository.GetByKey(quote.Key);
            IEnumerable<QuoteAutoVehicle> vehicles = _autoVehicleRepository.GetAllForQuote(quote.Key);
            IEnumerable<QuoteAutoDriver> drivers = _autoDriverRepository.GetAllForQuote(quote.Key);
            IEnumerable<QuoteAutoIncident> incidents = _autoIncidentRepository.GetAllForQuote(quote.Key);
            QuoteAuto auto = _autoRepository.GetByKey(quote.Key);

            AutoFormSaveModel model = new AutoFormSaveModel();
            model.ReverseMap(quote, contact, vehicles.ToList(), drivers.ToList(), incidents.ToList(), auto);
            List<KeyValuePair<string, string>> keyValueModel = model.ToKeyValuePair();
            return keyValueModel;
        }

        private CommonFormSaveResponseModel Save(AutoFormSaveModel form, bool isSubmitted, bool returningVisit)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var quote = new Quote();
            quote.Map(CoverageTypes.Auto, form, isSubmitted, returningVisit);
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
            
            quoteContact.PropertyCity = form.GetPropertyValueByIndex<AutoFormSaveModel, string>("Vehicle", "City", 0) ?? form.PolicyHolderCity;
            quoteContact.PropertyState = form.GetPropertyValueByIndex<AutoFormSaveModel, string>("Vehicle", "State", 0) ?? form.PolicyHolderState;
            quoteContact.PropertyStreet = form.GetPropertyValueByIndex<AutoFormSaveModel, string>("Vehicle", "Address", 0) ?? form.PolicyHolderMailingAddress;
            quoteContact.PropertyZipCode = form.GetPropertyValueByIndex<AutoFormSaveModel, string>("Vehicle", "Zip", 0) ?? form.PolicyHolderZip;

            _contactRepository.CreateOrUpdate(quoteContact);

            var quoteAuto = new QuoteAuto();
            quoteAuto.Map(form, quote.Key);
            _autoRepository.CreateOrUpdate(quoteAuto);

            List<int> vehicleIndexes = form.FormObjectIndexExtractor("Vehicle", "Year");
            List<QuoteAutoVehicle> vehicles = new List<QuoteAutoVehicle>();
            vehicleIndexes.ForEach(vehicleIndex =>
            {
                var vehicle = new QuoteAutoVehicle();
                vehicle.Map(form, quote.Key, vehicleIndex);
                vehicles.Add(vehicle);
            });
            if (vehicles.Any())
            {
                _autoVehicleRepository.CreateOrUpdate(vehicles);
            }

            List<QuoteAutoDriver> drivers = ParseDrivers(form, quote, quoteContact);
            if (drivers.Any())
            {
                _autoDriverRepository.CreateOrUpdate(drivers);
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

                    incident.DriverName = GetAutoIncidentDriverName(form, driverNameKey);
                    incident.Incident = form.GetPropertyValueByIndex<CommonFormSaveModel, string>(householdPrefix, "Type", i);
                    incident.Date = form.GetPropertyValueByIndex<CommonFormSaveModel, DateTime?>(householdPrefix, "Date", i);

                    incidents.Add(incident);
                });
                if (incidents.Any())
                {
                    _autoIncidentRepository.CreateOrUpdate(incidents);
                }
            }
            sw.Stop();
            Sitecore.Diagnostics.Log.Info("auto save" + sw.Elapsed, "stopwatch");
            return new CommonFormSaveResponseModel() { QuoteKey = quote.SaveForLaterKey, QuoteId = quote.Key, MemberNumber = form.MemberNumber?.ToString() ?? string.Empty };
        }

        private static string GetAutoIncidentDriverName(AutoFormSaveModel form, string driverNameKey)
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

        private static List<QuoteAutoDriver> ParseDrivers(AutoFormSaveModel form, FTData.Quote quote, QuoteContact quoteContact)
        {
            List<QuoteAutoDriver> drivers = new List<QuoteAutoDriver>();
            var quotePolicyHolderDriver = new QuoteAutoDriver();
            quotePolicyHolderDriver.MapPolicyHolder(form, quote.Key);
            drivers.Add(quotePolicyHolderDriver);
            bool availableSpouseOrCohabitantDriver = string.Equals(quoteContact.MaritalStatus, "Married", StringComparison.InvariantCultureIgnoreCase) || string.Equals(quote.Eligibility, "MilitarySpouse", StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(quoteContact.MaritalStatus, "Cohabitant", StringComparison.InvariantCultureIgnoreCase);

            if (availableSpouseOrCohabitantDriver)
            {
                var quoteCohabitantDriver = new QuoteAutoDriver();
                quoteCohabitantDriver.MapCohabitant(form, quote.Key);
                drivers.Add(quoteCohabitantDriver);
            }

            List<int> additionalDriversIndexes = form.FormObjectIndexExtractor("AdditionalDriver", "FirstName");
            if (availableSpouseOrCohabitantDriver)
            {
                additionalDriversIndexes = additionalDriversIndexes.Take(3).ToList();
            }

            foreach (var driverIndex in additionalDriversIndexes)
            {
                var additionalDriver = new QuoteAutoDriver();
                //INFO: DB [Key] for additional drivers goes from 1 - 4 (not 0 - 3 as it is in FE)
                additionalDriver.MapDriver(form, quote.Key, driverIndex);
                drivers.Add(additionalDriver);
            }

            return drivers;
        }

        public CommonFormSaveResponseModel SubmitForm(AutoFormSaveModel form)
        {
            return Save(form, true, form.QuoteId > 0);
        }

        public CommonFormSaveResponseModel SaveForm(AutoFormSaveModel form)
        {
            return Save(form, false, form.QuoteId > 0);
        }

        private UniqueCarData GetUniqueValues(Item uniqueItem)
        {

            if (uniqueItem != null)
            {
                return new UniqueCarData
                {

                    addDriverButtonText = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Add_Driver_Button],
                    addVehicleButtonText = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Add_Vehicle_Button],
                    addViolationButtonText = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Add_Violation_Button],
                    addDriverHeadings = new Heading
                    {
                        married = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Married],
                        cohabitant = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Cohabitant],
                        additionalDriver = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Additional_Driver]
                    },
                    enterByVinText = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Enter_By_Vin],
                    enterByDetailsText = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Enter_By_Details],
                    vehicleStartYear = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Vehicle_Start_Year],
                    firstVinLookupYear = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.First_Vin_Lookup_Year],
                    vehicleUses = CommonRepository.GetOptions(uniqueItem.Axes.GetDescendants().SingleOrDefault(x => x.TemplateID == Identifiers.Templates.Vehicle_Uses.TemplateId)?.Axes.GetDescendants()),
                    householdViolationTypes = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Identifiers.Templates.Household_Violations_Types.TemplateId)),
                    pipAmounts = CommonRepository.GetPipAmounts(GetCommonFormFieldsItemChildren(Identifiers.Templates.PipAmounts.TemplateId)),
                    physicalDamageDeductibles = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Identifiers.Templates.Physical_Damage_Deductibles.TemplateId)),
                    physicalDamageComprehensiveDeductibles = CommonRepository.GetOptions(GetCommonFormFieldsItemChildren(Identifiers.Templates.Physical_Damage_Comprehensive_Deductibles.TemplateId)),
                    overallCoveragesSidebars = GetOverallCoverages(uniqueItem),
                    bodilyInjuryLiabilitySublabel = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Bodily_Injury_Liability_Sub_Label],
                    propertyDamageLiabilitySublabel = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Property_Damage_Liability_Sub_Label],
                    medicalPaymentSublabel = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Medical_Payment_Sub_Label],
                    uninsuredMotoristBodilyInjurySublabel = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Uninsured_Motorist_Bodily_Injury_Sub_Label],
                    personalInjuryProtectionSublabel = uniqueItem[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Personal_Injury_Protection_Sub_Label]
                };
            }

            return null;
        }

        private OverallCoverageAuto GetOverallCoverages(Item item)
        {
            return new OverallCoverageAuto
            {
                @default = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Default_Sidebar])),
                bodilyInjuryLiability = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Bodily_Injury_Liability_Sidebar])),
                propertyDamageLiability = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Property_Damage_Liability_Sidebar])),
                medicalPayment = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Medical_Payment_Sidebar])),
                uninsuredMotoristBodilyInjury = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Uninsured_Motorist_Bodily_Injury_Sidebar])),
                personalInjuryProtection = CommonRepository.GetSideBar(Sitecore.Context.Database.GetItem(item[Identifiers.Templates.Unique_Auto_Form_Fields.FieldNames.Personal_Injury_Protection_Sidebar])),
            };
        }
    }
}