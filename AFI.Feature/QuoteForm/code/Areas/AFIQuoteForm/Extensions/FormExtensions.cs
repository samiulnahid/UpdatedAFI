using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using AFI.Feature.ZipLookup;
using AFI.Feature.ZipLookup.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions
{
    public static class FormExtensions
    {
        public const string ZipCodeSessionKey = "afi:zipcode";
        public const char DefaultArraySeparator = '|';

        public const string DefaultDateTimeFormat = "MM/dd/yyyy";
        public static void ApplyFieldChanges(this object model, IEnumerable<KeyValuePair<string, string>> values)
        {
            if (model == null) return;

            var type = model.GetType();
            if (type == typeof(Section))
            {
                var section = model as Section;
                if (section == null) return;

                foreach (var field in section.fields)
                {
                    if (values.Any(x => string.Equals(x.Key, field.id, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        if (field is ArrayField)
                        {
                            ((ArrayField) field).value = values.First(x => string.Equals(x.Key, field.id, StringComparison.CurrentCultureIgnoreCase)).Value.Split(DefaultArraySeparator);
							var array = ((ArrayField)field).value;

							if (array.Length == 1 && string.IsNullOrWhiteSpace(array[0]))
							{
								((ArrayField)field).value = new string[0];
							}
                        }
                        if(field is SimpleField || field is PipAmountField)
                        {
                            string fieldValue = values.First(x => string.Equals(x.Key, field.id, StringComparison.CurrentCultureIgnoreCase)).Value;
                            DateTime? possibleDateFieldValue;
                            if (TryParse(fieldValue, out possibleDateFieldValue))
                            {
                                ((SimpleField) field).value = possibleDateFieldValue?.ToString(DefaultDateTimeFormat) ?? string.Empty;
                            }
                            else
                            {
                                ((SimpleField)field).value = fieldValue ?? string.Empty;
                            }
                        }

                    }
                    else
                    {
                        if (field is ArrayField)
                        {
                            var array = ((ArrayField) field).value;

                            if (array.Length == 1 && string.IsNullOrWhiteSpace(array[0]))
                            {
                                ((ArrayField) field).value = new string[0];
                            }
                        }
                    }
                }
            }

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToArray();
            foreach (var prop in properties)
            {
                if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
                {
                    var collection = prop.GetValue(model) as IEnumerable;
                    if (collection == null) continue;

                    foreach (var item in collection)
                    {
                        item.ApplyFieldChanges(values);
                    }
                    continue;
                }

                if (prop.PropertyType == typeof(string)) continue;

                if (!prop.PropertyType.IsClass) continue;

                try
                {
                    var subObj = prop.GetValue(model);
                    subObj.ApplyFieldChanges(values);
                }
                catch { }
            }
        }
        public static bool TryParse(string text, out DateTime? nDate)
        {
            DateTime date;
            bool isParsed = DateTime.TryParse(text, out date);
            nDate = isParsed ? new DateTime?(date) : new DateTime?();
            return isParsed;
        }
        public static T MapJson<T>(string values) where T : class
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<T>(values, settings); 
        }

        public static T GetDeserializedForm<T>(Stream form) where T : class
        {
            var bodyStream = new StreamReader(form);
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            var formInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(bodyText);
            return formInfo;
        }


        public static List<KeyValuePair<string, string>> ToKeyValuePair(this CommonFormSaveModel me)
		{
			List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
			foreach (var property in me.GetType().GetProperties())
			{

				result.Add(new KeyValuePair<string, string>(property.Name, property.GetValue(me)?.ToString()));
			}
			return result;
		}

		public static List<KeyValuePair<string, string>> ToKeyValuePairs(this object me)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            foreach (var property in me.GetType().GetProperties())
            {

                result.Add(new KeyValuePair<string, string>(property.Name, property.GetValue(me)?.ToString()));
            }
            return result;
        }

        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePair(this JObject values)
        {
            return values.Properties().Select(prop => new KeyValuePair<string, string>(prop.Name, prop.Value.ToString()));
        }

        /// <summary>
        /// Applies zip code, city and state to form values if there's a zipcode stored in session
        /// </summary>
        /// <param name="sessionRepository"></param>
        /// <param name="zipLookupService"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> ApplyZipCodeLookup(ISessionRepository sessionRepository, IZipLookupService zipLookupService, string coverageType)
        {
            List<KeyValuePair<string, string>> zipCodeValue = sessionRepository.RetrieveValues(FormExtensions.ZipCodeSessionKey).ToList();
            if (!zipCodeValue.Any()) return new KeyValuePair<string, string>[]{};
            ZipLookupModel zipLookupModel = zipLookupService.LookupZipCode(zipCodeValue.First().Value);
            if (string.IsNullOrEmpty(zipLookupModel.ZipCode)) return new KeyValuePair<string, string>[] { };
            var mapper = new List<KeyValuePair<string, string>>();
            switch (coverageType)
            {
                case CoverageTypes.Umbrella:
                    mapper = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>(nameof(CommonFormSaveModel.PolicyHolderPrimaryResidenceCity), zipLookupModel.City),
                        new KeyValuePair<string, string>(nameof(CommonFormSaveModel.PolicyHolderPrimaryResidenceState), zipLookupModel.StateAbbreviation),
                        new KeyValuePair<string, string>(nameof(CommonFormSaveModel.PolicyHolderPrimaryResidenceZip), zipLookupModel.ZipCode)
                    };
                    break;
                default:
                    mapper = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>(nameof(CommonFormSaveModel.PolicyHolderCity), zipLookupModel.City),
                        new KeyValuePair<string, string>(nameof(CommonFormSaveModel.City), zipLookupModel.City),
                        new KeyValuePair<string, string>(nameof(CommonFormSaveModel.PolicyHolderState), zipLookupModel.StateAbbreviation),
                        new KeyValuePair<string, string>(nameof(CommonFormSaveModel.State), zipLookupModel.StateAbbreviation),
                        new KeyValuePair<string, string>(nameof(CommonFormSaveModel.PolicyHolderZip), zipLookupModel.ZipCode),
                        new KeyValuePair<string, string>(nameof(CommonFormSaveModel.Zip), zipLookupModel.ZipCode)
                    };
                    break;
            }
            return mapper;
        }
    }
}