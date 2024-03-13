using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Common;
using Sitecore.Resources.Media;
using Sitecore.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers.Strings
{
    public static class ReplaceTokensUsingDictionary
    {
        public static string ReplaceTokens(this Regex re, string input, IDictionary<string, string> args)
        {
            return re.Replace(input, delegate (Match match)
            {
                string val = null;
                bool isValueAvailable = args.TryGetValue(match.Groups[1].Value, out val);
                return isValueAvailable ? val : $"{match.Groups[1].Value}";
            });
        }
    }

    public static class PaginationUrlBuilder
    {
        /// <summary>
        /// Builds a URL from the Item and the current QueryString parameters.
        /// If the key exists, the value will be replaced by the passed one.
        /// </summary>
        /// <param name="item">Item to be used in the url</param>
        /// <param name="queryStringKeyValues">QueryString KeyValue pairs</param>
        /// <param name="keyToAdd">Key to be added as a query string parameter</param>
        /// <param name="value">Value for the query string key</param>
        /// <returns></returns>
        public static string Build(Item item, IEnumerable<KeyValue> queryStringKeyValues, string keyToAdd, string value)
        {
            string pageUrl = Sitecore.Links.LinkManager.GetItemUrl(item);
            bool existKey = false;
            foreach (var keyValue in queryStringKeyValues)
            {
                if (keyValue.Key == keyToAdd)
                {
                    pageUrl = WebUtil.AddQueryString(pageUrl, keyValue.Key, value);
                    existKey = true;
                }
                else
                {
                    pageUrl = WebUtil.AddQueryString(pageUrl, keyValue.Key, keyValue.Value);

                }
            }

            if (!existKey)
            {
                pageUrl = WebUtil.AddQueryString(pageUrl, keyToAdd, value);
            }

            return pageUrl;
        }
    }

    public static class SitecoreHelper
    {
        public static string GeneralLinkUrl(this LinkField lf)
        {
            switch (lf.LinkType.ToLower())
            {
                case "internal":
                    // Use LinkMananger for internal links, if link is not empty
                    return lf.TargetItem != null ? LinkManager.GetItemUrl(lf.TargetItem) : string.Empty;
                case "media":
                    // Use MediaManager for media links, if link is not empty
                    return lf.TargetItem != null ? MediaManager.GetMediaUrl(lf.TargetItem) : string.Empty;
                case "external":
                    // Just return external links
                    return lf.Url;
                case "anchor":
                    // Prefix anchor link with # if link if not empty
                    return !string.IsNullOrEmpty(lf.Anchor) ? "#" + lf.Anchor : string.Empty;
                case "mailto":
                    // Just return mailto link
                    return lf.Url;
                case "javascript":
                    // Just return javascript
                    return lf.Url;
                default:
                    return lf.Url;
            }
        }

        public static string GetImageUrl(Item item, ID id)
        {
            string url = "";
            ImageField imgField = item.Fields[id];
            if (imgField.MediaItem != null)
            {
                url = MediaManager.GetMediaUrl(imgField.MediaItem);
            }

            return url;
        }

        public static string GetImageUrl(Item item, string fieldName)
        {
            string url = "";
            ImageField imgField = item.Fields[fieldName];
            if (imgField != null && imgField.MediaItem != null)
            {
                url = MediaManager.GetMediaUrl(imgField.MediaItem);
            }

            return url;
        }

        public static bool IsFieldAvailable(Item item, string fieldName)
        {
            try
            {
                if (item == null || string.IsNullOrEmpty(fieldName))
                    return false;
                if (item.Fields != null && item.Fields.Any(x => x.Name == fieldName) && item.Fields[fieldName].HasValue)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return false;
        }

    }
}