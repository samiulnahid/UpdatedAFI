using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.PressRelease.Models;
using AFI.Foundation.Helper;
using Sitecore.Mvc.Presentation;
using AFI.Feature.PressRelease.Constant;
using Model = AFI.Feature.PressRelease.Models;

namespace AFI.Feature.PressRelease.Repository
{
    public interface IpressreleaseRepository
    {
        Model.PressReleaseList GetFilterPressRelease(Guid id, string year, int page);
    }
        public class PressReleasesRepository : IpressreleaseRepository
        {
            public PressReleaseList GetFilterPressRelease(Guid id, string year, int page)
            {
                if (page < 1)
                {
                    page = 1;
                }
                var defaultPageSize = 10;
                var parentItem = Sitecore.Context.Database.GetItem(Template.PressRelease.ContentRootFolder);
                if (parentItem == null)
                {
                    return new Model.PressReleaseList();
                }
                var childItems = GetChildItemsByTemplateID(parentItem.ID, Template.PressRelease.DetailsTemplateId, id);
                var pressreleasesList = FillNewsLetters(childItems);
                var yearsdetails = GetYearWisePress(parentItem.ID, Template.PressRelease.PressReleaseYearFolder, id);
                var yearllist = Fillyears(yearsdetails);
                if (year == "ALL YEARS" || year == null)
                {
                    pressreleasesList = pressreleasesList.OrderByDescending(x => x.PublishDate).ToList();
                }
                else
                {
                    pressreleasesList = pressreleasesList.Where(x => x.PublishDate.Year == Int32.Parse(year))
                      .OrderByDescending(x => x.PublishDate).ToList();
                }
                return new PressReleaseList
                {
                    PressRelease = pressreleasesList.Skip(defaultPageSize * (page - 1)).Take(defaultPageSize).ToList(),
                    YearList = yearllist,
                    PageSize = defaultPageSize,
                    CurrentPage = page,
                    TotalCount = pressreleasesList.Count
                };
            }
            private List<Item> GetYearWisePress(ID parentId, ID templateId, Guid id)
            {
                var parentitem = Sitecore.Context.Database.GetItem(parentId);
                if (parentitem == null)
                {
                    return new List<Item>();
                }
                var childItems = parentitem.Axes.GetDescendants().Where(x => x.TemplateID == templateId && !x.ID.Equals(id)).ToList();
                return childItems;
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
            private List<Yearlist> Fillyears(IEnumerable<Item> years)
            {
                //var yearitem = years.GetDescendants();
                var Yearlist = new List<Yearlist>();
                foreach (var item in years)
                {
                    var yearitem = cearteyeardropdown(item);
                    Yearlist.Add(yearitem);
                }
                return Yearlist.OrderByDescending(x => x.yearname).ToList();
            }
            private Yearlist cearteyeardropdown(Item item)
            {
                var year = new Yearlist();
                year.yearname = item.Name.ToString();
                year.yearvalue = item.Name.ToString();
                return year;
            }
            private List<PressReleases> FillNewsLetters(IEnumerable<Item> pressrelease)
            {
                var pressreleaseList = new List<PressReleases>();
                foreach (var item in pressrelease)
                {
                    var pressreleaseitem = CreateNewsletterFromItem(item);
                    pressreleaseList.Add(pressreleaseitem);
                }
                return pressreleaseList.OrderByDescending(x => x.PublishDate).ToList();
            }
            private PressReleases CreateNewsletterFromItem(Item item)
            {
                var PressRelease = new PressReleases();
                PressRelease.Title = GetFieldValue(item, FeatureTemplate.Page.Fields.Title);
                PressRelease.Description = GetFieldValue(item, Template.PressRelease.Fields.Description);
                PressRelease.Link = LinkManager.GetItemUrl(item);
                PressRelease.PublishDate = GetDateFieldValue(item, Template.PressRelease.Fields.Date);
                return PressRelease;
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
        }
    }