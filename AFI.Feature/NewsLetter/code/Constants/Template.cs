using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace AFI.Feature.NewsLetter.Constants
{
    public class Template
    {
        public struct NewsLetter
        {
            public static readonly ID ContentRootFolder = new ID("{96A122EA-6FDA-4C72-B812-7966CEFCF8E4}");
            public static readonly ID ArchivedRootFolder = new ID("{FF457E0E-52F5-4FF2-810A-59DF49F944B6}");
            public static readonly ID DetailsTemplateId = new ID("{63585D77-41B7-4A63-BBE9-DF28B487D6F8}");
            public static readonly ID LayoutTemplateId = new ID("{B23B49A7-30E5-4E91-B86E-B093EC97554C}");

            public struct Fields
            {
                public static readonly ID Date = new ID("{90A15A1E-50C9-44AC-A247-8D818A6F19DA}");
                public static readonly ID ShortDescription = new ID("{9567F8E3-D46F-4068-AE05-B8B955190D89}");
            }
            public struct RenderingParameter
            {
                public static readonly ID LeftSideID = new ID("{A2692812-E13E-42F6-9BC5-54CABE4056C5}");
                public static readonly ID ReadMoreID = new ID("{262AA24B-E9DE-4E53-98FE-1DD8A1145095}");
                public static readonly ID CountID = new ID("{A1FB1781-7D0B-4D72-94D0-14BEE4F5876A}");

                public static readonly string LeftSideText = "Left Side Text";
                public static readonly string ReadMoreText = "Read More Text";
                public static readonly string Count = "Count";

            }
        }
        
    }
}