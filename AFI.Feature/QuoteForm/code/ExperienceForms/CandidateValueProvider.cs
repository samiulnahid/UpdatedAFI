using Sitecore.Data;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.ValueProviders;
using Sitecore.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFI.Feature.QuoteForm.ExperienceForms
{
    public class CandidateValueProvider : IFieldValueProvider
    {
        public object GetValue(string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
                return null;

            string value = null;
            if (Sitecore.Context.User?.IsAuthenticated ?? false)
            {
                value = Sitecore.Context.User.Profile[parameters.Trim()];
            }

            var fieldTypeId = ValueProviderContext.FieldItem?.Fields[Sitecore.ExperienceForms.Mvc.Constants.FieldNames.FieldType]?.Value;
            if (!string.IsNullOrEmpty(fieldTypeId))
            {
                var fieldTypeItem = ValueProviderContext.Database.GetItem(new ID(fieldTypeId));
                var modelType = fieldTypeItem.Fields[Sitecore.ExperienceForms.Mvc.Constants.FieldNames.ModelType]?.Value;
                if (modelType != null)
                {
                    var model = ReflectionUtil.CreateObject(modelType) as IViewModel;
                    if (model is InputViewModel<List<string>>)
                    {
                        return value?.Split('|').ToList() ?? new List<string>();
                    }
                }
            }

            return value;
        }

        public FieldValueProviderContext ValueProviderContext { get; set; }
    }
}
