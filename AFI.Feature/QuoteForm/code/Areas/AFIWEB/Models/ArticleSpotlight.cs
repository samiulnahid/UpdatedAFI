using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
     public class ArticleSpotlight
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Count { get; set; }
        public List<Article> Articles { get; set; }
        public Article Article { get; set; }
    }
}