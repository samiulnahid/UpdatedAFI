using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Identifiers = AFI.Feature.Identifiers;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
    public abstract class QuoteFormRepository
    {
        internal readonly IAPIRepository ApiRepository;
        internal readonly ICommonRepository CommonRepository;

        protected QuoteFormRepository(IAPIRepository apiRepository, ICommonRepository commonRepository)
        {
            ApiRepository = apiRepository;
            CommonRepository = commonRepository;
        }

        /// <summary>
        /// Get a list of grandchild items specified by parent item and child template ID
        /// I.E: --- parentItem
        /// ------------- child template ID
        /// ------------------- returned grandchild item list
        /// </summary>
        /// <param name="parentItem"></param>
        /// <param name="templateIdToFind"></param>
        /// <returns></returns>
        public IEnumerable<Item> GetDescendantFieldOptions(Item parentItem, ID templateIdToFind)
        {
            return parentItem.Axes.GetDescendants().SingleOrDefault(x => x.TemplateID == templateIdToFind)?.Axes.GetDescendants();
        }

        public IEnumerable<Section> FillFormSections(IEnumerable<Item> commonSections, Item mainForm, ICommonRepository commonRepository)
        {
            var returnedSections = new List<Section>();
            IEnumerable<Item> mainFormSections = mainForm?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Section.TemplateId);
            if (mainFormSections != null)
            {
                commonSections = commonSections.Concat(mainFormSections);
            }
            foreach (var sectionItem in commonSections)
            {
                if (sectionItem != null)
                {
                    var section = new Section
                    {
                        id = sectionItem[Identifiers.Templates.Section.FieldNames.ID],
                        wayfinderId = sectionItem[Identifiers.Templates.Section.FieldNames.Way_Finder_ID],
                        heading = sectionItem[Identifiers.Templates.Section.FieldNames.Heading],
                        subheading = sectionItem[Identifiers.Templates.Section.FieldNames.Sub_Heading],
                        description = sectionItem[Identifiers.Templates.Section.FieldNames.Description].Split('|'),
                        disclaimer = sectionItem[Identifiers.Templates.Section.FieldNames.Disclaimer]
                    };
                    // make the sidebar
                    section.sidebar = commonRepository.GetSideBar(sectionItem?.Axes.GetDescendants().FirstOrDefault(x => x.TemplateID == Identifiers.Templates.Side_Bar.TemplateId));
                    // Get the fields
                    section.fields = commonRepository.GetFields(sectionItem?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Field.TemplateId));
                    returnedSections.Add(section);
                }
            }

            return returnedSections;
        }

        public IEnumerable<Item> GetCommonFormFieldsItemChildren(ID commonFieldsChildTemplateId)
        {
            Item commonFormField = ApiRepository.GetCommonFormFields();
            Item searchedItem = commonFormField.GetChildren().FirstOrDefault(c => c.TemplateID == commonFieldsChildTemplateId);
            return searchedItem == null ? new Item[0] : searchedItem.GetChildren().ToArray();
        }

        public string GetSimpleFieldFormValue(string coverage, ID fieldId)
        {
            Item form = null;
            switch (coverage)
            {
                case CoverageTypes.Auto:
                    form = ApiRepository.GetAutoForm();
                    break;
                case CoverageTypes.Business:
                    form = ApiRepository.GetBusinessForm();
                    break;
                case CoverageTypes.CollectorVehicle:
                    form = ApiRepository.GetCollectorVehicleForm();
                    break;
                case CoverageTypes.Flood:
                    form = ApiRepository.GetFloodForm();
                    break;
                case CoverageTypes.Homeowner:
                    form = ApiRepository.GetHomeownerForm();
                    break;
                case CoverageTypes.Motorcycle:
                    form = ApiRepository.GetMotorcycleForm();
                    break;
                case CoverageTypes.Motorhome:
                    form = ApiRepository.GetMotorhomeForm();
                    break;
                case CoverageTypes.Renters:
                    form = ApiRepository.GetRenterForm();
                    break;
                case CoverageTypes.Umbrella:
                    form = ApiRepository.GetUmbrellaForm();
                    break;
                case CoverageTypes.Watercraft:
                    form = ApiRepository.GetWatercraftForm();
                    break;
                case CoverageTypes.Mobilehome:
                    form = ApiRepository.GetMobilehomeForm();
                    break;
                case CoverageTypes.DwellingFire:
                    form = ApiRepository.GetHomenonownerForm();
                    break;
                case CoverageTypes.Condo:
                    form = ApiRepository.GetCondoForm();
                    break;
                default:
                    return string.Empty;
            }

            var valueToReturn = CommonRepository.GetSimpleFormValue(form, fieldId);

            return valueToReturn;
        }
    }
}
