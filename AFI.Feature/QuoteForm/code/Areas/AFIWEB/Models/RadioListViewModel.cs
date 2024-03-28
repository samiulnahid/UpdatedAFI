using Sitecore.ExperienceForms.FieldSettings;
using Sitecore.ExperienceForms.Mvc.Models;
using Sitecore.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    [Serializable]
    public class RadioListViewModel : Sitecore.ExperienceForms.Mvc.Models.Fields.ListViewModel
    {
        [NonSerialized]
        private IFieldSettingsManager<ListFieldItemCollection> _dataSourceSettingsManager;

        public List<SelectedValues> SelectedValue { get; set; }

        public List<SelectedValues> GetSelectedStringValue()
        {
            List<SelectedValues> selectedItems = new List<SelectedValues>();
            foreach (var item in RadioItems)
            {
                string radiobuttonName = HttpContext.Current.Request.Form.AllKeys.First(x => x.Contains(item.Name));
                string radiobuttonValue = HttpContext.Current.Request.Form[radiobuttonName];


                SelectedValues selectedValues = new SelectedValues() { Name = item.Name, Value = item.Value, IsCheckedFor = (radiobuttonValue == "For"), IsCheckedAgainst = (radiobuttonValue == "Against") };
                selectedItems.Add(selectedValues);
            }
            return selectedItems;
        }

        public List<SelectedValues> RadioItems
        {
            get
            {
                List<SelectedValues> selectedItems = new List<SelectedValues>();
                foreach (var item in Items)
                {
                    SelectedValues selectedValues = new SelectedValues() { Name = item.Text, Value = item.Value };
                    selectedItems.Add(selectedValues);
                }

                return selectedItems;
            }
           
        }

    }

    public class SelectedValues { 
        public string Value { get; set; }
        public string Name { get; set; }
        public bool IsCheckedFor { get; set; } = false;
        public bool IsCheckedAgainst { get; set; } = false;
    }
}