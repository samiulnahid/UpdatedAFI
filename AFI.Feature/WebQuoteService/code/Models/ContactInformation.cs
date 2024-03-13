using System;
using System.Collections.Generic;

namespace AFI.Feature.WebQuoteService.Models
{
    [Serializable]
    [System.Xml.Serialization.XmlRoot(ElementName = "ContactInformation")]
    public class ContactInformation : List<ContactInfo>
    {
        public ContactInfo[] ContactInfo { get; set; }
        public ContactInformation() { }
    }

    [Serializable]
    [System.Xml.Serialization.XmlRoot(ElementName = "Response")]
    public class WebQuoteServiceResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseData { get; set; }
    }

    [Serializable]
    [System.Xml.Serialization.XmlRoot(ElementName = "ContactInformation")]
    public class ContactInformationResponse
    {
        public ContactInfo ContactInfo { get; set; } = new ContactInfo(ContactType.Primary);
    }

    public enum ContactType
    {
        Primary,
        SpouseSecondary
    }

    [Serializable]
    public class ContactInfo
    {
        public ContactInfo() { }
        public string ContactType { get; set; } = string.Empty;
        
        public int? CNTCGroupID { get; set; }
        
        public decimal? MFNumber { get; set; }

        public string MFSuffix { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string MiddleName { get; set; }

        public string LastName { get; set; } = string.Empty;

        public string SSN { get; set; }

        
        public DateTime? DOB { get; set; }

        public string NameSuffix { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string NamePrefix { get; set; } = string.Empty;

        public string MaritalStatus { get; set; } = string.Empty;

        public string ClientType { get; set; } = string.Empty;

        public BranchOfServiceContainer BranchOfServiceInformation { get; set; }

        public AddressInfo[] AddressInformation { get; set; }

        public PhoneInfo[] PhoneInformation { get; set; }

        public EmailInfo[] EmailInformation { get; set; }

        public string MarketingSource { get; set; } = string.Empty;

        public string PartnerEmployeeId { get; set; } = string.Empty;

        public string MarketingOfferCode { get; set; }

        public string MarketingResponseCode { get; set; } = string.Empty;
        
        public bool AlreadySent { get; set; }

        public ContactInfo(ContactType contact)
        {
            switch (contact)
            {
                case Models.ContactType.Primary:
                    ContactType = "PRIMARY";
                    break;
                case Models.ContactType.SpouseSecondary:
                    ContactType = "SPOUSE/SECONDARY";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contact), contact, null);
            }
            CNTCGroupID = null;
            MFNumber = null;
        }
    }

    [Serializable]
    public class EmailInfo
    {
        public string EmailType { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;
    }

    [Serializable]
    public class PhoneInfo
    {
        public string PhoneType { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }

    [Serializable]
    public class AddressInfo
    {
        public string LocationType { get; set; } = string.Empty;

        public string AddressLine1 { get; set; } = string.Empty;

        public string AddressLine2 { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string County { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public bool PerformAddressScrub { get; set; }

        public string State { get; set; } = string.Empty;
    }       

    [Serializable]
    public class BranchOfServiceClass
    {
        public string BranchOfService { get; set; } = string.Empty;

        public string PayGrade { get; set; } = string.Empty;

        public string Rank { get; set; } = string.Empty;

        public string RankAbbreviation { get; set; } = string.Empty;

        public string BranchOfServiceStatus { get; set; } = string.Empty;
    }

    [Serializable]
    public class BranchOfServiceContainer
    {
        public BranchOfServiceClass BranchOfServiceInfo { get; set; }
    }

}
    
