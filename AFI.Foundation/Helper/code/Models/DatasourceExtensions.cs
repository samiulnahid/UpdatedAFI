using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AFI.Foundation.Helper.Models
{
    public static class DatasourceExtensions
    {
        public static Item ResolveDatasource(this Database db, string datasource, Item contextItem = null)
        {
            IEnumerable<Item> source = db.ResolveDatasourceItems(datasource, contextItem);
            return source != null ? source.FirstOrDefault<Item>() : (Item)null;
        }

        public static IEnumerable<Item> ResolveDatasourceItems(
          this Database db,
          string datasource,
          Item contextItem = null)
        {
            if (string.IsNullOrWhiteSpace(datasource))
                return (IEnumerable<Item>)new Item[0];
            try
            {
                ID[] array = ID.ParseArray(datasource);
                if ((uint)array.Length > 0U)
                    return ((IEnumerable<ID>)array).Select<ID, Item>(new Func<ID, Item>(db.GetItem)).Where<Item>((Func<Item, bool>)(x => x != null));
            }
            catch
            {
            }
            ID id = ShortID.IsShortID(datasource) ? ShortID.Parse(datasource).ToID() : ID.Null;
            if (!ID.IsNullOrEmpty(id))
            {
                Item obj = db.GetItem(id);
                Item[] objArray;
                if (obj != null)
                    objArray = new Item[1] { obj };
                else
                    objArray = new Item[0];
                return (IEnumerable<Item>)objArray;
            }
            string query = (string)null;
            if (!string.IsNullOrWhiteSpace(datasource) && datasource.StartsWith("query:"))
                query = datasource.Substring("query:".Length);
            else if (!string.IsNullOrWhiteSpace(datasource) && (datasource.StartsWith("/") || datasource.StartsWith("./") || datasource.StartsWith("../")))
                query = DatasourceExtensions.EscapeItemPathForQuery(datasource);
            return !string.IsNullOrWhiteSpace(query) ? (IEnumerable<Item>)((contextItem == null ? db.SelectItems(query) : contextItem.Axes.SelectItems(query)) ?? new Item[0]) : (IEnumerable<Item>)new Item[0];
        }

        public static string EscapeItemPathForQuery(string itemPath, params string[] reservedStrings)
        {
            if (reservedStrings == null || !((IEnumerable<string>)reservedStrings).Any<string>())
                reservedStrings = new string[15]
                {
          "ancestor",
          "and",
          "child",
          "descendant",
          "div",
          "false",
          "following",
          "mod",
          "or",
          "parent",
          "preceding",
          "self",
          "true",
          "xor",
          "-"
                };
            return Regex.Replace(itemPath, "(/)([^/#]*\\b(?:" + string.Join("|", reservedStrings) + ")\\b[^/#]*)(/|$)", "$1#$2#$3", RegexOptions.IgnoreCase);
        }

        public static IEnumerable<Item> GetLinkedItems(this Item item, string fieldName)
        {
            IEnumerable<Item> objs;
            if (item == null)
            {
                objs = (IEnumerable<Item>)null;
            }
            else
            {
                Database database = item.Database;
                objs = database != null ? database.ResolveDatasourceItems(item[fieldName], item) : (IEnumerable<Item>)null;
            }
            return objs;
        }

        public static IEnumerable<Item> GetLinkedItems(this Item item, ID fieldId)
        {
            IEnumerable<Item> objs;
            if (item == null)
            {
                objs = (IEnumerable<Item>)null;
            }
            else
            {
                Database database = item.Database;
                objs = database != null ? database.ResolveDatasourceItems(item[fieldId], item) : (IEnumerable<Item>)null;
            }
            return objs;
        }
    }
}