using System.Linq;
using AFI.Feature.Identifiers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Repository
{
    public interface IGlobalSettingsRepository
    {
        string GetSetting(string settingName);
        Field GetSettingField(string settingName);
    }
    public class GlobalSettingsRepository : IGlobalSettingsRepository
    {
        private Item GlobalSettingsItem { get; set; }

        public string GetSetting(string settingName)
        {
            var settingsItem = GetGlobalSettingsItem();
            if (settingsItem == null)
            {
                Sitecore.Diagnostics.Log.Error($"Failed to get setting {settingName}. Global settings item not found.", this);
                return string.Empty;
            }

            return settingsItem[settingName];
        }

        public Field GetSettingField(string settingName)
        {
            var settingsItem = GetGlobalSettingsItem();
            if (settingsItem == null)
            {
                Sitecore.Diagnostics.Log.Error($"Failed to get setting field {settingName}. Global settings item not found.", this);
                return null;
            }

            return settingsItem.Fields[settingName];
        }

        public Item GetGlobalSettingsItem()
        {
            if (GlobalSettingsItem != null)
                return GlobalSettingsItem;

            var globalSettingsPath = Sitecore.Configuration.Settings.GetSetting("AFI.GlobalSettingsPath",
                "/sitecore/system/Settings/AFI/");

            var globalSettingsFolder = Sitecore.Context.Database.GetItem(Template.FormSeeting.Global_Setting_Location);
            
            GlobalSettingsItem = globalSettingsFolder?
                .GetChildren()
                .FirstOrDefault(x => x.TemplateID == Templates.Global_Settings.TemplateId);

            // return GlobalSettingsItem;
            return globalSettingsFolder;
        }
    }
}