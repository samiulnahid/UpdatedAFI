using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Mappers.Helpers
{
    public static class MapperHelper
    {
        /// <summary>
        /// Returns the property value with the desired type of a property name formed by a prefix an index and a suffix.
        /// This happens when more than one property contains the same prefix and suffix but different indexes.
        /// i.e: Vehicle1Value where Vehicle is the prefix, 1 is the index and Value is the suffix
        /// </summary>
        /// <typeparam name="T">CommonFormSaveModel or inherited class</typeparam>
        /// <typeparam name="TU">Type of the value you need to return</typeparam>
        /// <param name="model"></param>
        /// <param name="prefix">Prefix of the property name of the model</param>
        /// <param name="suffix">Suffix of the property name of the model</param>
        /// <param name="index">Index of the property name of the model</param>
        /// <returns></returns>
        public static TU GetPropertyValueByIndex<T, TU>(this T model, string prefix, string suffix, int index) where T : CommonFormSaveModel
        {
            string regex = $"(?i)^{prefix}{index}{suffix}$";
            PropertyInfo property = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .FirstOrDefault(p => Regex.Match(p.Name, regex).Success);
            object value = property?.GetValue(model, null);
            var type = typeof(TU);
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (value == null)
                {
                    return default(TU);
                }

                type = Nullable.GetUnderlyingType(type);
            }
            return (TU) System.Convert.ChangeType(property?.GetValue(model, null), type);
        }

        public static void SetPropertyValueByIndex<T>(this T model, string prefix, string suffix, int index, object value) where T : CommonFormSaveModel
        {
            string regex = $"(?i)^{prefix}{index}{suffix}$";
            PropertyInfo property = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .FirstOrDefault(p => Regex.Match(p.Name, regex).Success);
            property?.SetValue(model, value);
        }

        public static List<int> FormObjectIndexExtractor<T>(this T model, string prefix, string suffix) where T : CommonFormSaveModel
        {
            string regex = "(?i)^" + prefix + @"(\d+)\.*?" + suffix + "$";
            return model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(p => Regex.Match(p.Name, regex).Success)
                .Where(p => !string.IsNullOrWhiteSpace(p.GetValue(model, null)?.ToString()))
                .Select(p => int.Parse(Regex.Match(p.Name, regex).Groups[1].Value))
                .ToList();
        }

        public static List<int> FormObjectIndexExtractor(this IEnumerable<KeyValuePair<string, string>> values, string prefix, string suffix)
        {
            string regex = "(?i)^" + prefix + @"(\d+)\.*?" + suffix + "$";
            return values.Select(k=> k.Key.ToLower())
                .Where(v => Regex.Match(v, regex).Success)
                .Select(v => int.Parse(Regex.Match(v, regex).Groups[1].Value))
                .ToList();
        }

        public static float? TryGetFloat(this string floatValue)
        {
            float val;
            if (float.TryParse(floatValue, out val))
            {
                return val;
            }

            return null;
        }

        public static int? TryGetInt(this string integerValue)
        {
            int val;
            if (int.TryParse(integerValue, out val))
            {
                return val;
            }
            return null;
        }

        public static DateTime? TryGetDate(this string date)
        {
			DateTime outputDate;
            if (DateTime.TryParse(date, out outputDate))
            {
                return outputDate;
            }
            return null;
        }

        public static string GetValueFromKey(this KeyValuePair<string, string>[] values, string key)
        {
            try
            {
                return values.FirstOrDefault(c => c.Key == key).Value;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }
    }
}