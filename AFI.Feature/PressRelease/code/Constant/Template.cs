using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace AFI.Feature.PressRelease.Constant
{
    public class Template
    {
        public struct PressRelease
        {
            public static readonly ID ContentRootFolder = new ID("{1DC727E2-3EB6-44B7-98A0-CB3CAD7DC0BC}");
            public static readonly ID DetailsTemplateId = new ID("{47836042-5E15-49E8-9832-C4ED6EAA94E0}");
            public static readonly ID PressRealeaseLandingPage = new ID("{0840C929-A5B9-4D13-B00E-0CE3DA1845C3}");
            public static readonly ID PressReleaseMonthFolder = new ID("{B8DC7A95-CC0C-49AB-856E-672928EBB052}");
            public static readonly ID PressReleaseYearFolder = new ID("{EC5C5C83-1CEB-4760-A5D7-BDAF3CC91C5C}");
            public struct Fields
            {
                public static readonly ID Date = new ID("{5C8C4AD2-6D5A-4A20-9195-B92B3D29C125}");
                public static readonly ID Description = new ID("{21CB0A4D-B101-4C30-BAC9-F8792D238676}");
            }
            public struct RenderingParameter
            {
                public static readonly ID ReadMoreID = new ID("{262AA24B-E9DE-4E53-98FE-1DD8A1145095}");
                public static readonly string ReadMoreText = "Read More Text";

                public static readonly ID LeftSideID = new ID("{A2692812-E13E-42F6-9BC5-54CABE4056C5}");
                public static readonly string LeftSideText = "Left Side Text";
            }

        }
    }
}