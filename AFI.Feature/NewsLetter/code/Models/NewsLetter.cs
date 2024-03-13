using Sitecore.Web.UI.HtmlControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AFI.Feature.NewsLetter.Models
{
    public class NewsLetter
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
        public DateTime PublishDate { get; set; }
        public string Content
        {
            get
            {
                // if (!string.IsNullOrEmpty(Description) && Description.Length > 180)
                // {
                //     return Description.Substring(0, 180) + "...";
                // }
                // else
                //{

                //     return Description;
                // }

                if (!string.IsNullOrEmpty(Description) && Description.Length > 180)
                {
                    string output = Regex.Replace(Description, @"<.*?>", string.Empty);
                    return output.Substring(0, 180) + "...";
                }
                else
                {
                    string output = Regex.Replace(Description, @"<.*?>", string.Empty);
                    return output;
                }
            }
        }

    }

    public class ReleatedNewsletter
    {
        public string LeftTitle { get; set; }
        public string ReadMoreText { get; set; }
        public List<NewsLetter> newsLetters { get; set; }
    }
}