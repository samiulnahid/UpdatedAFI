using System.Collections.Generic;
using Sitecore.ExperienceForms.Mvc.Models.Fields;

namespace AFI.Feature.QuoteForm.ExperienceForms
{
  
    public class CustomRadioButtonListViewModel : ListViewModel
    {
        public IEnumerable<OptionItem> PreloadedOptions { get; set; }

        public CustomRadioButtonListViewModel()
        {
            PreloadedOptions = new List<OptionItem>(); // Initialize with empty list
        }
    }
    public class OptionItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
