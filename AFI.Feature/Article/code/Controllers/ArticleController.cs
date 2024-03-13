using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using AFI.Feature.Article.Repository;
using AFI.Feature.Article.Models;

/// <summary>
/// carousal lines added
/// 47 - 52
/// </summary>

namespace AFI.Feature.Article.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        public ActionResult Article()
        {
            
             var dataSourceId = RenderingContext.Current.Rendering.Item;
            var data = _articleRepository.GetArticle(dataSourceId);

            return View("/Views/AFI/Common/Article.cshtml", data);
        }
        // GET: Aticle

        public ActionResult ArticlesGrid()
        {
            var dataSourceId = RenderingContext.Current.Rendering.Item;
            var data = _articleRepository.GetArticleGrid(dataSourceId);
            return View("/Views/AFI/Common/ArticleGrid.cshtml", data);
        }
        public ActionResult PromotedArticles() 
        {
            var dataSourceId = RenderingContext.Current.Rendering.Item;
            var data = _articleRepository.GetPromotedArticles(dataSourceId);
            return View("/Views/AFI/Common/PromotedArticles.cshtml", data);
        }
        public ActionResult CarouselArticles()
        {
            var dataSourceId = RenderingContext.Current.Rendering.Item;
            var data = _articleRepository.GetCarouselArticles(dataSourceId);
            return View("/Views/AFI/Common/CarouselArticles.cshtml", data);
        }

        // Newly Added 31.7.23
        public ActionResult Articles(string category = "", string tag = "")
        {
            var dataSourceId = RenderingContext.Current.Rendering.Item;
            var data = _articleRepository.GetAllArticle(dataSourceId, category, tag);
            return View("/Views/AFI/Common/Articles.cshtml", data);
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}