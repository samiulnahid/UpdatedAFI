using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using AFI.Feature.Identifiers;
using QuoteTemplate = AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using Sitecore.Data;
using Sitecore.StringExtensions;
using Identifiers = AFI.Feature.Identifiers;
using System;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using FoundationHelper =  AFI.Foundation.Helper;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers;


namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
    public interface ICommonRepository
    {
        string LandingPageFormGoTo(string formType);
        Dictionary<string, string> GetInsuranceLink(string formType);
        List<Step> GetSteps(MultilistField stepsField);
        SideBar GetSideBar(Item sidebarItem);
        List<ISectionField> GetFields(IEnumerable<Item> fields);
        List<Options> GetOptions(IEnumerable<Item> optionItemList);
        List<PipAmount> GetPipAmounts(IEnumerable<Item> pipAmountsList);
        string GetFormUrl(Item quoteForm);
        string GetSuccessReturnFormMessage(Item quoteForm);
        string GetExpiredReturnFormMessage(Item quoteForm);
        string GetSimpleFormValue(Item quoteForm, ID formId);

        Ranks FillRanks(string key);
        List<Options> GetStateList();
        List<Options> GetLeadStateList();
    }
    public class CommonRepository : ICommonRepository
    {
        private readonly IGlobalSettingsRepository _globalSettingsRepository;

        public CommonRepository(IGlobalSettingsRepository globalSettingsRepository)
        {
            _globalSettingsRepository = globalSettingsRepository;
        }

        public Ranks FillRanks(string rankName)
        {
            var rankList = new List<Ranks>();

            var rankLocationItem = Sitecore.Context.Database.GetItem(QuoteTemplate.Template.FormSeeting.Rank_Location);

            try
            {
                var rankItems = rankLocationItem.GetChildren();

                foreach (Item rankItem in rankItems)
                {
                    var rank = new Ranks();
                    rank.Name = rankItem.Name;
                    rank.Options = GetOptions(rankItem?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Form_Option.TemplateId));
                    rankList.Add(rank);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            var selectedRank = rankList.SingleOrDefault(x => x.Name == rankName);
            return selectedRank;
        }
        public List<Options> GetStateList()
        {
            var statelist = new List<Options>();
            try
            {
                var commondata = new CommonData();
                statelist = commondata.states;
            }
            catch (Exception ex)
            {
                // Handle the exception
            }
            return statelist;
        }
        public List<Options> GetLeadStateList()
        {
            var statelist = new List<Options>();
            try
            {
                var commondata = new CommonData();
                statelist = commondata.states_lead;
            }
            catch (Exception ex)
            {
                // Handle the exception
            }
            return statelist;
        }
        public Dictionary<string, string> GetInsuranceLink(string formType)
        {
            Dictionary<string, string> insurance = new Dictionary<string, string>();
            Item insuranceItem = Sitecore.Context.Database.GetItem(formType);
            string type = ""; string url = "";
            if (insuranceItem != null)
            {
                type = insuranceItem.Fields[FoundationHelper.FeatureTemplate.InsuranceType.Fields.InsuranceType].ToString();

                if (!string.IsNullOrEmpty(insuranceItem[FoundationHelper.FeatureTemplate.InsuranceType.Fields.QuoteFormLink]))
                {
                    url = ((Sitecore.Data.Fields.LinkField)insuranceItem.Fields[FoundationHelper.FeatureTemplate.InsuranceType.Fields.QuoteFormLink]).GetFriendlyUrl();
                }
                insurance.Add(type, url);
            }
            if (insurance.Count == 0)
            {
                insurance.Add(type, "/");
            }
            return insurance;
        }

        public string LandingPageFormGoTo(string formType)
        {
            foreach (var insuranceItem in GetAllInsuranceTypes())
            {
                // if (formType.Equals(insuranceItem[FoundationHelper.FeatureTemplate.InsuranceType.Fields.InsuranceType]))
                if (formType.Trim().Equals(insuranceItem.ID.ToString().Trim()))
                {
                    if (!string.IsNullOrEmpty(insuranceItem[FoundationHelper.FeatureTemplate.InsuranceType.Fields.QuoteFormLink]))
                    {
                        return ((Sitecore.Data.Fields.LinkField)insuranceItem.Fields[
                            FoundationHelper.FeatureTemplate.InsuranceType.Fields.QuoteFormLink]).GetFriendlyUrl();
                    }
                }
            }
            return "/";
        }
        public IEnumerable<Item> GetAllInsuranceTypes()
        {
            return GetInsuranceTypeRoot()?.Axes.GetDescendants().Where(x => x.TemplateID == FoundationHelper.FeatureTemplate.InsuranceType.TemplateId) ?? new Item[0];
        }
        public Item GetInsuranceTypeRoot()
        {
            return Sitecore.Context.Database.GetItem(Template.Seeting.Insurance_Type_Root);
        }
        public List<Step> GetSteps(MultilistField stepsField)
        {
            var stepsList = new List<Step>();
            foreach (var guid in stepsField.List)
            {
                var sitecoreItem = Sitecore.Context.Database.GetItem(guid);
                stepsList.Add(new Step
                {
                    id = sitecoreItem[Template.Form_Steps.Fields.ID],
                    label = sitecoreItem[Template.Form_Steps.Fields.Label]
                });
            }
            return stepsList;
        }
        public SideBar GetSideBar(Item sidebarItem)
        {
            // if the item does not have a sidebar then we just return null
            if (sidebarItem != null)
            {
                var sideBar = new SideBar
                {
                    triggerText = sidebarItem[Identifiers.Templates.Side_Bar.FieldNames.TriggerText],
                    heading = sidebarItem[Identifiers.Templates.Side_Bar.FieldNames.Title],
                    content = new List<SideBarContent>()
                };
                foreach (var sidebarContentItem in sidebarItem.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Side_Bar_Content.TemplateId))
                {
                    var content = new SideBarContent
                    {
                        heading = sidebarContentItem[Identifiers.Templates.Side_Bar_Content.FieldNames.Heading],
                        content = sidebarContentItem[Identifiers.Templates.Side_Bar_Content.FieldNames.Content]
                    };
                    sideBar.content.Add(content);
                }
                return sideBar;
            }
            return null;
        }
        public List<ISectionField> GetFields(IEnumerable<Item> fields)
        {
            if (fields != null)
            {
                var fieldsList = new List<ISectionField>();
                foreach (var fieldItem in fields)
                {
                    var field = GetField(fieldItem);
                    // get normal field options                  
                    fieldsList.Add(field);
                }
                return fieldsList;
            }
            return null;
        }
        private ISectionField GetField(Item fieldItem)
        {
            string optionsPath = fieldItem[Identifiers.Templates.Field.FieldNames.Child_Options_Path];
            ISectionField field = null;

            if (fieldItem.TemplateID == Identifiers.Templates.Field.TemplateId)
            {
                var valueIsArray = ((CheckboxField)fieldItem.Fields[Identifiers.Templates.Field.FieldIds.ValueIsArray]).Checked;
                if (valueIsArray)
                {
                    field = new ArrayField()
                    {
                        value = fieldItem[Identifiers.Templates.Field.FieldNames.Value].Split(',')
                    };
                }
                else
                {
                    field = new SimpleField()
                    {
                        value = fieldItem[Identifiers.Templates.Field.FieldNames.Value]
                    };
                }
                if (!optionsPath.IsNullOrEmpty())
                {
                    Item optionsMainItem = Sitecore.Context.Database.GetItem(optionsPath);
                    field.options = GetOptions(optionsMainItem?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Form_Option.TemplateId));
                }
                else
                {
                    field.options = GetOptions(fieldItem?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Form_Option.TemplateId));
                }
            }
            if (fieldItem.TemplateID == Identifiers.Templates.PipAmounts.TemplateId)
            {
                field = new PipAmountField();
                if (!optionsPath.IsNullOrEmpty())
                {
                    Item optionsMainItem = Sitecore.Context.Database.GetItem(optionsPath);
                    field.options = GetPipAmounts(optionsMainItem?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Pip_Amount.TemplateId));
                }
                else
                {
                    field.options = GetPipAmounts(fieldItem?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Pip_Amount.TemplateId));
                }
            }

            if (field == null) return null;
            field.id = fieldItem[Identifiers.Templates.Field.FieldNames.ID];
            field.label = fieldItem[Identifiers.Templates.Field.FieldNames.Label];
            field.note = fieldItem[Identifiers.Templates.Field.FieldNames.Note];
            field.placeholder = fieldItem[Identifiers.Templates.Field.FieldNames.Placeholder];
            return field;
        }
        //public List<Step> GetSteps(MultilistField stepsField)
        //{
        //    var stepsList = new List<Step>();
        //    foreach (var guid in stepsField.List)
        //    {
        //        var sitecoreItem = Sitecore.Context.Database.GetItem(guid);
        //        stepsList.Add(new Step
        //        {
        //            id = sitecoreItem[Identifiers.Templates.Form_Steps.FieldNames.ID],
        //            label = sitecoreItem[Identifiers.Templates.Form_Steps.FieldNames.Label]
        //        });
        //    }
        //    return stepsList;
        //}
        //public SideBar GetSideBar(Item sidebarItem)
        //{
        //    // if the item does not have a sidebar then we just return null
        //    if (sidebarItem != null)
        //    {
        //        var sideBar = new SideBar
        //        {
        //            triggerText = sidebarItem[Identifiers.Templates.Side_Bar.FieldNames.TriggerText],
        //            heading = sidebarItem[Identifiers.Templates.Side_Bar.FieldNames.Title],
        //            content = new List<SideBarContent>()
        //        };
        //        foreach (var sidebarContentItem in sidebarItem.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Side_Bar_Content.TemplateId))
        //        {
        //            var content = new SideBarContent
        //            {
        //                heading = sidebarContentItem[Identifiers.Templates.Side_Bar_Content.FieldNames.Heading],
        //                content = sidebarContentItem[Identifiers.Templates.Side_Bar_Content.FieldNames.Content]
        //            };
        //            sideBar.content.Add(content);
        //        }
        //        return sideBar;
        //    }
        //    return null;
        //}
        //public List<ISectionField> GetFields(IEnumerable<Item> fields)
        //{
        //    if (fields != null)
        //    {
        //        var fieldsList = new List<ISectionField>();
        //        foreach (var fieldItem in fields)
        //        {
        //            var field = GetField(fieldItem);
        //            // get normal field options                  
        //            fieldsList.Add(field);
        //        }
        //        return fieldsList;
        //    }
        //    return null;
        //}

        //private ISectionField GetField(Item fieldItem)
        //{
        //    string optionsPath = fieldItem[Identifiers.Templates.Field.FieldNames.Child_Options_Path];
        //    ISectionField field = null;

        //    if (fieldItem.TemplateID == Identifiers.Templates.Field.TemplateId)
        //    {
        //        var valueIsArray = ((CheckboxField) fieldItem.Fields[Identifiers.Templates.Field.FieldIds.ValueIsArray]).Checked;
        //        if (valueIsArray)
        //        {
        //            field = new ArrayField()
        //            {
        //                value = fieldItem[Identifiers.Templates.Field.FieldNames.Value].Split(',')
        //            };
        //        } else
        //        {
        //            field = new SimpleField()
        //            {
        //                value = fieldItem[Identifiers.Templates.Field.FieldNames.Value]
        //            };
        //        }
        //        if (!optionsPath.IsNullOrEmpty())
        //        {
        //            Item optionsMainItem = Sitecore.Context.Database.GetItem(optionsPath);
        //            field.options = GetOptions(optionsMainItem?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Form_Option.TemplateId));
        //        }
        //        else
        //        {
        //            field.options = GetOptions(fieldItem?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Form_Option.TemplateId));
        //        }
        //    }
        //    if (fieldItem.TemplateID == Identifiers.Templates.PipAmounts.TemplateId)
        //    {
        //        field = new PipAmountField();
        //        if (!optionsPath.IsNullOrEmpty())
        //        {
        //            Item optionsMainItem = Sitecore.Context.Database.GetItem(optionsPath);
        //            field.options = GetPipAmounts(optionsMainItem?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Pip_Amount.TemplateId));
        //        }
        //        else
        //        {
        //            field.options = GetPipAmounts(fieldItem?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Pip_Amount.TemplateId));
        //        }
        //    }

        //    if (field == null) return null;
        //    field.id = fieldItem[Identifiers.Templates.Field.FieldNames.ID];
        //    field.label = fieldItem[Identifiers.Templates.Field.FieldNames.Label];
        //    field.note = fieldItem[Identifiers.Templates.Field.FieldNames.Note];
        //    field.placeholder = fieldItem[Identifiers.Templates.Field.FieldNames.Placeholder];
        //    return field;
        //}

        public List<Options> GetOptions(IEnumerable<Item> optionItemList)
        {
            if (optionItemList.Any())
            {
                var optionList = new List<Options>();
                foreach (var optionItem in optionItemList)
                {
                    var option = new Options
                    {
                        label = optionItem[Identifiers.Templates.Field.FieldNames.Label],
                        value = optionItem[Identifiers.Templates.Field.FieldNames.Value]
                    };
                    optionList.Add(option);
                }
                return optionList;
            }
            return null;
        }
        public List<PipAmount> GetPipAmounts(IEnumerable<Item> pipAmountsList)
        {
            var pipList = new List<PipAmount>();
            if (pipAmountsList.Any())
            {
                foreach (var pipAmount in pipAmountsList)
                {
                    pipList.Add(new PipAmount
                    {
                        id = pipAmount[Identifiers.Templates.Pip_Amount.FieldNames.ID],
                        values = pipAmount.Axes.GetDescendants().Select(x => x[Identifiers.Templates.Value.FieldNames.Value]).ToArray()
                    });
                }
                return pipList;
            }
            return null;
        }

        //public string GetFormUrl(Item quoteForm)
        //{
        //    Item associatedInsuranceType = ((ReferenceField)quoteForm.Fields[Identifiers.Templates.Quote_Form.FieldNames.Associated_Ensurance_Type]).TargetItem;
        //    Item defaultRedirect = ((LinkField)_globalSettingsRepository.GetSettingField(Identifiers.Templates.Global_Settings.FieldNames.Default_Quote_Start_Page)).TargetItem;
        //    if (associatedInsuranceType == null)
        //    {
        //        return defaultRedirect.GetFullyQualifiedUrl();
        //    }
        //    LinkField formLink = associatedInsuranceType.Fields[Identifiers.Templates.InsuranceType.FieldNames.QuoteFormLink];
        //    if (formLink == null)
        //    {
        //        return defaultRedirect.GetFullyQualifiedUrl();
        //    }

        //    string url = formLink.TargetItem.GetFullyQualifiedUrl();
        //    return url;
        //}
        [Obsolete]
        public string GetFormUrl(Item quoteForm)
        {
            Item associatedInsuranceType = ((ReferenceField)quoteForm.Fields[Identifiers.Templates.Quote_Form.FieldNames.Associated_Ensurance_Type]).TargetItem;
            Item defaultRedirect = ((LinkField)_globalSettingsRepository.GetSettingField(Identifiers.Templates.Global_Settings.FieldNames.Default_Quote_Start_Page)).TargetItem;
            if (associatedInsuranceType == null)
            {
                return defaultRedirect.GetFullyQualifiedUrl();
            }
            LinkField formLink = associatedInsuranceType.Fields[Identifiers.Templates.InsuranceType.FieldNames.QuoteFormLink];
            if (formLink == null)
            {
                return defaultRedirect.GetFullyQualifiedUrl();
            }

            string url = formLink.TargetItem.GetFullyQualifiedUrl();
            return url;
        }
        private ReferenceField GetFormPageField(Item quoteForm, string fieldId)
        {
            Item associatedInsuranceType = ((ReferenceField)quoteForm.Fields[Identifiers.Templates.Quote_Form.FieldNames.Associated_Ensurance_Type]).TargetItem;
            if (associatedInsuranceType != null)
            {
                Item formItem = ((LinkField)associatedInsuranceType.Fields[Identifiers.Templates.InsuranceType.FieldNames.QuoteFormLink]).TargetItem;
                if (formItem != null)
                {
                    return formItem.Fields[fieldId];
                }
            }

            return null;
        }

        public string GetSuccessReturnFormMessage(Item quoteForm)
        {
            ReferenceField successMessage = GetFormPageField(quoteForm, Templates.Form_Page.FieldNames.Returning_Customer_Message);
            return successMessage != null ? successMessage.Value : string.Empty;
        }

        public string GetExpiredReturnFormMessage(Item quoteForm)
        {
            ReferenceField expiredMessage = GetFormPageField(quoteForm, Templates.Form_Page.FieldNames.Expired_Link_Message);
            return expiredMessage != null ? expiredMessage.Value : string.Empty;
        }

        public string GetSimpleFormValue(Item quoteForm, ID fieldId)
        {
            return quoteForm.Fields[fieldId].Value;
        }
    }
}