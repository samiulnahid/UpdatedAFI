using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Links;
using Sitecore.Mvc.Common;
using Sitecore.Resources.Media;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;


namespace AFI.Foundation.Helper
{
    public static class SitecoreHelper
    {
        private static string GetFieldValue(Item item, ID fieldId)
        {
            var field = item.Fields[fieldId];
            return field != null ? field.Value : String.Empty;
        }

        private static DateTime GetDateFieldValue(Item item, ID fieldId)
        {
            var field = item.Fields[fieldId];
            if (field != null && !String.IsNullOrEmpty(field.Value))
            {
                return Sitecore.DateUtil.IsoDateToDateTime(field.Value);
            }
            else
            {
                return DateTime.MinValue;
            }
        }
        public static bool FieldHasValue(this Item item, ID fieldID)
        {
            return (item.Fields[fieldID] == null ? false : !string.IsNullOrWhiteSpace(item.Fields[fieldID].Value));
        }
        [Obsolete]
        public static string GetFullyQualifiedUrl(this Item item)
        {
            var options = LinkManager.GetDefaultUrlOptions();
            options.AlwaysIncludeServerUrl = true;
            string link = LinkManager.GetItemUrl(item, options);
            return link;
        }
        public static string GeneralLinkUrl(this LinkField lf)
        {
            string url;
            string lower = lf.LinkType.ToLower();
            if (lower == "internal")
            {
                url = (lf.TargetItem != null ? LinkManager.GetItemUrl(lf.TargetItem) : string.Empty);
            }
            else if (lower == "media")
            {
                url = (lf.TargetItem != null ? MediaManager.GetMediaUrl(lf.TargetItem) : string.Empty);
            }
            else if (lower == "external")
            {
                url = lf.Url;
            }
            else if (lower == "anchor")
            {
                url = (!string.IsNullOrEmpty(lf.Anchor) ? string.Concat("#", lf.Anchor) : string.Empty);
            }
            else if (lower == "mailto")
            {
                url = lf.Url;
            }
            else
            {
                url = (lower == "javascript" ? lf.Url : lf.Url);
            }
            return url;
        }
        public static string GetMediaFileUrl(this LinkField lf)
        {
            var url = lf.TargetItem != null ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(lf.TargetItem) : string.Empty;
            return url;
        }


        //public static bool GetBool(this Item item, string fieldName)
        //{
        //    return item.Fields[fieldName].IsChecked();
        //}

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
        [Obsolete]
        //public static string ImageUrl(this ImageField imageField, int? width = null, int? height = null)
        //{
        //    int num;
        //    string empty;
        //    object mediaItem;
        //    if (imageField != null)
        //    {
        //        mediaItem = imageField.MediaItem;
        //    }
        //    else
        //    {
        //        mediaItem = null;
        //    }
        //    if (mediaItem != null)
        //    {
        //        MediaUrlOptions value = MediaUrlOptions.Empty;
        //        if (width.HasValue)
        //        {
        //            value.Width = width.Value;
        //        }
        //        else if (int.TryParse(imageField.Width, out num))
        //        {
        //            value.Width = num;
        //        }
        //        if (height.HasValue)
        //        {
        //            value.Height = height.Value;
        //        }
        //        else if (int.TryParse(imageField.Height, out num))
        //        {
        //            value.Height = num;
        //        }
        //        empty = imageField.ImageUrl(value.Width, value.Height);
        //    }
        //    else
        //    {
        //        empty = string.Empty;
        //    }
        //    return empty;
        //    //if (mediaItem == null)
        //    //{
        //    //    throw new ArgumentNullException("mediaItem");
        //    //}
        //    //string url = MediaManager.GetMediaUrl(mediaItem, new MediaUrlOptions());

        //    //return HashingUtils.ProtectAssetUrl(StringUtil.EnsurePrefix('/', url));
        //}
        public static string GetLinkUrl(Item item, ID id)
        {
            string url = "";
            LinkField linkfield = item.Fields[id];
            if (linkfield != null)
            {
                url = linkfield.GetFriendlyUrl();
            }
            return url;
        }

        public static string GetLinkUrlName(Item item, ID id)
        {
            string name = "";
            LinkField linkfield = item.Fields[id];
            if (linkfield != null)
            {
                name = linkfield.Text;
            }
            return name;
        }

        public static string GetLinkUrlTarget(Item item, ID id)
        {
            string name = "";
            LinkField linkfield = item.Fields[id];
            if (linkfield != null)
            {
                string lower = linkfield.LinkType.ToLower();
                if (lower == "internal")
                {
                    name = "_self";
                }
                else if (lower == "external")
                {
                    name = "_blank";
                }
                name = linkfield.Target;
            }
            return name;
        }

        public static bool HasField(this Item item, ID fieldId)
        {
            return TemplateManager.IsFieldPartOfTemplate(fieldId, item);
        }

        public static bool HasLayout(this Item item)
        {
            object layout;
            if (item != null)
            {
                ItemVisualization visualization = item.Visualization;
                if (visualization != null)
                {
                    layout = visualization.Layout;
                }
                else
                {
                    layout = null;
                }
            }
            else
            {
                layout = null;
            }
            return layout != null;
        }

        public static Item GetLinkedItem(Item item, string itemField)
        {
            string dropDownItemId = item[itemField];
            return Sitecore.Context.Database.GetItem(dropDownItemId);
        }


        public static bool IsChecked(this Field checkboxField)
        {
            if (checkboxField == null)
            {
                throw new ArgumentNullException("checkboxField");
            }
            return MainUtil.GetBool(checkboxField.Value, false);
        }

        public static bool IsFieldAvailable(Item item, string fieldName)
        {
            bool flag;
            try
            {
                if ((item == null ? true : string.IsNullOrEmpty(fieldName)))
                {
                    flag = false;
                }
                else if ((item.Fields == null || !item.Fields.Any<Field>((Field x) => x.Name == fieldName) ? true : !item.Fields[fieldName].HasValue))
                {
                    flag = false;
                    return flag;
                }
                else
                {
                    flag = true;
                }
            }
            catch (Exception exception)
            {
                flag = false;
            }
            return flag;
        }

        public static string LinkFieldUrl(this Item item, string fieldName)
        {
            string url;
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException("fieldName");
            }
            Field field = item.Fields[fieldName];
            if ((field == null ? false : FieldTypeManager.GetField(field) is LinkField))
            {
                LinkField linkField = field;
                string lower = linkField.LinkType.ToLower();
                if (lower == "internal")
                {
                    url = (linkField.TargetItem != null ? LinkManager.GetItemUrl(linkField.TargetItem) : string.Empty);
                }
                else if (lower == "media")
                {
                    url = (linkField.TargetItem != null ? MediaManager.GetMediaUrl(linkField.TargetItem) : string.Empty);
                }
                else if (lower == "external")
                {
                    url = linkField.Url;
                }
                else if (lower == "anchor")
                {
                    url = (!string.IsNullOrEmpty(linkField.Anchor) ? string.Concat("#", linkField.Anchor) : string.Empty);
                }
                else if (lower == "mailto")
                {
                    url = linkField.Url;
                }
                else
                {
                    url = (lower == "javascript" ? linkField.Url : linkField.Url);
                }
            }
            else
            {
                url = string.Empty;
            }
            return url;
        }

        [Obsolete]
        public static string Url(this Item item, UrlOptions options = null)
        {
            string itemUrl;
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (options == null)
            {
                itemUrl = (!item.Paths.IsMediaItem ? LinkManager.GetItemUrl(item) : MediaManager.GetMediaUrl(item));
            }
            else
            {
                itemUrl = LinkManager.GetItemUrl(item, options);
            }
            return itemUrl;
        }

        public static string GetMonth(int month)
        {
            bool isArabic = Sitecore.Context.Language.Name.Equals("ar-qa", StringComparison.InvariantCultureIgnoreCase);
            string monthName = "";
            try
            {
                DateTimeFormatInfo mfi = new DateTimeFormatInfo();
                if (isArabic)
                {
                    monthName = CultureInfo.CreateSpecificCulture("ar-qa").DateTimeFormat.GetAbbreviatedMonthName(month);
                }
                else
                {
                    monthName = mfi.GetMonthName(month).ToString();
                }

            }
            catch (Exception)
            {

            }

            return monthName;
        }
        public static class GlobalConfigurations
        {
            public const string DefaultDateformat = "dd MMMM yyyy";
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

    }
}
