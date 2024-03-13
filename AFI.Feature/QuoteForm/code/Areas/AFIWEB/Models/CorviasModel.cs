using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class CorviasModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string SSN { get; set; }
        public string CoverageType { get; set; }
        public string CoverageAmount { get; set; }
        public string CoverageMonth { get; set; }
        public string CoverageYear { get; set; }
        public string BranchOfService { get; set; }
        public string MilitaryStatus { get; set; }
        public string MilitaryRank { get; set; }
        public string AdditionalQuestions { get; set; }
        public string PaymentMethod { get; set; }
        public string AgentCallDate { get; set; }
        public string AgentCallTime { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string SecurityCode { get; set; }
        public string Routing { get; set; }
        public string AccountNumber { get; set; }
        public string Eligibility { get; set; }
        public int QuoteKey { get; set; }
        public DateTime? SpouseBirthDate { get; set; }
        public string SpouseSSN { get; set; }
        public string SpouseFirstName { get; set; }
        public string SpouseLastName { get; set; }
        public string SpouseSuffix { get; set; }

        // newly added model
        public string Suffixdetails { get; set; }
        public string address2 { get; set; }
        public string MaritalStatus { get; set; }
        public string MaritalStatusFirstName { get; set; }
        public string MaritalStatusLastName { get; set; }
        public string MaritalStatusInformationBirthDate { get; set; }
        public string MaritalStatusssn { get; set; }
        public string spousebranchOfService { get; set; }
        public string spousemilitaryStatus { get; set; }
        public string spousemilitaryRank { get; set; }
        public DateTime staringcoverage { get; set; }
        public string insurancebeencancelled { get; set; }
        public string swimmingpool { get; set; }
        public string exoticanimals { get; set; }
        public string LossHistory { get; set; }
        public string NumberofLosses { get; set; }
        public string lossdetails { get; set; }
        public string NumberofLossesliability { get; set; }
        public string OpenClaims { get; set; }
        public string ownorrentproperty { get; set; }
    }
}