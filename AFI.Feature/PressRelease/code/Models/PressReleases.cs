using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.PressRelease.Models
{
    public class PressReleases
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime PublishDate { get; set; }
        public string Content
        {
            get
            {
                if (!string.IsNullOrEmpty(Description) && Description.Length > 180)
                {
                    return Description.Substring(0, 180) + "...";
                }
                else
                {

                    return Description;
                }
            }
        }
    }
    public class Yearlist
    {
        public string yearname { get; set; }
        public string yearvalue { get; set; }
        public string selected { get; set; }
    }
}