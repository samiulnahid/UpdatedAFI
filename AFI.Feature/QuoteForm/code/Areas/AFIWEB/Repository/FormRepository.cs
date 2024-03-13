using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AFI.Feature.Data.Models;
using AFI.Feature.Data.Repositories;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

using Sitecore.Data.Items;
using Templates = AFI.Feature.Identifiers.Templates;
using AFI.Feature.WebQuoteService.Repositories;
using Sitecore.Diagnostics;
using System.Configuration;
using System.Diagnostics;
using AFI.Feature.Data.Providers;
using Dapper;
using System.Data.SqlClient;
using Sitecore.Data.Fields;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers.Configurations;
using AFI.Foundation.Helper.Models;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Repository
{
    public interface IFormRepository
    {
        InsuranceType GetInsuranceTypeForm(Item item);
        Item GetInsuranceTypeRoot();
        IEnumerable<Item> GetAllInsuranceTypes();
        string LandingPageFormGoTo(string zip, string formType);
        void SubmitReferralForm(ReferralModel form);
        List<SelectListItem> GetAllPrefixes();
        HomePageQuoteViewModel GetHomePageQuoteViewModel(Item item);
        HomePageAdvisorQuoteViewModel GetHomePageAdvisorQuoteViewModel(Item item);
        ArticleSpotlight GetRelatedArticle(Item item);
        ArticleSpotlight GetRelatedArticleWidget(Item item);
        ArticleSpotlight GetPromotedArticles(Item item);


        ArticleSpotlight GetArticleGrid(Item item);
        Article GetArticle(Item item);
        ArticleSpotlight GetArticles(Item item, string category = "", string tag = "");
        SurveyForm GetSurveyForm(Item item, string cn);
        int InsertClaimSurvey(SurveyForm surveyForm);
        bool CheckInsertedClaim(string claimNumber);
        IEnumerable<SurveyForm> GetSurveyReport(string startDate = "", string endDate = "");
        Article GetArticleCategory(Item item, string article = "");
        List<LogMail> GetAllEmailLog();
    }

    public class FormRepository : IFormRepository
    {
        private readonly IGlobalSettingsRepository _globalSettingsRepository;
        private readonly IReferralRepository _referralRepository;
        private readonly IPartnerAdvisorRepository _partnerAdvisorRepository;
        private readonly IDbConnectionProvider _dbConnectionProvider;


        public FormRepository(IGlobalSettingsRepository globalSettingsRepository, IReferralRepository referralRepository, IPartnerAdvisorRepository partnerAdvisorRepository, IDbConnectionProvider dbConnectionProvider)
        {
            _globalSettingsRepository = globalSettingsRepository;
            _referralRepository = referralRepository;
            _partnerAdvisorRepository = partnerAdvisorRepository;
            _dbConnectionProvider = dbConnectionProvider;
        }

        public IEnumerable<Item> GetAllInsuranceTypes()
        {
            return GetInsuranceTypeRoot()?.Axes.GetDescendants().Where(x => x.TemplateID == Templates.InsuranceType.TemplateId) ?? new Item[0];
        }

        public Item GetInsuranceTypeRoot()
        {
            return Sitecore.Context.Database.GetItem(
                _globalSettingsRepository.GetSetting(Templates.Global_Settings.FieldNames.Insurance_Type_Root));
        }

        [Obsolete]
        public InsuranceType GetInsuranceTypeForm(Item item)
        {
            if (item != null)
            {
                var insuranceItem = Sitecore.Context.Database.GetItem(item
                    [Templates.Landing_Page_Start_Form.FieldNames.Insurance_Types]);
                if (insuranceItem != null)
                {
                    var animatedIcon = insuranceItem.GetLinkedItems(Templates.InsuranceType.FieldNames.Animated_Icon).FirstOrDefault();

                    var insurancetype = new InsuranceType
                    {
                        Title = insuranceItem[Templates.InsuranceType.FieldNames.Title],
                        URL = ((Sitecore.Data.Fields.LinkField)insuranceItem.Fields[Templates.InsuranceType.FieldNames.QuoteFormLink]).GetFriendlyUrl(),
                        ImageUrl = ((Sitecore.Data.Fields.ImageField)insuranceItem.Fields[
                            Templates.InsuranceType.FieldNames.Image]).ImageUrl(),
                        ImageAltText =
                            ((Sitecore.Data.Fields.ImageField)insuranceItem.Fields[
                                Templates.InsuranceType.FieldNames.Image]).Alt,
                        Type = insuranceItem[Templates.InsuranceType.FieldNames.Insurance_Type],
                        Icon = animatedIcon == null ? string.Empty : animatedIcon[Templates.Pod_Icon.FieldNames.Icon]

                    };
                    return insurancetype;
                }
                else
                {
                    return new InsuranceType();
                }
            }

            return new InsuranceType();
        }

        public string LandingPageFormGoTo(string zip, string formType)
        {
            foreach (var insuranceItem in GetAllInsuranceTypes())
            {
                if (formType.Equals(insuranceItem[Templates.InsuranceType.FieldNames.Insurance_Type]))
                {
                    if (!string.IsNullOrEmpty(insuranceItem[Templates.InsuranceType.FieldNames.QuoteFormLink]))
                    {
                        return ((Sitecore.Data.Fields.LinkField)insuranceItem.Fields[
                            Templates.InsuranceType.FieldNames.QuoteFormLink]).GetFriendlyUrl();
                    }
                }
            }
            return "/";
        }



        public List<SelectListItem> GetAllPrefixes()
        {
            var prefixes = new List<SelectListItem>();         
            Item prefixesList = Sitecore.Context.Database.GetItem(Prefix_List_Location.ID);
            if (prefixesList != null)
            {
                foreach (var prefix in prefixesList.Axes.GetDescendants())
                {
                    prefixes.Add(new SelectListItem()
                    {
                        Text = prefix[Templates.Prefix_Option.FieldNames.Title],
                        Value = prefix[Templates.Prefix_Option.FieldNames.Value]
                    });
                }
            }
            return prefixes;
        }

        public void SubmitReferralForm(ReferralModel form)
        {
            _referralRepository.InsertReferral(form.ToDataModel());
        }

        [Obsolete]
        public HomePageQuoteViewModel GetHomePageQuoteViewModel(Item item)
        {
            HomePageQuoteViewModel model = new HomePageQuoteViewModel
            {
                InsuranceTypes = new List<InsuranceType>(),
                ImageUrl = ((Sitecore.Data.Fields.ImageField)item.Fields[
                    Templates.Request_A_Quote_Form.FieldNames.Image])?.ImageUrl()
            };
            if (item != null)
            {
                foreach (var guid in item[Templates.Request_A_Quote_Form.FieldNames.Insurance_Types].Split('|'))
                {
                    var insuranceItem = Sitecore.Context.Database.GetItem(guid);
                    if (insuranceItem != null)
                    {
                        var animatedIcon = insuranceItem.GetLinkedItems(Templates.InsuranceType.FieldNames.Animated_Icon).FirstOrDefault();
                        InsuranceType insuranceType = (new InsuranceType
                        {
                            Title = insuranceItem[Templates.InsuranceType.FieldNames.Title],
                            Icon = animatedIcon == null ? string.Empty : animatedIcon[Templates.Pod_Icon.FieldNames.Icon],
                            Type = insuranceItem[Templates.InsuranceType.FieldNames.Insurance_Type],
                            ImageUrl = ((Sitecore.Data.Fields.ImageField)insuranceItem.Fields[
                    Templates.InsuranceType.FieldNames.Image])?.ImageUrl(),
                            QuoteFormLink = ((Sitecore.Data.Fields.LinkField)insuranceItem.Fields[
                    Templates.InsuranceType.FieldNames.QuoteFormLink])?.GetFriendlyUrl()

                        });
                        foreach (var tempguid in insuranceItem["RelatedForms"].Split('|'))
                        {
                            var tempinsuranceItem = Sitecore.Context.Database.GetItem(tempguid);
                            if (tempinsuranceItem != null)
                            {
                                var tempanimatedIcon = tempinsuranceItem.GetLinkedItems(Templates.InsuranceType.FieldNames.Animated_Icon).FirstOrDefault();
                                insuranceType.RelatedInsuranceTypes.Add(new InsuranceType
                                {
                                    Title = tempinsuranceItem[Templates.InsuranceType.FieldNames.Title],
                                    Icon = tempanimatedIcon == null ? string.Empty : tempanimatedIcon[Templates.Pod_Icon.FieldNames.Icon],
                                    Type = tempinsuranceItem[Templates.InsuranceType.FieldNames.Insurance_Type],
                                    PreviewTitle = tempinsuranceItem.Fields["__Preview"] != null && !string.IsNullOrEmpty(tempinsuranceItem.Fields["__Preview"].Value) ? tempinsuranceItem.Fields["__Preview"].Value : string.Empty
                                });
                            }
                        }
                        model.InsuranceTypes.Add(insuranceType);
                    }
                }

            }
            return model;
        }

        public HomePageAdvisorQuoteViewModel GetHomePageAdvisorQuoteViewModel(Item item)
        {
            HomePageAdvisorQuoteViewModel model = new HomePageAdvisorQuoteViewModel
            {
                InsuranceTypes = new List<InsuranceType>(),
                AdvisorTypes = new List<Options>(),
                ImageUrl = ((Sitecore.Data.Fields.ImageField)item.Fields[Templates.Request_A_Quote_Form.FieldNames.Image])?.ImageUrl()
            };
            if (item != null)
            {
                foreach (var guid in item[Templates.Request_A_Quote_Form.FieldNames.Insurance_Types].Split('|'))
                {
                    var insuranceItem = Sitecore.Context.Database.GetItem(guid);
                    if (insuranceItem != null)
                    {
                        var animatedIcon = insuranceItem.GetLinkedItems(Templates.InsuranceType.FieldNames.Animated_Icon).FirstOrDefault();
                        model.InsuranceTypes.Add(new InsuranceType
                        {
                            Title = insuranceItem[Templates.InsuranceType.FieldNames.Title],
                            Icon = animatedIcon == null ? string.Empty : animatedIcon[Templates.Pod_Icon.FieldNames.Icon],
                            Type = insuranceItem[Templates.InsuranceType.FieldNames.Insurance_Type]
                        });
                    }
                }

                try
                {
                    var advisorNameList = _partnerAdvisorRepository.GetAll();

                    if (advisorNameList != null && advisorNameList.Any())
                    {
                        advisorNameList = advisorNameList.OrderBy(x => x.LastName);

                        foreach (var advisor in advisorNameList)
                        {
                            model.AdvisorTypes.Add(new Options
                            {
                                value = advisor.Id.ToString(),
                                label = $"{advisor.LastName}, {advisor.FirstName} {advisor.MiddleInitial}"
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"{nameof(FormRepository)}: An error occured while retrieving Partner Advisor Names.", ex, this);
                    //swallow
                }

            }

            return model;
        }

        [Obsolete]
        public ArticleSpotlight GetRelatedArticle(Item item)
        {
            ArticleSpotlight article = new ArticleSpotlight();
            List<Article> articles = new List<Article>();
            if (item != null)
            {
                article.Title = item.Fields["Title"] != null && item.Fields["Title"].HasValue ? item.Fields["Title"].ToString() : "";
                int count = item.Fields["DisplayItemCount"] != null && item.Fields["DisplayItemCount"].HasValue ? Convert.ToInt32(item.Fields["DisplayItemCount"].Value) : 3;
                var rootTag = item.Fields["Tag"] != null && item.Fields["Tag"].HasValue ? item.Fields["Tag"].ToString() : "";
                if (!string.IsNullOrEmpty(rootTag))
                {
                    Sitecore.Diagnostics.Log.Info("Mehedi>> Tag wise articles!", this);
                    var allData = Sitecore.Context.Database.GetItem(Helpers.Configurations.ArticleRoot.RootID);
                    var isFound = false;
                    foreach (Sitecore.Data.Items.Item child in allData.Children.Where(x => x.TemplateID.ToString() == Helpers.Configurations.ArticleRoot.ArticleTemplateID))
                    {
                        Sitecore.Diagnostics.Log.Info("Mehedi>> Tag wise articles" + child.ID.ToString(), this);
                        var data = Sitecore.Context.Database.GetItem(child.ID);
                        if (data != null)
                        {
                            string tags = "";
                            foreach (var tg in data["Tags"].Split('|'))
                            {
                                if (tg == rootTag)
                                {
                                    isFound = true;
                                }

                                var t_data = Sitecore.Context.Database.GetItem(tg);
                                if (t_data != null)
                                {
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

                            string imageUrl = string.Empty;
                            var img = (Sitecore.Data.Fields.ImageField)data.Fields["Image"];
                            if (img != null)
                            {
                                imageUrl = img.ImageUrl();
                            }
                            string spotlightImageUrl = string.Empty;
                            var spotimg = (Sitecore.Data.Fields.ImageField)data.Fields["SpotlightImage"];
                            if (spotimg != null)
                            {
                                spotlightImageUrl = spotimg.ImageUrl();
                            }

                            string date = "";
                            if (item.Fields["Date"] != null && item.Fields["Date"].HasValue)
                            {
                                var dtStart = (Sitecore.Data.Fields.DateField)item.Fields["Date"];
                                var serverTime = Sitecore.DateUtil.ToServerTime(dtStart.DateTime);
                                date = Sitecore.DateUtil.FormatShortDateTime(serverTime);
                            }

                            if (isFound)
                            {
                                articles.Add(new Article
                                {
                                    Title = data["Title"],
                                    Date = date,
                                    ImageUrl = imageUrl,
                                    ID = data.ID.ToString(),
                                    LinkText = data["LinkText"],
                                    LinkUrl = Sitecore.Links.LinkManager.GetItemUrl(data),
                                    ShortDescription = data["ShortDescription"],
                                    SpotlightImageUrl = spotlightImageUrl,
                                    Tags = tags,
                                    TagId = data["Tags"]
                                });
                            }
                            isFound = false;
                        }
                    }
                    Sitecore.Diagnostics.Log.Info("Mehedi>> Tag wise last", this);
                    var rnd = new Random();
                    articles = articles.OrderBy(x => rnd.Next()).Take(count).ToList();
                }
                else
                {
                    foreach (var guid in item["Articles"].Split('|'))
                    {
                        Sitecore.Diagnostics.Log.Info("Mehedi>> select wise last", this);
                        var data = Sitecore.Context.Database.GetItem(guid);
                        if (data != null)
                        {
                            string tags = "";
                            foreach (var tg in data["Tags"].Split('|'))
                            {
                                var t_data = Sitecore.Context.Database.GetItem(tg);
                                if (t_data != null)
                                {
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

                            string imageUrl = string.Empty;
                            var img = (Sitecore.Data.Fields.ImageField)data.Fields["Image"];
                            if (img != null)
                            {
                                imageUrl = img.ImageUrl();
                            }
                            string spotlightImageUrl = string.Empty;
                            var spotimg = (Sitecore.Data.Fields.ImageField)data.Fields["SpotlightImage"];
                            if (spotimg != null)
                            {
                                spotlightImageUrl = spotimg.ImageUrl();
                            }

                            string date = "";
                            if (item.Fields["Date"] != null && item.Fields["Date"].HasValue)
                            {
                                var dtStart = (Sitecore.Data.Fields.DateField)item.Fields["Date"];
                                var serverTime = Sitecore.DateUtil.ToServerTime(dtStart.DateTime);
                                date = Sitecore.DateUtil.FormatShortDateTime(serverTime);
                            }

                            articles.Add(new Article
                            {
                                Title = data["Title"],
                                Date = date,
                                ImageUrl = imageUrl,
                                ID = data.ID.ToString(),
                                LinkText = data["LinkText"],
                                LinkUrl = Sitecore.Links.LinkManager.GetItemUrl(data),
                                ShortDescription = data["ShortDescription"],
                                SpotlightImageUrl = spotlightImageUrl,
                                Tags = tags,
                                TagId = data["Tags"]
                            });
                            Sitecore.Diagnostics.Log.Info("Mehedi>> Tag wise last" + data.ID.ToString(), this);
                        }

                    }
                    Sitecore.Diagnostics.Log.Info("Mehedi>> before count", this);
                    articles = articles.Take(count).ToList();
                    Sitecore.Diagnostics.Log.Info("Mehedi>> after count", this);
                }

                article.Articles = articles;
            }
            return article;
        }

        [Obsolete]
        public ArticleSpotlight GetRelatedArticleWidget(Item item)
        {
            ArticleSpotlight article = new ArticleSpotlight();
            List<Article> articles = new List<Article>();
            if (item != null)
            {
                article.Title = item.Fields["Title"] != null && item.Fields["Title"].HasValue ? item.Fields["Title"].ToString() : "";
                int count = item.Fields["DisplayItemCount"] != null && item.Fields["DisplayItemCount"].HasValue ? Convert.ToInt32(item.Fields["DisplayItemCount"].Value) : 3;
                var rootCategory = item.Fields["Category"] != null && item.Fields["Category"].HasValue ? item.Fields["Category"].ToString() : "";
                if (!string.IsNullOrEmpty(rootCategory))
                {
                    Sitecore.Diagnostics.Log.Info("Mehedi>> Category wise articles!", this);
                    var allData = Sitecore.Context.Database.GetItem(Helpers.Configurations.ArticleRoot.RootID);
                    var isFound = false;
                    foreach (Sitecore.Data.Items.Item child in allData.Children.Where(x => x.TemplateID.ToString() == Helpers.Configurations.ArticleRoot.ArticleTemplateID))
                    {
                        Sitecore.Diagnostics.Log.Info("Mehedi>> Category wise articles" + child.ID.ToString(), this);
                        var data = Sitecore.Context.Database.GetItem(child.ID);
                        if (data != null)
                        {
                            if (data.Fields["Category"].Value == rootCategory)
                            {
                                isFound = true;
                            }

                            if (isFound)
                            {
                                articles.Add(new Article
                                {
                                    Title = data["Title"],
                                    ID = data.ID.ToString(),
                                    LinkText = data["LinkText"],
                                    LinkUrl = Sitecore.Links.LinkManager.GetItemUrl(data)
                                });
                            }
                            isFound = false;
                        }
                    }
                    Sitecore.Diagnostics.Log.Info("Mehedi>> Category wise last", this);
                    var rnd = new Random();
                    articles = articles.OrderBy(x => rnd.Next()).Take(count).ToList();
                }
                else
                {
                    foreach (var guid in item["Articles"].Split('|'))
                    {
                        Sitecore.Diagnostics.Log.Info("Mehedi>> select wise last", this);
                        var data = Sitecore.Context.Database.GetItem(guid);
                        if (data != null)
                        {
                            string tags = "";
                            foreach (var tg in data["Tags"].Split('|'))
                            {
                                var t_data = Sitecore.Context.Database.GetItem(tg);
                                if (t_data != null)
                                {
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

                            string imageUrl = string.Empty;
                            var img = (Sitecore.Data.Fields.ImageField)data.Fields["Image"];
                            if (img != null)
                            {
                                imageUrl = img.ImageUrl();
                            }
                            string spotlightImageUrl = string.Empty;
                            var spotimg = (Sitecore.Data.Fields.ImageField)data.Fields["SpotlightImage"];
                            if (spotimg != null)
                            {
                                spotlightImageUrl = spotimg.ImageUrl();
                            }

                            string date = "";
                            if (item.Fields["Date"] != null && item.Fields["Date"].HasValue)
                            {
                                var dtStart = (Sitecore.Data.Fields.DateField)item.Fields["Date"];
                                var serverTime = Sitecore.DateUtil.ToServerTime(dtStart.DateTime);
                                date = Sitecore.DateUtil.FormatShortDateTime(serverTime);
                            }

                            articles.Add(new Article
                            {
                                Title = data["Title"],
                                Date = date,
                                ImageUrl = imageUrl,
                                ID = data.ID.ToString(),
                                LinkText = data["LinkText"],
                                LinkUrl = Sitecore.Links.LinkManager.GetItemUrl(data),
                                ShortDescription = data["ShortDescription"],
                                SpotlightImageUrl = spotlightImageUrl,
                                Tags = tags,
                                TagId = data["Tags"]
                            });
                            Sitecore.Diagnostics.Log.Info("Mehedi>> Tag wise last" + data.ID.ToString(), this);
                        }

                    }
                    Sitecore.Diagnostics.Log.Info("Mehedi>> before count", this);
                    articles = articles.Take(count).ToList();
                    Sitecore.Diagnostics.Log.Info("Mehedi>> after count", this);
                }

                article.Articles = articles;
            }
            return article;
        }

        [Obsolete]
        public ArticleSpotlight GetPromotedArticles(Item item)
        {
            ArticleSpotlight Promoted = new ArticleSpotlight();
            List<Article> articles = new List<Article>();
            if (item != null)
            {

                if (item.Fields["BannerArticle"] != null)
                {
                    Sitecore.Data.Fields.ReferenceField referenceField = item.Fields["BannerArticle"];
                    if(referenceField!=null && referenceField.TargetItem != null)
                    {
                        Item data = referenceField.TargetItem;
                        //var largeField = item.Fields["BannerArticle"];
                        //var fieldSource = largeField.Source ?? string.Empty;
                        //var selectedItemPath = fieldSource.TrimEnd('/') + "/" + largeField.Value;
                        //Item data = Sitecore.Context.Database.GetItem(selectedItemPath);
                        Article article = new Article();
                        if (data != null)
                        {
                            string imageUrl = string.Empty;
                            var img = (Sitecore.Data.Fields.ImageField)data.Fields["Image"];
                            if (img != null)
                            {
                                imageUrl = img.ImageUrl();
                            }

                            string spotlightImageUrl = string.Empty;
                            var spotimg = (Sitecore.Data.Fields.ImageField)data.Fields["SpotlightImage"];
                            if (spotimg != null)
                            {
                                spotlightImageUrl = spotimg.ImageUrl();
                            }

                            Promoted.Article = new Article
                            {
                                Title = data["Title"],
                                ID = data.ID.ToString(),
                                LinkText = data["LinkText"],
                                LinkUrl = Sitecore.Links.LinkManager.GetItemUrl(data),
                                Category = Sitecore.Context.Item.Name,
                                ImageUrl = imageUrl,
                                SpotlightImageUrl = spotlightImageUrl
                            };
                        }
                    }
                   

                 
                }



                foreach (var guid in item["Articles"].Split('|'))
                {
                    Sitecore.Diagnostics.Log.Info("Mehedi>> select wise last", this);
                    var data = Sitecore.Context.Database.GetItem(guid);
                    if (data != null)
                    {
                        string tags = "";
                        foreach (var tg in data["Tags"].Split('|'))
                        {
                            var t_data = Sitecore.Context.Database.GetItem(tg);
                            if (t_data != null)
                            {
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

                        string imageUrl = string.Empty;
                        var img = (Sitecore.Data.Fields.ImageField)data.Fields["Image"];
                        if (img != null)
                        {
                            imageUrl = img.ImageUrl();
                        }
                        string spotlightImageUrl = string.Empty;
                        var spotimg = (Sitecore.Data.Fields.ImageField)data.Fields["SpotlightImage"];
                        if (spotimg != null)
                        {
                            spotlightImageUrl = spotimg.ImageUrl();
                        }

                        string date = "";
                        if (item.Fields["Date"] != null && item.Fields["Date"].HasValue)
                        {
                            var dtStart = (Sitecore.Data.Fields.DateField)item.Fields["Date"];
                            var serverTime = Sitecore.DateUtil.ToServerTime(dtStart.DateTime);
                            date = Sitecore.DateUtil.FormatShortDateTime(serverTime);
                        }

                        articles.Add(new Article
                        {
                            Title = data["Title"],
                            Date = date,
                            ImageUrl = imageUrl,
                            ID = data.ID.ToString(),
                            LinkText = data["LinkText"],
                            LinkUrl = Sitecore.Links.LinkManager.GetItemUrl(data),
                            ShortDescription = data["ShortDescription"],
                            SpotlightImageUrl = spotlightImageUrl,
                            Tags = tags,
                            TagId = data["Tags"],
                            Category = Sitecore.Context.Item.Name
                        });
                        Sitecore.Diagnostics.Log.Info("Mehedi>> Tag wise last" + data.ID.ToString(), this);
                    }

                }

                Promoted.Articles = articles;
            }
            return Promoted;
        }

        [Obsolete]
        public ArticleSpotlight GetArticleGrid(Item item)
        {
            string category = Sitecore.Context.Item.Name;
            ArticleSpotlight article = new ArticleSpotlight();
            List<Article> articles = new List<Article>();
            var allData = Sitecore.Context.Database.GetItem(Helpers.Configurations.ArticleRoot.RootID);
            if (item != null)
            {
                Sitecore.Diagnostics.Log.Info("Mehedi>> Tag wise articles!", this);
                var isFound = false;
                foreach (Sitecore.Data.Items.Item child in allData.Children.Where(x=>x.TemplateID.ToString()== Helpers.Configurations.ArticleRoot.ArticleTemplateID))
                {
                    Sitecore.Diagnostics.Log.Info("Mehedi>> Tag wise articles" + child.ID.ToString(), this);
                    var data = Sitecore.Context.Database.GetItem(child.ID);
                    if (data != null)
                    {


                        string item_Cat = "";
                        if (data.Fields["Category"].Value != null)
                        {
                            var category_data = Sitecore.Context.Database.GetItem(data.Fields["Category"].Value);
                            if (category_data != null)
                            {
                                item_Cat = category_data.Fields["Name"].ToString();
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
                        var img = (Sitecore.Data.Fields.ImageField)data.Fields["Image"];
                        if (img != null)
                        {
                            imageUrl = img.ImageUrl();
                        }
                        string spotlightImageUrl = string.Empty;
                        var spotimg = (Sitecore.Data.Fields.ImageField)data.Fields["SpotlightImage"];
                        if (spotimg != null)
                        {
                            spotlightImageUrl = spotimg.ImageUrl();
                        }

                        string date = "";
                        if (data.Fields["Date"] != null && data.Fields["Date"].HasValue)
                        {
                            var dtStart = (Sitecore.Data.Fields.DateField)data.Fields["Date"];
                            var serverTime = Sitecore.DateUtil.ToServerTime(dtStart.DateTime);
                            date = Sitecore.DateUtil.FormatShortDateTime(serverTime);
                        }


                        if (isFound)
                        {
                            articles.Add(new Article
                            {
                                Title = data["Title"],
                                Date = date,
                                ImageUrl = imageUrl,
                                ID = data.ID.ToString(),
                                LinkText = data["LinkText"],
                                LinkUrl = Sitecore.Links.LinkManager.GetItemUrl(data),
                                ShortDescription = data["ShortDescription"],
                                SpotlightImageUrl = spotlightImageUrl,
                                TagId = data["Tags"],
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

        [Obsolete]
        public Article GetArticle(Item item)
        {
            var tagpath = ConfigurationManager.AppSettings["ArticleTagPath"];
            Article article = new Article();
            if (item != null)
            {
                string imageUrl = string.Empty;
                var img = (Sitecore.Data.Fields.ImageField)item.Fields["Image"];
                if (img != null)
                {
                    imageUrl = img.ImageUrl();
                }
                string spotlightImageUrl = string.Empty;
                var spotimg = (Sitecore.Data.Fields.ImageField)item.Fields["SpotlightImage"];
                if (spotimg != null)
                {
                    spotlightImageUrl = spotimg.ImageUrl();
                }
                string category = ""; string categoryURL = "";
                if (item.Fields["Category"] != null)
                {
                    var data = Sitecore.Context.Database.GetItem(item.Fields["Category"].Value);
                    if (data != null)
                    {
                        category = data.Fields["Name"].ToString();
                        categoryURL = Sitecore.Links.LinkManager.GetItemUrl(data);
                    }
                }
                string tags = "";
                foreach (var tg in item["Tags"].Split('|'))
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
                if (item.Fields["Date"].Value != null && !((Sitecore.Data.Fields.DateField)item.Fields["Date"]).DateTime.Equals(DateTime.MinValue) &&
                !((Sitecore.Data.Fields.DateField)item.Fields["Date"]).DateTime.Equals(DateTime.MaxValue))
                {
                    Sitecore.Data.Fields.DateField df = item.Fields["Date"];
                    date = df.DateTime.ToString("MMMM dd, yyyy");
                }

                article = new Article
                {
                    Title = item["Title"],
                    Date = date,
                    ImageUrl = imageUrl,
                    ID = item.ID.ToString(),
                    LinkText = item["LinkText"],
                    LinkUrl = Sitecore.Links.LinkManager.GetItemUrl(item),
                    SpotlightImageUrl = spotlightImageUrl,
                    Tags = tags,
                    Category = category,
                    CategoryPageURL = categoryURL
                };


            }
            return article;
        }

        [Obsolete]
        public ArticleSpotlight GetArticles(Item item, string category = "", string tag = "")
        {
            ArticleSpotlight article = new ArticleSpotlight();
            List<Article> articles = new List<Article>();
            var allData = Sitecore.Context.Database.GetItem(Helpers.Configurations.ArticleRoot.RootID);
            if (item != null)
            {
                article.Title = "Articles";
                Sitecore.Diagnostics.Log.Info("Mehedi>> Tag wise articles!", this);
                var isFound = false;
                foreach (Sitecore.Data.Items.Item child in allData.Children.Where(x => x.TemplateID.ToString() == Helpers.Configurations.ArticleRoot.ArticleTemplateID))
                {
                    Sitecore.Diagnostics.Log.Info("Mehedi>> Tag wise articles" + child.ID.ToString(), this);
                    var data = Sitecore.Context.Database.GetItem(child.ID);
                    if (data != null)
                    {

                        string tags = "";
                        foreach (var tg in data["Tags"].Split('|'))
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
                        if (data.Fields["Category"].Value != null)
                        {
                            var category_data = Sitecore.Context.Database.GetItem(data.Fields["Category"].Value);
                            if (category_data != null)
                            {
                                item_Cat = category_data.Fields["Name"].ToString();
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
                        var img = (Sitecore.Data.Fields.ImageField)data.Fields["Image"];
                        if (img != null)
                        {
                            imageUrl = img.ImageUrl();
                        }
                        string spotlightImageUrl = string.Empty;
                        var spotimg = (Sitecore.Data.Fields.ImageField)data.Fields["SpotlightImage"];
                        if (spotimg != null)
                        {
                            spotlightImageUrl = spotimg.ImageUrl();
                        }

                        string date = "";
                        if (data.Fields["Date"] != null && data.Fields["Date"].HasValue)
                        {
                            var dtStart = (Sitecore.Data.Fields.DateField)data.Fields["Date"];
                            var serverTime = Sitecore.DateUtil.ToServerTime(dtStart.DateTime);
                            date = Sitecore.DateUtil.FormatShortDateTime(serverTime);
                        }

                        if (!string.IsNullOrEmpty(category) || !string.IsNullOrEmpty(tag))
                        {
                            if (isFound)
                            {
                                articles.Add(new Article
                                {
                                    Title = data["Title"],
                                    Date = date,
                                    ImageUrl = imageUrl,
                                    ID = data.ID.ToString(),
                                    LinkText = data["LinkText"],
                                    LinkUrl = Sitecore.Links.LinkManager.GetItemUrl(data),
                                    ShortDescription = data["ShortDescription"],
                                    SpotlightImageUrl = spotlightImageUrl,
                                    Tags = tags,
                                    TagId = data["Tags"],
                                    Category = item_Cat
                                });
                            }
                        }
                        else
                        {
                            articles.Add(new Article
                            {
                                Title = data["Title"],
                                Date = date,
                                ImageUrl = imageUrl,
                                ID = data.ID.ToString(),
                                LinkText = data["LinkText"],
                                LinkUrl = Sitecore.Links.LinkManager.GetItemUrl(data),
                                ShortDescription = data["ShortDescription"],
                                SpotlightImageUrl = spotlightImageUrl,
                                Tags = tags,
                                TagId = data["Tags"],
                                Category = item_Cat
                            });
                        }
                        isFound = false;
                    }
                }
            }

            article.Articles = articles;
            return article;
        }

        [Obsolete]
        public Article GetArticleCategory(Item item, string blog = "")
        {
            var path = Sitecore.Links.UrlOptions.DefaultOptions.Site.StartPath + "/" + blog;
            string article_Title = "";
            string article_Link = "";
            Article article = new Article();
            if(!string.IsNullOrEmpty(blog))
            {
                Item data = Sitecore.Context.Database.GetItem(path);
                if(data!=null)
                {
                    article_Title = data.Fields["Title"].Value;
                    article_Link = Sitecore.Links.LinkManager.GetItemUrl(data);
                }
            }
            if (item != null)
            {
                article = new Article
                {
                    Category = item["Name"],
                    ID = item.ID.ToString(),
                    LinkUrl = Sitecore.Links.LinkManager.GetItemUrl(item),
                    Title=article_Title,
                    ArticlePageURL=article_Link

                };


            }
            return article;
        }

        public SurveyForm GetSurveyForm(Item item, string cn)
        {

            SurveyForm survey = new SurveyForm();
            if (item != null)
            {

                if (!string.IsNullOrEmpty(item.Fields["Q1"].Value))
                {
                    survey.Q_1 = item.Fields["Q1"].Value;
                }
                if (!string.IsNullOrEmpty(item.Fields["Q2"].Value))
                {
                    survey.Q_2 = item.Fields["Q2"].Value;
                }
                if (!string.IsNullOrEmpty(item.Fields["Q3"].Value))
                {
                    survey.Q_3 = item.Fields["Q3"].Value;
                }
                if (!string.IsNullOrEmpty(item.Fields["Q4"].Value))
                {
                    survey.Q_4 = item.Fields["Q4"].Value;
                }
                if (!string.IsNullOrEmpty(item.Fields["CommentsLabel"].Value))
                {
                    survey.CommentsLabel = item.Fields["CommentsLabel"].Value;
                }
                if (!string.IsNullOrEmpty(item.Fields["MaxSelectionScale"].Value))
                {
                    survey.MaxSelectionScale = Convert.ToInt32(item.Fields["MaxSelectionScale"].Value);
                }

                if (!string.IsNullOrEmpty(cn))
                {
                    survey.ClaimNumber = cn;
                }

                List<Satisfaction> satisfactions = new List<Satisfaction>();
                foreach (var tg in item["Satisfactions"].Split('|'))
                {
                    Satisfaction satisfaction = new Satisfaction();
                    var t_data = Sitecore.Context.Database.GetItem(tg);
                    if (t_data != null)
                    {
                        if (!string.IsNullOrEmpty(t_data.Fields["LevelName"].Value))
                        {
                            satisfaction.Name = t_data.Fields["LevelName"].Value;
                        }
                        if (!string.IsNullOrEmpty(t_data.Fields["SelectionScaleFrom"].Value))
                        {
                            satisfaction.ScaleForm = Convert.ToInt32(t_data.Fields["SelectionScaleFrom"].Value);
                        }
                        if (!string.IsNullOrEmpty(t_data.Fields["SelectionScaleTo"].Value))
                        {
                            satisfaction.ScaleTo = Convert.ToInt32(t_data.Fields["SelectionScaleTo"].Value);
                        }
                        satisfactions.Add(satisfaction);
                    }
                }
                survey.Satisfactions = satisfactions;

            }

            survey.IsSubmitted = CheckInsertedClaim(survey.ClaimNumber);
            return survey;
        }

        public int InsertClaimSurvey(SurveyForm surveyForm)
        {
            surveyForm.DateResponseReceived = DateTime.Now.ToString();
            surveyForm.DateSurveySent = DateTime.Now.ToString();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = "INSERT INTO [Survey].[ClaimsResponse] ([ClaimNumber],[Howwellinformedwereyoukeptduringyourclaimprocess], [Howtimelywasyourclaimprocess], [Howwelldidyouunderstandtheoutcomeofyourclaimprocess], [HowlikelyareyoutorecommendAFItoafriendorcolleague], [Comments], [Datesurveysent], [Dateresponsereceived]) values(@ClaimNumber, @Q_1, @Q_2, @Q_3, @Q_4, @Comments, @DateSurveySent, @DateResponseReceived); select scope_identity();";
                        var id = db.QueryFirstOrDefault<int>(sql, surveyForm, transaction);
                        transaction.Commit();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: create auto " + sw.Elapsed, "stopwatch");
                        return id;
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"Error while attempting to insert Quote.", ex, this);
                        transaction.Rollback();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: create auto quote contact" + sw.Elapsed, "stopwatch");
                        return 0;
                    }
                }
            }
        }

        public bool CheckInsertedClaim(string claimNumber)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = "Select count(*) Total from [Survey].[ClaimsResponse] Where ClaimNumber='" + claimNumber + "'";
                        var data = db.QueryFirstOrDefault<int>(sql, claimNumber, transaction);

                        return data > 0 ? true : false;
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"Error while attempting to insert Quote.", ex, this);
                        return false;
                    }
                }
            }
        }

        public IEnumerable<SurveyForm> GetSurveyReport(string startDate = "", string endDate = "")
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var pram = "";
                if (!string.IsNullOrEmpty(startDate))
                {
                    if (string.IsNullOrEmpty(endDate))
                        endDate = DateTime.Now.Date.ToString();
                    pram = "WHERE CONVERT(date, DateResponseReceived)  >= '" + startDate + "' AND CONVERT(date, DateResponseReceived)  <= '" + endDate + "'";
                }
                var sql = " SELECT [Id] ,[ClaimNumber] AS ClaimNumber ,[Howwellinformedwereyoukeptduringyourclaimprocess] AS Q_1 ,[Howtimelywasyourclaimprocess] AS Q_2 ,[Howwelldidyouunderstandtheoutcomeofyourclaimprocess] AS Q_3 ,[HowlikelyareyoutorecommendAFItoafriendorcolleague] AS Q_4 ,[Comments] AS Comments ,[Datesurveysent] AS DateSurveySent ,CONVERT(date, DateResponseReceived) AS DateResponseReceived FROM [Survey].[ClaimsResponse] " + pram + " Order by DateResponseReceived Desc ";
                return db.Query<SurveyForm>(sql).ToList();
            }

        }

        public List<LogMail> GetAllEmailLog()
        {
            int daysToKeep = ConfigurationManager.AppSettings["ClaimLogDays"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["ClaimLogDays"]) : 7;

            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "SELECT * FROM AFI_Email_Log WHERE CreatedDate >= DATEADD(day, -@daysToKeep, GETDATE()) ORDER BY CreatedDate DESC;";
                return db.Query<LogMail>(sql, new { daysToKeep }).ToList();
            }
        }

    }
}