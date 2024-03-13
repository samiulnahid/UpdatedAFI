using System;
using System.Collections.Generic;
using System.Linq;
using AFI.Foundation.Helper.Models;
using Identifiers = AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers.Configurations;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using Templates = AFI.Feature.Identifiers.Templates;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Repository
{
    public interface INewsLetterRepository
    {
        NewsLetterList GetNewsLetters(int page);
        List<NewsLetter> GetMoreNewsLetters(Guid id);
        ArchivedNewsLetters GetArchivedNewsletters(int page);

    }
    public class NewsLetterRepository : INewsLetterRepository
    {
        private readonly IGlobalSettingsRepository _globalSettingsRepository;
        public NewsLetterRepository(IGlobalSettingsRepository globalSettingsRepository)
        {
            _globalSettingsRepository = globalSettingsRepository;

        }

        private Item GetNewslettersRootByFolder(string folderPath)
        {
            string path = _globalSettingsRepository.GetSetting(folderPath);
            Item newslettersFolderRoot = Sitecore.Context.Database.GetItem(path);
            return newslettersFolderRoot;
        }

        [Obsolete]
        public NewsLetterList GetNewsLetters(int page)
        {
            var pageSizeSetting = _globalSettingsRepository.GetSetting(Templates.Global_Settings.FieldNames.Results_Per_Page);
            var defaultPageSize = Int32.Parse(!string.IsNullOrEmpty(pageSizeSetting) ? pageSizeSetting : "10");
            var list = new NewsLetterList
            {
                Date = DateTime.Now.ToString(GlobalConfigurations.DefaultDateformat),
                CurrentPage = page,
                NewsLetters = new List<NewsLetter>(),
                PageSize = defaultPageSize,
                TotalCount = 0
            };
            Item newslettersFolderRoot = GetNewslettersRootByFolder(Templates.Global_Settings.FieldNames.Newsletter_Current_Folder_Path);
            if (newslettersFolderRoot == null) return list;
            Item newsletterPageRoot = GetNewsLettersRoot();
            if (newsletterPageRoot == null) return list;
            list.Headline = newsletterPageRoot[Templates.NewsLetter_Listing_Page.FieldNames.Newsletters_Headline] ?? string.Empty;
            int? totalCount = newslettersFolderRoot?.Axes.GetDescendants()?.Count(x => x.TemplateID == Templates.News_Letter_Page.TemplateId);
            int skip = defaultPageSize * (page - 1);
            IEnumerable<Item> newsletters = newslettersFolderRoot?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.News_Letter_Page.TemplateId).Skip(skip).Take(defaultPageSize);
            List<NewsLetter> newslettersList = FillNewsLetters(newsletters);
            list.NewsLetters = newslettersList;
            list.TotalCount = totalCount ?? 0;
            return list;
        }

        [Obsolete]
        public List<NewsLetter> GetMoreNewsLetters(Guid id)
        {
            Item newslettersFolderRoot = GetNewslettersRootByFolder(Templates.Global_Settings.FieldNames.Newsletter_Current_Folder_Path);
            var newsLetters = newslettersFolderRoot?.Axes.GetDescendants().Where(x => x.TemplateID == Templates.News_Letter_Page.TemplateId && !x.ID.Equals(id));
            List<NewsLetter> newslettersList = FillNewsLetters(newsLetters);
            newslettersList = newslettersList.OrderByDescending(x => x.PublishDate).Take(3).ToList();
            return newslettersList;
        }

        [Obsolete]
        private List<NewsLetter> FillNewsLetters(IEnumerable<Item> newsletters)
        {
            var newslettersList = new List<NewsLetter>();
            foreach (var item in newsletters)
            {
                var newsletter = new NewsLetter();
                newsletter.Headline = item.Fields[Identifiers.Templates.News_Letter_Page.FieldNames.Title].Value;
                newsletter.Description = TextTruncator(item.Fields[Identifiers.Templates.News_Letter_Page.FieldNames.Description].Value);
                var image = (Sitecore.Data.Fields.ImageField)item.Fields[Identifiers.Templates.News_Letter_Page.FieldNames.Image];
                if (image != null && !string.IsNullOrEmpty(image.ImageUrl()))
                {
                    newsletter.Image = image.ImageUrl();
                }
                else
                {
                    newsletter.Image = _globalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.News_Letter_Default_Image);
                }
                newsletter.Link = LinkManager.GetItemUrl(item);
                newsletter.PublishDate = Sitecore.DateUtil.IsoDateToDateTime(item.Fields[Identifiers.Templates.News_Letter_Page.FieldNames.Date].Value);
                newslettersList.Add(newsletter);
            }
            return newslettersList;
        }

        public Item GetNewsLettersRoot()
        {
            var newsLetters = Sitecore.Context.Database.GetItem(_globalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.NewsLetter_Root));
            return newsLetters;
        }

        public ArchivedNewsLetters GetArchivedNewsletters(int page)
        {
            if (page < 1)
            {
                page = 1;
            }
            var defaultPageSize = 3;
            var archivedNewsLetter = new ArchivedNewsLetters();
            archivedNewsLetter.ArchivedList = new Dictionary<DateTime, List<NewsLetter>>();
            archivedNewsLetter.PageSize = defaultPageSize;
            archivedNewsLetter.CurrentPage = page;
            archivedNewsLetter.TotalCount = 0;
            Item newslettersRoot = GetNewslettersRootByFolder(Templates.Global_Settings.FieldNames.Newsletter_Archived_Folder_Path);
            if (newslettersRoot == null) return archivedNewsLetter;
            var newsLetters = newslettersRoot?.Axes.GetDescendants().Where(x => x.TemplateID == Identifiers.Templates.News_Letter_Page.TemplateId);
            foreach (var item in newsLetters)
            {

                var newsletter = new NewsLetter();
                newsletter.PublishDate = Sitecore.DateUtil.IsoDateToDateTime(item.Fields[Identifiers.Templates.News_Letter_Page.FieldNames.Date].Value);
                newsletter.Headline = item.Fields[Identifiers.Templates.News_Letter_Page.FieldNames.Title].Value;
                newsletter.Description = TextTruncator(item.Fields[Identifiers.Templates.News_Letter_Page.FieldNames.Description].Value);
                newsletter.Link = LinkManager.GetItemUrl(item);
                newsletter.PublishDate = Sitecore.DateUtil.IsoDateToDateTime(item.Fields[Identifiers.Templates.News_Letter_Page.FieldNames.Date].Value);
                var potentialKey = new DateTime(newsletter.PublishDate.Year, newsletter.PublishDate.Month, 1);
                if (archivedNewsLetter.ArchivedList.ContainsKey(potentialKey))
                {
                    archivedNewsLetter.ArchivedList[potentialKey].Add(newsletter);
                    archivedNewsLetter.ArchivedList[potentialKey] = archivedNewsLetter.ArchivedList[potentialKey].OrderByDescending(x => x.PublishDate).ToList();
                }
                else
                {
                    var newsLetterList = new List<NewsLetter>();
                    newsLetterList.Add(newsletter);
                    archivedNewsLetter.ArchivedList.Add(potentialKey, newsLetterList);
                }
            }

            archivedNewsLetter.ArchivedList = archivedNewsLetter.ArchivedList.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            archivedNewsLetter.TotalCount = archivedNewsLetter.ArchivedList.Keys.Count;
            archivedNewsLetter.ArchivedList = archivedNewsLetter.ArchivedList.Skip(defaultPageSize * (page - 1)).Take(defaultPageSize).ToDictionary(x => x.Key, x => x.Value);

            return archivedNewsLetter;
        }
        private static string TextTruncator(string text, int charLimit = 200)
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