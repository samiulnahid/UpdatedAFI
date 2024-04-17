using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Models;
using Identifiers =AFI.Feature.Identifiers;
using  AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIWEB.ModelBuilders;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Services;
using AFI.Foundation.QuoteForm.Models;
using Sitecore.Links;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using NameValueCollectionExtensions = Sitecore.Mvc.Extensions.NameValueCollectionExtensions;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers;
using Newtonsoft.Json;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers.Configurations;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using System.Net;
using Sitecore.Data;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Controllers
{
    public class FormsController : Controller
    {
        private readonly IEmailService _EmailService;
        private readonly IFormRepository _FormRepository;
        private readonly IGlobalSettingsRepository _GlobalSettingsRepository;
        private readonly IReCaptchaService _reCaptchaService;
        private const string PhoneValidationRegex = @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}";
        private readonly string AnalyticsQueryStringKey = "analytics:querystring";
        private readonly string AnalyticsRefererKey = "analytics:referrer";
        private readonly IContactUsFormViewModelBuilder _contactUsFormVmb;


        public FormsController(IFormRepository formRepository, IEmailService emailService,
            IGlobalSettingsRepository globalSettingsRepository, IReCaptchaService reCaptchaService,
            IContactUsFormViewModelBuilder contactUsFormVmb)
        {
            _FormRepository = formRepository;
            _EmailService = emailService;
            _GlobalSettingsRepository = globalSettingsRepository;
            _reCaptchaService = reCaptchaService;
            _contactUsFormVmb = contactUsFormVmb;
        }

        public ActionResult HomepageQuoteForm()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            Session["dataSourceId"] = dataSourceId;
            var currentItem = Sitecore.Context.Database.GetItem(dataSourceId);
            return View(_FormRepository.GetHomePageQuoteViewModel(currentItem));
        }
        public JsonResult GetInsuranceItemById(int id)
        {
            var dataSourceId = Session["dataSourceId"] != null ? Session["dataSourceId"] as string : "{A8D54682-BC27-4398-9872-754CD0099CE3}";
            var currentItem = Sitecore.Context.Database.GetItem(dataSourceId);
            var homePageQuoteViewModel = _FormRepository.GetHomePageQuoteViewModel(currentItem);

            var insuranceTypes = homePageQuoteViewModel.InsuranceTypes;
            homePageQuoteViewModel.InsuranceTypes = new List<InsuranceType>();
            homePageQuoteViewModel.InsuranceTypes.Add(insuranceTypes[id]);
            return Json(homePageQuoteViewModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInsuranceItemByType(string type)
        {
            //var dataSourceId = "{60084932-A3BB-4344-B163-B6BE98FC0359}";
            var dataSourceId = "{A8D54682-BC27-4398-9872-754CD0099CE3}";

            var currentItem = Sitecore.Context.Database.GetItem(dataSourceId);
            var homePageQuoteViewModel = _FormRepository.GetHomePageQuoteViewModel(currentItem);

            var insuranceTypes = homePageQuoteViewModel.InsuranceTypes;
            homePageQuoteViewModel.InsuranceTypes = new List<InsuranceType>();
            InsuranceType iType = insuranceTypes.Find(x => x.Type == type);
            homePageQuoteViewModel.InsuranceTypes.Add(iType);
            return Json(homePageQuoteViewModel, JsonRequestBehavior.AllowGet);
        }
        private string ToTitleCase(string input)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var textInfo = culture.TextInfo;
            return textInfo.ToTitleCase(input);
        }

        public ActionResult HomepageQuoteFormSubmit(FormCollection form)
        {
            //return Redirect(_FormRepository.LandingPageFormGoTo(form["zip"], form["type"])+"?ZipCode=" + form["zip"] + "&CoverageType=" + ToTitleCase(form["type"]));
            return Redirect(_FormRepository.LandingPageFormGoTo("", form["type"]) + "?CoverageType=" + ToTitleCase(form["type"]));
        }

        public ActionResult RequestAQuoteGeneralForm()
        {
            //var dataSourceId = RenderingContext.Current.Rendering.DataSource;
            //var currentItem = Sitecore.Context.Database.GetItem(dataSourceId);
            //return View(_FormRepository.GetHomePageQuoteViewModel(currentItem));
            var dataSourceId = RenderingContext.Current.Rendering.Item;
            return View(_FormRepository.GetHomePageQuoteViewModel(dataSourceId));
        }

        [HttpPost]
        public ActionResult RequestAQuoteGeneralFormSubmit(FormCollection form)
        {
            string marketingCode = Sitecore.Context.Item[Templates.Landing_Page.FieldNames.Marketing_Code];
            //string queryString = $"?ZipCode={form["quote-zip"]}&CoverageType={form["quoteSelector"]}";
            string queryString = $"?CoverageType={form["quoteSelector"]}";
            //if (form["quoteSelector"]=="Condo")
            //{
            //    queryString = $"?ZipCode={form["quote-zip"]}&CoverageType=Homeowners";
            //}

            queryString = ProcessAnalytics(queryString, marketingCode);
            return Redirect(_FormRepository.LandingPageFormGoTo("", form["quoteSelector"]) + queryString);
        }

        public ActionResult FirstCommandRequestAQuoteGeneralForm()
        {
            var dataSourceId = RenderingContext.Current.Rendering.Item;
            return View(_FormRepository.GetHomePageAdvisorQuoteViewModel(dataSourceId));
        }

        [HttpPost]
        public ActionResult FirstCommandRequestAQuoteGeneralFormSubmit(FormCollection form)
        {
            string marketingCode = Sitecore.Context.Item[Templates.Landing_Page.FieldNames.Marketing_Code];
            string queryString = $"?ZipCode={form["quote-zip"]}&CoverageType={form["quoteSelector"]}&firstCommandAdvisorName={form["advisorSelector"]}";
            queryString = ProcessAnalytics(queryString, marketingCode);
            return Redirect(_FormRepository.LandingPageFormGoTo(form["quote-zip"], form["quoteSelector"]) + queryString);
        }

        public ActionResult LandingPageStartForm()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var currentItem = Sitecore.Context.Database.GetItem(dataSourceId);
            return View(_FormRepository.GetInsuranceTypeForm(currentItem));
        }

        [HttpPost]
        public ActionResult LandingPageFormSubmit(FormCollection form)
        {
            string marketingCode = Sitecore.Context.Item[Templates.Landing_Page.FieldNames.Marketing_Code];
            //string queryString = $"?ZipCode={form["quoteZip"]}&CoverageType={form["type"]}";// -Mehedi 12/16/2021
            string queryString = $"?CoverageType={form["type"]}";
            queryString = ProcessAnalytics(queryString, marketingCode);
            //return Redirect(_FormRepository.LandingPageFormGoTo(form["quoteZip"], form["type"]) + queryString);//-Mehedi 12/16/2021
            var aa = _FormRepository.LandingPageFormGoTo("", form["type"]) + queryString;
            return Redirect(_FormRepository.LandingPageFormGoTo("", form["type"]) + queryString);

        }
        public ActionResult LandingPageVideoForm()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var currentItem = Sitecore.Context.Database.GetItem(dataSourceId);
            return View(_FormRepository.GetInsuranceTypeForm(currentItem));
        }
        public ActionResult ContactUsForm()
        {
            var model = _contactUsFormVmb.Build();
            return View(model);
        }

        public ActionResult ReferralForm()
        {
            return View(new ReferralFormView
            {
                Prefixes = _FormRepository.GetAllPrefixes()
            });
        }
        [HttpPost]
        public ActionResult SubmitReferralForm(FormCollection form)
        {
            Regex regex = new Regex(PhoneValidationRegex, RegexOptions.IgnoreCase);
            //if (_reCaptchaService.VerifyResponse(form["token"]).Success)
            //{
                var referralModel = new ReferralModel
                {
                    ReferringFirstName = !string.IsNullOrEmpty(form["Member First Name"]) ? form["Member First Name"] : "",
                    ReferringLastName = !string.IsNullOrEmpty(form["Member Last Name"]) ? form["Member Last Name"] : "",
                    ReferringAFIMemberNumber = !string.IsNullOrEmpty(form["member number"]) ? form["member number"] : "",
                    FirstName = !string.IsNullOrEmpty(form["First Name"]) ? form["First Name"] : "",
                    LastName = !string.IsNullOrEmpty(form["Last Name"]) ? form["Last Name"] : "",
                    NamePrefix = !string.IsNullOrEmpty(form["prefix"]) ? form["prefix"] : "",
                    Rank = !string.IsNullOrEmpty(form["Rank"]) ? form["Rank"] : "",
                    Email = !string.IsNullOrEmpty(form["email"]) ? form["email"] : "",
                    PhoneNumber = !string.IsNullOrEmpty(form["phone"]) ? form["phone"] : ""
                };

                if (referralModel.ReferringAFIMemberNumber.Length > 7 || referralModel.Rank.Length > 50 ||
                    referralModel.ReferringFirstName.Length > 50 || referralModel.ReferringLastName.Length > 50 ||
                    referralModel.FirstName.Length > 50 || referralModel.LastName.Length > 50)
                {
                    if ((!string.IsNullOrEmpty(referralModel.PhoneNumber) && !regex.IsMatch(referralModel.PhoneNumber)) || !IsValidEmail(referralModel.Email))
                    {
                        return Redirect(LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(
                            _GlobalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames
                                .Error_Page))));
                    }
                }

                _FormRepository.SubmitReferralForm(referralModel);

            //string confirmationUrl = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Site_Seeting.Global_Setting_Location));
            //return string.IsNullOrWhiteSpace(confirmationUrl) ? (ActionResult)PartialView(form) : Redirect(confirmationUrl);

            string confirmationUrl = "";
            Item item = Sitecore.Context.Database.GetItem(Site_Seeting.Global_Setting_Location);
            if (item != null)
            {
                confirmationUrl = ((Sitecore.Data.Fields.LinkField)item.Fields[Referral_Form_Thanks_Location.ID]).GetFriendlyUrl();
            }
          

            //LinkField confirmationUrlField = _globalSettingsRepository.GetSettingField(Templates.Global_Settings.FieldNames.Short_Form_Confirmation_Page);
            //string confirmationUrl = confirmationUrlField.GetFriendlyUrl();
            return string.IsNullOrWhiteSpace(confirmationUrl) ? (ActionResult)PartialView(form) : Redirect(confirmationUrl);

            //   return Redirect(LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Referral_Form_Thanks_Location.ID)));
            //}
            //return Redirect(LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(_GlobalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.Error_Page))));
        }

        private string ProcessAnalytics(string queryString, string marketingCode)
        {
            try
            {
                var session = HttpContext.Session;
                if (session?[AnalyticsRefererKey] != null)
                {
                    Request.Headers.Add("Referer", session[AnalyticsRefererKey].ToString());
                }
                if (session?[AnalyticsQueryStringKey] != null)
                {
                    NameValueCollection queryStrings = (NameValueCollection)session[AnalyticsQueryStringKey];

                    if (!string.IsNullOrWhiteSpace(marketingCode))
                    {
                        //INFO: Session values are read-only
                        NameValueCollection marketingCodeCollection = new NameValueCollection { queryStrings };
                        marketingCodeCollection["ResponseType"] = "Internet%20Advertising";
                        marketingCodeCollection["ResponseDescription"] = marketingCode;
                        queryString += string.Concat(marketingCodeCollection.ToKeyValues().Select(c => $"&{c.Key}={c.Value}"));
                    }
                    else
                    {
                        queryString += $"&{queryStrings}";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(marketingCode))
                {
                    queryString += "&ResponseType=Internet%20Advertising";
                    queryString += $"&ResponseDescription={marketingCode}";
                }
            }
            catch (Exception)
            {


            }


            return queryString;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public ActionResult RelatedArticles()
        {
            var dataSourceId = RenderingContext.Current.Rendering.Item;
            var data = _FormRepository.GetRelatedArticle(dataSourceId);
            return View("/Areas/AFIWEB/Views/Components/Pods/RelatedArticles.cshtml", data);
        }
        public ActionResult RelatedArticlesWidget()
        {
            var dataSourceId = RenderingContext.Current.Rendering.Item;
            var data = _FormRepository.GetRelatedArticleWidget(dataSourceId);
            return View("/Areas/AFIWEB/Views/Components/Pods/RelatedArticlesWidget.cshtml", data);
        }
        public ActionResult Article()
        {
            var dataSourceId = RenderingContext.Current.Rendering.Item;
            var data = _FormRepository.GetArticle(dataSourceId);
            return View("/Areas/AFIWEB/Views/Components/Pods/Article.cshtml", data);
        }
        public ActionResult Articles(string category = "", string tag = "")
        {
            var dataSourceId = RenderingContext.Current.Rendering.Item;
            var data = _FormRepository.GetArticles(dataSourceId, category, tag);
            return View("/Areas/AFIWEB/Views/Components/Pods/Articles.cshtml", data);
        }
        public ActionResult ArticleCategory(string article = "")
        {
            var dataSourceId = RenderingContext.Current.Rendering.Item;
            var data = _FormRepository.GetArticleCategory(dataSourceId, article);
            return View("/Areas/AFIWEB/Views/Components/Pods/ArticleCategory.cshtml", data);
        }
        public ActionResult PromotedArticles()
        {
            var dataSourceId = RenderingContext.Current.Rendering.Item;
            var data = _FormRepository.GetPromotedArticles(dataSourceId);
            return View("/Areas/AFIWEB/Views/Components/Pods/PromotedArticles.cshtml", data);
        }
        public ActionResult ArticlesGrid()
        {
            var dataSourceId = RenderingContext.Current.Rendering.Item;
            var data = _FormRepository.GetArticleGrid(dataSourceId);
            return View("/Areas/AFIWEB/Views/Components/Pods/ArticlesGrid.cshtml", data);
        }
        //public ActionResult ImportantSubject()
        //{
        //    var dataSourceId = RenderingContext.Current.Rendering.Item;
        //    var data = _FormRepository.GetArticleImportantSubject(dataSourceId);
        //    return View("/Areas/AFIWEB/Views/Components/Pods/ImportantSubject.cshtml", data);
        //}
        public ActionResult CTAHorizontal()
        {
            var dataSourceId = RenderingContext.Current.Rendering.Item;
            var data = _FormRepository.GetRelatedArticleWidget(dataSourceId);
            return View("/Areas/AFIWEB/Views/Components/Pods/CTAHorizontal.cshtml", data);
        }
        public ActionResult ClaimSurveyForm(string cn = "")
        {
            if (!string.IsNullOrEmpty(Request.QueryString["cn"]))
            {
                var dataSourceId = RenderingContext.Current.Rendering.Item;
                var data = _FormRepository.GetSurveyForm(dataSourceId, cn);
                return View("/Views/AFI/Forms/ClaimSurveyForm.cshtml", data);
            }
            else if(Sitecore.Context.PageMode.IsExperienceEditor)
            {
                var dataSourceId = RenderingContext.Current.Rendering.Item;
                var data = _FormRepository.GetSurveyForm(dataSourceId, cn);
                return View("/Views/AFI/Forms/ClaimSurveyForm.cshtml", data);
            }
            else
            {
                return Redirect("/");
            }
        }
        [HttpPost]
        public ActionResult ClaimSurveyFormSubmit(SurveyForm surveyForm)
        {
            var response = 0;
            var isSubmitted = _FormRepository.CheckInsertedClaim(surveyForm.ClaimNumber);
            if (isSubmitted)
            {
                response = 0;
            }
            else
            {
                response = _FormRepository.InsertClaimSurvey(surveyForm);
            }

            var json = JsonConvert.SerializeObject(response);
            return Json(json, JsonRequestBehavior.AllowGet);

        }
        public ActionResult EmailLog()
        {
            var data = _FormRepository.GetAllEmailLog();
            return View("/Views/AFI/Forms/AfiEmailLog.cshtml", data);
        }


        [EnableCors]
        [HttpPost]
        public JsonResult ProcessJsonArray(string urlList)
        {
            
            List<string> urls = urlList.Split(',').Select(url => url.Trim()).ToList();
            List<string> modifiedUrls = new List<string>();

            foreach (var url in urls)
            {
                bool isValidUrl = false;

                try
                {
                    if (url.Contains("link.aspx"))
                    {
                        string itemId = url.Split(new[] { "id=" }, StringSplitOptions.RemoveEmptyEntries)[1].Split('&')[0];
                        // Ensure that the item ID is in the correct format (GUID)
                        if (Guid.TryParseExact(itemId, "N", out Guid itemIdGuid))
                        {
                            // Attempt to retrieve the Sitecore item
                            Item item = Sitecore.Context.Database.GetItem(new ID(itemIdGuid));

                            // Check if the item is found
                            isValidUrl = (item != null);
                        }
                    }
                    else
                    {
                        isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) &&
                                        (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                        if (isValidUrl)
                        {
                            try
                            {
                                IPHostEntry hostEntry = Dns.GetHostEntry(uriResult.Host);
                                isValidUrl = hostEntry.AddressList.Length > 0;
                            }
                            catch (Exception)
                            {
                                isValidUrl = false; // Host resolution failed, mark URL as invalid
                            }
                        }
                    }
                    modifiedUrls.Add(new { PostURl = url, ValidURL = isValidUrl }.ToString());
                }
                catch (Exception ex)
                {
                    modifiedUrls.Add(new { PostURl = url, Error = ex.Message }.ToString());
                }
            }

            return Json(modifiedUrls);
        }
    }
}