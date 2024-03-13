using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// carousel lines added
/// 26
/// </summary>

namespace AFI.Feature.Article.Constants
{
    public class Template
    {
        public struct ArticleContent
        {
            public static readonly ID TemplateId = new ID("{59682D17-3B15-4BB6-88E2-20BEA14C5B0D}");
            public struct Fields
            {
                public static readonly ID Category = new ID("{F29C40C7-FEBE-479E-AE66-0B81A984DDE1}");
                public static readonly ID Tags = new ID("{F5D738BA-D5EE-49D4-B63B-C69590DD1D2E}");
                public static readonly ID Date = new ID("{79468AE3-EADB-403D-9701-C4E881DB4B85}");
                public static readonly ID ThumbImage = new ID("{EDD7A827-1FAC-4CB4-8B92-BDA8F6B0DC58}");
                public static readonly ID LinkText = new ID("{6B50643A-263B-4FEB-B6C2-1FA941014A40}");
                public static readonly ID Description = new ID("{95455D4C-520D-46A6-9FF0-A7CFAB360497}");
            }
        }
        	
        public struct PromotedArticles
        {
            public static readonly ID TemplateId = new ID("{E752CDDB-6052-4BF2-B3CA-2B4E2136F0AE}");
            public struct Fields
            {
                public static readonly ID BannerArticle = new ID("{82791857-E2D0-44D6-802B-A1286A7E47E3}");
                public static readonly ID Articles = new ID("{1094D8A2-BBC3-4A6C-8610-0BF9F6142E0D}");
            }
        }
        public static class ArticleRoot
        {
            public const string RootID = "{E2D17BAF-65B8-4825-9207-3DB1E2DD9F99}";
            public const string ArticleTemplateID = "{82E5A984-7805-4667-9CAA-66A2B69F65E9}";
        }
    }
}