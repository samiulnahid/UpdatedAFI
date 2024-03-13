using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class Article
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Tags { get; set; }
        public string TagId { get; set; }
        public string SpotlightImageUrl { get; set; }
        public string ImageUrl { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public string Category { get; set; }
        public string CategoryPageURL { get; set; }
        public string ArticlePageURL { get; set; }
    }
}