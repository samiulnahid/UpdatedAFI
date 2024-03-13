using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/// <summary>
///Carousal lines added 
///  24
/// </summary>
namespace AFI.Feature.Article.Models
{
    public class Article
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string ThumbImage { get; set; }
        public string Date { get; set; }
        public string Tags { get; set; }
        public string TagId { get; set; }
        public string LinkUrl { get; set; }
        public string Category { get; set; }
        public string CategoryPageURL { get; set; }
        public string ArticlePageURL{get; set;}
        public string LinkText { get; set; }
        public string Description { get; set; }
    }
}