using Sitecore.Data;
using Sitecore.Shell.Applications.ContentEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.Quote.Constants
{
    public class Template
    {
        public struct FormSeeting
        {
            public static readonly ID BusinessFormID = new ID("{92EB7A51-5BBC-498D-BC45-40DBD50F9028}");
            public static readonly ID CommonSections_Location = new ID("{F825F1DF-4560-4B0F-82A0-EE58AC767E35}");
            public static readonly ID Rank_Location = new ID("{A1976CA0-45DF-4D95-8757-34D056B8B4C9}");           
          
        }
        public struct Seeting
        {      
            public static readonly ID Insurance_Type_Root = new ID("{B141D878-7D30-4C12-80A2-0D9A08A6D9A1}");

        }
        public struct InsuranceType
        {
            public static readonly ID TemplateId = new ID("{88776424-4279-464D-B1E9-3FDFF32D0490}");
            public struct Fields
            {
                public static readonly ID InsuranceType = new ID("{8E8C6F8D-049F-4F1F-85AE-67444DB77088}");
                public static readonly ID AnimatedIcon = new ID("{5426C0FC-0334-43B4-9751-7B6518896884}");
                public static readonly ID RelatedForms = new ID("{4E2658A8-B44D-445B-B155-B85FFC2DF464}");
                public static readonly ID Title = new ID("{C55B72FE-9506-44D2-82EF-CF111EB4016E}");
                public static readonly ID QuoteFormLink = new ID("{5C9AFD1D-8DD5-4CC5-8D37-7CCE9B357994}");
                public static readonly ID Image = new ID("{D4CEFCC3-9A1A-498E-8810-7B379E69DF23}");
            }
        }
        public struct QuoteFrom
        {
            public static readonly ID TemplateID = new ID("{C1EF9FC0-3530-425F-A8FE-BE5DF32624E1}");
            public struct Fields
            {
                public static readonly ID AssociatedEnsuranceType = new ID("{F356D432-A471-48AD-B96E-8913B2F18A62}");
                public static readonly ID AssociatedCustomerServicePhone = new ID("{33277015-82A9-47B8-97A2-15D986980649}");
                public static readonly ID WayFinderSteps = new ID("{67CAA43B-B547-4B47-B088-74FC8E6D8977}");
                public static readonly ID ID = new ID("{962A7A1A-FFB6-48E6-81B9-34AC15C06E5D}");
                public static readonly ID Back_Button_Text = new ID("{8B7ACEAB-FDD4-40AB-9B8B-BA3E9BD44C60}");
                public static readonly ID Pre_Save_Button_Text = new ID("{9EB6AEC3-7C21-452F-A159-598A39A89BAB}");
                public static readonly ID Save_Button_Text = new ID("{C9884653-8827-48B5-B9F0-093F39AEB68C}");
                public static readonly ID Next_Button_Text = new ID("{91E9C9E9-9CBA-4839-9FB8-7C9C83F79052}");
                public static readonly ID Return_Button_Text = new ID("{3B58F105-B418-4C85-9B24-52CD307EC606}");
                public static readonly ID Submit_Button_Text = new ID("{49B61F61-0B66-47A4-87CA-0455C2D1B658}");
                public static readonly ID Select_Menu_Default_Text = new ID("{84E566FA-D29B-4773-80D7-996E8E79CEFC}");
                public static readonly ID SaveSuccessRedirect = new ID("{4DBF7863-0903-41C0-BFF4-91C268C0A59A}");
                public static readonly ID SubmitSuccessRedirect = new ID("{757AEC5B-3C6D-4A8C-A496-8638E8D6440F}");
               
            }
        }

        public struct Form_Steps
        {
            public static readonly ID TemplateID = new ID("{1AF9546B-4FF6-4038-BD69-158542FBB354}");
            public struct Fields
            {
                public static readonly ID ID = new ID("{DC0297A2-EE86-4F6C-B066-3E9697E9DB15}");
                public static readonly ID Label = new ID("{34F91D7F-464F-4ED7-B81A-DDBCFCB61B45}");
            }
        }
        //public struct Form_Steps
        //{
        //    public static readonly ID TemplateID = new ID("{1AF9546B-4FF6-4038-BD69-158542FBB354}");
        //    public struct Fields
        //    {
        //        public static readonly ID ID = new ID("{DC0297A2-EE86-4F6C-B066-3E9697E9DB15}");
        //        public static readonly ID Label = new ID("{34F91D7F-464F-4ED7-B81A-DDBCFCB61B45}");
        //    }
        //}
    }
}