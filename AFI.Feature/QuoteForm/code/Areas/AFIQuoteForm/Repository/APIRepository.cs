using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using Sitecore.Data.Items;
using Identifiers = AFI.Feature.Identifiers;
namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
    public interface IAPIRepository
    {
        Item GetAutoForm();
        Item GetWatercraftForm();
        Item GetCommonFormFields();
        Item GetMotorcycleForm();
        Item GetBusinessForm();
        Item GetHillAFBForm();
        Item GetUmbrellaForm();
        IEnumerable<Item> GetCommonSections();
        Item GetFloodForm();
        Item GetCollectorVehicleForm();
        Item GetMotorhomeForm();
        Item GetHomeownerForm();
        Item GetRenterForm();
        Item GetMobilehomeForm();
        Item GetHomenonownerForm();
        Item GetLeadGenerationForm();
        Item GetCondoForm();
        Item GetQuoteLeadForm();
    }
    public class APIRepository : IAPIRepository
    {
        private readonly IGlobalSettingsRepository _globalSettingsRepository;

        public APIRepository(IGlobalSettingsRepository globalSettingsRepository)
        {
            _globalSettingsRepository = globalSettingsRepository;
        }

        private Item GetGlobalSettingItem(string fieldName)
        {
            return Sitecore.Context.Database.GetItem(_globalSettingsRepository.GetSetting(fieldName));
        }

        public Item GetCommonFormFields()
        {
            Item item = Sitecore.Context.Database.GetItem(Template.FormSeeting.Common_Fields_Location);
            return item;
        }

        public Item GetAutoForm()
        {
            return GetGlobalSettingItem(Identifiers.Templates.Global_Settings.FieldNames.Auto_Quote_Location);
        }

        public Item GetWatercraftForm()
        {
            Item item = Sitecore.Context.Database.GetItem(Template.FormSeeting.WatercraftFormID);
            return item;
        }

        public Item GetMotorcycleForm()
        {
            Item item = Sitecore.Context.Database.GetItem(Template.FormSeeting.MotorCycleFormID);
            return item;
        }

        public Item GetBusinessForm()
        {
            Item businessFormItem = Sitecore.Context.Database.GetItem(Template.FormSeeting.BusinessFormID);
            return businessFormItem;
        }
        public Item GetHillAFBForm()
        {
            Item item = Sitecore.Context.Database.GetItem(Template.FormSeeting.HillAFBFormID);
            return item;
            //return GetGlobalSettingItem(Identifiers.Templates.Global_Settings.FieldNames.HillAFB_Quote_Location);
        }
        public Item GetLeadGenerationForm()
        {
            return GetGlobalSettingItem(Identifiers.Templates.Global_Settings.FieldNames.Lead_Generation_Quote_Location);
        }

        #region Common Fields

        public IEnumerable<Item> GetCommonSections()
        {
            Item commonSections = Sitecore.Context.Database.GetItem(Template.FormSeeting.CommonSections_Location);      
            return commonSections?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Section.TemplateId);
        }

        #endregion Common Fields

        #region Umbrella

        public Item GetUmbrellaForm()
        {
            return GetGlobalSettingItem(Identifiers.Templates.Global_Settings.FieldNames.Umbrella_Quote_Location);
        }
        #endregion Umbrella

        #region Flood

        public Item GetFloodForm()
        {
            Item floodFormItem = Sitecore.Context.Database.GetItem(Template.FormSeeting.FloodFormID);
            return floodFormItem;
        }
        
        public Item GetCollectorVehicleForm()
        {
            Item collector_VehicleItem = Sitecore.Context.Database.GetItem(Template.FormSeeting.Collector_Vehicle_Quote_Location);
            return collector_VehicleItem;
        }

        #endregion Flood

        public Item GetMotorhomeForm()
        {
            Item item = Sitecore.Context.Database.GetItem(Template.FormSeeting.MotorhomeFormID);
            return item;           
        }

        public Item GetHomeownerForm()
        {
            return GetGlobalSettingItem(Identifiers.Templates.Global_Settings.FieldNames.Homeowner_Form_Location);
        }
        public Item GetHomenonownerForm()
        {
            return GetGlobalSettingItem(Identifiers.Templates.Global_Settings.FieldNames.Homenonowner_Form_Location);
        }
        public Item GetCondoForm()
        {
            Item item = Sitecore.Context.Database.GetItem(Template.FormSeeting.Condo_Quote_Location);
            return item;
        }
        public Item GetMobilehomeForm()
        {
            Item item = Sitecore.Context.Database.GetItem(Template.FormSeeting.MobilehomeFormID);
            return item;
        }

        public Item GetRenterForm()
        {
            return GetGlobalSettingItem(Identifiers.Templates.Global_Settings.FieldNames.Renter_Quote_Location);
        }
        public Item GetQuoteLeadForm()
        {
            Item item = Sitecore.Context.Database.GetItem(Template.FormSeeting.Quote_Lead_Location);
            return item;
        }
    }
}