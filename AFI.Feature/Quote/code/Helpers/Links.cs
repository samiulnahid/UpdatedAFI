using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace AFI.Feature.Quote.Helpers
{
    public static class Links
    {
        [Obsolete]
        public static string GetFullyQualifiedUrl(this Item item)
        {
            var options = LinkManager.GetDefaultUrlOptions();
            options.AlwaysIncludeServerUrl = true;
            string link = LinkManager.GetItemUrl(item, options);
            return link;
        }
    }
}