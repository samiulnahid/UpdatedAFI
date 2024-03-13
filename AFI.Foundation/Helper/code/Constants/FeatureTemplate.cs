using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFI.Foundation.Helper
{
    public class FeatureTemplate
    {
        public struct RequestAQuoteFormBanner
        {
            public static readonly ID TemplateId = new ID("{45CE87C9-9548-4A0C-9086-B103F0A814C4}");
            public struct Fields
            {
                public static readonly ID Image = new ID("{2AE9C056-98D8-4C17-AE3C-866982D3546F}");
                public static readonly ID Title = new ID("{689DC04B-1F6B-4399-8760-87E99BA7DE2A}");
                public static readonly ID SubTitle = new ID("{B893D3FC-AAAD-4EAA-A388-E8390D8A8FC0}");
                public static readonly ID Link = new ID("{A9EBA163-AFC9-4ECC-9152-70D84DD0FF79}");              
                public static readonly ID PlaceHolderText = new ID("{372385D0-F1D9-4604-9E24-59C495778EE8}");
                public static readonly ID InsuranceTypeList = new ID("{F7FAD69A-7244-492A-A1DD-B9C40210E665}");
                public static readonly ID ButtonText = new ID("{4F84CB3D-70E4-440E-938A-EB5DB31BC4ED}");
            }
        }
        public struct InsuranceHeroBanner
        {
            public static readonly ID TemplateId = new ID("{255E0E39-56B1-46D6-8AB7-48430AC4843E}");
            public struct Fields
            {
                public static readonly ID BackgroundImage = new ID("{D48C2560-AFD6-40E4-8A4D-2C6DBD3A5D90}");
                public static readonly ID LinkText = new ID("{B4DC9CFC-C4AA-4D4A-9440-50D42415FE46}");
                public static readonly ID ShortDescription = new ID("{BE26F13E-DDFF-46D8-AE43-9D0E97DE6FE1}");
                public static readonly ID Title = new ID("{425977CF-2707-4D1C-A765-A3070CABCDE8}");
                public static readonly ID YoutubeLink = new ID("{4AEBB17E-3221-482B-B466-E91BEC0910E1}");
                public static readonly ID InsuranceTypeList = new ID("{66499C71-D3CA-4B3F-BEA1-8E20FE9BDD70}");
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
                public static readonly ID BannerIcon = new ID("{01B04672-8CDA-480E-ACF1-A35EACBC3BA7}");
            }
        }

       

        public struct Page
        {
            public static readonly ID TemplateId = new ID("{1A7A31FE-6BFD-453D-AD39-D7EF95AD07BB}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{A66CFA5D-9741-4DC3-8B9A-F1D066BEAAFC}");
                public static readonly ID Content = new ID("{22767785-F559-49ED-8347-DA5D6F6A6498}");
            }
        }
        public struct InteriorHero
        {
            public static readonly ID TemplateId = new ID("{7801C3CF-075B-44CE-BE0C-02279907B038}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{07545472-BED6-4DD2-B972-12EC73FB75C1}");
                public static readonly ID BackgroundImage = new ID("{9C95E660-8C37-4FFE-BC49-47B010F5B19D}");
            }
        }

        public struct DepsOrLocPodsGroup
        {
            public static readonly ID TemplateId = new ID("{49B66310-FB28-4FD9-9F9D-2AB3B9919364}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{A2EC9A2C-F707-4D1B-A8EE-C6230931688C}");
            }
        }

        public struct DepsOrLocTitle
        {
            public static readonly ID TemplateId = new ID("{63EB1CB3-65C6-4334-B7DD-4DD09B85E0DE}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{8FB9A0AA-5F59-4677-A761-146D14FD7E1B}");
            }
        }

        public struct TelephoneDetail
        {
            public static readonly ID TemplateId = new ID("{C64B31C6-B8E3-4236-8B25-03063BFED8EA}");
            public struct Fields
            {
                public static readonly ID TelephoneNumber = new ID("{B9A60E81-2CF6-4B75-BA7C-D7C5A5F1223A}");
                public static readonly ID FaxNumber = new ID("{0E091D71-6A1A-443A-B6B2-4050732DF8F7}");
                public static readonly ID FaxText = new ID("{D7631CBC-17B4-47FB-9B41-2FEF0F8AC641}");
            }
        }

        public struct EmailDetail
        {
            public static readonly ID TemplateId = new ID("{17ECE510-5318-4E8D-AB03-586D2C4A0C59}");
            public struct Fields
            {
                public static readonly ID Email = new ID("{376059B4-3002-4CBE-A624-8FA626728380}");
            }
        }

        public struct OpeningTimeDetail
        {
            public static readonly ID TemplateId = new ID("{E247BDB1-601B-45DE-A224-8615D18580FA}");
            public struct Fields
            {
                public static readonly ID OpeningHoursContentSchema = new ID("{91041E5B-5CDF-4311-B2BF-2F5ACCC2711D}");
                public static readonly ID OpenDaysOfTheWeek = new ID("{590D2C0F-A5C8-4587-BC81-4640AEF831B8}");
                public static readonly ID OpeningTime = new ID("{8539A44E-47D2-41CD-8924-A18B1A48905F}");
                public static readonly ID ClosingTime = new ID("{61E68CCA-23ED-4E20-9C68-54E8D14E52DF}");
                public static readonly ID TimeZone = new ID("{AAFD3485-8CAF-4950-AFEC-C67455ED5151}");
            }
        }
       
        public struct ContactUs
        {
            public static readonly ID TemplateId = new ID("{58DF569A-3E4B-4482-8559-D1A3909879DB}");
            public struct Fields
            {
                public static readonly ID Subjects = new ID("{4650F12A-2D12-4B0B-B050-54CB46F7D765}");
                public static readonly ID Title = new ID("{92031B39-DD23-4495-8E0E-FC080FC833B5}");
                public static readonly ID Subtitle = new ID("{85FCB04F-A179-43F7-9435-377457CAB9DC}");
                public static readonly ID NameLabel = new ID("{3354832B-980A-4933-8855-72D6BE7E4F7E}");
                public static readonly ID EmailLabel = new ID("{4B517ECE-E1F4-4B35-AD25-BDD0126D4A80}");
                public static readonly ID PhoneLabel = new ID("{EC6CFDEF-F047-4B98-8425-3257BF8C4D50}");
                public static readonly ID PhoneHelpText = new ID("{D393AD66-A8F7-4563-891B-E81268666D5F}");
                public static readonly ID SubjectLabel = new ID("{F1ADBF19-B6F4-4902-B104-42D820900C76}");
                public static readonly ID DefaultSubjectText = new ID("{3BFC9C9E-FBAA-4255-944D-BF51579D6235}");
                public static readonly ID MessageLabel = new ID("{6C81FC81-97D3-43FB-8402-B2464F581032}");
                public static readonly ID SubmitButtonText = new ID("{86124CB1-73CE-43EC-8410-98B8ECAF1EC8}");
                // newly added
                public static readonly ID ConfirmationMessage = new ID("{114B6D2E-F419-46DF-9CD4-FBE81A70020D}");
                public static readonly ID AnotherMessageSubmitButtonText = new ID("{2F2442B7-0265-4063-8D1E-3CCE4DE2C1BD}");
                public static readonly ID ErrorMessage = new ID("{DEF672D6-B5B2-4649-AE69-ECEFDCA38874}");
            }
        }
        public struct SubjectSettings
        {
            public static readonly ID TemplateId = new ID("{909E8B06-4357-478A-9666-79588E09F9EE}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{D51A776F-3B55-4726-8CC2-2C29EC171170}");
                public static readonly ID EmailListTo = new ID("{1ED0F691-F5A4-4ED1-8AF7-73ACD1E51D04}");
                public static readonly ID EmailListCC = new ID("{4E4A3BE5-D8E7-4094-9743-6CE81550A330}");
                public static readonly ID EmailListBcc = new ID("{1B887E66-AAED-4E6F-992E-9F1383A49F5C}");
            }
        }
        // newly added end
        public struct MiscellaneousDetail
        {
            public static readonly ID TemplateId = new ID("{5A5C0E8A-2FE1-454A-94F8-3357328F8AB7}");
            public struct Fields
            {
                public static readonly ID Miscellaneous = new ID("{99BF1EE5-43B6-4304-AAD9-C8E025732E8F}");
            }
        }

        public struct AddressDetail
        {
            public static readonly ID TemplateId = new ID("{44F07107-5984-42F1-B0BB-7035128BE93D}");
            public struct Fields
            {
                public static readonly ID StreetAddress = new ID("{26BA5933-F7AF-4375-AF08-B482AE0AC1FC}");
                public static readonly ID City = new ID("{2CD51E33-5E7D-431F-8361-AFEB2490CA17}");
                public static readonly ID State = new ID("{2F3E7EDA-D887-4C8A-9843-3B7A3E2098B3}");
                public static readonly ID Zip = new ID("{4259C500-EC52-4451-B70E-61AAD0300900}");
            }
        }
        public struct DropdownQuoteForm
        {
            public static readonly ID ContentRootFolder = new ID("{90B63CBB-E827-4FA6-B568-98A94C759AD0}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{48E01C79-063F-4E2E-A191-BA0E6BD8A175}");
                public static readonly ID Image = new ID("{C9F63DD2-699F-4D7B-9A7B-8F3B540DAA10}");
                public static readonly ID InsuranceTypeText = new ID("{846F1EEF-07FC-4E1A-AD97-243516764666}");
                public static readonly ID FieldDefaultText = new ID("{D0717B4F-B384-4BD7-A504-276F1EA904D5}");
                public static readonly ID InsuranceTypeList = new ID("{2A0C6ED2-FB30-421B-9A64-A4F5B4F56270}");
                public static readonly ID PlaceHolderText = new ID("{076CB70F-5365-4445-9723-62A5839415C7}");
                public static readonly ID ButtonText = new ID("{B114CD63-07E6-47AA-9FB5-8F63D27F2AF7}");

            }
            public struct RenderingParameter
            {
                public static readonly ID DropDownPositionId = new ID("{481D3020-402B-4068-98FD-8E7AA89A8AC7}");

                public static readonly string DropDownPosition = "DropDownPosition";

                public static readonly ID DropDownCTAcolorId = new ID("{A864EDEB-DB3F-429E-BFC4-9A94DD2793BD}");

                public static readonly string DropDownCTAcolor = "DropDownCTAcolor";

            }
        }
        public struct VideoRequestDropdownQuoteForm
        {
            public static readonly ID TemplateId = new ID("{287F2A0E-C8F2-445E-A75C-93AA57ADE131}");
            public struct Fields
            {
                public static readonly ID Link = new ID("{9CA25051-E7F0-4F16-A1B9-64868E264319}");
            }
        }
        public struct GlobalBanner
        {
            public static readonly ID TemplateId = new ID("{0C34BECF-AA31-4A92-8EC1-65A381699876}");
            public struct Fields
            {
                public static readonly ID Text = new ID("{DBA2AF06-1029-4C0F-B979-1D65C1C01E89}");
                public static readonly ID IsShow = new ID("{D4034B04-9AEA-4381-AEF3-8B6BCB9D74D2}");
                public static readonly ID Link = new ID("{655B3C7C-9AB2-4288-8D51-F0A75D02EF75}");
            }
        }
        public struct Cookie
        {
            public static readonly ID CookieTemplateID = new ID("{8CA313C0-2BF7-4899-8FF7-3E3EBD2CC3DC}");
            public static readonly ID CookieDatasource = new ID("{486AA8DB-9FBE-4341-A6D1-43878FE3AC91}");
            public struct Fields
            {
                public static readonly ID CookieShortDescription = new ID("{F81039DB-8F0F-44D2-82B6-DAAE4C3B0103}");
                public static readonly ID CookieButtonLink = new ID("{5234AA43-CB95-437B-A08E-96CA220B2F8B}");
            }
        }
        public struct GlobalSettings
        {
            public static readonly ID TemplateId = new ID("{D476BFED-304C-45C0-AA54-D28524B2129B}");
            public struct Fields
            {
                public static readonly ID NewsLetterRoot = new ID("{CB267A8F-1285-46AC-B463-ABDDB18C1455}");
                public static readonly ID NewsLetterDefaultImage = new ID("{C35E7A69-9557-43A2-B3AA-7665E3E09A6C}");
                public static readonly ID NewsletterArchivedFolderPath = new ID("{D99FE7E3-04F0-4D9A-9918-214AA34277E9}");
                public static readonly ID NewsletterCurrentFolderPath = new ID("{A19F9F19-A947-4433-8960-3EB8F421DB29}");
                public static readonly ID InsuranceTypeRoot = new ID("{FCD86F18-7510-4AF2-B711-F4D19BAAB5A8}");
                public static readonly ID QuoteFormSubmittedEmailFromList = new ID("{D0BF5B53-F64E-4435-9A94-F46BE22D06F1}");
                public static readonly ID QuoteFormSubmittedEmailSubject = new ID("{F19A119A-A65D-46C9-979C-9FECEF872447}");
                public static readonly ID QuoteFormSubmittedEmailBody = new ID("{B02B2377-7BE4-4614-843C-17F6FD15534E}");
                public static readonly ID QuoteFormSavedEmailFromList = new ID("{C7A9EFBA-8948-4FC4-B636-58DD7DD08A32}");
                public static readonly ID QuoteFormSavedEmailSubject = new ID("{8F1B6ACC-3349-407A-9664-0E98F83E3B36}");
                public static readonly ID QuoteFormSavedEmailBody = new ID("{2E686D5F-2FBE-494B-A922-B2B1538B2426}");
                public static readonly ID DefaultQuoteStartPage = new ID("{2B1B5919-2915-44A9-9FDA-92D21F5115D6}");
                public static readonly ID PressReleasesRoot = new ID("{ED73C451-952D-4EFE-8254-019FD5AF794A}");
                public static readonly ID PressReleaseDefaultImage = new ID("{722A78A6-698D-4520-8ABA-E38DEEEBB1C3}");
                public static readonly ID ResultsPerPage = new ID("{BB09D03D-FE37-4297-BA77-4EBDB563A892}");
                public static readonly ID ContactUsInternalFrom = new ID("{08C0A7F0-D4C4-4DCA-A1E4-A3BCD4D28E9C}");
                public static readonly ID ContactUsInternalSubject = new ID("{B9C06CA2-7B7D-4445-93D9-2D29C43080C3}");
                public static readonly ID ContactUsInternalBody = new ID("{F49336D7-0423-4CF3-ABB3-600AF1031034}");
                public static readonly ID ContactUsClientFromList = new ID("{4F0D6E90-ED09-4BA2-8606-CB5E5008011E}");
                public static readonly ID ContactUsClientSubject = new ID("{E3D480B5-083A-400E-A8EF-D9695F07A8DF}");
                public static readonly ID ContactUsClientBody = new ID("{193C33A0-C3B7-4CFE-9AAE-879923F5BA21}");
                public static readonly ID ThankYouPage = new ID("{2915B07F-7E2A-4C8C-A2F3-083BF1BCBBDD}");
                public static readonly ID ClosingTime = new ID("{61E68CCA-23ED-4E20-9C68-54E8D14E52DF}");
                public static readonly ID ErrorPage = new ID("{D144ECC1-9271-4FB2-84C8-7D22D4BA17C9}");
                public static readonly ID AFITestingIPAddress = new ID("{54889FEE-6F58-4285-AE88-59DDD722251C}");
                public static readonly ID ContactUsFormResponseMessageTitleText = new ID("{806B5395-DB46-4D03-89D0-4727494E022F}");
                public static readonly ID ContactUsFormResponseMessageMessageText = new ID("{C382915C-2ED7-4682-B506-3E5F75289254}");
                public static readonly ID ContactUsFormResponseMessageCTAInstructionText = new ID("{AD293DC7-735C-419E-9096-F3DC74E72C15}");
                public static readonly ID ContactUsFormResponseMessageCTAbuttonText = new ID("{34F74499-77E9-49AD-8E39-72A60E2CCDF9}");
                public static readonly ID PrefixesDataSourceLocation = new ID("{BAD1B3B5-7D55-449E-9557-0E3138B5B7B3}");
                public static readonly ID ShortFormConfirmationPage = new ID("{250A0835-5EDF-439C-942C-A5A93682287B}");
                public static readonly ID ShortFormCustomerServiceEmailSubject = new ID("{AD3CDB4C-447B-4CD5-BD1B-377A27205CDD}");
                public static readonly ID ShortFormCustomerServiceEmailRecipients = new ID("{CCD00825-0C9C-4A17-B400-AADC15FCDBE1}");
                public static readonly ID ShortFormCustomerServiceEmailSender = new ID("{6BFDD8EF-9D1E-417D-9BA0-B106A750F9AF}");
                public static readonly ID ShortFormCustomerServiceEmailBody = new ID("{EDBA0246-D4AB-41A0-B91E-9460A7C0E81F}");
                public static readonly ID ShortFormConfirmationEmailSubject = new ID("{3AED64A0-DB82-4736-870C-262D121528D5}");
                public static readonly ID ShortFormConfirmationEmailSender = new ID("{1EF9CB87-573A-4E67-8EFE-8B9DD790EA48}");
                public static readonly ID ShortFormConfirmationEmailBody = new ID("{367807E4-1144-48FE-BECD-BDDD5C44C606}");

            }
        }


        public struct ReferralForm
        {
            public static readonly ID TemplateId = new ID("{9791844F-E4EF-41DE-B118-9EDE82549433}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{F450EEA0-8B7A-488F-88E7-2FECBD3AC3CE}");
                public static readonly ID Sub_Title = new ID("{B3AC4EB9-CF1C-4742-BA56-3EBAECF653F9}");
                public static readonly ID First_Name_Text = new ID("{15345E75-5A9A-4893-9EA7-4F093141104F}");
                public static readonly ID Last_Name_Text = new ID("{973F1689-48BB-449F-9201-3F7E51E8FBF6}");

                public static readonly ID Your_Information_Title_Text = new ID("{7014F410-89DC-4710-9EDC-A9A872FE17BA}");
                public static readonly ID Member_Number_Text = new ID("{7D7226D5-7BC4-46E8-AEDA-631402D331A0}");
                public static readonly ID Member_Number_Helper_Text = new ID("{4F6D317C-E217-40E9-BA64-69CACC650040}");

                public static readonly ID Referral_Title = new ID("{0E45D05E-4ADC-47DA-9B16-EF1C192C5375}");
                public static readonly ID Referral_Subtitle = new ID("{A74E80AB-B627-4C7E-A13C-F077D314CA17}");
                public static readonly ID Prefix_Text = new ID("{E2FA778B-E427-497E-8D7E-340DC4C9DD88}");
                public static readonly ID Rank_Text = new ID("{06CFFCB7-32D2-4AEB-B042-5618751BB70E}");
                public static readonly ID Contact_Title = new ID("{1F840FD6-D7CC-4516-BB63-4F4D58EAD009}");
                public static readonly ID Email_Text = new ID("{1F840FD6-D7CC-4516-BB63-4F4D58EAD009}");
                public static readonly ID Phone_Number_Text = new ID("{B4B3B6F8-3DBC-41BC-A90E-792F3A1CB937}");
                public static readonly ID Phone_Number_Helper_Text = new ID("{A1097BE4-71A2-4A73-9EC9-13E1B59510DC}");
                public static readonly ID Submit_Text = new ID("{CD214FC4-276A-463C-82C0-38D37ED3B471}");
                public static readonly ID Default_Prefix_Text = new ID("{2FB664A9-F3E1-48FB-B1FF-6B3EE5289C90}");


            }
        }
       
        public struct LandingBannerPageWithLogo
        {
            public static readonly ID TemplateId = new ID("{71E916FE-51AE-4C72-B7B3-90301A6B6264}");
            public struct Fields
            {
                public static readonly ID BackgroundImage = new ID("{28A92A30-3374-45DE-BBAF-67FA5F810104}");
                public static readonly ID LinkText = new ID("{2555A1C3-D897-4F68-85E4-94E8DF36F1BE}");
                public static readonly ID ShortDescription = new ID("{959B9E06-0C65-4403-A123-BDF728711AA5}");
                public static readonly ID Title = new ID("{44C81A71-18DC-4680-A721-F565F82B31D5}");
                public static readonly ID Logo = new ID("{56D16A49-82CF-4065-ACF0-42423C66134A}");
                public static readonly ID InsuranceTypeList = new ID("{2FBD2530-5C45-4B12-B599-19F25275A93A}");
            }
        }

       
        public struct SingleImageTextBlock
        {
            public static readonly ID TemplateId = new ID("{3E16D3D5-44FC-479C-9DC7-3E9E0326672C}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{C8460B83-6311-4AC7-B25D-5BC0554AC898}");
                public static readonly ID Link = new ID("{53566120-AF6F-4E50-A4A2-EF37ADA646EF}");
                public static readonly ID Image = new ID("{6EDFAF9A-A57D-4716-960B-9D2918F077E4}");
                public static readonly ID RichTextEditor = new ID("{7CE96359-EC25-41C5-9415-06748F8D9936}");
            }
        }
        public struct MosendMailing
        {
            public static readonly ID TemplateId = new ID("{180AC8EB-BC61-4E84-8378-D78334108208}");
            public struct Fields
            {
                public static readonly ID baseAddress = new ID("{F1906C65-9E63-460A-BD73-99A90B59C9E1}");
                public static readonly ID SitecoreSendApiKey = new ID("{FF649735-EEEB-4E25-BF53-1DAB36B8EA88}");
                public static readonly ID StaticApiKey = new ID("{0F2DE9F4-E30B-4B9D-A91B-42CBCBF40394}");

            }
        }

        public struct MarketingSection
        {
            public static readonly ID MarketingCode = new ID("{2426634B-AABB-4102-905F-908134FA4777}");
            public static readonly ID ResponseType = new ID("{167F9D72-5F25-4F60-BD10-72DAA3D3A3B5}");
        }
    }
}
