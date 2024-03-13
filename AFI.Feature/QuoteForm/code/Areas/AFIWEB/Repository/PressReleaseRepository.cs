using System;
using System.Collections.Generic;
using System.Linq;
using AFI.Foundation.Helper.Models;
using AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using Identifiers = AFI.Feature.Identifiers;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Repository
{
    public interface IPressReleaseRepository
    {      
        IEnumerable<Item> GetAll(Database database);
        PressReleaseList GetPressReleases(Database database, int year, int page);
    }
    public class PressReleaseRepository : IPressReleaseRepository
    {
        private readonly IGlobalSettingsRepository _globalSettingsRepository;
        public PressReleaseRepository(IGlobalSettingsRepository globalSettingsRepository)
        {
            _globalSettingsRepository = globalSettingsRepository;
        }

        public IEnumerable<Item> GetAll(Database database)
        {
            var PressReleaseRoot = GetPressReleaseRoot(database);
            return PressReleaseRoot?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.Press_Release_Article_Page.TemplateId) ?? new Item[0];
        }

        [Obsolete]
        public PressReleaseList GetPressReleases(Database database,int year, int page)
        {
            if (page < 1)
            {
                page = 1;
            } 
            var defaultPageSize = 10;
            var pressReleasesList = new List<PressRelease>();
            var pressReleaseItems = GetAll(database);
            foreach (var item in pressReleaseItems)
            {
                var pressRelease = new PressRelease();
                pressRelease.Headline =
                    item[Identifiers.Templates.News_Letter_Page.FieldNames.Title];
                pressRelease.Description = TextTruncator(
                    item[Identifiers.Templates.News_Letter_Page.FieldNames.Description]);
                var image =
                    (Sitecore.Data.Fields.ImageField)item.Fields[
                        Identifiers.Templates.News_Letter_Page.FieldNames.Image];

                pressRelease.Image = !string.IsNullOrEmpty(image.ImageUrl())
                    ? pressRelease.Image = image.ImageUrl()
                    : pressRelease.Image = _globalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings
                        .FieldNames.Press_Release_Default_Image);
                pressRelease.Link = LinkManager.GetItemUrl(item);
                pressRelease.PublishDate = Sitecore.DateUtil.IsoDateToDateTime(item.Fields[Identifiers.Templates.News_Letter_Page.FieldNames.Date].Value);
                pressReleasesList.Add(pressRelease);
            }

            var yearList = pressReleasesList.Select(x => x.PublishDate.Year).Distinct().ToList();
            if (year != Int32.MinValue)
            {
                pressReleasesList = pressReleasesList
                    .Where(x => x.PublishDate.Year == year)
                    .OrderByDescending(x => x.PublishDate).ToList();
            }
            else
            {
                pressReleasesList = pressReleasesList.OrderByDescending(x => x.PublishDate).ToList();
            }

            return new PressReleaseList
            {
                PressReleases = pressReleasesList.Skip(defaultPageSize * (page - 1)).Take(defaultPageSize).ToList(),
                Years = yearList,
                PageSize = defaultPageSize,
                CurrentPage = page,
                TotalCount = pressReleasesList.Count,
                SelectedYear = year.ToString()
            };
        }
        private Item GetPressReleaseRoot(Database database)
        {
            return database.GetItem(
                   _globalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.Press_Releases_Root));

        }
        private  static string TextTruncator(string text, int charLimit = 200)
        {
            if (text.Length <= charLimit)
                return text;

            var charArray = text.ToCharArray();
            for (var c = charLimit - 3; c > 0; c--)
            {
                if (charArray[c] == ' ')
                {
                    return text.Substring(0, c) + " ...";
                }
            }

            return text.Substring(0, 0) + " ...";
        }
    }
}