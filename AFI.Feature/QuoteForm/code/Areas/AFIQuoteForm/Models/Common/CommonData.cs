using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using Sitecore.Data.Items;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models
{
    public class CommonData
    {
        public List<Options> genders { get; set; }
        public List<Options> suffixes { get; set; }
        public List<Options> maritalStatuses { get; set; }
        public List<Options> phoneTypes { get; set; }
        public List<Options> educationLevels { get; set; }
        public List<Options> states { get; set; }
        public SubHeadings eligibilityDetailsSubheadings { get; set; }
        public SubHeadings eligibilityDetailsFirstNameLabels { get; set; }
        public SubHeadings eligibilityDetailsLastNameLabels { get; set; }
        public List<Options> militaryRanksArmy { get; set; }
        public List<Options> militaryRanksNationalGuard { get; set; }
        public List<Options> militaryRanksAirForce { get; set; }
        public List<Options> militaryRanksAirNationalGuard { get; set; }
        public List<Options> militaryRanksCoastGuard { get; set; }
        public List<Options> militaryRanksMarines { get; set; }
        public List<Options> militaryRanksNavy { get; set; }
        public List<Options> militaryRanksNoaa { get; set; }
        public List<Options> militaryRanksPhs { get; set; }
        public List<Options> militaryRanksSpaceForce { get; set; }

        public List<Options> states_lead { get; set; }

        #region QH API
        public List<Options> militaryRanksArmyQH { get; set; }
        public List<Options> militaryRanksNationalGuardQH { get; set; }
        public List<Options> militaryRanksAirForceQH { get; set; }
        public List<Options> militaryRanksAirNationalGuardQH { get; set; }
        public List<Options> militaryRanksCoastGuardQH { get; set; }
        public List<Options> militaryRanksMarinesQH { get; set; }
        public List<Options> militaryRanksNavyQH { get; set; }
        public List<Options> militaryRanksNoaaQH { get; set; }
        public List<Options> militaryRanksPhsQH { get; set; }
        public List<Options> militaryRanksSpaceForceQH { get; set; }
        #endregion

        public CommonData()
        {
            genders = GetGenders();
            suffixes = GetSuffixes();
            maritalStatuses = GetMaritalStatus();
            phoneTypes = GetPhoneTypes();
            educationLevels = GetEducationLevels();
            states = GetStateList();
            states_lead = GetStateListForLead();
            eligibilityDetailsSubheadings = new SubHeadings
            {
                military = "Military Information",
                spouse = "Current Military Spouse",
                child = "Child of Current or Former Armed Forces Insurance Member",
                parent = "Parent of Current or Former Armed Forces Insurance Member"
            };
            eligibilityDetailsFirstNameLabels = new SubHeadings
            {
                military = "First Name",
                spouse = "First Name of Spouse in Military",
                child = "Parent's First Name",
                parent = "Child's First Name"
            };
            eligibilityDetailsLastNameLabels = new SubHeadings
            {
                military = "Last Name",
                spouse = "Last Name of Spouse in Military",
                child = "Parent's Last Name",
                parent = "Child's Last Name"
            };
            militaryRanksArmy = GetArmyRanks();
            militaryRanksNationalGuard = GetArmyRanks();
            militaryRanksAirForce = GetAirForceRanks();
            militaryRanksAirNationalGuard = GetAirForceRanks();
            militaryRanksCoastGuard = GetCoastGuardRanks();
            militaryRanksMarines = GetMarineCorpsRanks();
            militaryRanksNavy = GetNavyRanks();
            militaryRanksNoaa = GetNOAARanks();
            militaryRanksPhs = GetPHSRanks();
            militaryRanksSpaceForce = GetSpaceForceRanks();

            #region QH API
            militaryRanksArmyQH = GetArmyRanksForQH();
            militaryRanksNationalGuardQH = GetArmyNGRanksQH();
            militaryRanksAirForceQH = GetAirForceRanksQH();
            militaryRanksAirNationalGuardQH = GetAirForceNGRanksQH();
            militaryRanksCoastGuardQH = GetCoastGuardRanksQH();
            militaryRanksMarinesQH = GetMarineCorpsRanksQH();
            militaryRanksNavyQH = GetNavyRanksQH();
            militaryRanksNoaaQH = GetNOAARanksQH();
            militaryRanksPhsQH = GetPHSRanksQH();
            militaryRanksSpaceForceQH = GetSpaceForceRanksQH();
            #endregion
        }

        private List<Options> GetGenders()
        {
            var genders = new List<Options>();
            genders.Add(new Options
            {
                label = "Male",
                value = "Male"
            });
            genders.Add(new Options
            {
                label = "Female",
                value = "Female"
            });
            return genders;
        }
        private List<Options> GetSuffixes()
        {
            var suffixes = new List<Options>();
            suffixes.Add(new Options
            {
                label = "Jr.",
                value = "Jr."
            });
            suffixes.Add(new Options
            {
                label = "Sr.",
                value = "Sr."
            });
            suffixes.Add(new Options
            {
                label = "III",
                value = "III"
            });
            suffixes.Add(new Options
            {
                label = "IV",
                value = "IV"
            });
            return suffixes;
        }
        private List<Options> GetMaritalStatus()
        {
            var marriagestatus = new List<Options>();

            marriagestatus.Add(new Options
            {
                label = "Married",
                value = "Married"
            });
            marriagestatus.Add(new Options
            {
                label = "Single",
                value = "Single"
            });
            marriagestatus.Add(new Options
            {
                label = "Divorced",
                value = "Divorced"
            });
            marriagestatus.Add(new Options
            {
                label = "Widowed",
                value = "Widowed"
            });
            marriagestatus.Add(new Options
            {
                label = "Cohabitant",
                value = "Cohabitant"
            });
            marriagestatus.Add(new Options
            {
                label = "Civil Union Or Domestic Partner",
                value = "Civil Union Or Domestic Partner"
            });
            return marriagestatus;
        }
        private List<Options> GetPhoneTypes()
        {
            var phoneTypes = new List<Options>();
            phoneTypes.Add(new Options
            {
                label = "Home",
                value = "Home"
            });
            phoneTypes.Add(new Options
            {
                label = "Business",
                value = "Business"
            });
            phoneTypes.Add(new Options
            {
                label = "Cell",
                value = "Cell"
            });
            return phoneTypes;
        }
        private List<Options> GetEducationLevels()
        {
            var educationLevels = new List<Options>();

            educationLevels.Add(new Options
            {
                label = "Military Training/Vocational",
                value = "Military Training/Vocational"
            });
            educationLevels.Add(new Options
            {
                label = "High School",
                value = "High School"
            });
            educationLevels.Add(new Options
            {
                label = "Completed Some College",
                value = "Completed Some College"
            });
            educationLevels.Add(new Options
            {
                label = "College Degree",
                value = "College Degree"
            });
            return educationLevels;
        }
        public static List<Options> GetStateList()
        {
            var stateList = new List<Options>();
            stateList.Add(new Options { label = "Alabama", value = "AL" });
            stateList.Add(new Options { label = "Alaska", value = "AK" });
            stateList.Add(new Options { label = "Arizona", value = "AZ" });
            stateList.Add(new Options { label = "Arkansas", value = "AR" });
            stateList.Add(new Options { label = "California", value = "CA" });
            stateList.Add(new Options { label = "Colorado", value = "CO" });
            stateList.Add(new Options { label = "Connecticut", value = "CT" });
            stateList.Add(new Options { label = "Delaware", value = "DE" });
            stateList.Add(new Options { label = "District of Columbia", value = "DC" });
            stateList.Add(new Options { label = "Florida", value = "FL" });
            stateList.Add(new Options { label = "Georgia", value = "GA" });
            stateList.Add(new Options { label = "Hawaii", value = "HI" });
            stateList.Add(new Options { label = "Idaho", value = "ID" });
            stateList.Add(new Options { label = "Illinois", value = "IL" });
            stateList.Add(new Options { label = "Indiana", value = "IN" });
            stateList.Add(new Options { label = "Iowa", value = "IA" });
            stateList.Add(new Options { label = "Kansas", value = "KS" });
            stateList.Add(new Options { label = "Kentucky", value = "KY" });
            stateList.Add(new Options { label = "Louisiana", value = "LA" });
            stateList.Add(new Options { label = "Maine", value = "ME" });
            stateList.Add(new Options { label = "Maryland", value = "MD" });
            stateList.Add(new Options { label = "Massachusetts", value = "MA" });
            stateList.Add(new Options { label = "Michigan", value = "MI" });
            stateList.Add(new Options { label = "Minnesota", value = "MN" });
            stateList.Add(new Options { label = "Mississippi", value = "MS" });
            stateList.Add(new Options { label = "Missouri", value = "MO" });
            stateList.Add(new Options { label = "Montana", value = "MT" });
            stateList.Add(new Options { label = "Nebraska", value = "NE" });
            stateList.Add(new Options { label = "Nevada", value = "NV" });
            stateList.Add(new Options { label = "New Hampshire", value = "NH" });
            stateList.Add(new Options { label = "New Jersey", value = "NJ" });
            stateList.Add(new Options { label = "New Mexico", value = "NM" });
            stateList.Add(new Options { label = "New York", value = "NY" });
            stateList.Add(new Options { label = "North Carolina", value = "NC" });
            stateList.Add(new Options { label = "North Dakota", value = "ND" });
            stateList.Add(new Options { label = "Ohio", value = "OH" });
            stateList.Add(new Options { label = "Oklahoma", value = "OK" });
            stateList.Add(new Options { label = "Oregon", value = "OR" });
            stateList.Add(new Options { label = "Pennsylvania", value = "PA" });
            stateList.Add(new Options { label = "Rhode Island", value = "RI" });
            stateList.Add(new Options { label = "South Carolina", value = "SC" });
            stateList.Add(new Options { label = "South Dakota", value = "SD" });
            stateList.Add(new Options { label = "Tennessee", value = "TN" });
            stateList.Add(new Options { label = "Texas", value = "TX" });
            stateList.Add(new Options { label = "Utah", value = "UT" });
            stateList.Add(new Options { label = "Vermont", value = "VT" });
            stateList.Add(new Options { label = "Virginia", value = "VA" });
            stateList.Add(new Options { label = "Washington", value = "WA" });
            stateList.Add(new Options { label = "West Virginia", value = "WV" });
            stateList.Add(new Options { label = "Wisconsin", value = "WI" });
            stateList.Add(new Options { label = "Wyoming", value = "WY" });
            stateList.Add(new Options { label = "American Samoa", value = "AS" });
            stateList.Add(new Options { label = "Federated States of Micronesia", value = "FM" });
            stateList.Add(new Options { label = "Guam", value = "GU" });
            stateList.Add(new Options { label = "Marshall Islands", value = "MH" });
            stateList.Add(new Options { label = "Northern Mariana Islands", value = "MP" });
            stateList.Add(new Options { label = "Palau", value = "PW" });
            stateList.Add(new Options { label = "Puerto Rico", value = "PR" });
            stateList.Add(new Options { label = "Virgin Islands", value = "VI" });
            stateList.Add(new Options { label = "Armed Forces Africa", value = "AE" });
            stateList.Add(new Options { label = "Armed Forces Americas", value = "AA" });
            stateList.Add(new Options { label = "Armed Forces Canada", value = "AE" });
            stateList.Add(new Options { label = "Armed Forces Europe", value = "AE" });
            stateList.Add(new Options { label = "Armed Forces Middle East", value = "AE" });
            stateList.Add(new Options { label = "Armed Forces Pacific", value = "AP" });
            return stateList;
        }

        public static List<Options> GetStateListForLead()
        {
            var stateList = new List<Options>();
            stateList.Add(new Options { label = "Alabama", value = "AL" });
            stateList.Add(new Options { label = "Alaska", value = "AK" });
            stateList.Add(new Options { label = "Arizona", value = "AZ" });
            stateList.Add(new Options { label = "Arkansas", value = "AR" });
            stateList.Add(new Options { label = "California", value = "CA" });
            stateList.Add(new Options { label = "Colorado", value = "CO" });
            stateList.Add(new Options { label = "Connecticut", value = "CT" });
            stateList.Add(new Options { label = "Delaware", value = "DE" });
            stateList.Add(new Options { label = "District of Columbia", value = "DC" });
            stateList.Add(new Options { label = "Florida", value = "FL" });
            stateList.Add(new Options { label = "Georgia", value = "GA" });
            stateList.Add(new Options { label = "Hawaii", value = "HI" });
            stateList.Add(new Options { label = "Idaho", value = "ID" });
            stateList.Add(new Options { label = "Illinois", value = "IL" });
            stateList.Add(new Options { label = "Indiana", value = "IN" });
            stateList.Add(new Options { label = "Iowa", value = "IA" });
            stateList.Add(new Options { label = "Kansas", value = "KS" });
            stateList.Add(new Options { label = "Kentucky", value = "KY" });
            stateList.Add(new Options { label = "Louisiana", value = "LA" });
            stateList.Add(new Options { label = "Maine", value = "ME" });
            stateList.Add(new Options { label = "Maryland", value = "MD" });
            stateList.Add(new Options { label = "Massachusetts", value = "MA" });
            stateList.Add(new Options { label = "Michigan", value = "MI" });
            stateList.Add(new Options { label = "Minnesota", value = "MN" });
            stateList.Add(new Options { label = "Mississippi", value = "MS" });
            stateList.Add(new Options { label = "Missouri", value = "MO" });
            stateList.Add(new Options { label = "Montana", value = "MT" });
            stateList.Add(new Options { label = "Nebraska", value = "NE" });
            stateList.Add(new Options { label = "Nevada", value = "NV" });
            stateList.Add(new Options { label = "New Hampshire", value = "NH" });
            stateList.Add(new Options { label = "New Jersey", value = "NJ" });
            stateList.Add(new Options { label = "New Mexico", value = "NM" });
            stateList.Add(new Options { label = "New York", value = "NY" });
            stateList.Add(new Options { label = "North Carolina", value = "NC" });
            stateList.Add(new Options { label = "North Dakota", value = "ND" });
            stateList.Add(new Options { label = "Ohio", value = "OH" });
            stateList.Add(new Options { label = "Oklahoma", value = "OK" });
            stateList.Add(new Options { label = "Oregon", value = "OR" });
            stateList.Add(new Options { label = "Pennsylvania", value = "PA" });
            stateList.Add(new Options { label = "Rhode Island", value = "RI" });
            stateList.Add(new Options { label = "South Carolina", value = "SC" });
            stateList.Add(new Options { label = "South Dakota", value = "SD" });
            stateList.Add(new Options { label = "Tennessee", value = "TN" });
            stateList.Add(new Options { label = "Texas", value = "TX" });
            stateList.Add(new Options { label = "Utah", value = "UT" });
            stateList.Add(new Options { label = "Vermont", value = "VT" });
            stateList.Add(new Options { label = "Virginia", value = "VA" });
            stateList.Add(new Options { label = "Washington", value = "WA" });
            stateList.Add(new Options { label = "West Virginia", value = "WV" });
            stateList.Add(new Options { label = "Wisconsin", value = "WI" });
            stateList.Add(new Options { label = "Wyoming", value = "WY" });

            return stateList;
        }
        private List<Options> GetArmyRanks()
        {
            var armyRanks = new List<Options>();
            armyRanks.Add(new Options { label = "E-1 Private", value = "E-1 Private" });
            armyRanks.Add(new Options { label = "E-2 Private", value = "E-2 Private" });
            armyRanks.Add(new Options { label = "E-3 Private First Class", value = "E-3 Private First Class" });
            armyRanks.Add(new Options { label = "E-4 Specialist 4", value = "E-4 Specialist 4" });
            armyRanks.Add(new Options { label = "E-4 Corporal", value = "E-4 Corporal" });
            armyRanks.Add(new Options { label = "E-5 Sergeant", value = "E-5 Sergeant" });
            armyRanks.Add(new Options { label = "E-6 Staff Sergeant", value = "E-6 Staff Sergeant" });
            armyRanks.Add(new Options { label = "E-7 Platoon Sergeant", value = "E-7 Platoon Sergeant" });
            armyRanks.Add(new Options { label = "E-7 Sergeant First Class", value = "E-7 Sergeant First Class" });
            armyRanks.Add(new Options { label = "E-8 First Sergeant", value = "E-8 First Sergeant" });
            armyRanks.Add(new Options { label = "E-8 Master Sergeant", value = "E-8 Master Sergeant" });
            armyRanks.Add(new Options { label = "E-9 Sergeant Major of the Army", value = "E-9 Sergeant Major of the Army" });
            armyRanks.Add(new Options { label = "E-9 Command Sergeant Major", value = "E-9 Command Sergeant Major" });
            armyRanks.Add(new Options { label = "E-9 Sergeant Major", value = "E-9 Sergeant Major" });
            armyRanks.Add(new Options { label = "O-1 Second Lieutenant", value = "O-1 Second Lieutenant" });
            armyRanks.Add(new Options { label = "O-2 First Lieutenant", value = "O-2 First Lieutenant" });
            armyRanks.Add(new Options { label = "O-3 Captain", value = "O-3 Captain" });
            armyRanks.Add(new Options { label = "O-4 Major", value = "O-4 Major" });
            armyRanks.Add(new Options { label = "O-5 Lieutenant Colonel", value = "O-5 Lieutenant Colonel" });
            armyRanks.Add(new Options { label = "O-6 Colonel", value = "O-6 Colonel" });
            armyRanks.Add(new Options { label = "O-7 Brigadier General", value = "O-7 Brigadier General" });
            armyRanks.Add(new Options { label = "O-8 Major General", value = "O-8 Major General" });
            armyRanks.Add(new Options { label = "O-9 Lieutenant General", value = "O-9 Lieutenant General" });
            armyRanks.Add(new Options { label = "O-10 General", value = "O-10 General" });
            armyRanks.Add(new Options { label = "O-11 General of the Army", value = "O-11 General of the Army" });
            armyRanks.Add(new Options { label = "W-1 Warrant Officer", value = "W-1 Warrant Officer" });
            armyRanks.Add(new Options { label = "W-2 Chief Warrant Officer", value = "W-2 Chief Warrant Officer" });
            armyRanks.Add(new Options { label = "W-3 Chief Warrant Officer", value = "W-3 Chief Warrant Officer" });
            armyRanks.Add(new Options { label = "W-4 Chief Warrant Officer", value = "W-4 Chief Warrant Officer" });
            armyRanks.Add(new Options { label = "W-5 Master Warrant Officer", value = "W-5 Master Warrant Officer" });
            return armyRanks;
        }
        private List<Options> GetAirForceRanks()
        {
            var airForceRanks = new List<Options>();
            airForceRanks.Add(new Options { label = "E-1 Airman Basic", value = "E-1 Airman Basic" });
            airForceRanks.Add(new Options { label = "E-2 Airman", value = "E-2 Airman" });
            airForceRanks.Add(new Options { label = "E-3 Airman First Class", value = "E-3 Airman First Class" });
            airForceRanks.Add(new Options { label = "E-4 Sergeant", value = "E-4 Sergeant" });
            airForceRanks.Add(new Options { label = "E-4 Senior Airman", value = "E-4 Senior Airman" });
            airForceRanks.Add(new Options { label = "E-5 Staff Sergeant", value = "E-5 Staff Sergeant" });
            airForceRanks.Add(new Options { label = "E-6 Technical Sergeant", value = "E-6 Technical Sergeant" });
            airForceRanks.Add(new Options { label = "E-7 Master Sergeant", value = "E-7 Master Sergeant" });
            airForceRanks.Add(new Options { label = "E-8 First Sergeant", value = "E-8 First Sergeant" });
            airForceRanks.Add(new Options { label = "E-8 Senior Master Sergeant", value = "E-8 Senior Master Sergeant" });
            airForceRanks.Add(new Options { label = "E-9 Chief Master Sergeant", value = "E-9 Chief Master Sergeant" });
            airForceRanks.Add(new Options { label = "E-9 Command Chief Master Sergeant", value = "E-9 Command Chief Master Sergeant" });
            airForceRanks.Add(new Options { label = "E-9 Chief Master Sergeant of the Air Force", value = "E-9 Chief Master Sergeant of the Air Force" });
            airForceRanks.Add(new Options { label = "O-1 Second Lieutenant", value = "O-1 Second Lieutenant" });
            airForceRanks.Add(new Options { label = "O-2 First Lieutenant", value = "O-2 First Lieutenant" });
            airForceRanks.Add(new Options { label = "O-3 Captain", value = "O-3 Captain" });
            airForceRanks.Add(new Options { label = "O-4 Major", value = "O-4 Major" });
            airForceRanks.Add(new Options { label = "O-5 Lieutenant Colonel", value = "O-5 Lieutenant Colonel" });
            airForceRanks.Add(new Options { label = "O-6 Colonel", value = "O-6 Colonel" });
            airForceRanks.Add(new Options { label = "O-7 Brigadier General", value = "O-7 Brigadier General" });
            airForceRanks.Add(new Options { label = "O-8 Major General", value = "O-8 Major General" });
            airForceRanks.Add(new Options { label = "O-9 Lieutenant General", value = "O-9 Lieutenant General" });
            airForceRanks.Add(new Options { label = "O-10 General", value = "O-10 General" });
            airForceRanks.Add(new Options { label = "O-11 General of the Air Force", value = "O-11 General of the Air Force" });
            airForceRanks.Add(new Options { label = "W-1 Warrant Officer", value = "W-1 Warrant Officer" });
            airForceRanks.Add(new Options { label = "W-2 Chief Warrant Officer", value = "W-2 Chief Warrant Officer" });
            airForceRanks.Add(new Options { label = "W-3 Chief Warrant Officer", value = "W-3 Chief Warrant Officer" });
            airForceRanks.Add(new Options { label = "W-4 Chief Warrant Officer", value = "W-4 Chief Warrant Officer" });
            return airForceRanks;
        }
        private List<Options> GetCoastGuardRanks()
        {
            var coastGuardRanks = new List<Options>();
            coastGuardRanks.Add(new Options { label = "E-1 Seaman Recruit", value = "E-1 Seaman Recruit" });
            coastGuardRanks.Add(new Options { label = "E-2 Seaman Apprentice", value = "E-2 Seaman Apprentice" });
            coastGuardRanks.Add(new Options { label = "E-3 Seaman", value = "E-3 Seaman" });
            coastGuardRanks.Add(new Options { label = "E-4 Petty Officer 3rd Class", value = "E-4 Petty Officer 3rd Class" });
            coastGuardRanks.Add(new Options { label = "E-5 Petty Officer 2nd Class", value = "E-5 Petty Officer 2nd Class" });
            coastGuardRanks.Add(new Options { label = "E-6 Petty Officer 1st Class", value = "E-6 Petty Officer 1st Class" });
            coastGuardRanks.Add(new Options { label = "E-7 Chief Petty Officer", value = "E-7 Chief Petty Officer" });
            coastGuardRanks.Add(new Options { label = "E-8 Senior Chief Petty Officer", value = "E-8 Senior Chief Petty Officer" });
            coastGuardRanks.Add(new Options { label = "E-9 Master Chief Petty Officer", value = "E-9 Master Chief Petty Officer" });
            coastGuardRanks.Add(new Options { label = "O-1 Ensign", value = "O-1 Ensign" });
            coastGuardRanks.Add(new Options { label = "O-2 Lieutenant Junior Grade", value = "O-2 Lieutenant Junior Grade" });
            coastGuardRanks.Add(new Options { label = "O-3 Lieutenant", value = "O-3 Lieutenant" });
            coastGuardRanks.Add(new Options { label = "O-4 Lieutenant Commander", value = "O-4 Lieutenant Commander" });
            coastGuardRanks.Add(new Options { label = "O-5 Commander", value = "O-5 Commander" });
            coastGuardRanks.Add(new Options { label = "O-6 Captain", value = "O-6 Captain" });
            coastGuardRanks.Add(new Options { label = "O-7 Commodore", value = "O-7 Commodore" });
            coastGuardRanks.Add(new Options { label = "O-8 Rear Admiral", value = "O-8 Rear Admiral" });
            coastGuardRanks.Add(new Options { label = "O-9 Vice Admiral", value = "O-9 Vice Admiral" });
            coastGuardRanks.Add(new Options { label = "O-10 Admiral", value = "O-10 Admiral" });
            coastGuardRanks.Add(new Options { label = "O-11 Fleet Admiral", value = "O-11 Fleet Admiral" });
            coastGuardRanks.Add(new Options { label = "W-1 Warrant Officer", value = "W-1 Warrant Officer" });
            coastGuardRanks.Add(new Options { label = "W-2 Chief Warrant Officer", value = "W-2 Chief Warrant Officer" });
            coastGuardRanks.Add(new Options { label = "W-3 Chief Warrant Officer", value = "W-3 Chief Warrant Officer" });
            coastGuardRanks.Add(new Options { label = "W-4 Chief Warrant Officer", value = "W-4 Chief Warrant Officer" });
            return coastGuardRanks;
        }
        private List<Options> GetMarineCorpsRanks()
        {
            var marineCorpsRanks = new List<Options>();
            marineCorpsRanks.Add(new Options { label = "E-1 Private", value = "E-1 Private" });
            marineCorpsRanks.Add(new Options { label = "E-2 Private First Class", value = "E-2 Private First Class" });
            marineCorpsRanks.Add(new Options { label = "E-3 Lance Corporal", value = "E-3 Lance Corporal" });
            marineCorpsRanks.Add(new Options { label = "E-4 Corporal", value = "E-4 Corporal" });
            marineCorpsRanks.Add(new Options { label = "E-5 Sergeant", value = "E-5 Sergeant" });
            marineCorpsRanks.Add(new Options { label = "E-6 Staff Sergeant", value = "E-6 Staff Sergeant" });
            marineCorpsRanks.Add(new Options { label = "E-7 Gunnery Sergeant", value = "E-7 Gunnery Sergeant" });
            marineCorpsRanks.Add(new Options { label = "E-8 First Sergeant", value = "E-8 First Sergeant" });
            marineCorpsRanks.Add(new Options { label = "E-8 Master Sergeant", value = "E-8 Master Sergeant" });
            marineCorpsRanks.Add(new Options { label = "E-9 Sergeant Major", value = "E-9 Sergeant Major" });
            marineCorpsRanks.Add(new Options { label = "E-9 Master Gunnery Sergeant", value = "E-9 Master Gunnery Sergeant" });
            marineCorpsRanks.Add(new Options { label = "O-1 Second Lieutenant", value = "O-1 Second Lieutenant" });
            marineCorpsRanks.Add(new Options { label = "O-2 First Lieutenant", value = "O-2 First Lieutenant" });
            marineCorpsRanks.Add(new Options { label = "O-3 Captain", value = "O-3 Captain" });
            marineCorpsRanks.Add(new Options { label = "O-4 Major", value = "O-4 Major" });
            marineCorpsRanks.Add(new Options { label = "O-5 Lieutenant Colonel", value = "O-5 Lieutenant Colonel" });
            marineCorpsRanks.Add(new Options { label = "O-6 Colonel", value = "O-6 Colonel" });
            marineCorpsRanks.Add(new Options { label = "O-7 Brigadier General", value = "O-7 Brigadier General" });
            marineCorpsRanks.Add(new Options { label = "O-8 Major General", value = "O-8 Major General" });
            marineCorpsRanks.Add(new Options { label = "O-9 Lieutenant General", value = "O-9 Lieutenant General" });
            marineCorpsRanks.Add(new Options { label = "O-10 General", value = "O-10 General" });
            marineCorpsRanks.Add(new Options { label = "W-1 Warrant Officer", value = "W-1 Warrant Officer" });
            marineCorpsRanks.Add(new Options { label = "W-2 Chief Warrant Officer", value = "W-2 Chief Warrant Officer" });
            marineCorpsRanks.Add(new Options { label = "W-3 Chief Warrant Officer", value = "W-3 Chief Warrant Officer" });
            marineCorpsRanks.Add(new Options { label = "W-4 Chief Warrant Officer", value = "W-4 Chief Warrant Officer" });
            marineCorpsRanks.Add(new Options { label = "W-5 Master Warrant Officer", value = "W-5 Master Warrant Officer" });
            return marineCorpsRanks;
        }
        private List<Options> GetNavyRanks()
        {
            var navyRanks = new List<Options>();
            navyRanks.Add(new Options { label = "E-1 Seaman Recruit", value = "E-1 Seaman Recruit" });
            navyRanks.Add(new Options { label = "E-2 Seaman Apprentice", value = "E-2 Seaman Apprentice" });
            navyRanks.Add(new Options { label = "E-3 Seaman", value = "E-3 Seaman" });
            navyRanks.Add(new Options { label = "E-4 Petty Officer 3rd Class", value = "E-4 Petty Officer 3rd Class" });
            navyRanks.Add(new Options { label = "E-5 Petty Officer 2nd Class", value = "E-5 Petty Officer 2nd Class" });
            navyRanks.Add(new Options { label = "E-6 Petty Officer 1st Class", value = "E-6 Petty Officer 1st Class" });
            navyRanks.Add(new Options { label = "E-7 Chief Petty Officer", value = "E-7 Chief Petty Officer" });
            navyRanks.Add(new Options { label = "E-8 Senior Chief Petty Officer", value = "E-8 Senior Chief Petty Officer" });
            navyRanks.Add(new Options { label = "E-9 Master Chief Petty Officer", value = "E-9 Master Chief Petty Officer" });
            navyRanks.Add(new Options { label = "O-1 Ensign", value = "O-1 Ensign" });
            navyRanks.Add(new Options { label = "O-2 Lieutenant Junior Grade", value = "O-2 Lieutenant Junior Grade" });
            navyRanks.Add(new Options { label = "O-3 Lieutenant", value = "O-3 Lieutenant" });
            navyRanks.Add(new Options { label = "O-4 Lieutenant Commander", value = "O-4 Lieutenant Commander" });
            navyRanks.Add(new Options { label = "O-5 Commander", value = "O-5 Commander" });
            navyRanks.Add(new Options { label = "O-6 Captain", value = "O-6 Captain" });
            navyRanks.Add(new Options { label = "O-7 Rear Admiral (Lower Half)", value = "O-7 Rear Admiral (Lower Half)" });
            navyRanks.Add(new Options { label = "O-8 Rear Admiral (Upper Half)", value = "O-8 Rear Admiral (Upper Half)" });
            navyRanks.Add(new Options { label = "O-9 Vice Admiral", value = "O-9 Vice Admiral" });
            navyRanks.Add(new Options { label = "O-10 Admiral", value = "O-10 Admiral" });
            navyRanks.Add(new Options { label = "O-11 Fleet Admiral", value = "O-11 Fleet Admiral" });
            navyRanks.Add(new Options { label = "W-1 Warrant Officer", value = "W-1 Warrant Officer" });
            navyRanks.Add(new Options { label = "W-2 Chief Warrant Officer", value = "W-2 Chief Warrant Officer" });
            navyRanks.Add(new Options { label = "W-3 Chief Warrant Officer", value = "W-3 Chief Warrant Officer" });
            navyRanks.Add(new Options { label = "W-4 Chief Warrant Officer", value = "W-4 Chief Warrant Officer" });
            return navyRanks;
        }
        private List<Options> GetNOAARanks()
        {
            var noaaRanks = new List<Options>();
            noaaRanks.Add(new Options { label = "E-1 Seaman Recruit", value = "E-1 Seaman Recruit" });
            noaaRanks.Add(new Options { label = "E-2 Seaman Apprentice", value = "E-2 Seaman Apprentice" });
            noaaRanks.Add(new Options { label = "E-3 Seaman", value = "E-3 Seaman" });
            noaaRanks.Add(new Options { label = "E-4 Petty Officer 3rd Class", value = "E-4 Petty Officer 3rd Class" });
            noaaRanks.Add(new Options { label = "E-5 Petty Officer 2nd Class", value = "E-5 Petty Officer 2nd Class" });
            noaaRanks.Add(new Options { label = "E-6 Petty Officer 1st Class", value = "E-6 Petty Officer 1st Class" });
            noaaRanks.Add(new Options { label = "E-7 Chief Petty Officer", value = "E-7 Chief Petty Officer" });
            noaaRanks.Add(new Options { label = "E-8 Senior Chief Petty Officer", value = "E-8 Senior Chief Petty Officer" });
            noaaRanks.Add(new Options { label = "E-9 Master Chief Petty Officer", value = "E-9 Master Chief Petty Officer" });
            noaaRanks.Add(new Options { label = "O-1 Ensign", value = "O-1 Ensign" });
            noaaRanks.Add(new Options { label = "O-2 Lieutenant Junior Grade", value = "O-2 Lieutenant Junior Grade" });
            noaaRanks.Add(new Options { label = "O-3 Lieutenant", value = "O-3 Lieutenant" });
            noaaRanks.Add(new Options { label = "O-4 Lieutenant Commander", value = "O-4 Lieutenant Commander" });
            noaaRanks.Add(new Options { label = "O-5 Commander", value = "O-5 Commander" });
            noaaRanks.Add(new Options { label = "O-6 Captain", value = "O-6 Captain" });
            noaaRanks.Add(new Options { label = "O-7 Rear Admiral", value = "O-7 Rear Admiral" });
            noaaRanks.Add(new Options { label = "O-7 Commodore", value = "O-7 Commodore" });
            noaaRanks.Add(new Options { label = "O-8 Rear Admiral", value = "O-8 Rear Admiral" });
            noaaRanks.Add(new Options { label = "O-9 Vice Admiral", value = "O-9 Vice Admiral" });
            noaaRanks.Add(new Options { label = "O-10 Admiral", value = "O-10 Admiral" });
            noaaRanks.Add(new Options { label = "O-11 Fleet Admiral", value = "O-11 Fleet Admiral" });
            noaaRanks.Add(new Options { label = "W-1 Warrant Officer", value = "W-1 Warrant Officer" });
            noaaRanks.Add(new Options { label = "W-2 Chief Warrant Officer", value = "W-2 Chief Warrant Officer" });
            noaaRanks.Add(new Options { label = "W-3 Chief Warrant Officer", value = "W-3 Chief Warrant Officer" });
            noaaRanks.Add(new Options { label = "W-4 Chief Warrant Officer", value = "W-4 Chief Warrant Officer" });
            return noaaRanks;
        }
        private List<Options> GetPHSRanks()
        {
            var phsRanks = new List<Options>();
            phsRanks.Add(new Options { label = "O-1 Ensign", value = "O-1 Ensign" });
            phsRanks.Add(new Options { label = "O-1 JRA", value = "O-1 JRA" });
            phsRanks.Add(new Options { label = "O-2 Lieutenant Junior Grade", value = "O-2 Lieutenant Junior Grade" });
            phsRanks.Add(new Options { label = "O-2 ASST", value = "O-2 ASST" });
            phsRanks.Add(new Options { label = "O-3 Lieutenant", value = "O-3 Lieutenant" });
            phsRanks.Add(new Options { label = "O-3 SRA", value = "O-3 SRA" });
            phsRanks.Add(new Options { label = "O-4 Full", value = "O-4 Full" });
            phsRanks.Add(new Options { label = "O-4 Lieutenant Commander", value = "O-4 Lieutenant Commander" });
            phsRanks.Add(new Options { label = "O-5 SR", value = "O-5 SR" });
            phsRanks.Add(new Options { label = "O-6 DIR", value = "O-6 DIR" });
            phsRanks.Add(new Options { label = "O-6 Captain", value = "O-6 Captain" });
            phsRanks.Add(new Options { label = "O-7 Rear Admiral", value = "O-7 Rear Admiral" });
            phsRanks.Add(new Options { label = "O-7 Commodore", value = "O-7 Commodore" });
            phsRanks.Add(new Options { label = "O-7 AST", value = "O-7 AST" });
            phsRanks.Add(new Options { label = "O-8 Rear Admiral", value = "O-8 Rear Admiral" });
            phsRanks.Add(new Options { label = "O-8 ASG", value = "O-8 ASG" });
            return phsRanks;
        }
        private List<Options> GetSpaceForceRanks()
        {
            var spaceForceRank = new List<Options>();
            spaceForceRank.Add(new Options { label = "E-1 Specialist 1", value = "E-1 Specialist 1" });
            spaceForceRank.Add(new Options { label = "E-2 Specialist 2", value = "E-2 Specialist 2" });
            spaceForceRank.Add(new Options { label = "E-3 Specialist 3", value = "E-3 Specialist 3" });
            spaceForceRank.Add(new Options { label = "E-4 Specialist 4", value = "E-4 Specialist 4" });
            spaceForceRank.Add(new Options { label = "E-5 Sergeant", value = "E-5 Sergeant" });
            spaceForceRank.Add(new Options { label = "E-6 Technical Sergeant", value = "E-6 Technical Sergeant" });
            spaceForceRank.Add(new Options { label = "E-7 Master Sergeant", value = "E-7 Master Sergeant" });
            spaceForceRank.Add(new Options { label = "E-8 Senior Master Sergeant", value = "E-8 Senior Master Sergeant" });
            spaceForceRank.Add(new Options { label = "E-9 Chief Master Sergeant", value = "E-9 Chief Master Sergeant" });
            spaceForceRank.Add(new Options { label = "E-9 Chief Master Sergeant of the Space Force", value = "E-9 Chief Master Sergeant of the Space Force" });
            spaceForceRank.Add(new Options { label = "O-1 Second Lieutenant", value = "O-1 Second Lieutenant" });
            spaceForceRank.Add(new Options { label = "O-2 First Lieutenant", value = "O-2 First Lieutenant" });
            spaceForceRank.Add(new Options { label = "O-3 Captain", value = "O-3 Captain" });
            spaceForceRank.Add(new Options { label = "O-4 Major", value = "O-4 Major" });
            spaceForceRank.Add(new Options { label = "O-5 Lieutenant Colonel", value = "O-5 Lieutenant Colonel" });
            spaceForceRank.Add(new Options { label = "O-6 Colonel", value = "O-6 Colonel" });
            spaceForceRank.Add(new Options { label = "O-7 Brigadier General", value = "O-7 Brigadier General" });
            spaceForceRank.Add(new Options { label = "O-8 Major General", value = "O-8 Major General" });
            spaceForceRank.Add(new Options { label = "O-9 Lieutenant General", value = "O-9 Lieutenant General" });
            spaceForceRank.Add(new Options { label = "O-10 General", value = "O-10 General" });
            return spaceForceRank;
        }

        #region Load For QH API
        private List<Options> GetArmyRanksForQH()
        {
            var ranks = new List<Options>
{

    new Options { label = "Brigadier General", value = "Brigadier General" },
    new Options { label = "Cadet", value = "Cadet" },
    new Options { label = "Captain", value = "Captain" },
    new Options { label = "Chief Warrant Officer", value = "Chief Warrant Officer" },
    new Options { label = "Colonel", value = "Colonel" },
    new Options { label = "Command Sergeant Major", value = "Command Sergeant Major" },
    new Options { label = "Corporal", value = "Corporal" },
    new Options { label = "First Lieutenant", value = "First Lieutenant" },
    new Options { label = "First Sergeant", value = "First Sergeant" },
    new Options { label = "General", value = "General" },
    new Options { label = "General Of The Army", value = "General Of The Army" },
    new Options { label = "Lieutenant Colonel", value = "Lieutenant Colonel" },
    new Options { label = "Lieutenant General", value = "Lieutenant General" },
    new Options { label = "Major", value = "Major" },
    new Options { label = "Major General", value = "Major General" },
    new Options { label = "Master Sergeant", value = "Master Sergeant" },
    new Options { label = "Master Warrant Officer", value = "Master Warrant Officer" },
    new Options { label = "Platoon Sergeant", value = "Platoon Sergeant" },
    new Options { label = "Private", value = "Private" },
    new Options { label = "Private First Class", value = "Private First Class" },
    new Options { label = "Second Lieutenant", value = "Second Lieutenant" },
    new Options { label = "Sergeant", value = "Sergeant" },
    new Options { label = "Sergeant First Class", value = "Sergeant First Class" },
    new Options { label = "Sergeant Major", value = "Sergeant Major" },
    new Options { label = "Sergeant Major Of The Army", value = "Sergeant Major Of The Army" },
    new Options { label = "Specialist 4", value = "Specialist 4" },
    new Options { label = "Specialist 5", value = "Specialist 5" },
    new Options { label = "Specialist 6", value = "Specialist 6" },
    new Options { label = "Specialist 7", value = "Specialist 7" },
    new Options { label = "Staff Sergeant", value = "Staff Sergeant" },
    new Options { label = "Warrant Officer", value = "Warrant Officer" },
};

            return ranks;
        }

        private List<Options> GetArmyNGRanksQH()
        {
            var ranks = new List<Options>
{

    new Options { label = "Brigadier General", value = "Brigadier General" },
    new Options { label = "Cadet", value = "Cadet" },
    new Options { label = "Captain", value = "Captain" },
    new Options { label = "Chief Warrant Officer", value = "Chief Warrant Officer" },
    new Options { label = "Colonel", value = "Colonel" },
    new Options { label = "Command Sergeant Major", value = "Command Sergeant Major" },
    new Options { label = "Corporal", value = "Corporal" },
    new Options { label = "First Lieutenant", value = "First Lieutenant" },
    new Options { label = "First Sergeant", value = "First Sergeant" },
    new Options { label = "General", value = "General" },
    new Options { label = "General Of The Army", value = "General Of The Army" },
    new Options { label = "Lieutenant Colonel", value = "Lieutenant Colonel" },
    new Options { label = "Lieutenant General", value = "Lieutenant General" },
    new Options { label = "Major", value = "Major" },
    new Options { label = "Major General", value = "Major General" },
    new Options { label = "Master Sergeant", value = "Master Sergeant" },
    new Options { label = "Master Warrant Officer", value = "Master Warrant Officer" },
    new Options { label = "Platoon Sergeant", value = "Platoon Sergeant" },
    new Options { label = "Private", value = "Private" },
    new Options { label = "Private First Class", value = "Private First Class" },
    new Options { label = "Second Lieutenant", value = "Second Lieutenant" },
    new Options { label = "Sergeant", value = "Sergeant" },
    new Options { label = "Sergeant First Class", value = "Sergeant First Class" },
    new Options { label = "Sergeant Major", value = "Sergeant Major" },
    new Options { label = "Sergeant Major Of The Army", value = "Sergeant Major Of The Army" },
    new Options { label = "Specialist 4", value = "Specialist 4" },
    new Options { label = "Specialist 5", value = "Specialist 5" },
    new Options { label = "Specialist 6", value = "Specialist 6" },
    new Options { label = "Specialist 7", value = "Specialist 7" },
    new Options { label = "Staff Sergeant", value = "Staff Sergeant" },
    new Options { label = "Warrant Officer", value = "Warrant Officer" }
};
            return ranks;
        }
        private List<Options> GetAirForceRanksQH()
        {
            var ranks = new List<Options>
{

    new Options { label = "Airman", value = "Airman" },
    new Options { label = "Airman Basic", value = "Airman Basic" },
    new Options { label = "Airman First Class", value = "Airman First Class" },
    new Options { label = "Brigadier General", value = "Brigadier General" },
    new Options { label = "Cadet", value = "Cadet" },
    new Options { label = "Captain", value = "Captain" },
    new Options { label = "Chief Master Sergeant", value = "Chief Master Sergeant" },
    new Options { label = "Chief Master Sergeant Of The Air Force", value = "Chief Master Sergeant Of The Air Force" },
    new Options { label = "Chief Warrant Officer", value = "Chief Warrant Officer" },
    new Options { label = "Colonel", value = "Colonel" },
    new Options { label = "Command Chief Master Sergeant", value = "Command Chief Master Sergeant" },
    new Options { label = "First Lieutenant", value = "First Lieutenant" },
    new Options { label = "First Sergeant", value = "First Sergeant" },
    new Options { label = "General", value = "General" },
    new Options { label = "General Of The Air Force", value = "General Of The Air Force" },
    new Options { label = "Lieutenant Colonel", value = "Lieutenant Colonel" },
    new Options { label = "Lieutenant General", value = "Lieutenant General" },
    new Options { label = "Major", value = "Major" },
    new Options { label = "Major General", value = "Major General" },
    new Options { label = "Master Sergeant", value = "Master Sergeant" },
    new Options { label = "Second Lieutenant", value = "Second Lieutenant" },
    new Options { label = "Senior Airman", value = "Senior Airman" },
    new Options { label = "Senior Master Sergeant", value = "Senior Master Sergeant" },
    new Options { label = "Sergeant", value = "Sergeant" },
    new Options { label = "Staff Sergeant", value = "Staff Sergeant" },
    new Options { label = "Technical Sergeant", value = "Technical Sergeant" },
    new Options { label = "Warrant Officer", value = "Warrant Officer" },
};
            return ranks;
        }
        private List<Options> GetAirForceNGRanksQH()
        {
            var ranks = new List<Options>
{

  new Options { label = "Airman", value = "Airman" },
  new Options { label = "Airman Basic", value = "Airman Basic" },
  new Options { label = "Airman First Class", value = "Airman First Class" },
  new Options { label = "Brigadier General", value = "Brigadier General" },
  new Options { label = "Cadet", value = "Cadet" },
  new Options { label = "Captain", value = "Captain" },
  new Options { label = "Chief Master Sergeant", value = "Chief Master Sergeant" },
  new Options { label = "Chief Master Sergeant Of The Air Force", value = "Chief Master Sergeant Of The Air Force" },
  new Options { label = "Chief Warrant Officer", value = "Chief Warrant Officer" },
  new Options { label = "Colonel", value = "Colonel" },
  new Options { label = "Command Chief Master Sergeant", value = "Command Chief Master Sergeant" },
  new Options { label = "First Lieutenant", value = "First Lieutenant" },
  new Options { label = "First Sergeant", value = "First Sergeant" },
  new Options { label = "General", value = "General" },
  new Options { label = "General Of The Air Force", value = "General Of The Air Force" },
  new Options { label = "Lieutenant Colonel", value = "Lieutenant Colonel" },
  new Options { label = "Lieutenant General", value = "Lieutenant General" },
  new Options { label = "Major", value = "Major" },
  new Options { label = "Major General", value = "Major General" },
  new Options { label = "Master Sergeant", value = "Master Sergeant" },
  new Options { label = "Second Lieutenant", value = "Second Lieutenant" },
  new Options { label = "Senior Airman", value = "Senior Airman" },
  new Options { label = "Senior Master Sergeant", value = "Senior Master Sergeant" },
  new Options { label = "Sergeant", value = "Sergeant" },
  new Options { label = "Staff Sergeant", value = "Staff Sergeant" },
  new Options { label = "Technical Sergeant", value = "Technical Sergeant" },
  new Options { label = "Warrant Officer", value = "Warrant Officer" }
};
            return ranks;
        }
        private List<Options> GetCoastGuardRanksQH()
        {
            var navyRanks = new List<Options>
{

    new Options { label = "Admiral", value = "Admiral" },
    new Options { label = "Cadet", value = "Cadet" },
    new Options { label = "Captain", value = "Captain" },
    new Options { label = "Chief Petty Officer", value = "Chief Petty Officer" },
    new Options { label = "Chief Warrant Officer", value = "Chief Warrant Officer" },
    new Options { label = "Commander", value = "Commander" },
    new Options { label = "Commodore", value = "Commodore" },
    new Options { label = "Ensign", value = "Ensign" },
    new Options { label = "Fleet Admiral", value = "Fleet Admiral" },
    new Options { label = "Lieutenant", value = "Lieutenant" },
    new Options { label = "Lieutenant Commander", value = "Lieutenant Commander" },
    new Options { label = "Lieutenant Junior Grade", value = "Lieutenant Junior Grade" },
    new Options { label = "Master Chief Petty Officer", value = "Master Chief Petty Officer" },
    new Options { label = "Petty Officer 1st Class", value = "Petty Officer 1st Class" },
    new Options { label = "Petty Officer 2nd Class", value = "Petty Officer 2nd Class" },
    new Options { label = "Petty Officer 3rd Class", value = "Petty Officer 3rd Class" },
    new Options { label = "Rear Admiral", value = "Rear Admiral" },
    new Options { label = "Seaman", value = "Seaman" },
    new Options { label = "Seaman Apprentice", value = "Seaman Apprentice" },
    new Options { label = "Seaman Recruit", value = "Seaman Recruit" },
    new Options { label = "Senior Chief Petty Officer", value = "Senior Chief Petty Officer" },
    new Options { label = "Vice Admiral", value = "Vice Admiral" },
    new Options { label = "Warrant Officer", value = "Warrant Officer" }
};
            return navyRanks;
        }
        private List<Options> GetMarineCorpsRanksQH()
        {
            var marineCorpsRanks = new List<Options>
{

    new Options { label = "Brigadier General", value = "Brigadier General" },
    new Options { label = "Captain", value = "Captain" },
    new Options { label = "Chief Warrant Officer", value = "Chief Warrant Officer" },
    new Options { label = "Colonel", value = "Colonel" },
    new Options { label = "Corporal", value = "Corporal" },
    new Options { label = "First Lieutenant", value = "First Lieutenant" },
    new Options { label = "First Sergeant", value = "First Sergeant" },
    new Options { label = "General", value = "General" },
    new Options { label = "Gunnery Sergeant", value = "Gunnery Sergeant" },
    new Options { label = "Lance Corporal", value = "Lance Corporal" },
    new Options { label = "Lieutenant Colonel", value = "Lieutenant Colonel" },
    new Options { label = "Lieutenant General", value = "Lieutenant General" },
    new Options { label = "Major", value = "Major" },
    new Options { label = "Major General", value = "Major General" },
    new Options { label = "Master Gunnery Sergeant", value = "Master Gunnery Sergeant" },
    new Options { label = "Master Sergeant", value = "Master Sergeant" },
    new Options { label = "Private", value = "Private" },
    new Options { label = "Private First Class", value = "Private First Class" },
    new Options { label = "Second Lieutenant", value = "Second Lieutenant" },
    new Options { label = "Sergeant", value = "Sergeant" },
    new Options { label = "Sergeant Major", value = "Sergeant Major" },
    new Options { label = "Sergeant Major Of The Marine Corps", value = "Sergeant Major Of The Marine Corps" },
    new Options { label = "Staff Sergeant", value = "Staff Sergeant" },
    new Options { label = "Warrant Officer", value = "Warrant Officer" }
};
            return marineCorpsRanks;
        }
        private List<Options> GetSpaceForceRanksQH()
        {
            var ranks = new List<Options>
{

    new Options { label = "Brigadier General", value = "Brigadier General" },
    new Options { label = "Captain", value = "Captain" },
    new Options { label = "Chief Master Sergeant", value = "Chief Master Sergeant" },
    new Options { label = "Chief Master Sergeant Of The Space Force", value = "Chief Master Sergeant Of The Space Force" },
    new Options { label = "Colonel", value = "Colonel" },
    new Options { label = "First Lieutenant", value = "First Lieutenant" },
    new Options { label = "General", value = "General" },
    new Options { label = "Lieutenant Colonel", value = "Lieutenant Colonel" },
    new Options { label = "Lieutenant General", value = "Lieutenant General" },
    new Options { label = "Major", value = "Major" },
    new Options { label = "Major General", value = "Major General" },
    new Options { label = "Master Sergeant", value = "Master Sergeant" },
    new Options { label = "Second Lieutenant", value = "Second Lieutenant" },
    new Options { label = "Senior Master Sergeant", value = "Senior Master Sergeant" },
    new Options { label = "Sergeant", value = "Sergeant" },
    new Options { label = "Specialist 1", value = "Specialist 1" },
    new Options { label = "Specialist 2", value = "Specialist 2" },
    new Options { label = "Specialist 3", value = "Specialist 3" },
    new Options { label = "Specialist 4", value = "Specialist 4" },
    new Options { label = "Technical Sergeant", value = "Technical Sergeant" },
};
            return ranks;
        }
        private List<Options> GetNavyRanksQH()
        {
            var navyRanks = new List<Options>
{

    new Options { label = "Admiral", value = "Admiral" },
    new Options { label = "Captain", value = "Captain" },
    new Options { label = "Chief Petty Officer", value = "Chief Petty Officer" },
    new Options { label = "Chief Warrant Officer", value = "Chief Warrant Officer" },
    new Options { label = "Commander", value = "Commander" },
    new Options { label = "Commodore", value = "Commodore" },
    new Options { label = "Ensign", value = "Ensign" },
    new Options { label = "Fleet Admiral", value = "Fleet Admiral" },
    new Options { label = "Lieutenant", value = "Lieutenant" },
    new Options { label = "Lieutenant Commander", value = "Lieutenant Commander" },
    new Options { label = "Lieutenant Junior Grade", value = "Lieutenant Junior Grade" },
    new Options { label = "Master Chief Petty Officer", value = "Master Chief Petty Officer" },
    new Options { label = "Master Chief Petty Officer Of The Navy", value = "Master Chief Petty Officer Of The Navy" },
    new Options { label = "Midshipman", value = "Midshipman" },
    new Options { label = "Petty Officer 1st Class", value = "Petty Officer 1st Class" },
    new Options { label = "Petty Officer 2nd Class", value = "Petty Officer 2nd Class" },
    new Options { label = "Petty Officer 3rd Class", value = "Petty Officer 3rd Class" },
    new Options { label = "Rear Admiral", value = "Rear Admiral" },
    new Options { label = "Seaman", value = "Seaman" },
    new Options { label = "Seaman Apprentice", value = "Seaman Apprentice" },
    new Options { label = "Seaman Recruit", value = "Seaman Recruit" },
    new Options { label = "Senior Chief Petty Officer", value = "Senior Chief Petty Officer" },
    new Options { label = "Vice Admiral", value = "Vice Admiral" },
    new Options { label = "Warrant Officer", value = "Warrant Officer" }
};
            return navyRanks;
        }
        private List<Options> GetNOAARanksQH()
        {
            var navyRanks = new List<Options>
{

    new Options { label = "Admiral", value = "Admiral" },
    new Options { label = "Captain", value = "Captain" },
    new Options { label = "Chief Petty Officer", value = "Chief Petty Officer" },
    new Options { label = "Chief Warrant Officer", value = "Chief Warrant Officer" },
    new Options { label = "Commander", value = "Commander" },
    new Options { label = "Commodore", value = "Commodore" },
    new Options { label = "Ensign", value = "Ensign" },
    new Options { label = "Fleet Admiral", value = "Fleet Admiral" },
    new Options { label = "Lieutenant", value = "Lieutenant" },
    new Options { label = "Lieutenant Commander", value = "Lieutenant Commander" },
    new Options { label = "Lieutenant Junior Grade", value = "Lieutenant Junior Grade" },
    new Options { label = "Master Chief Petty Officer", value = "Master Chief Petty Officer" },
    new Options { label = "Petty Officer 1st Class", value = "Petty Officer 1st Class" },
    new Options { label = "Petty Officer 2nd Class", value = "Petty Officer 2nd Class" },
    new Options { label = "Petty Officer 3rd Class", value = "Petty Officer 3rd Class" },
    new Options { label = "Rear Admiral", value = "Rear Admiral" },
    new Options { label = "Seaman", value = "Seaman" },
    new Options { label = "Seaman Apprentice", value = "Seaman Apprentice" },
    new Options { label = "Seaman Recruit", value = "Seaman Recruit" },
    new Options { label = "Senior Chief Petty Officer", value = "Senior Chief Petty Officer" },
    new Options { label = "Vice Admiral", value = "Vice Admiral" },
    new Options { label = "Warrant Officer", value = "Warrant Officer" }
};
            return navyRanks;
        }
        private List<Options> GetPHSRanksQH()
        {
            var ranks = new List<Options>
{
    new Options { label = "ASG", value = "Asg" },
    new Options { label = "ASST", value = "Asst" },
    new Options { label = "AST", value = "Ast" },
    new Options { label = "Captain", value = "Captain" },
    new Options { label = "Commander", value = "Commander" },
    new Options { label = "Commodore", value = "Commodore" },
    new Options { label = "DIR", value = "Dir" },
    new Options { label = "Ensign", value = "Ensign" },
    new Options { label = "FULL", value = "Full" },
    new Options { label = "JRA", value = "Jra" },
    new Options { label = "Lieutenant", value = "Lieutenant" },
    new Options { label = "Lieutenant Commander", value = "Lieutenant Commander" },
    new Options { label = "Lieutenant Junior Grade", value = "Lieutenant Junior Grade" },
    new Options { label = "Rear Admiral", value = "Rear Admiral" },
    new Options { label = "SR", value = "Sr" },
    new Options { label = "SRA", value = "Sra" }
};
            return ranks;
        }
        #endregion
    }
}