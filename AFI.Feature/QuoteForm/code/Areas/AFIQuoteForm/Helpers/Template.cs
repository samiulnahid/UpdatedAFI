using Sitecore.Data;
using Sitecore.Shell.Applications.ContentEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers
{
    public class Template
    {
        public struct FormSeeting
        {
            public static readonly ID BusinessFormID = new ID("{92EB7A51-5BBC-498D-BC45-40DBD50F9028}");
            public static readonly ID CommonSections_Location = new ID("{F825F1DF-4560-4B0F-82A0-EE58AC767E35}");
            public static readonly ID Common_Fields_Location = new ID("{B367DFF9-D5B2-4A6B-A9C8-C81278BCFCE6}");
            public static readonly ID Rank_Location = new ID("{A1976CA0-45DF-4D95-8757-34D056B8B4C9}");
            public static readonly ID Global_Setting_Location = new ID("{B848F1F8-1014-46A3-A79F-85D4376C7553}");
            public static readonly ID Global_Setting_TemplateID = new ID("{D476BFED-304C-45C0-AA54-D28524B2129B}");

            public static readonly ID Collector_Vehicle_Quote_Location = new ID("{AAB7B1C7-EE26-4E73-8D5D-DC9CB594C3E5}");
            public static readonly ID FloodFormID = new ID("{E5BE5A8B-24D1-4D69-8985-11E26D72BAC6}");
            public static readonly ID Condo_Quote_Location = new ID("{66A7BEC5-3502-4F95-A518-A387D72D0874}");
            public static readonly ID MotorCycleFormID = new ID("{2846E2B7-9E36-44A9-B9D5-C0A9F4CD4D09}");
            public static readonly ID MotorhomeFormID = new ID("{6285F4D8-B261-47DE-957A-434F01A79FDB}");
            public static readonly ID MobilehomeFormID = new ID("{9E6D5FFA-204F-4825-B776-945C18C73C99}");
            public static readonly ID WatercraftFormID = new ID("{39E64096-C3E5-491A-9E2D-33A0D0F32356}");

            public static readonly ID HillAFBFormID = new ID("{BEF7FF17-A160-4EE4-8390-0572FE14B7B4}");
            public static readonly ID Prefixpathlocation = new ID("{1F44539A-ABD7-46F5-80BF-5FAD9F35E3A8}");

            public static readonly ID Quote_Lead_Location = new ID("{6468627C-DACA-4E00-A011-372A310DBEE6}");

        }
       
        public struct Seeting
        {      
            public static readonly ID Insurance_Type_Root = new ID("{B141D878-7D30-4C12-80A2-0D9A08A6D9A1}");

        }
        public struct Prefix
        {
            public static readonly ID TemplateID = new ID("{DBEE7A07-5599-4981-9C4C-77F238D444C4}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{58E6A05A-B619-44F7-9B10-7C822F5D3569}");
                public static readonly ID Value = new ID("{F60C6B10-7112-4ADE-8143-6CF0D704AE73}");
            }
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
        public struct Quote_Flood_Form_Parts
        {
            public static readonly ID TemplateId = new ID("{78AE0811-6367-4646-B2B6-D7CD3C740C01}");
            public struct Fields
            {
                public static readonly ID PropertyBuiltYear = new ID("{0CB1600B-FB8B-49BC-9352-5E93A3FC2774}");
                public static readonly ID PropertyOldestYearText = new ID("{7EF34B9B-5FBB-4F73-8CBF-60F0028ABED3}");
            }
        }
        public struct Unique_Collector_Vehicle_Form_Fields
        {
            public static readonly ID TemplateID = new ID("{2CC494D6-1A58-4604-8F2E-10BBCB376078}");
            public struct Fields
            {
                public static readonly ID Add_Vehicle_Button_Text = new ID("{B77B4402-0FB5-497C-9B4B-AA02147AFE67}");
                public static readonly ID Add_Violation_Button_Text = new ID("{056CA89E-91FF-40D6-B3CE-D9E9CE67B608}");
                public static readonly ID Married = new ID("{4C67FC42-65F8-48F4-8C81-399C0D443C37}");
                public static readonly ID Cohabitant = new ID("{A2D37C4E-5E80-412B-8F83-DCF1E8AAF49C}");
                public static readonly ID Additional_Driver = new ID("{6F397C6E-BAC6-4390-8C38-AB202DF27CCF}");
                public static readonly ID Bodily_Injury_Liability_Sidebar = new ID("{87277640-BE35-4435-9C0B-34ABAF39EC3E}");
                public static readonly ID Personal_Injury_Protection_Sidebar = new ID("{3C25E385-EC02-4B92-A521-C9C6CB9806CF}");
                public static readonly ID Uninsured_Motorist_Bodily_Injury_Sidebar = new ID("{11B86DC9-DCEF-4208-9735-A2C2F51E2070}");
                public static readonly ID Property_Damage_Liability_Sidebar = new ID("{3DB0F127-DCFB-4B8F-A3E2-61F3A94A3A8A}");
                public static readonly ID Medical_Payment_Sidebar = new ID("{DA70E376-5DF6-427C-BE69-46B65B511DA3}");
                public static readonly ID Default_Sidebar = new ID("{A9A04BA4-DC4C-4C31-96CE-7C282E58AC4D}");
                public static readonly ID Bodily_Injury_Liability_Sub_Label = new ID("{E946714D-4C32-4740-B780-E65E3B553675}");
                public static readonly ID Property_Damage_Liability_Sub_Label = new ID("{9D6DDE67-43FE-4AC2-AC5A-6BDDE7F1DCC8}");
                public static readonly ID Medical_Payment_Sub_Label = new ID("{455C26D5-3CFC-4D04-B510-6741FE2E6CD4}");
                public static readonly ID Uninsured_Motorist_Bodily_Injury_Sub_Label = new ID("{270F0047-1ABF-4F19-BE80-B81ABCAE61C8}");
                public static readonly ID Personal_Injury_Protection_Sub_Label = new ID("{E741B5E3-E3D7-41CF-955F-D35F588E9A4C}");
            }
        }
        public struct VehicleCommon
        {
            public static readonly ID Household_Violations_Types = new ID("{C156EEA1-22DB-4E86-A26C-FEF180B5C75D}");
            public static readonly ID Physical_Damage_Deductibles = new ID("{EA84962D-E707-4152-9091-BDFCC3F5D292}");
            public static readonly ID Physical_Damage_Comprehensive_Deductibles = new ID("{4212DF74-EC56-4B7D-8828-277DB75A5FD6}");
            public static readonly ID PipAmounts = new ID("{725BF743-AE28-40D4-8662-3450C5E0280E}");
            public static readonly ID Associated_Customer_Service_Phone = new ID("{33277015-82A9-47B8-97A2-15D986980649}");

        }
        public struct Unique_Motorcycle_Form_Fields
        {
            public static readonly ID TemplateId = new ID("{06A45E30-F0A3-4053-9B5E-337DDF0DDC01}");
            public struct Fields
            {
                public static readonly ID Add_Driver_Button = new ID("{5951F8F3-62B8-44DC-83E5-F4D86CFC8561}");
                public static readonly ID Add_Vehicle_Button = new ID("{12E91F29-FB16-49E1-A9F7-A3D1BB571F4A}");
                public static readonly ID Add_Violation_Button = new ID("{4AB820EF-88D7-45DF-B87D-8E8F5B2ABF6F}");

                public static readonly ID Bodily_Injury_Liability_Sub_Label = new ID("{DC283689-D4D4-40F0-A43D-A9C60004D799}");
                public static readonly ID Property_Damage_Liability_Sub_Label = new ID("{BD263851-3216-4DCC-A32C-974C38A98E7C}");
                public static readonly ID Medical_Payment_Sub_Label = new ID("{D2472D26-81D5-4A43-B8F2-80D09DC32BDF}");
                public static readonly ID Uninsured_Motorist_Bodily_Injury_Sub_Label = new ID("{B41777D1-72C9-48CF-9FC5-B6D27B99EFBC}");
                public static readonly ID Personal_Injury_Protection_Sub_Label = new ID("{09B869D2-BA88-4856-BE36-DF4A7FCECFE9}");

                public static readonly ID Married = new ID("{8C30ABD8-A8AE-4FDB-8DDA-32D530EBB779}");
                public static readonly ID Cohabitant = new ID("{843DF1B3-A149-42BD-8A84-A43CA1141809}");
                public static readonly ID Additional_Driver = new ID("{ED637AD4-4FA8-4DDB-B5EA-6755563987F0}");

                public static readonly ID Bodily_Injury_Liability_Sidebar = new ID("{BF6900CF-1D34-4C4F-BCA5-F201AE7FD83F}");
                public static readonly ID Property_Damage_Liability_Sidebar = new ID("{24557D14-ADA7-49F6-89F4-E83FF90E4324}");
                public static readonly ID Medical_Payment_Sidebar = new ID("{6579CE5B-54F2-43F7-95F3-A97EF1AC22E6}");
                public static readonly ID Uninsured_Motorist_Bodily_Injury_Sidebar = new ID("{CD2D9B1A-5512-420C-A6AD-94106EF449C5}");
                public static readonly ID Personal_Injury_Protection_Sidebar = new ID("{2BF311FC-5047-49C7-AE9B-3FCB73F26AD3}");
                public static readonly ID Default_Sidebar = new ID("{2F683798-69E0-4B10-AB9B-156B2B1C8153}");

            }
        }
        public struct Motorcycle_Types
        {
            public static readonly ID TemplateId = new ID("{9B209D73-C7DC-49D7-8BC3-2BA848A23E92}");

        }
        public struct Unique_Condo_Form_Fields
        {
            public static readonly ID TemplateID = new ID("{EA87F86B-971C-4B95-9055-F054FA31165A}");
            public struct Fields
            {
                public static readonly ID Property_Age_Of_Roof_Before_Text = new ID("{6F9EBCE4-48F1-4791-A29A-F81CA0BABED2}");
                public static readonly ID Property_Age_Of_Roof_End = new ID("{84BEDF44-C8D1-4FE5-8DC8-3747172824A2}");
                public static readonly ID Property_Age_Of_Roof_Start = new ID("{ED1BCF02-BE35-449B-93C1-92244E953A3B}");
                public static readonly ID Property_Year_Built_Before_Text = new ID("{00769750-C1B0-4526-8BA7-2D45CE82ADDD}");
                public static readonly ID Property_Year_Built_Start_Year = new ID("{D0280C89-43BC-45AC-BC4F-314D4D020B8D}");
            }
        }
        public struct Unique_Homeowner_Form_Fields
        {
            public static readonly ID TemplateID = new ID("{008EF7BE-9FA2-4258-B091-FA6698440987}");
            public struct Fields
            {
                public static readonly ID Property_Age_Of_Roof_Before_Text = new ID("{0FEC17B5-2637-4F83-ADDA-63D2C366760E}");
                public static readonly ID Property_Age_Of_Roof_End = new ID("{65083C5E-DA51-4296-9A14-5B905DC00C5A}}");
                public static readonly ID Property_Age_Of_Roof_Start = new ID("{3F2F4012-65FF-477F-AB36-428340448071}");
                public static readonly ID Property_Year_Built_Before_Text = new ID("{11DD1019-0227-46CF-ABE1-813A7D16FDC6}");
                public static readonly ID Property_Year_Built_Start_Year = new ID("{F94EF7D7-6665-47CC-BDDF-8692BCBE5EF0}");
            }
        }
    }
}