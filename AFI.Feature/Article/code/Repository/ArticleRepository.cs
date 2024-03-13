using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Sitecore.Data.Fields;
using Sitecore.Resources.Media;
using Sitecore.Data.Items;
using System.Configuration;
using AFI.Feature.Article.Models;
using AFI.Feature.Article.Constants;

/// <summary>
/// carousel added lines no.
/// 28, 228-265
/// </summary>

namespace AFI.Feature.Article.Repository
{
    public interface IArticleRepository
    {
        
        Models.Article GetArticle(Item item);
        Models.ArticleSpotlight GetArticleGrid(Item item);
        Models.ArticleSpotlight GetPromotedArticles(Item item);

        // Carousel added
        Models.ArticleSpotlight GetCarouselArticles(Item item);
        //object GetArticles(Item dataSourceId, string category, string tag);

        // newly added All Articles
        Models.ArticleSpotlight GetAllArticle(Item item, string category = "", string tag = "");

        // Carousel end


    }
    public class ArticleRepository : IArticleRepository
    {
        public Models.Article GetArticle(Item item)
        {
            var tagpath = ConfigurationManager.AppSettings["ArticleTagPath"];
            Models.Article article = new Models.Article();
            if (item != null)
            {
                string category = ""; string categoryURL = "";
                if (item.Fields[Template.ArticleContent.Fields.Category] != null)
                {
                    var data = Sitecore.Context.Database.GetItem(item.Fields[Template.ArticleContent.Fields.Category].Value);
                    if (data != null)
                    {
                        category = data.Fields["Title"].ToString();
                        categoryURL = Sitecore.Links.LinkManager.GetItemUrl(data);
                    }
                }

                string tags = "";
                foreach (var tg in item[Template.ArticleContent.Fields.Tags].Split('|'))
                {
                    var t_data = Sitecore.Context.Database.GetItem(tg);
                    if (t_data != null)
                    {
                        if (!string.IsNullOrEmpty(tags))
                        {
                            // tags = tags + ", " + t_data.Name;
                            tags = tags + ", " + "<a href=" + tagpath + "?tag=" + t_data.Name.Replace(" ", "-") + "><span class='Mtag'>" + t_data.Name + "</span></a>";
                        }
                        else
                        {
                            tags = "<a href=" + tagpath + "?tag=" + t_data.Name.Replace(" ", "-") + "><span class='Mtag'>" + t_data.Name + "</span></a>";// t_data.Name;

                        }

                    }
                }

                string date = "";
                if (item.Fields[Template.ArticleContent.Fields.Date].Value != null && !((Sitecore.Data.Fields.DateField)item.Fields[Template.ArticleContent.Fields.Date]).DateTime.Equals(DateTime.MinValue) &&
                !((Sitecore.Data.Fields.DateField)item.Fields[Template.ArticleContent.Fields.Date]).DateTime.Equals(DateTime.MaxValue))
                {
                    Sitecore.Data.Fields.DateField df = item.Fields[Template.ArticleContent.Fields.Date];
                    date = df.DateTime.ToString("MMMM dd, yyyy");
                }
                article = new Models.Article
                {

                    Date = date,
                    LinkUrl = Sitecore.Links.LinkManager.GetItemUrl(item),
                    Tags = tags,
                    Category = category,
                    CategoryPageURL = categoryURL
                };
            }
            return article;
        }
        public ArticleSpotlight GetArticleGrid(Item item)
        {
            string category = Sitecore.Context.Item.Name;
            ArticleSpotlight article = new ArticleSpotlight();
            List<Models.Article> articles = new List<Models.Article>();
            var allData = Sitecore.Context.Database.GetItem(Template.ArticleRoot.RootID);

            if(item != null)
            {
                var isFound = false;
                foreach (Sitecore.Data.Items.Item child in allData.Children.Where(x=>x.TemplateID.ToString() == Template.ArticleRoot.ArticleTemplateID))
                {
                    var data = Sitecore.Context.Database.GetItem(child.ID);
                    

                    if (data != null)
                    {
                        string item_Cat = "";
                        string categoryURL = "";

                        if (data.Fields["Category"].Value != null)
                        {
                            var category_data = Sitecore.Context.Database.GetItem(data.Fields["Category"].Value);
                            if (category_data != null)
                            {
                                item_Cat = category_data.Fields["Title"].ToString();
                                categoryURL = Sitecore.Links.LinkManager.GetItemUrl(category_data);

                                if (!string.IsNullOrEmpty(category))
                                {
                                    if (item_Cat.Trim().ToLower() == category.Trim().ToLower())
                                    {
                                        isFound = true;
                                    }
                                }
                            }
                        }

                        string imageUrl = string.Empty;
                        ImageField img = data.Fields["Thumb_Image"];
                        if (img.MediaItem != null)
                        {
                            imageUrl =  MediaManager.GetMediaUrl(img.MediaItem);
                        }
                        if(isFound)
                        {
                            articles.Add(new Models.Article
                            {
                                Title =  data["Title"],
                                ThumbImage = imageUrl,
                                ID = data.ID.ToString(),
                                ArticlePageURL = Sitecore.Links.LinkManager.GetItemUrl(data),
                                CategoryPageURL = categoryURL,
                                Category = item_Cat
                            });
                        }
                    }
                    isFound = false;
                }
            }

            article.Articles = articles;
            return article;
        }

        public ArticleSpotlight GetPromotedArticles(Item item)
        {
            ArticleSpotlight Promoted = new ArticleSpotlight();
            List<Models.Article> articlses = new List<Models.Article>();
            if (item !=null)
            {
                if(item.Fields[Template.PromotedArticles.Fields.BannerArticle] != null)
                {
                    Sitecore.Data.Fields.ReferenceField referenceField = item.Fields[Template.PromotedArticles.Fields.BannerArticle];

                    if(referenceField!=null && referenceField.TargetItem!=null)
                    {
                        Item data = referenceField.TargetItem;

                        Models.Article article = new Models.Article();
                        if(data!=null)
                        {
                            string imageUrl = string.Empty;
                            ImageField img = data.Fields["Thumb_Image"];
                            if (img.MediaItem != null)
                            {
                                imageUrl = MediaManager.GetMediaUrl(img.MediaItem);
                            }
                            var category_data = Sitecore.Context.Database.GetItem(data.Fields["Category"].Value);

                            Promoted.Article = new Models.Article
                            {
                                Title = data["Title"],
                                ID = data.ID.ToString(),
                                ThumbImage = imageUrl,
                                ArticlePageURL = Sitecore.Links.LinkManager.GetItemUrl(data),
                                CategoryPageURL = Sitecore.Links.LinkManager.GetItemUrl(category_data),
                                Category = category_data.Fields["Title"].ToString()
                            };
                        }
                    }
                }

                foreach(var guid in item["Articles"].Split('|'))
                {
                    var data = Sitecore.Context.Database.GetItem(guid);
                    if (data != null)
                    {
                        string imageUrl = string.Empty;
                        ImageField img = data.Fields["Thumb_Image"];
                        if (img.MediaItem != null)
                        {
                            imageUrl = MediaManager.GetMediaUrl(img.MediaItem);
                        }
                        var category_data = Sitecore.Context.Database.GetItem(data.Fields["Category"].Value);
                        articlses.Add(new Models.Article {

                            Title = data["Title"],
                            ID = data.ID.ToString(),
                            ThumbImage = imageUrl,
                            ArticlePageURL = Sitecore.Links.LinkManager.GetItemUrl(data),
                            CategoryPageURL = Sitecore.Links.LinkManager.GetItemUrl(category_data),
                            Category = category_data.Fields["Title"].ToString(),
                            LinkText = data[Template.ArticleContent.Fields.LinkText]
                        });

                    }
                }
                Promoted.Articles = articlses;
            }


            return Promoted;
        }

        // Carousel added
        public ArticleSpotlight GetCarouselArticles(Item item)
        {
            ArticleSpotlight Carousal = new ArticleSpotlight();
            List<Models.Article> articles = new List<Models.Article>();
            if (item != null)
            {
                foreach (var guid in item["Articles"].Split('|'))
                {
                    var data = Sitecore.Context.Database.GetItem(guid);
                    if (data != null)
                    {
                        string imageUrl = string.Empty;
                        ImageField img = data.Fields["Thumb_Image"];
                        if(img.MediaItem != null)
                        {
                            imageUrl = MediaManager.GetMediaUrl(img.MediaItem);
                        }
                        var category_data = Sitecore.Context.Database.GetItem(data.Fields["Category"].Value);

                        articles.Add(new Models.Article
                        {
                            Title = data["Title"],
                            ID = data.ID.ToString(),
                            ThumbImage = imageUrl,
                            Description = data[Template.ArticleContent.Fields.Description],
                            ArticlePageURL = Sitecore.Links.LinkManager.GetItemUrl(data),
                            CategoryPageURL = Sitecore.Links.LinkManager.GetItemUrl(category_data),
                            Category = category_data.Fields["Title"].ToString(),
                            LinkText = data[Template.ArticleContent.Fields.LinkText]

                        });
                    }
                }
                Carousal.Articles = articles;
            }
            
            return Carousal;
        }


        // Newly Added All Articles 

        public ArticleSpotlight GetAllArticle(Item item, string category = "", string tag = "")
        {
            ArticleSpotlight article = new ArticleSpotlight();
            List<Models.Article> articles = new List<Models.Article>();
            var allData = Sitecore.Context.Database.GetItem(Template.ArticleRoot.RootID);
            if (item != null)
            {
                article.Title = "Articles";
                Sitecore.Diagnostics.Log.Info("Mehedi>> Tag wise articles!", this);
                var isFound = false;
                foreach (Sitecore.Data.Items.Item child in allData.Children.Where(x => x.TemplateID.ToString() == Template.ArticleRoot.ArticleTemplateID))
                {
                    Sitecore.Diagnostics.Log.Info("Mehedi>> Tag wise articles" + child.ID.ToString(), this);
                    var data = Sitecore.Context.Database.GetItem(child.ID);
                    if (data != null)
                    {

                        string tags = "";
                        foreach (var tg in data[Template.ArticleContent.Fields.Tags].Split('|'))
                        {

                            var t_data = Sitecore.Context.Database.GetItem(tg);
                            if (t_data != null)
                            {
                                if (!string.IsNullOrEmpty(tag))
                                {
                                    if (t_data.Name.Replace(" ", "-") == tag)
                                    {
                                        isFound = true;
                                    }
                                }
                                if (!string.IsNullOrEmpty(tags))
                                {
                                    tags += ", " + t_data.Name;
                                }
                                else
                                {
                                    tags = t_data.Name;
                                }
                            }
                        }

                        string item_Cat = "";
                        if (data.Fields[Template.ArticleContent.Fields.Category].Value != null)
                        {
                            var category_data = Sitecore.Context.Database.GetItem(data.Fields[Template.ArticleContent.Fields.Category].Value);
                            if (category_data != null)
                            {
                                item_Cat = category_data.Fields["Title"].ToString();
                                if (!string.IsNullOrEmpty(category))
                                {
                                    if (item_Cat == category)
                                    {
                                        isFound = true;
                                    }
                                }
                            }
                        }
                        string imageUrl = string.Empty;
                        ImageField img = data.Fields["Thumb_Image"];
                        // var img = (Sitecore.Data.Fields.ImageField)data.Fields["Thumb_Image"];
                        //if (img != null)
                        //{
                        //    imageUrl = img.ImageUrl();
                        //}
                        if (img.MediaItem != null)
                        {
                            imageUrl = MediaManager.GetMediaUrl(img.MediaItem);
                        }
                        //string spotlightImageUrl = string.Empty;
                        //var spotimg = (Sitecore.Data.Fields.ImageField)data.Fields["SpotlightImage"];
                        //if (spotimg != null)
                        //{
                        //    spotlightImageUrl = spotimg.ImageUrl();
                        //}

                        string date = "";
                        if (data.Fields[Template.ArticleContent.Fields.Date] != null && data.Fields[Template.ArticleContent.Fields.Date].HasValue)
                        {
                            //var dtStart = (Sitecore.Data.Fields.DateField)data.Fields["Date"];
                            //var serverTime = Sitecore.DateUtil.ToServerTime(dtStart.DateTime);
                            //date = Sitecore.DateUtil.FormatShortDateTime(serverTime);
                            Sitecore.Data.Fields.DateField df = item.Fields[Template.ArticleContent.Fields.Date];
                            date = df.DateTime.ToString("MMMM dd, yyyy");
                        }

                        if (!string.IsNullOrEmpty(category) || !string.IsNullOrEmpty(tag))
                        {
                            if (isFound)
                            {
                                articles.Add(new Models.Article
                                {
                                    Title = data["Title"],
                                    Date = date,
                                    ThumbImage = imageUrl,
                                    ID = data.ID.ToString(),
                                    ArticlePageURL = Sitecore.Links.LinkManager.GetItemUrl(data),
                                    LinkText = data[Template.ArticleContent.Fields.LinkText],
                                    Category = item_Cat,
                                    Tags = tags,
                                    Description = data[Template.ArticleContent.Fields.Description]
                                });
                            }
                        }
                        else
                        {
                            articles.Add(new Models.Article
                            {
                                Title = data["Title"],
                                Date = date,
                                ThumbImage = imageUrl,
                                ID = data.ID.ToString(),
                                ArticlePageURL = Sitecore.Links.LinkManager.GetItemUrl(data),
                                LinkText = data[Template.ArticleContent.Fields.LinkText],
                                Category = item_Cat,
                                Tags = tags,
                                Description = data[Template.ArticleContent.Fields.Description]
                            });
                        }
                        isFound = false;
                    }
                }
            }

            article.Articles = articles;
            return article;
        }
        // Carousel end
    }

}