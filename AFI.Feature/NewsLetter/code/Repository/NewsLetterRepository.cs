using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model = AFI.Feature.NewsLetter.Models;
using AFI.Foundation.Helper;
using Sitecore.Mvc.Presentation;
using AFI.Feature.NewsLetter.Constants;
using AFI.Feature.NewsLetter.Models;

namespace AFI.Feature.NewsLetter.Repository
{

    public interface INewsLetterRepository
    {
        Model.ReleatedNewsletter GetMoreNewsLetters(Guid id);
        Model.ArchivedNewsLetters GetArchivedNewsletters(int page);

    }
    public class NewsLetterRepository : INewsLetterRepository
    {

        public ReleatedNewsletter GetMoreNewsLetters(Guid id)
        {
            ReleatedNewsletter data = new ReleatedNewsletter();
            var parentItem = Sitecore.Context.Database.GetItem(Template.NewsLetter.ContentRootFolder);
            if (parentItem == null)
            {
                return new Model.ReleatedNewsletter();
            }
            int count = Convert.ToInt32(RenderingContext.Current.Rendering.Parameters["Count"]);
           
            var childItems = GetChildItemsByTemplateID(parentItem.ID, Template.NewsLetter.DetailsTemplateId, id);
            var newslettersList = FillNewsLetters(childItems);
            newslettersList = newslettersList.OrderByDescending(x => x.PublishDate).Take(count).ToList();
            data.newsLetters = newslettersList;

          
            return data;
        }

        private List<Item> GetChildItemsByTemplateID(ID parentId, ID templateId, Guid id)
        {
            var parentItem = Sitecore.Context.Database.GetItem(parentId);
            if (parentItem == null)
            {
                return new List<Item>();
            }

            var childItems = parentItem.Axes.GetDescendants().Where(x => x.TemplateID == templateId && !x.ID.Equals(id)).ToList();
            return childItems;
        }

        private List<Model.NewsLetter> FillNewsLetters(IEnumerable<Item> newsletters)
        {
            var newslettersList = new List<Model.NewsLetter>();
            foreach (var item in newsletters)
            {
                var newsletter = CreateNewsletterFromItem(item);
                newslettersList.Add(newsletter);
            }
            return newslettersList;
        }

        private Model.NewsLetter CreateNewsletterFromItem(Item item)
        {
            var newsletter = new Model.NewsLetter();
            newsletter.Title = GetFieldValue(item, FeatureTemplate.Page.Fields.Title);
            newsletter.Description = GetFieldValue(item, FeatureTemplate.Page.Fields.Content);
            newsletter.Link = LinkManager.GetItemUrl(item);
            newsletter.PublishDate = GetDateFieldValue(item, Template.NewsLetter.Fields.Date);
            newsletter.ShortDescription = GetFieldValue(item, Template.NewsLetter.Fields.ShortDescription);
            return newsletter;
        }

        private string GetFieldValue(Item item, ID fieldId)
        {
            var field = item.Fields[fieldId];
            return field != null ? field.Value : String.Empty;
        }

        private DateTime GetDateFieldValue(Item item, ID fieldId)
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

        //Archived Code Start
        public ArchivedNewsLetters GetArchivedNewsletters(int page)
        {
            if (page < 1)
            {
                page = 1;
            }
            var defaultPageSize = 3;
            var archivedNewsLetter = new ArchivedNewsLetters();
            archivedNewsLetter.ArchivedList = new Dictionary<DateTime, List<Model.NewsLetter>>();

            archivedNewsLetter.PageSize = defaultPageSize;
            archivedNewsLetter.CurrentPage = page;
            archivedNewsLetter.TotalCount = 0;

            var parentItem = Sitecore.Context.Database.GetItem(Template.NewsLetter.ArchivedRootFolder);
            if (parentItem == null)
            {
                return new Model.ArchivedNewsLetters();
            }

            var childItems = GetArchivedChildItemsByTemplateID(parentItem.ID, Template.NewsLetter.DetailsTemplateId);

            var archivedList = ArchivedFillNewsLetters(childItems);

            archivedNewsLetter.ArchivedList = archivedList;
            //archivedNewsLetter.ArchivedList = archivedNewsLetter.ArchivedList.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            archivedNewsLetter.TotalCount = archivedNewsLetter.ArchivedList.Keys.Count;

            archivedNewsLetter.ArchivedList = archivedNewsLetter.ArchivedList.Skip(defaultPageSize * (page - 1)).Take(defaultPageSize).ToDictionary(x => x.Key, x => x.Value);

            return archivedNewsLetter;
        }

        private List<Item> GetArchivedChildItemsByTemplateID(ID parentId, ID templateId)
        {
            var parentItem = Sitecore.Context.Database.GetItem(parentId);
            if (parentItem == null)
            {
                return new List<Item>();
            }
            var childItems = parentItem?.Axes.GetDescendants().Where(x => x.TemplateID == templateId).ToList();
            return childItems;
        }
        
        private Dictionary<DateTime, List<Model.NewsLetter>> ArchivedFillNewsLetters(IEnumerable<Item> newsletters)
        {
            var archivedList = new Dictionary<DateTime, List<Model.NewsLetter>>();

            foreach (var item in newsletters)
            {
                var newsletter = new Model.NewsLetter();
                newsletter.Title = GetFieldValue(item, FeatureTemplate.Page.Fields.Title);
                newsletter.Description = GetFieldValue(item, FeatureTemplate.Page.Fields.Content);
                newsletter.Link = LinkManager.GetItemUrl(item);
                newsletter.PublishDate = GetDateFieldValue(item, Template.NewsLetter.Fields.Date);
                newsletter.ShortDescription = GetFieldValue(item, Template.NewsLetter.Fields.ShortDescription);
                var potentialKey = new DateTime(newsletter.PublishDate.Year, newsletter.PublishDate.Month, 1);

                if (archivedList.ContainsKey(potentialKey))
                {
                    archivedList[potentialKey].Add(newsletter);
                    archivedList[potentialKey] = archivedList[potentialKey].OrderByDescending(x => x.PublishDate).ToList();
                }
                else
                {
                    var newsLetterList = new List<Model.NewsLetter>();
                    newsLetterList.Add(newsletter); 
                    archivedList.Add(potentialKey, newsLetterList);

                }
            }
            archivedList = archivedList.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            return archivedList;
        }
    }
}