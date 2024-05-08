using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.ExperienceForms.Samples.SubmitActions;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using AFI.Feature.Data.Repositories;
using System.Diagnostics;
using Sitecore.Diagnostics;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using System.Threading.Tasks;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Repository
{
    public interface IAFIReportRepository
    {
        IEnumerable<ReportView> GetAll();
        IEnumerable<ReportView> GetAllAutoReport();
        AFIReportView GetStepWiseAutoReport();
        IEnumerable<ReportView> GetAllBusinessReport();
        AFIReportView GetStepWiseBusinessReport();
        IEnumerable<ReportView> GetAllFloodReport();
        AFIReportView GetStepWiseFloodReport();
        IEnumerable<ReportView> GetAllHomeReport();
        IEnumerable<ReportView> GetAllMotorcycleReport();
        AFIReportView GetStepWiseMotorcycleReport();
        ReportView GetByKey(long key);
        AFIReportView GetAutoDetailsByKey(long key);
        AFIReportView GetBusinessDetailsByKey(long key);
        AFIReportView GetFloodDetailsByKey(long key);
        AFIReportView GetHomeDetailsByKey(long key);
        AFIReportView GetStepWiseHomeReport();
        //Update report
        IEnumerable<ReportView> GetUpdateReports(string startDate = "", string endDate = "", string coverageType = "");
        IEnumerable<ReportView> GetSectionPerformance(string startDate = "", string endDate = "");
        IEnumerable<ReportView> GetUpdateReportDetails(string coverageType, string startDate = "", string endDate = "");
        // Quotehub Homeowners
        IEnumerable<QHReport> GetQHOUpdateReportDetails(string startDate = "", string formState = "", string endDate = "", string coverageType = "");
        IEnumerable<ReportView> GetQHO(string startDate = "", string endDate = "", string coverageType = "");
        IEnumerable<ReportView> GeQuoteHubReports(string startDate = "", string endDate = "", string coverageType = "");
        IEnumerable<QHReport> GeTabularReport(string startDate = "", string endDate = "", string coverageType = "");
        IEnumerable<VoteReport> GetVoteCountReport(string voatingPeriodId = "");
        IEnumerable<QHReport> GetQHSummaryReport(string startDate = "", string endDate = "");
        IEnumerable<QHReport> GetQHActivity(string startDate = "", string endDate = "");
        IEnumerable<QHCSVReport> GetQHActivityTabularReport(string startDate = "", string endDate = "");
        EXMUnSubscription CheckStatus(string email);
        int InsertData(EXMUnSubscription entity);
        IEnumerable<VoteReport> GetDemoVoteCountReportForResult();
        IEnumerable<VoteCandidate> GetVoteCandidateList(string periodId);
        IEnumerable<VotingPeriod> GetAllVotingPeriod();

        int InsertMemberVote(ProxyVoteMember entity);

        int InsertVotingMember(VoteMember voteMember);
        VotingPeriod GetVotingPeriodById(int votingPeriodId);
        int CreateVotingPeriodData(VotingPeriod votingPeriod);
        int UpdateVotingPeriodData(VotingPeriod votingPeriod);
        int DeleteVotingPeriodData(int VotingPeriodId);

        IEnumerable<VoteCandidate> GetAllCandidateData();
        int CreateCandidateData(VoteCandidate voteCandidate);
        int UpdateCandidateData(VoteCandidate voteCandidate);
        int DeleteCandidateData(int CandidateId);
        VoteCandidate GetCandidateById(int candidateId);
        IEnumerable<VoteMember> GetAllVotingMemberData(int page, int pageSize, int VotingPeriodId, string IsEmail);
        IEnumerable<VoteReport> GetVoteCountReportForResult(string voatingPeriodId = "");
        int DeleteMemberData(int MemberId);
        int UpdateMemberData(VoteMember voteMember);
        int GetMemberVoteCountByMemberIdAndVotePeriodId(int memberId, int votingPeriodId);
        bool GetCandidateVoteBallotStatus(string voatingPeriodId, string MemberId);
        object GetTotalVoteCountDetailsForResult(string voatingPeriodId = "");
        string GetMemberEmailByMemberNumberAndPIN(string MemberNumber, string PIN);
        object GetMemberInfoByMemberNumber(string MemberNumber);
        IEnumerable<VoteCandidate> GetAllLatestVotingPeriodCandidateData();
        IEnumerable<VotingMemberCSV> GetAllFilterVotingMemberData(int page, int pageSize, int VotingPeriodId, string IsEmail);
        int SubmitMoosendMemberVote(ProxyVoteMemberMoosend entity);
        IEnumerable<VotingMemberCSV> GetTotalMemberByVoting(int VotingId,string memberVote);
    }

    public class AFIReportRepository : IAFIReportRepository
    {

        private readonly IDbConnectionProvider _dbConnectionProvider;

        public AFIReportRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }
        public IEnumerable<ReportView> GetAll()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "Select q.[Key],q.CoverageType,q.Eligibility,qc.FirstName,qc.LastName, qc.Email, qc.PhoneNumber from [dbo].[Quote] q left Join [dbo].[QuoteContact] qc ON qc.[key]=q.[Key] WHERE  q.Finished IS NULL";
                return db.Query<ReportView>(sql);
            }
        }
        public IEnumerable<ReportView> GetUpdateReportDetails(string coverageType, string startDate = "", string endDate = "")
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                string srcCoverageType = "";
                srcCoverageType = "where CoverageType='" + coverageType + "'";
                if (string.IsNullOrEmpty(startDate))
                {
                    var sql = "SELECT [CoverageType] ,[Section] ,[FieldName] ,[Completed],[Incomplete] FROM [AFIWEB_BETA].[dbo].[vw_Report] " + srcCoverageType;
                    return db.Query<ReportView>(sql);
                }
                else
                {
                    srcCoverageType += " AND [Started] BETWEEN '" + startDate + "' AND '" + endDate + "' ";
                    var sql = "SELECT [CoverageType], [Section] ,[FieldName], sum(case when[Completed]=0 then 0 else 1 end) as Completed, sum(case when[Incomplete]=0 then 0 else 1 end) as Incomplete FROM ( SELECT [CoverageType] , [Section] ,[FieldName] ,[Completed],[Incomplete] FROM [AFIWEB_BETA].[dbo].[vw_Report] " + srcCoverageType + ") as ww GROUP BY [CoverageType], [Section] ,[FieldName] Order by [Section]";
                    return db.Query<ReportView>(sql);
                }
            }
        }
        public IEnumerable<ReportView> GetUpdateReports(string startDate = "", string endDate = "", string coverageType = "")
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                string srcCoverageType = "";
                if (string.IsNullOrEmpty(startDate))
                {
                    if (!string.IsNullOrEmpty(coverageType))
                    {
                        srcCoverageType = "where CoverageType='" + coverageType + "'";
                    }
                    var sql = "Select [CoverageType], COUNT(*) as Quotes_Started, sum(case when Finished IS  NULL then 1 else 0 end) as Quotes_Completed, sum(case when Finished IS  NOT NULL then 1 else 0 end) as Quotes_Abandoned FROM [AFIWEB_BETA].[dbo].[Quote] " + srcCoverageType + " Group by [CoverageType]";
                    return db.Query<ReportView>(sql);
                }
                else
                {
                    if (!string.IsNullOrEmpty(coverageType))
                    {
                        srcCoverageType = " AND CoverageType='" + coverageType + "'";
                    }
                    var sql = "Select [CoverageType], COUNT(*) as Quotes_Started, sum(case when Finished IS  NULL then 1 else 0 end) as Quotes_Completed, sum(case when Finished IS  NOT NULL then 1 else 0 end) as Quotes_Abandoned FROM [AFIWEB_BETA].[dbo].[Quote] where [Started] BETWEEN '" + startDate + "' AND '" + endDate + "' " + srcCoverageType + " Group by [CoverageType]";
                    return db.Query<ReportView>(sql);
                }
            }
        }

        public IEnumerable<ReportView> GetSectionPerformance(string startDate = "", string endDate = "")
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                if (string.IsNullOrEmpty(startDate))
                {

                    startDate = DateTime.Now.AddMonths(-5).ToString();
                    endDate = DateTime.Now.ToString("MM/dd/yyyy");
                    // var sql = "Select [CoverageType],  0 as Policyholder_Info, 0 as Property_Info, 0 as Coverage_Info, 0 as Submit_Quote FROM [AFIWEB_BETA].[dbo].[Quote] Group by [CoverageType]";
                    var sql = "Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qa.CurrentInsurance IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q left join [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] left join [dbo].[QuoteAuto] as qa ON q.[Key]=qa.[Key] WHERE q.CoverageType='Auto' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qc.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qud.CurrentInsuranceCompany IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q left JOIN [dbo].[QuoteCommercial] as qud ON q.[Key]=qud.[Key] left join [dbo].[QuoteContact] as qc ON q.[Key]=qud.[Key] WHERE q.CoverageType='Business' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qa.CurrentInsurance IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteAuto] as qa ON q.[Key]=qa.[Key] WHERE q.CoverageType='Collector Vehicle' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qf.TotalLivingArea IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteFlood] as qf ON q.[Key]=qf.[Key] WHERE q.CoverageType='Flood' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qa.CurrentInsurance IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteAuto] as qa ON q.[Key]=qa.[Key] WHERE q.CoverageType='Homeowners' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qa.CurrentInsurance IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteAuto] as qa ON q.[Key]=qa.[Key] WHERE q.CoverageType='Motorcycle' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qa.CurrentInsurance IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteAuto] as qa ON q.[Key]=qa.[Key] WHERE q.CoverageType='Motorhome' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qr.[Key] IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].QuoteRenter as qr ON q.[Key]=qr.[Key] WHERE q.CoverageType='Renters' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when quw.[QuoteKey] IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteUmbrellaWatercraft] as quw ON q.[Key]=quw.[QuoteKey] WHERE q.CoverageType='Umbrella' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qa.CurrentInsurance IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteAuto] as qa ON q.[Key]=qa.[Key] WHERE q.CoverageType='Watercraft' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qr.[Key] IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteRenter] as qr ON q.[Key]=qr.[Key] WHERE q.CoverageType='Rental' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qh.[Key] IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteHomeowner] as qh ON q.[Key]=qh.[Key] WHERE q.CoverageType='Homeowner' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType]";
                    return db.Query<ReportView>(sql);
                }
                else
                {
                    var sql = "Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qa.CurrentInsurance IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q left join [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] left join [dbo].[QuoteAuto] as qa ON q.[Key]=qa.[Key] WHERE q.CoverageType='Auto' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qc.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qud.CurrentInsuranceCompany IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q left JOIN [dbo].[QuoteCommercial] as qud ON q.[Key]=qud.[Key] left join [dbo].[QuoteContact] as qc ON q.[Key]=qud.[Key] WHERE q.CoverageType='Business' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qa.CurrentInsurance IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteAuto] as qa ON q.[Key]=qa.[Key] WHERE q.CoverageType='Collector Vehicle' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qf.TotalLivingArea IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteFlood] as qf ON q.[Key]=qf.[Key] WHERE q.CoverageType='Flood' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qa.CurrentInsurance IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteAuto] as qa ON q.[Key]=qa.[Key] WHERE q.CoverageType='Homeowners' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qa.CurrentInsurance IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteAuto] as qa ON q.[Key]=qa.[Key] WHERE q.CoverageType='Motorcycle' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qa.CurrentInsurance IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteAuto] as qa ON q.[Key]=qa.[Key] WHERE q.CoverageType='Motorhome' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qr.[Key] IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].QuoteRenter as qr ON q.[Key]=qr.[Key] WHERE q.CoverageType='Renters' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when quw.[QuoteKey] IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteUmbrellaWatercraft] as quw ON q.[Key]=quw.[QuoteKey] WHERE q.CoverageType='Umbrella' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qa.CurrentInsurance IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteAuto] as qa ON q.[Key]=qa.[Key] WHERE q.CoverageType='Watercraft' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qr.[Key] IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteRenter] as qr ON q.[Key]=qr.[Key] WHERE q.CoverageType='Rental' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType] UNION Select q.[CoverageType], sum(case when q.[Key] IS NOT NULL then 1 else 0 end) as Policyholder_Info, sum(case when qud.FirstName IS NOT NULL then 1 else 0 end) as Property_Info, sum(case when qh.[Key] IS NOT NULL then 1 else 0 end) as Coverage_Info, sum(case when q.Finished IS NULL then 1 else 0 end) as Submit_Quote FROM [dbo].[Quote] as q LEFT JOIN [dbo].[QuoteContact] as qud ON q.[Key]=qud.[Key] LEFT JOIN [dbo].[QuoteHomeowner] as qh ON q.[Key]=qh.[Key] WHERE q.CoverageType='Homeowner' AND q.[Started] BETWEEN '" + startDate + "' AND '" + endDate + "' Group by [CoverageType]";
                    return db.Query<ReportView>(sql);
                }

            }
        }
        public IEnumerable<VoteReport> GetVoteCountReport(string voatingPeriodId = "")
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {

                if (string.IsNullOrEmpty(voatingPeriodId))
                {
                    voatingPeriodId = "(Select top 1 VotingPeriodId from ProxyVote.VotingPeriod Order by 1 desc)";
                }
                var sql = @"SELECT c.[Name] CandidateName,  sum(CASE WHEN b.Ballot='For' THEN 1 ELSE 0 END) AS VoteFor, 
sum(CASE WHEN b.Ballot='Against' THEN 1 ELSE 0 END) AS VoteAgainst FROM  ProxyVote.MemberCandidateBallot b INNER JOIN ProxyVote.Candidate c ON b.CandidateId=c.CandidateId
Where b.VotingPeriodId= " + voatingPeriodId + " GROUP BY c.[Name]";
                return db.Query<VoteReport>(sql);
            }
        }
        public IEnumerable<QHReport> GeTabularReport(string startDate = "", string endDate = "", string coverageType = "")
        {
            IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["QuoteHubDbContext"].ConnectionString);
            using (var db = connection)
            {
                string srcCoverageType = "";
                if (string.IsNullOrEmpty(startDate))
                {
                    if (!string.IsNullOrEmpty(coverageType))
                    {
                        srcCoverageType = "where CoverageType='" + coverageType + "'";
                    }
                    var sql = @"SELECT [ID] AS [Id],[QuoteID] ,[RiskAddressID] ,[CoverageType] ,[UserFName] ,[UserLName] ,[emailID] ,[PhoneNumber] ,REPLACE([Eligibility], ',', ' ') AS [Eligibility] ,[MailingstreeAddress] ,[MailingState] ,[MailingCity] ,[MailingZipCode] ,[UserType] ,[QuoteNo] ,CONVERT(VARCHAR(10), [LastSaveDate], 103) LastSaveDate ,[QuoteStatus] ,[FormState] ,CONVERT(VARCHAR(10), [CreatedOn], 103) CreatedOn ,CONVERT(VARCHAR(10), [Completed], 103) CompletedDate ,[ResponseType] ,[ResponseDescription] ,[WebCampaign] ,[WebContent] ,[WebMedium] ,[WebSource] ,[WebTerm] ,[WebClickID] ,[gclid] ,[msclkid] ,[fbclid] ,[ChipMemberId] ,[InsIdBranchOfService] ,[InsIdMilitaryStatus] ,[InsIdRank] ,[InsRiskAddress1] ,[InsRiskAddress2] ,[InsRiskZip] ,[InsIdRiskCity] ,[InsIdRiskState] ,[InsIdRiskCounty] 
,(Select Top 1 CONCAT(v.URValidationIDField,' => ', REPLACE(v.URValidationIDFieldValue, ',', ' '))  from trnURvalidationdetails v where v.URType='NoQuote' AND v.IdRiskAddress=[RiskAddressID]) as NoQuoteReason
FROM [dbo].[QuoteMarketingDetails] " + srcCoverageType;
                    return db.Query<QHReport>(sql);
                }
                else
                {
                    if (!string.IsNullOrEmpty(coverageType))
                    {
                        srcCoverageType = " AND CoverageType='" + coverageType + "'";
                    }
                    var sql = @"SELECT [ID]  AS [Id],[QuoteID] ,[RiskAddressID] ,[CoverageType] ,[UserFName] ,[UserLName] ,[emailID] ,[PhoneNumber] ,REPLACE([Eligibility], ',', ' ') AS [Eligibility],[MailingstreeAddress] ,[MailingState] ,[MailingCity] ,[MailingZipCode] ,[UserType] ,[QuoteNo] ,CONVERT(VARCHAR(10), [LastSaveDate], 103) LastSaveDate ,[QuoteStatus] ,[FormState] ,CONVERT(VARCHAR(10), [CreatedOn], 103) CreatedOn ,CONVERT(VARCHAR(10), [Completed], 103) CompletedDate ,[ResponseType] ,[ResponseDescription] ,[WebCampaign] ,[WebContent] ,[WebMedium] ,[WebSource] ,[WebTerm] ,[WebClickID] ,[gclid] ,[msclkid] ,[fbclid] ,[ChipMemberId] ,[InsIdBranchOfService] ,[InsIdMilitaryStatus] ,[InsIdRank] ,[InsRiskAddress1] ,[InsRiskAddress2] ,[InsRiskZip] ,[InsIdRiskCity] ,[InsIdRiskState] ,[InsIdRiskCounty] 
,(Select Top 1 CONCAT(v.URValidationIDField,' => ', REPLACE(v.URValidationIDFieldValue, ',', ' '))  from trnURvalidationdetails v where v.URType='NoQuote' AND v.IdRiskAddress=[RiskAddressID]) as NoQuoteReason
FROM [dbo].[QuoteMarketingDetails] where CreatedOn >='" + startDate + "' AND CreatedOn <='" + endDate + "' " + srcCoverageType;
                    return db.Query<QHReport>(sql);
                }
            }
        }
        public IEnumerable<QHReport> GetQHSummaryReport(string startDate = "", string endDate = "")
        {
            IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["QuoteHubDbContext"].ConnectionString);
            using (var db = connection)
            {

                if (string.IsNullOrEmpty(startDate))
                {
                    var sql = string.Format(@"
SELECT * INTO #TempTable
FROM(
SELECT
qra.InsIdRiskState AS RiskState
,GWNTxn.LOB
,CAST(qra.QuoteNumber AS VARCHAR(10)) + CAST(qra.QuoteRunningNumber AS VARCHAR(5)) + '-' +  + CAST(qra.QuoteVersion AS VARCHAR(5)) AS QuoteNumber
,CAST(qh.CreatedOn AS DATE) AS CreatedDate
,CASE WHEN qh.UserType = 'A' THEN 'Agent'
              WHEN qh.UserType = 'C' THEN 'Customer'
              ELSE '' END AS 'WhoEntered'
,qh.AgentName
,'GWIN' AS ProgramState
,CASE WHEN qra.QuoteType = 'NQ' THEN 'No Quote'
              WHEN qra.QuoteType = 'MO' THEN 'Moratorium'
              WHEN qra.QuoteStatus = 1 THEN 'Complete'
              WHEN qra.QuoteStatus = 0 THEN 'Incomplete'
              END AS QuoteStatus
,qh.ChipMemberId
FROM QuoteTransactions AS GWNTxn
INNER JOIN QuoteHdr qh ON GWNTxn.IdQuote = qh.IdQuote
INNER JOIN QuoteRiskAddress qra ON qra.IdQuote = qh.IdQuote
INNER JOIN 
(
SELECT MAX(ISNULL(ModifiedOn, CreatedOn)) AS Latest, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip, IdQuote
FROM QuoteRiskAddress 
GROUP BY IdQuote, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip
)AS LatestRecord ON LatestRecord.IdQuote = qra.IdQuote AND LatestRecord.Latest = ISNULL(qra.ModifiedOn, qra.CreatedOn) AND qra.IdRiskAddress = GWNTxn.IdRiskAddress

UNION
SELECT qra.InsIdRiskState AS RiskState ,McFTxn.LOB 
,CAST(qra.QuoteNumber AS VARCHAR(10)) + CAST(qra.QuoteRunningNumber AS VARCHAR(5)) + '-' +  + CAST(qra.QuoteVersion AS VARCHAR(5)) AS QuoteNumber
,CAST(qh.CreatedOn AS DATE) AS CreatedDate
,CASE WHEN qh.UserType = 'A' THEN 'Agent'
              WHEN qh.UserType = 'C' THEN 'Customer'
              ELSE '' END AS 'WhoEntered'
,qh.AgentName
,'McF' AS ProgramState
,CASE WHEN qra.QuoteType = 'NQ' THEN 'No Quote'
              WHEN qra.QuoteType = 'MO' THEN 'Moratorium'
              WHEN qra.QuoteStatus = 1 THEN 'Complete'
              WHEN qra.QuoteStatus = 0 THEN 'Incomplete'
              END AS QuoteStatus
,qh.ChipMemberId
FROM McF_QuoteTransactions AS McFTxn
INNER JOIN QuoteHdr qh ON McFTxn.IdQuote = qh.IdQuote
INNER JOIN QuoteRiskAddress qra ON qra.IdQuote = qh.IdQuote
INNER JOIN 
(
SELECT MAX(ISNULL(ModifiedOn, CreatedOn)) AS Latest, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip, IdQuote
FROM QuoteRiskAddress 
GROUP BY IdQuote, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip
)AS LatestRecord ON LatestRecord.IdQuote = qra.IdQuote AND LatestRecord.Latest = ISNULL(qra.ModifiedOn, qra.CreatedOn) AND qra.IdRiskAddress = McFTxn.IdRiskAddress
)a
Update #TempTable Set AgentName ='' Where AgentName Is Null
Select t_m.AgentName,
(Select Count(*) From #TempTable t_1 Where t_1.QuoteStatus='Complete' AND t_1.AgentName= t_m.AgentName) AS Completed,
(Select Count(*) From #TempTable t_1 Where t_1.QuoteStatus='Incomplete' AND t_1.AgentName= t_m.AgentName) AS IncompleteQuote,
(Select Count(*) From #TempTable t_1 Where t_1.QuoteStatus='No Quote' AND t_1.AgentName= t_m.AgentName) AS NoQuote,
((Select Count(*) From #TempTable t_1 Where t_1.QuoteStatus='Complete' AND t_1.AgentName= t_m.AgentName) + 
(Select Count(*) From #TempTable t_1 Where t_1.QuoteStatus='Incomplete' AND t_1.AgentName= t_m.AgentName) +
(Select Count(*) From #TempTable t_1 Where t_1.QuoteStatus='No Quote' AND t_1.AgentName= t_m.AgentName)) AS Total
From #TempTable t_m
Group by t_m.AgentName
Drop table  #TempTable
");

                    return db.Query<QHReport>(sql);
                }
                else
                {

                    var sql = string.Format(@"

SELECT * INTO #TempTable
FROM(
SELECT
qra.InsIdRiskState AS RiskState
,GWNTxn.LOB
,CAST(qra.QuoteNumber AS VARCHAR(10)) + CAST(qra.QuoteRunningNumber AS VARCHAR(5)) + '-' +  + CAST(qra.QuoteVersion AS VARCHAR(5)) AS QuoteNumber
,CAST(qh.CreatedOn AS DATE) AS CreatedDate
,CASE WHEN qh.UserType = 'A' THEN 'Agent'
              WHEN qh.UserType = 'C' THEN 'Customer'
              ELSE '' END AS 'WhoEntered'
,qh.AgentName
,'GWIN' AS ProgramState
,CASE WHEN qra.QuoteType = 'NQ' THEN 'No Quote'
              WHEN qra.QuoteType = 'MO' THEN 'Moratorium'
              WHEN qra.QuoteStatus = 1 THEN 'Complete'
              WHEN qra.QuoteStatus = 0 THEN 'Incomplete'
              END AS QuoteStatus
,qh.ChipMemberId
FROM QuoteTransactions AS GWNTxn
INNER JOIN QuoteHdr qh ON GWNTxn.IdQuote = qh.IdQuote
INNER JOIN QuoteRiskAddress qra ON qra.IdQuote = qh.IdQuote
INNER JOIN 
(
SELECT MAX(ISNULL(ModifiedOn, CreatedOn)) AS Latest, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip, IdQuote
FROM QuoteRiskAddress 
GROUP BY IdQuote, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip
)AS LatestRecord ON LatestRecord.IdQuote = qra.IdQuote AND LatestRecord.Latest = ISNULL(qra.ModifiedOn, qra.CreatedOn) AND qra.IdRiskAddress = GWNTxn.IdRiskAddress

UNION
SELECT qra.InsIdRiskState AS RiskState ,McFTxn.LOB 
,CAST(qra.QuoteNumber AS VARCHAR(10)) + CAST(qra.QuoteRunningNumber AS VARCHAR(5)) + '-' +  + CAST(qra.QuoteVersion AS VARCHAR(5)) AS QuoteNumber
,CAST(qh.CreatedOn AS DATE) AS CreatedDate
,CASE WHEN qh.UserType = 'A' THEN 'Agent'
              WHEN qh.UserType = 'C' THEN 'Customer'
              ELSE '' END AS 'WhoEntered'
,qh.AgentName
,'McF' AS ProgramState
,CASE WHEN qra.QuoteType = 'NQ' THEN 'No Quote'
              WHEN qra.QuoteType = 'MO' THEN 'Moratorium'
              WHEN qra.QuoteStatus = 1 THEN 'Complete'
              WHEN qra.QuoteStatus = 0 THEN 'Incomplete'
              END AS QuoteStatus
,qh.ChipMemberId
FROM McF_QuoteTransactions AS McFTxn
INNER JOIN QuoteHdr qh ON McFTxn.IdQuote = qh.IdQuote
INNER JOIN QuoteRiskAddress qra ON qra.IdQuote = qh.IdQuote
INNER JOIN 
(
SELECT MAX(ISNULL(ModifiedOn, CreatedOn)) AS Latest, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip, IdQuote
FROM QuoteRiskAddress 
GROUP BY IdQuote, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip
)AS LatestRecord ON LatestRecord.IdQuote = qra.IdQuote AND LatestRecord.Latest = ISNULL(qra.ModifiedOn, qra.CreatedOn) AND qra.IdRiskAddress = McFTxn.IdRiskAddress
)a
WHERE a.CreatedDate >= '{0}' AND a.CreatedDate <= '{1}'
Update #TempTable Set AgentName ='' Where AgentName Is Null
Select t_m.WhoEntered,t_m.AgentName,
(Select Count(*) From #TempTable t_1 Where t_1.QuoteStatus='Complete' AND t_1.AgentName= t_m.AgentName) AS Completed,
(Select Count(*) From #TempTable t_1 Where t_1.QuoteStatus='Incomplete' AND t_1.AgentName= t_m.AgentName) AS IncompleteQuote,
(Select Count(*) From #TempTable t_1 Where t_1.QuoteStatus='No Quote' AND t_1.AgentName= t_m.AgentName) AS NoQuote,
((Select Count(*) From #TempTable t_1 Where t_1.QuoteStatus='Complete' AND t_1.AgentName= t_m.AgentName) + 
(Select Count(*) From #TempTable t_1 Where t_1.QuoteStatus='Incomplete' AND t_1.AgentName= t_m.AgentName) +
(Select Count(*) From #TempTable t_1 Where t_1.QuoteStatus='No Quote' AND t_1.AgentName= t_m.AgentName)) AS Total
From #TempTable t_m
Group by t_m.WhoEntered,t_m.AgentName
Drop table  #TempTable ", startDate, endDate);
                    return db.Query<QHReport>(sql);
                }

            }
        }
        public IEnumerable<QHReport> GetQHActivity(string startDate = "", string endDate = "")
        {
            IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["QuoteHubDbContext"].ConnectionString);
            using (var db = connection)
            {

                if (string.IsNullOrEmpty(startDate))
                {

                    var sql = string.Format(@"SELECT * FROM(
SELECT
qra.InsIdRiskState AS RiskState
,GWNTxn.LOB
,CAST(qra.QuoteNumber AS VARCHAR(10)) + CAST(qra.QuoteRunningNumber AS VARCHAR(5)) + '-' +  + CAST(qra.QuoteVersion AS VARCHAR(5)) AS QuoteNumber
,CAST(qh.CreatedOn AS DATE) AS CreatedDate
,CASE WHEN qh.UserType = 'A' THEN 'Agent'
              WHEN qh.UserType = 'C' THEN 'Customer'
              ELSE '' END AS 'WhoEntered'
,qh.AgentName
,'GWIN' AS ProgramState
,CASE WHEN qra.QuoteType = 'NQ' THEN 'No Quote'
              WHEN qra.QuoteType = 'MO' THEN 'Moratorium'
              WHEN qra.QuoteStatus = 1 THEN 'Complete'
              WHEN qra.QuoteStatus = 0 THEN 'Incomplete'
              END AS QuoteStatus
,qh.ChipMemberId
FROM QuoteTransactions AS GWNTxn
INNER JOIN QuoteHdr qh ON GWNTxn.IdQuote = qh.IdQuote
INNER JOIN QuoteRiskAddress qra ON qra.IdQuote = qh.IdQuote
INNER JOIN 
(
SELECT MAX(ISNULL(ModifiedOn, CreatedOn)) AS Latest, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip, IdQuote
FROM QuoteRiskAddress 
GROUP BY IdQuote, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip
)AS LatestRecord ON LatestRecord.IdQuote = qra.IdQuote AND LatestRecord.Latest = ISNULL(qra.ModifiedOn, qra.CreatedOn) AND qra.IdRiskAddress = GWNTxn.IdRiskAddress

UNION
SELECT qra.InsIdRiskState AS RiskState ,McFTxn.LOB 
,CAST(qra.QuoteNumber AS VARCHAR(10)) + CAST(qra.QuoteRunningNumber AS VARCHAR(5)) + '-' +  + CAST(qra.QuoteVersion AS VARCHAR(5)) AS QuoteNumber
,CONVERT(VARCHAR(10),CAST(qh.CreatedOn AS DATE)) AS CreatedDate
,CASE WHEN qh.UserType = 'A' THEN 'Agent'
              WHEN qh.UserType = 'C' THEN 'Customer'
              ELSE '' END AS 'WhoEntered'
,qh.AgentName
,'McF' AS ProgramState
,CASE WHEN qra.QuoteType = 'NQ' THEN 'No Quote'
              WHEN qra.QuoteType = 'MO' THEN 'Moratorium'
              WHEN qra.QuoteStatus = 1 THEN 'Complete'
              WHEN qra.QuoteStatus = 0 THEN 'Incomplete'
              END AS QuoteStatus
,qh.ChipMemberId
FROM McF_QuoteTransactions AS McFTxn
INNER JOIN QuoteHdr qh ON McFTxn.IdQuote = qh.IdQuote
INNER JOIN QuoteRiskAddress qra ON qra.IdQuote = qh.IdQuote
INNER JOIN 
(
SELECT MAX(ISNULL(ModifiedOn, CreatedOn)) AS Latest, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip, IdQuote
FROM QuoteRiskAddress 
GROUP BY IdQuote, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip
)AS LatestRecord ON LatestRecord.IdQuote = qra.IdQuote AND LatestRecord.Latest = ISNULL(qra.ModifiedOn, qra.CreatedOn) AND qra.IdRiskAddress = McFTxn.IdRiskAddress
)a

ORDER BY a.CreatedDate");
                    return db.Query<QHReport>(sql);
                }
                else
                {
                    var sql = string.Format(@"SELECT * FROM(
SELECT
qra.InsIdRiskState AS RiskState
,GWNTxn.LOB
,CAST(qra.QuoteNumber AS VARCHAR(10)) + CAST(qra.QuoteRunningNumber AS VARCHAR(5)) + '-' +  + CAST(qra.QuoteVersion AS VARCHAR(5)) AS QuoteNumber
,CONVERT(VARCHAR(10),CAST(qh.CreatedOn AS DATE)) AS CreatedDate
,CASE WHEN qh.UserType = 'A' THEN 'Agent'
              WHEN qh.UserType = 'C' THEN 'Customer'
              ELSE '' END AS 'WhoEntered'
,qh.AgentName
,'GWIN' AS ProgramState
,CASE WHEN qra.QuoteType = 'NQ' THEN 'No Quote'
              WHEN qra.QuoteType = 'MO' THEN 'Moratorium'
              WHEN qra.QuoteStatus = 1 THEN 'Complete'
              WHEN qra.QuoteStatus = 0 THEN 'Incomplete'
              END AS QuoteStatus
,qh.ChipMemberId
FROM QuoteTransactions AS GWNTxn
INNER JOIN QuoteHdr qh ON GWNTxn.IdQuote = qh.IdQuote
INNER JOIN QuoteRiskAddress qra ON qra.IdQuote = qh.IdQuote
INNER JOIN 
(
SELECT MAX(ISNULL(ModifiedOn, CreatedOn)) AS Latest, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip, IdQuote
FROM QuoteRiskAddress 
GROUP BY IdQuote, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip
)AS LatestRecord ON LatestRecord.IdQuote = qra.IdQuote AND LatestRecord.Latest = ISNULL(qra.ModifiedOn, qra.CreatedOn) AND qra.IdRiskAddress = GWNTxn.IdRiskAddress

UNION
SELECT qra.InsIdRiskState AS RiskState ,McFTxn.LOB 
,CAST(qra.QuoteNumber AS VARCHAR(10)) + CAST(qra.QuoteRunningNumber AS VARCHAR(5)) + '-' +  + CAST(qra.QuoteVersion AS VARCHAR(5)) AS QuoteNumber
,CAST(qh.CreatedOn AS DATE) AS CreatedDate
,CASE WHEN qh.UserType = 'A' THEN 'Agent'
              WHEN qh.UserType = 'C' THEN 'Customer'
              ELSE '' END AS 'WhoEntered'
,qh.AgentName
,'McF' AS ProgramState
,CASE WHEN qra.QuoteType = 'NQ' THEN 'No Quote'
              WHEN qra.QuoteType = 'MO' THEN 'Moratorium'
              WHEN qra.QuoteStatus = 1 THEN 'Complete'
              WHEN qra.QuoteStatus = 0 THEN 'Incomplete'
              END AS QuoteStatus
,qh.ChipMemberId
FROM McF_QuoteTransactions AS McFTxn
INNER JOIN QuoteHdr qh ON McFTxn.IdQuote = qh.IdQuote
INNER JOIN QuoteRiskAddress qra ON qra.IdQuote = qh.IdQuote
INNER JOIN 
(
SELECT MAX(ISNULL(ModifiedOn, CreatedOn)) AS Latest, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip, IdQuote
FROM QuoteRiskAddress 
GROUP BY IdQuote, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip
)AS LatestRecord ON LatestRecord.IdQuote = qra.IdQuote AND LatestRecord.Latest = ISNULL(qra.ModifiedOn, qra.CreatedOn) AND qra.IdRiskAddress = McFTxn.IdRiskAddress
)a
WHERE a.CreatedDate >= '{0}' AND a.CreatedDate <= '{1}'

ORDER BY a.CreatedDate", startDate, endDate);
                    return db.Query<QHReport>(sql);
                }
            }
        }
        public IEnumerable<QHCSVReport> GetQHActivityTabularReport(string startDate = "", string endDate = "")
        {
            IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["QuoteHubDbContext"].ConnectionString);
            using (var db = connection)
            {

                if (string.IsNullOrEmpty(startDate))
                {

                    var sql = string.Format(@"SELECT Distinct a.*,  
qmkt.CoverageType,  qmkt.QuoteStatus, qmkt.FormState, qmkt.Completed AS CompletedDate, qmkt.ResponseType, qmkt.ResponseDescription, qmkt.WebCampaign, qmkt.WebContent,
qmkt.WebMedium, qmkt.WebSource, qmkt.WebTerm, qmkt.WebClickID, qmkt.gclid, qmkt.msclkid, qmkt.fbclid, qmkt.ChipMemberId, qmkt.InsIdBranchOfService, qmkt.InsIdMilitaryStatus,
qmkt.InsIdRank, qmkt.InsRiskAddress1, qmkt.InsRiskAddress2, qmkt.InsRiskZip, qmkt.InsIdRiskCity, qmkt.InsIdRiskState, qmkt.InsIdRiskCounty
,(Select Top 1 CONCAT(v.URValidationIDField,' => ', REPLACE(v.URValidationIDFieldValue, ',', ' '))  from trnURvalidationdetails v where v.URType='NoQuote' AND v.IdRiskAddress=qmkt.[RiskAddressID]) as NoQuoteReason
FROM 
(
	-- From GWNTransaction
	SELECT 
	qra.InsIdRiskState AS RiskState
	,GWNTxn.LOB
	,CAST(qra.QuoteNumber AS VARCHAR(10)) + CAST(qra.QuoteRunningNumber AS VARCHAR(5)) + '-' +  + CAST(qra.QuoteVersion AS VARCHAR(5)) AS QuoteNumber
	,CAST(qh.CreatedOn AS DATE) AS CreatedDate
	,CASE WHEN qh.UserType = 'A' THEN 'Agent'
				  WHEN qh.UserType = 'C' THEN 'Customer'
				  ELSE '' END AS 'WhoEntered'
	,qh.AgentName
	,'GWIN' AS ProgramState
	,CASE WHEN qra.QuoteType = 'NQ' THEN 'No Quote' 
				  WHEN qra.QuoteType = 'MO' THEN 'Moratorium'
				  WHEN qra.QuoteStatus = 1 THEN 'Complete'
				  WHEN qra.QuoteStatus = 0 THEN 'Incomplete'
				  END AS QuoteStatus
	,qh.ChipMemberId, 

	GWNTxn.IdQuote as QuoteId,
	GWNTxn.IdRiskAddress as RiskAddressId,
	qh.FirstName as UserFName,
	qh.LastName as UserLName,
	qh.emailID,
	pinf.InsPhoneNumber as PhoneNumber,
	REPLACE(cmElg.CodeName, ',', ' ') as Eligibility,
	qh.MailingAddr1 as MailingstreeAddress,
	qh.MailingState as MailingState,
	qh.MailingCity,
	qh.MailingZip as MailingZipCode,
	qh.UserType,
	LastSaveRecord.LastSaveDate

	FROM QuoteTransactions AS GWNTxn
	INNER JOIN QuoteHdr qh ON GWNTxn.IdQuote = qh.IdQuote

	INNER JOIN PersonalInfo pinf ON pinf.IdQuote = qh.IdQuote
	INNER JOIN CodeMaster cmElg ON pinf.IdQuote = qh.IdQuote and pinf.IdEligibility = cmElg.IdCode and cmElg.codetypename = 'Eligibility'

	INNER JOIN QuoteRiskAddress qra ON qra.IdQuote = qh.IdQuote

	INNER JOIN --To get latest record against each Risk Address for a given Member
	(
		SELECT MAX(ISNULL(ModifiedOn, CreatedOn)) AS Latest, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip, IdQuote
		FROM QuoteRiskAddress --Where IdQuote = @QuoteId
		GROUP BY IdQuote, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip
	)AS LatestRecord ON LatestRecord.IdQuote = qra.IdQuote AND LatestRecord.Latest = ISNULL(qra.ModifiedOn, qra.CreatedOn) AND qra.IdRiskAddress = GWNTxn.IdRiskAddress

	LEFT OUTER JOIN --To get latest record against each Risk Address for a given Member
	(
		SELECT MAX(LastSaveDate) AS LastSaveDate, QuoteId, RiskAddressId
		FROM QuoteMarketingDetails mkt --Where IdQuote = @QuoteId
		GROUP BY QuoteId, RiskAddressId
	)AS LastSaveRecord ON LastSaveRecord.QuoteId = qra.IdQuote AND qra.IdRiskAddress = LastSaveRecord.RiskAddressId

	UNION 
	-- From McfTransaction
	SELECT 
	qra.InsIdRiskState AS RiskState
	,McFTxn.LOB
	,CAST(qra.QuoteNumber AS VARCHAR(10)) + CAST(qra.QuoteRunningNumber AS VARCHAR(5)) + '-' +  + CAST(qra.QuoteVersion AS VARCHAR(5)) AS QuoteNumber
	,CAST(qh.CreatedOn AS DATE) AS CreatedDate
	,CASE WHEN qh.UserType = 'A' THEN 'Agent'
				  WHEN qh.UserType = 'C' THEN 'Customer'
				  ELSE '' END AS 'WhoEntered'
	,qh.AgentName
	,'McF' AS ProgramState
	,CASE WHEN qra.QuoteType = 'NQ' THEN 'No Quote' 
				  WHEN qra.QuoteType = 'MO' THEN 'Moratorium'
				  WHEN qra.QuoteStatus = 1 THEN 'Complete'
				  WHEN qra.QuoteStatus = 0 THEN 'Incomplete'
				  END AS QuoteStatus
	,qh.ChipMemberId, 

	McFTxn.IdQuote as QuoteId,
	McFTxn.IdRiskAddress as RiskAddressId,
	qh.FirstName as UserFName,
	qh.LastName as UserLName,
	qh.emailID,
	pinf.InsPhoneNumber as PhoneNumber,
	REPLACE(cmElg.CodeName, ',', ' ') as Eligibility,
	qh.MailingAddr1 as MailingstreeAddress,
	qh.MailingState as MailingState,
	qh.MailingCity,
	qh.MailingZip as MailingZipCode,
	qh.UserType,
	LastSaveRecord.LastSaveDate

	FROM McF_QuoteTransactions AS McFTxn
	INNER JOIN QuoteHdr qh ON McFTxn.IdQuote = qh.IdQuote

	INNER JOIN PersonalInfo pinf ON pinf.IdQuote = qh.IdQuote
	INNER JOIN CodeMaster cmElg ON pinf.IdQuote = qh.IdQuote and pinf.IdEligibility = cmElg.IdCode and cmElg.codetypename = 'Eligibility'

	INNER JOIN QuoteRiskAddress qra ON qra.IdQuote = qh.IdQuote
	INNER JOIN --To get latest record against each Risk Address for a given Member
	(
		SELECT MAX(ISNULL(ModifiedOn, CreatedOn)) AS Latest, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip, IdQuote
		FROM QuoteRiskAddress --Where IdQuote = 101
		GROUP BY IdQuote, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip
	)AS LatestRecord ON LatestRecord.IdQuote = qra.IdQuote AND LatestRecord.Latest = ISNULL(qra.ModifiedOn, qra.CreatedOn) AND qra.IdRiskAddress = McFTxn.IdRiskAddress

	LEFT OUTER JOIN --To get latest record against each MemberId and Risk Address from marketingDtail to avoid duplicate record exist due to multiple entries for Same Customer and RiskAddress
	(
		SELECT MAX(LastSaveDate) AS LastSaveDate, QuoteId, RiskAddressId
		FROM QuoteMarketingDetails mkt --Where IdQuote = @QuoteId
		GROUP BY QuoteId, RiskAddressId
	)AS LastSaveRecord ON LastSaveRecord.QuoteId = qra.IdQuote AND qra.IdRiskAddress = LastSaveRecord.RiskAddressId

)a
Left outer Join Quotemarketingdetails qmkt on a.RiskAddressId = qmkt.RiskAddressId and a.Quoteid =  qmkt.quoteid and qmkt.LastSaveDate = a.LastSaveDate
--WHERE a.CreatedDate >= '2021-05-09' AND a.CreatedDate <= '2021-05-15'
ORDER BY a.CreatedDate");
                    return db.Query<QHCSVReport>(sql);
                }
                else
                {
                    var sql = string.Format(@"SELECT Distinct a.*,  
qmkt.CoverageType,  qmkt.QuoteStatus, qmkt.FormState, qmkt.Completed AS CompletedDate, qmkt.ResponseType, qmkt.ResponseDescription, qmkt.WebCampaign, qmkt.WebContent,
qmkt.WebMedium, qmkt.WebSource, qmkt.WebTerm, qmkt.WebClickID, qmkt.gclid, qmkt.msclkid, qmkt.fbclid, qmkt.ChipMemberId, qmkt.InsIdBranchOfService, qmkt.InsIdMilitaryStatus,
qmkt.InsIdRank, qmkt.InsRiskAddress1, qmkt.InsRiskAddress2, qmkt.InsRiskZip, qmkt.InsIdRiskCity, qmkt.InsIdRiskState, qmkt.InsIdRiskCounty
,(Select Top 1 CONCAT(v.URValidationIDField,' => ', REPLACE(v.URValidationIDFieldValue, ',', ' '))  from trnURvalidationdetails v where v.URType='NoQuote' AND v.IdRiskAddress=qmkt.[RiskAddressID]) as NoQuoteReason
FROM 
(
	-- From GWNTransaction
	SELECT 
	qra.InsIdRiskState AS RiskState
	,GWNTxn.LOB
	,CAST(qra.QuoteNumber AS VARCHAR(10)) + CAST(qra.QuoteRunningNumber AS VARCHAR(5)) + '-' +  + CAST(qra.QuoteVersion AS VARCHAR(5)) AS QuoteNumber
	,CAST(qh.CreatedOn AS DATE) AS CreatedDate
	,CASE WHEN qh.UserType = 'A' THEN 'Agent'
				  WHEN qh.UserType = 'C' THEN 'Customer'
				  ELSE '' END AS 'WhoEntered'
	,qh.AgentName
	,'GWIN' AS ProgramState
	,CASE WHEN qra.QuoteType = 'NQ' THEN 'No Quote' 
				  WHEN qra.QuoteType = 'MO' THEN 'Moratorium'
				  WHEN qra.QuoteStatus = 1 THEN 'Complete'
				  WHEN qra.QuoteStatus = 0 THEN 'Incomplete'
				  END AS QuoteStatus
	,qh.ChipMemberId, 

	GWNTxn.IdQuote as QuoteId,
	GWNTxn.IdRiskAddress as RiskAddressId,
	qh.FirstName as UserFName,
	qh.LastName as UserLName,
	qh.emailID,
	pinf.InsPhoneNumber as PhoneNumber,
	REPLACE(cmElg.CodeName, ',', ' ') as Eligibility,
	qh.MailingAddr1 as MailingstreeAddress,
	qh.MailingState as MailingState,
	qh.MailingCity,
	qh.MailingZip as MailingZipCode,
	qh.UserType,
	LastSaveRecord.LastSaveDate

	FROM QuoteTransactions AS GWNTxn
	INNER JOIN QuoteHdr qh ON GWNTxn.IdQuote = qh.IdQuote

	INNER JOIN PersonalInfo pinf ON pinf.IdQuote = qh.IdQuote
	INNER JOIN CodeMaster cmElg ON pinf.IdQuote = qh.IdQuote and pinf.IdEligibility = cmElg.IdCode and cmElg.codetypename = 'Eligibility'

	INNER JOIN QuoteRiskAddress qra ON qra.IdQuote = qh.IdQuote

	INNER JOIN --To get latest record against each Risk Address for a given Member
	(
		SELECT MAX(ISNULL(ModifiedOn, CreatedOn)) AS Latest, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip, IdQuote
		FROM QuoteRiskAddress --Where IdQuote = @QuoteId
		GROUP BY IdQuote, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip
	)AS LatestRecord ON LatestRecord.IdQuote = qra.IdQuote AND LatestRecord.Latest = ISNULL(qra.ModifiedOn, qra.CreatedOn) AND qra.IdRiskAddress = GWNTxn.IdRiskAddress

	LEFT OUTER JOIN --To get latest record against each Risk Address for a given Member
	(
		SELECT MAX(LastSaveDate) AS LastSaveDate, QuoteId, RiskAddressId
		FROM QuoteMarketingDetails mkt --Where IdQuote = @QuoteId
		GROUP BY QuoteId, RiskAddressId
	)AS LastSaveRecord ON LastSaveRecord.QuoteId = qra.IdQuote AND qra.IdRiskAddress = LastSaveRecord.RiskAddressId

	UNION 
	-- From McfTransaction
	SELECT 
	qra.InsIdRiskState AS RiskState
	,McFTxn.LOB
	,CAST(qra.QuoteNumber AS VARCHAR(10)) + CAST(qra.QuoteRunningNumber AS VARCHAR(5)) + '-' +  + CAST(qra.QuoteVersion AS VARCHAR(5)) AS QuoteNumber
	,CAST(qh.CreatedOn AS DATE) AS CreatedDate
	,CASE WHEN qh.UserType = 'A' THEN 'Agent'
				  WHEN qh.UserType = 'C' THEN 'Customer'
				  ELSE '' END AS 'WhoEntered'
	,qh.AgentName
	,'McF' AS ProgramState
	,CASE WHEN qra.QuoteType = 'NQ' THEN 'No Quote' 
				  WHEN qra.QuoteType = 'MO' THEN 'Moratorium'
				  WHEN qra.QuoteStatus = 1 THEN 'Complete'
				  WHEN qra.QuoteStatus = 0 THEN 'Incomplete'
				  END AS QuoteStatus
	,qh.ChipMemberId, 

	McFTxn.IdQuote as QuoteId,
	McFTxn.IdRiskAddress as RiskAddressId,
	qh.FirstName as UserFName,
	qh.LastName as UserLName,
	qh.emailID,
	pinf.InsPhoneNumber as PhoneNumber,
	REPLACE(cmElg.CodeName, ',', ' ') as Eligibility,
	qh.MailingAddr1 as MailingstreeAddress,
	qh.MailingState as MailingState,
	qh.MailingCity,
	qh.MailingZip as MailingZipCode,
	qh.UserType,
	LastSaveRecord.LastSaveDate

	FROM McF_QuoteTransactions AS McFTxn
	INNER JOIN QuoteHdr qh ON McFTxn.IdQuote = qh.IdQuote

	INNER JOIN PersonalInfo pinf ON pinf.IdQuote = qh.IdQuote
	INNER JOIN CodeMaster cmElg ON pinf.IdQuote = qh.IdQuote and pinf.IdEligibility = cmElg.IdCode and cmElg.codetypename = 'Eligibility'

	INNER JOIN QuoteRiskAddress qra ON qra.IdQuote = qh.IdQuote
	INNER JOIN --To get latest record against each Risk Address for a given Member
	(
		SELECT MAX(ISNULL(ModifiedOn, CreatedOn)) AS Latest, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip, IdQuote
		FROM QuoteRiskAddress --Where IdQuote = 101
		GROUP BY IdQuote, InsRiskAddress1, InsRiskAddress2, InsIdRiskCity, InsIdRiskState, InsRiskZip
	)AS LatestRecord ON LatestRecord.IdQuote = qra.IdQuote AND LatestRecord.Latest = ISNULL(qra.ModifiedOn, qra.CreatedOn) AND qra.IdRiskAddress = McFTxn.IdRiskAddress

	LEFT OUTER JOIN --To get latest record against each MemberId and Risk Address from marketingDtail to avoid duplicate record exist due to multiple entries for Same Customer and RiskAddress
	(
		SELECT MAX(LastSaveDate) AS LastSaveDate, QuoteId, RiskAddressId
		FROM QuoteMarketingDetails mkt --Where IdQuote = @QuoteId
		GROUP BY QuoteId, RiskAddressId
	)AS LastSaveRecord ON LastSaveRecord.QuoteId = qra.IdQuote AND qra.IdRiskAddress = LastSaveRecord.RiskAddressId

)a
Left outer Join Quotemarketingdetails qmkt on a.RiskAddressId = qmkt.RiskAddressId and a.Quoteid =  qmkt.quoteid and qmkt.LastSaveDate = a.LastSaveDate
WHERE a.CreatedDate >= '{0}' AND a.CreatedDate <= '{1}'
ORDER BY a.CreatedDate", startDate, endDate);
                    return db.Query<QHCSVReport>(sql);
                }
            }
        }
        public IEnumerable<ReportView> GeQuoteHubReports(string startDate = "", string endDate = "", string coverageType = "")
        {
            IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["QuoteHubDbContext"].ConnectionString);
            using (var db = connection)
            {
                string srcCoverageType = "";
                if (string.IsNullOrEmpty(startDate))
                {
                    if (!string.IsNullOrEmpty(coverageType))
                    {
                        srcCoverageType = "where CoverageType='" + coverageType + "'";
                    }
                    var sql = @"Select [CoverageType], COUNT(*) as Quotes_Started, sum(case when [QuoteStatus]<>'InCompleted'  then 1 else 0 end) as Quotes_Completed, 
sum(case when[QuoteStatus] = 'InCompleted' then 1 else 0 end) as Quotes_Abandoned FROM [dbo].[QuoteMarketingDetails] " + srcCoverageType + " Group by[CoverageType]";
                    return db.Query<ReportView>(sql);
                }
                else
                {
                    if (!string.IsNullOrEmpty(coverageType))
                    {
                        srcCoverageType = " AND CoverageType='" + coverageType + "'";
                    }
                    var sql = @"Select [CoverageType], COUNT(*) as Quotes_Started, sum(case when [QuoteStatus]<>'InCompleted' then 1 else 0 end) as Quotes_Completed, 
sum(case when[QuoteStatus] = 'InCompleted' then 1 else 0 end) as Quotes_Abandoned FROM [dbo].[QuoteMarketingDetails] 
  where CreatedOn >='" + startDate + "' AND CreatedOn <= '" + endDate + "' " + srcCoverageType + " Group by[CoverageType]";
                    return db.Query<ReportView>(sql);
                }
            }
        }
        public IEnumerable<ReportView> GetQHO(string startDate = "", string endDate = "", string coverageType = "")
        {
            IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["QuoteHubDbContext"].ConnectionString);
            using (var db = connection)
            {

                if (string.IsNullOrEmpty(startDate))
                {

                    var sql = @"select CoverageType,FormState, count(*) AS Total from [dbo].[QuoteMarketingDetails] Where CoverageType='" + coverageType + "' group by FormState,CoverageType";

                    return db.Query<ReportView>(sql, null, null, true, 500);
                }
                else
                {

                    var sql = string.Format(@"select CoverageType,FormState, count(*) AS Total from [dbo].[QuoteMarketingDetails] Where CoverageType='" + coverageType + "' AND CreatedOn >='{0}' AND CreatedOn <='{1}' group by FormState,CoverageType", startDate, endDate);
                    return db.Query<ReportView>(sql, null, null, true, 500);
                }

            }
        }
        public IEnumerable<QHReport> GetQHOUpdateReportDetails(string coverageType, string formState, string startDate = "", string endDate = "")
        {
            IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["QuoteHubDbContext"].ConnectionString);
            using (var db = connection)
            {
                string srctxt = "";
                if (string.IsNullOrEmpty(coverageType))
                {
                    srctxt = "where CoverageType= 'Homeowners'";
                }
                else
                {
                    srctxt = "where CoverageType='" + coverageType + "'";
                }

                if (string.IsNullOrEmpty(startDate))
                {
                    var sql = "";
                    if (!string.IsNullOrEmpty(formState))
                    {
                        srctxt += " AND FormState='" + formState + "'";
                        sql = @"SELECT [ID] AS [Id],[QuoteID] ,[RiskAddressID] ,[CoverageType] ,[UserFName] ,[UserLName] ,[emailID] ,[PhoneNumber] ,[Eligibility] ,[MailingstreeAddress] ,[MailingState] ,[MailingCity] ,[MailingZipCode] ,[UserType] ,[QuoteNo] ,CONVERT(VARCHAR(10), [LastSaveDate], 103) LastSaveDate ,[QuoteStatus] ,[FormState] ,CONVERT(VARCHAR(10), [CreatedOn], 103) CreatedOn ,CONVERT(VARCHAR(10), [Completed], 103) Completed ,[ResponseType] ,[ResponseDescription] ,[WebCampaign] ,[WebContent] ,[WebMedium] ,[WebSource] ,[WebTerm] ,[WebClickID] ,[gclid] ,[msclkid] ,[fbclid] ,[ChipMemberId] ,[InsIdBranchOfService] ,[InsIdMilitaryStatus] ,[InsIdRank] ,[InsRiskAddress1] ,[InsRiskAddress2] ,[InsRiskZip] ,[InsIdRiskCity] ,[InsIdRiskState] ,[InsIdRiskCounty] 
FROM[dbo].[QuoteMarketingDetails] " + srctxt;
                    }
                    else
                    {
                        sql = @"SELECT FormState,CreatedOn,QuoteNo,UserFName,UserLName,emailID,PhoneNumber,QuoteStatus,CoverageType
FROM [dbo].[QuoteMarketingDetails] " + srctxt + " group by FormState,CreatedOn,QuoteNo,UserFName,UserLName,emailID,PhoneNumber,QuoteStatus,CoverageType";
                    }

                    return db.Query<QHReport>(sql);
                }
                else
                {
                    var sql = "";
                    srctxt += " AND CreatedOn >= '" + startDate + "' AND CreatedOn <= '" + endDate + "'";
                    if (!string.IsNullOrEmpty(formState))
                    {
                        srctxt += " AND FormState='" + formState + "'";
                        sql = @"SELECT [ID] AS [Id],[QuoteID] ,[RiskAddressID] ,[CoverageType] ,[UserFName] ,[UserLName] ,[emailID] ,[PhoneNumber] ,[Eligibility] ,[MailingstreeAddress] ,[MailingState] ,[MailingCity] ,[MailingZipCode] ,[UserType] ,[QuoteNo] ,CONVERT(VARCHAR(10), [LastSaveDate], 103) LastSaveDate ,[QuoteStatus] ,[FormState] ,CONVERT(VARCHAR(10), [CreatedOn], 103) CreatedOn ,CONVERT(VARCHAR(10), [Completed], 103) Completed ,[ResponseType] ,[ResponseDescription] ,[WebCampaign] ,[WebContent] ,[WebMedium] ,[WebSource] ,[WebTerm] ,[WebClickID] ,[gclid] ,[msclkid] ,[fbclid] ,[ChipMemberId] ,[InsIdBranchOfService] ,[InsIdMilitaryStatus] ,[InsIdRank] ,[InsRiskAddress1] ,[InsRiskAddress2] ,[InsRiskZip] ,[InsIdRiskCity] ,[InsIdRiskState] ,[InsIdRiskCounty] 
FROM[dbo].[QuoteMarketingDetails] " + srctxt;
                    }
                    else
                    {
                        sql = @"SELECT FormState,CreatedOn,QuoteNo,UserFName,UserLName,emailID,PhoneNumber,QuoteStatus,CoverageType
FROM [dbo].[QuoteMarketingDetails] " + srctxt + " group by FormState,CreatedOn,QuoteNo,UserFName,UserLName,emailID,PhoneNumber,QuoteStatus,CoverageType";
                    }

                    return db.Query<QHReport>(sql);
                }
            }
        }
        public ReportView GetByKey(long key)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"select qc.FirstName, qc.LastName, qc.Street, qc.City, qc.State, qc.ZipCode, qc.Email, qc.PhoneNumber, qc.PhoneType, qc.HowToContact, qc.BirthDate, qc.AFIMemberLength, qc.ServiceBranch, qc.ServiceRank, qc.ServiceStatus, qc.ServiceDischargeType, qc.MaritalStatus,
                            qc.Gender, qc.SSN, qc.SpouseOfMilitary, qc.CommissioningProgram, qc.EmploymentStatus,qc.AFIExistingPolicyType,qc.ResidenceStatus,qc.WantToReceiveInfo,qc.HowDidYouHearAboutUs,qc.Suffix,qc.Prefix, qc.InsuredParent, qc.PropertyStreet,
                             qc.PropertyCity, qc.PropertyState,qc.PropertyZipCode,qc.ServiceSpouseFirstName,qc.ServiceSpouseLastName,qc.SpouseBirthDate,qc.SpouseSSN,
qc.SpouseFirstName, qc.SpouseLastName,qc.SpouseGender,qc.SpouseSuffix,qc.CallForReview,qc.ReviewPhoneNum,qc.CNTCLegacyNum,qc.CNTCLegacySuffix,qc.FirstCommandAdvisorName,qc.UnderMoratorium,
                            q.CoverageType, q.Eligibility, q.Started, q.Finished, q.ResponseType, q.ResponseDescription, q.Offer, q.OfferDescription,
qa.BodilyInjury, qa.PropertyDamage, qa.MedicalCoverage, qa.PersonalInjury,qa.UninsuredBodilyInjury,qa.ComprehensiveDeductible,qa.CollisionDeductible,qa.CurrentInsurance,qa.CurrentPolicyDate,qa.CurrentPolicyAction,qa.CurrentBodilyInjury,
qad.AgeLicensed,qad.SafetyCourse,qad.HouseholdMovingViolation,qad.ExperienceYears,qad.GoodStudentDiscount,
qai.Incident,qai.[Date],qai.DriverName,
qav.AnnualMileage,qav.AntiTheftDevice,qav.GaragingZip,qav.LiabilityOnly,qav.Make,qav.MilesOneWay,qav.Model,qav.VehicleUse,qav.VIN
                            from [dbo].[Quote] q left join [dbo].[QuoteContact] qc on qc.[key] = q.[Key] left join [dbo].[QuoteAuto] qa on qa.[Key] = qc.[key] 
							 left join [dbo].[QuoteAutoDriver] qad on qad.[QuoteKey] = q.[key]
							 left join [dbo].[QuoteAutoIncident] qai on qai.[QuoteKey] = q.[key]
							 left join [dbo].[QuoteAutoVehicle] qav on qav.[QuoteKey] = q.[key] where q.[Key] =" + key;

                return db.QueryFirstOrDefault<ReportView>(sql);
            }
        }
        public AFIReportView GetStepWiseAutoReport()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sqlQuote = @"SELECT * FROM [dbo].[Quote]  WHERE CoverageType='Auto' AND Finished IS NULL";
                var sqlContact = @"SELECT qc.* FROM [dbo].[QuoteContact] as qc Inner Join [dbo].[Quote] as q On q.[Key] = qc.[Key] WHERE q.CoverageType='Auto' AND q.Finished IS NULL AND (qc.HowToContact IS NULL OR qc.HowToContact='Email')";
                var sqlDrivers = @"SELECT qad.* FROM [dbo].[QuoteAutoDriver] as qad Inner Join [dbo].[Quote] as q On q.[Key] = qad.[QuoteKey] 
WHERE q.CoverageType='Auto' AND q.Finished IS NULL
AND (qad.FirstName IS NULL OR qad.LastName IS NULL OR qad.BirthDate IS NULL OR qad.AgeLicensed IS NULL
 OR qad.MaritalStatus IS NULL  OR qad.Gender IS NULL) ";
                var sqlVehicles = @"SELECT qav.* FROM [dbo].[QuoteAutoVehicle] as qav Inner Join [dbo].[Quote] as q On q.[Key] = qav.[QuoteKey]
WHERE q.CoverageType='Auto' AND q.Finished IS NULL 
AND (qav.Make IS NULL OR qav.Model IS NULL OR qav.[Year] IS NULL 
OR qav.BodyStyle IS NULL OR qav.GaragingZip IS NULL OR qav.AnnualMileage IS NULL  OR qav.VehicleUse IS NULL)";
                var sqlIncidents = @"SELECT qai.* FROM [dbo].[QuoteAutoIncident] as qai Inner Join [dbo].[Quote] as q On q.[Key] = qai.[QuoteKey] 
WHERE q.CoverageType='Auto' AND q.Finished IS NULL
AND (qai.Incident IS NULL OR qai.[Date] IS NULL)";
                var sqlAutos = @"SELECT qa.* FROM [dbo].[QuoteAuto] as qa Inner Join [dbo].[Quote] as q On q.[Key] = qa.[Key] 
WHERE q.CoverageType='Auto' AND q.Finished IS NULL 
AND (qa.BodilyInjury IS NULL OR qa.PropertyDamage IS NULL OR qa.MedicalCoverage IS NULL
 OR qa.UninsuredBodilyInjury IS NULL  OR qa.PersonalInjury IS NULL)";


                AFIReportView afi = new AFIReportView();
                afi.quotes = db.Query<Quote>(sqlQuote);
                afi.quotesCount = db.Query<Quote>(sqlQuote).Count();
                afi.contacts = db.Query<QuoteContact>(sqlContact);
                afi.contactsCount = db.Query<QuoteContact>(sqlContact).Count();
                afi.autos = db.Query<QuoteAuto>(sqlAutos);
                afi.autosCount = db.Query<QuoteAuto>(sqlAutos).Count();
                afi.vehicles = db.Query<QuoteAutoVehicle>(sqlVehicles);
                afi.vehiclesCount = db.Query<QuoteAutoVehicle>(sqlVehicles).Count();
                afi.drivers = db.Query<QuoteAutoDriver>(sqlDrivers);
                afi.driversCount = db.Query<QuoteAutoDriver>(sqlDrivers).Count();
                afi.incidents = db.Query<QuoteAutoIncident>(sqlIncidents);
                afi.incidentsCount = db.Query<QuoteAutoIncident>(sqlIncidents).Count();
                return afi;
            }
        }
        public IEnumerable<ReportView> GetAllAutoReport()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "Select q.[Key], q.CoverageType, q.Eligibility ,q.Started, qc.ServiceBranch, qc.ServiceRank, qc.ServiceStatus, qc.FirstName, qc.LastName, qc.Email, qc.PhoneNumber from [dbo].[Quote] q inner Join [dbo].[QuoteContact] qc ON qc.[key]=q.[Key] WHERE q.CoverageType='Auto' and q.Finished IS NULL";
                return db.Query<ReportView>(sql);
            }
        }
        public AFIReportView GetAutoDetailsByKey(long key)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sqlQuote = @"select top 1 * from dbo.[Quote] where [Key] = " + key;
                var sqlQuoteContact = @"select top 1 * from dbo.[QuoteContact] where [Key] = " + key;
                var sqlQuoteAutoDriver = @"select * from dbo.[QuoteAutoDriver] where [QuoteKey] =" + key;
                var sqlQuoteAutoVehicle = "select * from dbo.[QuoteAutoVehicle] where [QuoteKey] =" + key;
                var sqlQuoteAutoIncident = "select * from dbo.[QuoteAutoIncident] where [QuoteKey] = " + key;
                var sqlQuoteAuto = "select top 1 * from dbo.[QuoteAuto] where [Key] = " + key;


                AFIReportView afi = new AFIReportView();
                afi.Quote = db.QueryFirstOrDefault<Quote>(sqlQuote);
                afi.QuoteContact = db.QueryFirstOrDefault<QuoteContact>(sqlQuoteContact);
                afi.QuoteAuto = db.QueryFirstOrDefault<QuoteAuto>(sqlQuoteAuto);
                afi.incidents = db.Query<QuoteAutoIncident>(sqlQuoteAutoIncident);
                afi.drivers = db.Query<QuoteAutoDriver>(sqlQuoteAutoDriver);
                afi.vehicles = db.Query<QuoteAutoVehicle>(sqlQuoteAutoVehicle);
                return afi;
            }
        }

        public IEnumerable<ReportView> GetAllBusinessReport()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "Select q.[Key], q.CoverageType, q.Eligibility ,q.Started, qc.ServiceBranch, qc.ServiceRank, qc.ServiceStatus, qc.FirstName, qc.LastName, qc.Email, qc.PhoneNumber from [dbo].[Quote] q inner Join [dbo].[QuoteContact] qc ON qc.[key]=q.[Key] WHERE q.CoverageType='Business'";
                return db.Query<ReportView>(sql);
            }
        }
        public EXMUnSubscription CheckStatus(string email)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"Select * from EXMUnSubscription where Email= '" + email + "'";

                return db.QueryFirstOrDefault<EXMUnSubscription>(sql);
            }
        }
        public int InsertData(EXMUnSubscription entity)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = "insert into EXMUnSubscription ([Email],[MemberID], [Campaign], [Status], [Date]) " +
                            "values (@Email,@MemberID, @Campaign, @Status, @Date); select scope_identity();";
                        var id = db.QueryFirstOrDefault<int>(sql, entity, transaction);
                        transaction.Commit();
                        return id;
                    }
                    catch (System.Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
        public AFIReportView GetStepWiseBusinessReport()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sqlQuote = @"SELECT * FROM [dbo].[Quote]  WHERE CoverageType='Business' AND Finished IS NULL";
                var sqlContact = @"SELECT qc.* FROM [dbo].[QuoteContact] as qc Inner Join [dbo].[Quote] as q On q.[Key] = qc.[Key] 
WHERE  q.CoverageType='Business' AND q.Finished IS NULL 
AND (qc.FirstName IS NULL OR qc.LastName IS NULL OR qc.Street IS NULL OR qc.City IS NULL
 OR qc.ZipCode IS NULL OR qc.BirthDate IS NULL OR qc.Gender IS NULL OR qc.MaritalStatus IS NULL OR qc.Email IS NULL
  OR qc.PhoneNumber IS NULL)";
                var sqlQuoteStep = @"SELECT qc.* FROM [dbo].[QuoteContact] as qc Inner Join [dbo].[Quote] as q On q.[Key] = qc.[Key] 
WHERE  q.CoverageType='Business' AND q.Finished IS NULL 
 AND (qc.HowToContact IS NULL OR qc.HowToContact='Email')";
                var sqlCommercial = @"SELECT c.* FROM [dbo].[QuoteCommercial] as c  Inner Join [dbo].[Quote] as q On q.[Key] = c.[Key] 
WHERE q.CoverageType='Business' AND q.Finished IS NULL
AND (c.FirstName IS NULL OR c.LastName IS NULL OR c.EmailAddress IS NULL OR c.BusinessName IS NULL
OR c.BusinessType IS NULL OR c.BusinessAddress IS NULL OR c.BusinessCity IS NULL OR c.BusinessZip IS NULL)";
                var sqlCommercialStep = @"SELECT c.* FROM [dbo].[QuoteCommercial] as c  Inner Join [dbo].[Quote] as q On q.[Key] = c.[Key] 
WHERE q.CoverageType='Business' AND q.Finished IS NULL
AND c.InsuranceTypeWanted IS NULL";


                AFIReportView afi = new AFIReportView();
                afi.quotes = db.Query<Quote>(sqlQuote);
                afi.quotesCount = db.Query<Quote>(sqlQuote).Count();
                afi.contacts = db.Query<QuoteContact>(sqlContact);
                afi.contactsCount = db.Query<QuoteContact>(sqlContact).Count();
                afi.commercials = db.Query<QuoteCommercial>(sqlCommercial);
                afi.CommercialsCount = db.Query<QuoteAuto>(sqlCommercial).Count();
                afi.commercialsStep = db.Query<QuoteCommercial>(sqlCommercialStep);
                afi.CommercialStepCount = db.Query<QuoteAuto>(sqlCommercialStep).Count();
                afi.quotesStep = db.Query<QuoteContact>(sqlQuoteStep);
                afi.quoteStepCount = db.Query<QuoteContact>(sqlQuoteStep).Count();
                return afi;
            }
        }
        public AFIReportView GetBusinessDetailsByKey(long key)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sqlQuote = @"select top 1 * from dbo.[Quote] where [Key] = " + key;
                var sqlContact = @"select top 1 * from dbo.[QuoteContact] where [Key] = " + key;
                var sqlCommercial = @"select * from dbo.[QuoteCommercial] where [Key] =" + key;

                AFIReportView afi = new AFIReportView();
                afi.Quote = db.QueryFirstOrDefault<Quote>(sqlQuote);
                afi.QuoteContact = db.QueryFirstOrDefault<QuoteContact>(sqlContact);
                afi.QuoteCommercial = db.QueryFirstOrDefault<QuoteCommercial>(sqlCommercial);

                return afi;
            }
        }

        public IEnumerable<ReportView> GetAllFloodReport()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "Select q.[Key], q.CoverageType, q.Eligibility ,q.Started, qc.ServiceBranch, qc.ServiceRank, qc.ServiceStatus, qc.FirstName, qc.LastName, qc.Email, qc.PhoneNumber from [dbo].[Quote] q inner Join [dbo].[QuoteContact] qc ON qc.[key]=q.[Key] WHERE q.CoverageType='Flood' and q.Finished IS NULL";
                return db.Query<ReportView>(sql);
            }
        }
        public AFIReportView GetStepWiseFloodReport()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sqlQuote = @"SELECT * FROM [dbo].[Quote]  WHERE CoverageType='Flood' AND Finished IS NULL";
                var sqlContact = @"SELECT qc.* FROM [dbo].[QuoteContact] as qc Inner Join [dbo].[Quote] as q On q.[Key] = qc.[Key] 
WHERE  q.CoverageType='Flood' AND q.Finished IS NULL 
AND (qc.FirstName IS NULL OR qc.LastName IS NULL OR qc.Street IS NULL OR qc.City IS NULL
 OR qc.ZipCode IS NULL OR qc.BirthDate IS NULL OR qc.Gender IS NULL OR qc.MaritalStatus IS NULL OR qc.Email IS NULL
  OR qc.PhoneNumber IS NULL)";
                var sqlQuoteStep = @"SELECT qc.* FROM [dbo].[QuoteContact] as qc Inner Join [dbo].[Quote] as q On q.[Key] = qc.[Key] 
WHERE  q.CoverageType='Flood' AND q.Finished IS NULL 
 AND (qc.HowToContact IS NULL OR qc.HowToContact='Email')";
                var sqlFlood = @"SELECT f.* FROM [dbo].[QuoteFlood] as f  Inner Join [dbo].[Quote] as q On q.[Key] = f.[Key] 
WHERE q.CoverageType='Flood' AND q.Finished IS NULL
AND (f.[Address] IS NULL OR f.ZipCode IS NULL OR f.City IS NULL OR f.[State] IS NULL  
OR f.ConstructionDate IS NULL OR f.OccupiedType IS NULL OR f.TotalLivingArea IS NULL OR f.StructureValue IS NULL)";


                AFIReportView afi = new AFIReportView();
                afi.quotes = db.Query<Quote>(sqlQuote);
                afi.quotesCount = db.Query<Quote>(sqlQuote).Count();
                afi.contacts = db.Query<QuoteContact>(sqlContact);
                afi.contactsCount = db.Query<QuoteContact>(sqlContact).Count();
                afi.quotesStep = db.Query<QuoteContact>(sqlQuoteStep);
                afi.quoteStepCount = db.Query<QuoteContact>(sqlQuoteStep).Count();
                afi.floods = db.Query<QuoteFlood>(sqlFlood);
                afi.FloodCount = db.Query<QuoteFlood>(sqlFlood).Count();
                return afi;
            }
        }

        public AFIReportView GetFloodDetailsByKey(long key)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {

                var sqlQuote = @"select top 1 * from dbo.[Quote] where [Key] = " + key;
                var sqlQuoteContact = @"select top 1 * from dbo.[QuoteContact] where [Key] = " + key;
                var sqlQuoteFlood = @"select top 1 * from dbo.[QuoteFlood] where [Key]  =" + key;

                AFIReportView afi = new AFIReportView();
                afi.Quote = db.QueryFirstOrDefault<Quote>(sqlQuote);
                afi.QuoteContact = db.QueryFirstOrDefault<QuoteContact>(sqlQuoteContact);
                afi.QuoteFlood = db.QueryFirstOrDefault<QuoteFlood>(sqlQuoteFlood);

                return afi;
            }

        }

        public IEnumerable<ReportView> GetAllHomeReport()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "Select q.[Key], q.CoverageType, q.Eligibility ,q.Started, qc.ServiceBranch, qc.ServiceRank, qc.ServiceStatus, qc.FirstName, qc.LastName, qc.Email, qc.PhoneNumber from [dbo].[Quote] q inner Join [dbo].[QuoteContact] qc ON qc.[key]=q.[Key] WHERE q.CoverageType='Homeowners'";
                return db.Query<ReportView>(sql);
            }
        }
        public AFIReportView GetStepWiseHomeReport()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sqlQuote = @"SELECT * FROM [dbo].[Quote]  WHERE CoverageType='Homeowners' AND Finished IS NULL";
                var sqlContact = @"SELECT qc.* FROM [dbo].[QuoteContact] as qc Inner Join [dbo].[Quote] as q On q.[Key] = qc.[Key] 
WHERE  q.CoverageType='Homeowners' AND q.Finished IS NULL 
AND (qc.FirstName IS NULL OR qc.LastName IS NULL OR qc.Street IS NULL OR qc.City IS NULL
 OR qc.ZipCode IS NULL OR qc.BirthDate IS NULL OR qc.Gender IS NULL OR qc.MaritalStatus IS NULL OR qc.Email IS NULL
  OR qc.PhoneNumber IS NULL)";

                var sqlHomeowner = @"SELECT qh.* FROM [dbo].[QuoteHomeowner] as qh Inner Join [dbo].[Quote] as q On q.[Key] = qh.[Key] 
WHERE  q.CoverageType='Homeowners' AND q.Finished IS NULL 
AND (qh.PurchaseDate IS NULL OR qh.NumberOfOccupants IS NULL OR qh.[Address] IS NULL OR qh.City IS NULL
 OR qh.Zip IS NULL OR qh.[State] IS NULL OR qh.Country IS NULL OR qh.[Type] IS NULL OR qh.Style IS NULL
  OR qh.NumberOfStories IS NULL OR qh.NumberOfUnits IS NULL OR qh.TotalLivingArea IS NULL OR qh.Condition IS NULL
  OR qh.ConstructionType IS NULL OR qh.ConstructionType IS NULL OR qh.YearBuilt IS NULL OR qh.QuoteAmount IS NULL)";


                AFIReportView afi = new AFIReportView();
                afi.quotes = db.Query<Quote>(sqlQuote);
                afi.quotesCount = db.Query<Quote>(sqlQuote).Count();
                afi.contacts = db.Query<QuoteContact>(sqlContact);
                afi.contactsCount = db.Query<QuoteContact>(sqlContact).Count();
                afi.homeowners = db.Query<QuoteHomeowner>(sqlHomeowner);
                afi.homeownerCount = db.Query<QuoteHomeowner>(sqlHomeowner).Count();

                return afi;
            }
        }
        public AFIReportView GetHomeDetailsByKey(long key)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sqlQuote = @"select top 1 * from dbo.[Quote] where [Key] = " + key;
                var sqlQuoteContact = @"select top 1 * from dbo.[QuoteContact] where [Key] = " + key;
                var sqlQuoteHomeowner = @"select top 1 * from dbo.[QuoteHomeowner] where [Key]  =" + key;
                var sqlQuoteHomeownerLoss = @"select top 1 * from QuoteHomeownerLoss where [QuoteKey] =" + key;

                AFIReportView afi = new AFIReportView();
                afi.Quote = db.QueryFirstOrDefault<Quote>(sqlQuote);
                afi.QuoteContact = db.QueryFirstOrDefault<QuoteContact>(sqlQuoteContact);
                afi.QuoteHomeowner = db.QueryFirstOrDefault<QuoteHomeowner>(sqlQuoteHomeowner);
                afi.QuoteHomeownerLoss = db.QueryFirstOrDefault<QuoteHomeownerLoss>(sqlQuoteHomeownerLoss);
                return afi;
            }
        }
        public IEnumerable<ReportView> GetAllMotorcycleReport()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "Select q.[Key], q.CoverageType, q.Eligibility ,q.Started, qc.ServiceBranch, qc.ServiceRank, qc.ServiceStatus, qc.FirstName, qc.LastName, qc.Email, qc.PhoneNumber from [dbo].[Quote] q inner Join [dbo].[QuoteContact] qc ON qc.[key]=q.[Key] WHERE q.CoverageType='Motorcycle'";
                return db.Query<ReportView>(sql);
            }
        }
        public AFIReportView GetStepWiseMotorcycleReport()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sqlQuote = @"SELECT * FROM [dbo].[Quote]  WHERE CoverageType='Motorcycle' AND Finished IS NULL";
                var sqlContact = @"SELECT qc.* FROM [dbo].[QuoteContact] as qc Inner Join [dbo].[Quote] as q On q.[Key] = qc.[Key] WHERE q.CoverageType='Motorcycle' AND q.Finished IS NULL AND (qc.HowToContact IS NULL OR qc.HowToContact='Email')";
                var sqlDrivers = @"SELECT qad.* FROM [dbo].[QuoteAutoDriver] as qad Inner Join [dbo].[Quote] as q On q.[Key] = qad.[QuoteKey] 
WHERE q.CoverageType='Motorcycle' AND q.Finished IS NULL
AND (qad.FirstName IS NULL OR qad.LastName IS NULL OR qad.BirthDate IS NULL OR qad.AgeLicensed IS NULL
 OR qad.MaritalStatus IS NULL  OR qad.Gender IS NULL) ";
                var sqlVehicles = @"SELECT qav.* FROM [dbo].[QuoteMotorcycleVehicle] as qav Inner Join [dbo].[Quote] as q On q.[Key] = qav.[QuoteKey]
WHERE q.CoverageType='Motorcycle' AND q.Finished IS NULL 
AND (qav.Make IS NULL OR qav.Model IS NULL OR qav.[Year] IS NULL 
OR qav.VehicleType IS NULL OR qav.PurchaseYear IS NULL OR qav.[Value] IS NULL  OR qav.CCSize IS NULL)";
                var sqlIncidents = @"SELECT qai.* FROM [dbo].[QuoteAutoIncident] as qai Inner Join [dbo].[Quote] as q On q.[Key] = qai.[QuoteKey] 
WHERE q.CoverageType='Motorcycle' AND q.Finished IS NULL
AND (qai.Incident IS NULL OR qai.[Date] IS NULL)";
                var sqlAutos = @"SELECT qa.* FROM [dbo].[QuoteAuto] as qa Inner Join [dbo].[Quote] as q On q.[Key] = qa.[Key] 
WHERE q.CoverageType='Motorcycle' AND q.Finished IS NULL 
AND (qa.BodilyInjury IS NULL OR qa.PropertyDamage IS NULL OR qa.MedicalCoverage IS NULL
 OR qa.UninsuredBodilyInjury IS NULL  OR qa.PersonalInjury IS NULL)";


                AFIReportView afi = new AFIReportView();
                afi.quotes = db.Query<Quote>(sqlQuote);
                afi.quotesCount = db.Query<Quote>(sqlQuote).Count();
                afi.contacts = db.Query<QuoteContact>(sqlContact);
                afi.contactsCount = db.Query<QuoteContact>(sqlContact).Count();
                afi.autos = db.Query<QuoteAuto>(sqlAutos);
                afi.autosCount = db.Query<QuoteAuto>(sqlAutos).Count();
                afi.vehicles = db.Query<QuoteAutoVehicle>(sqlVehicles);
                afi.vehiclesCount = db.Query<QuoteAutoVehicle>(sqlVehicles).Count();
                afi.drivers = db.Query<QuoteAutoDriver>(sqlDrivers);
                afi.driversCount = db.Query<QuoteAutoDriver>(sqlDrivers).Count();
                afi.incidents = db.Query<QuoteAutoIncident>(sqlIncidents);
                afi.incidentsCount = db.Query<QuoteAutoIncident>(sqlIncidents).Count();
                return afi;
            }
        }

        public IEnumerable<VoteReport> GetDemoVoteCountReportForResult()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"SELECT 
    c.[Name] AS CandidateName,
    SUM(CASE WHEN b.Ballot='For' THEN 1 ELSE 0 END) AS VoteFor, 
    SUM(CASE WHEN b.Ballot='Against' THEN 1 ELSE 0 END) AS VoteAgainst 
FROM  
    [dbo].[Demo_MemberCandidateBallot] b 
INNER JOIN 
    [ProxyVote].[Candidate]  c ON b.CandidateId = c.CandidateId
GROUP BY 
    c.[Name];";
                return db.Query<VoteReport>(sql);
            }
        }

        public IEnumerable<VoteCandidate> GetVoteCandidateList(string periodId)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"SELECT [CandidateId]
      ,[VotingPeriodId]
      ,[Name]
      ,[Content] FROM [ProxyVote].[Candidate]";

                // Conditionally add the WHERE clause for periodId
                if (!string.IsNullOrEmpty(periodId))
                {
                    sql += " WHERE VotingPeriodId = @PeriodId";
                    return db.Query<VoteCandidate>(sql, new { PeriodId = periodId });
                }
                else
                {
                    return db.Query<VoteCandidate>(sql);
                }
            }
        }
        public IEnumerable<VotingPeriod> GetAllVotingPeriod()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"SELECT *,
       CASE
           WHEN GETDATE() >= [Start] AND CAST(GETDATE() AS DATE) <= CAST([End] AS DATE) AND [End] >= CAST(GETDATE() AS DATE) THEN 'true'
           ELSE 'false'
       END AS IsActive
FROM [ProxyVote].[VotingPeriod]
ORDER BY VotingPeriodId DESC;";
                return db.Query<VotingPeriod>(sql);
            }
        }

        public int InsertMemberVote(ProxyVoteMember entity)
        {
            //if(!string.IsNullOrEmpty(entity.EmailAddress))
            //{
            //    entity.IsEmailUpdated = true;
            //}
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                       
                        // Check if the record already exists
                        var existingRecord = db.QueryFirstOrDefault<ProxyVoteMember>("SELECT Top 1 * FROM [ProxyVote].[Member] WHERE MemberNumber = @MemberNumber Order by 1 DESC", new
                        {
                            entity.MemberNumber
                        }, transaction);

                        if (existingRecord !=null)
                        {
                            Log.Error($"{nameof(QuoteRepository)}: Existing Member Found >."+ existingRecord.MemberNumber,  this);
                            if (existingRecord.VotingPeriodId==entity.VotingPeriodId)
                            {
                                Log.Error($"{nameof(QuoteRepository)}: Existing Member VotingPeriodId Found >." + existingRecord.MemberNumber, this);
                                return -1;
                            }
                            else
                            {
                                entity.PIN = existingRecord.PIN;
                            }
                            
                            // Record already exists, return its MemberId
                           
                        }

                        var sql = @"
                    INSERT INTO [ProxyVote].[Member] 
                    (
                        [MemberNumber],
                        [PIN],
                        [VotingPeriodId],
                        [Enabled],
                        [EmailAddress],
                        [FullName],
                        [ResidentialOccupied],
                        [ResidentialDwelling],
                        [Renters],
                        [Flood],
                        [Life],
                        [PersonalLiabilityRenters],
                        [PersonalLiabilityCatastrophy],
                        [Auto],
                        [RV],
                        [Watercraft],
                        [Motorcycle],
                        [Supplemental],
                        [AnnualReport],
                        [StatutoryFinancialStatements],
                        [MobileHome],
                        [PetHealth],
                        [Business],
                        [LongTermCare],
                        [MailFinancials],
                        [EmailFinancials],
                        [IsEmailUpdated]
 ,[Prefix]
           ,[Salutation]
           ,[InsuredFirstName]
           ,[InsuredLastName]
           ,[ClientType]
           ,[ServiceStatus]
           ,[MailingAddressLine1]
           ,[MailingAddressLine2]
           ,[MailingCityName]
           ,[MailingCountyName]
           ,[MailingStateAbbreviation]
           ,[MailingZip]
           ,[MailingCountry]
           ,[MembershipDate]
           ,[YearsAsMember]
           ,[Gender]
           ,[Deceased]
            ,[MarketingCode]
           ,[ProperFirstName]
           ,[MiddleName]
           ,[Suffix]
           ,[CreateDate]
                    )
                    VALUES 
                    (
                        @MemberNumber,
                        @PIN,
                        @VotingPeriodId,
                        @Enabled,
                        @EmailAddress,
                        @FullName,
                        @ResidentialOccupied,
                        @ResidentialDwelling,
                        @Renters,
                        @Flood,
                        @Life,
                        @PersonalLiabilityRenters,
                        @PersonalLiabilityCatastrophy,
                        @Auto,
                        @RV,
                        @Watercraft,
                        @Motorcycle,
                        @Supplemental,
                        @AnnualReport,
                        @StatutoryFinancialStatements,
                        @MobileHome,
                        @PetHealth,
                        @Business,
                        @LongTermCare,
                        @MailFinancials,
                        @EmailFinancials,
                        @IsEmailUpdated,
 @Prefix,
           @Salutation,
           @InsuredFirstName,
           @InsuredLastName,
           @ClientType,
           @ServiceStatus,
           @MailingAddressLine1,
           @MailingAddressLine2,
           @MailingCityName,
           @MailingCountyName,
           @MailingStateAbbreviation,
           @MailingZip,
           @MailingCountry,
           @MembershipDate,
           @YearsAsMember,
           @Gender,
           @Deceased,
           @MarketingCode,
           @ProperFirstName,
           @MiddleName,
           @Suffix,
           @CreateDate
                    );
                ";

                        // Execute the SQL query using Dapper
                        var newId = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: create Member " + sw.Elapsed, "stopwatch");

                        return newId;
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteRepository)}: Error while attempting to insert Member.", ex, this);
                        transaction.Rollback();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: create Member" + sw.Elapsed, "stopwatch");
                        return 0;
                    }
                }
            }
        }

       

        public VotingPeriod GetVotingPeriodById(int votingPeriodId)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"SELECT * FROM [ProxyVote].[VotingPeriod] WHERE VotingPeriodId = @VotingPeriodId";
                return db.QueryFirstOrDefault<VotingPeriod>(sql, new { VotingPeriodId = votingPeriodId });
            }
        }


        public int CreateVotingPeriodData(VotingPeriod votingPeriod)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"
                    IF NOT EXISTS (SELECT 1 FROM [ProxyVote].[VotingPeriod] WHERE Title = @Title)
                    BEGIN
                        INSERT INTO [ProxyVote].[VotingPeriod] (Start, [End], Title, [Content]) 
                        VALUES (@Start, @End, @Title, @Content);
                        SELECT SCOPE_IDENTITY();
                    END
                    ELSE
                    BEGIN
                        SELECT -1;
                    END";

                        var id = db.QueryFirstOrDefault<int>(sql, votingPeriod, transaction);
                        if (id == -1)
                        {
                            return -1;
                        }

                        transaction.Commit();
                        return id;
                    }
                    catch (System.Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }



        public int UpdateVotingPeriodData(VotingPeriod votingPeriod)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"UPDATE [ProxyVote].[VotingPeriod] 
                            SET Start = @Start, [End] = @End, Title = @Title, [Content] = @Content 
                            WHERE VotingPeriodId = @VotingPeriodId";

                        var rowsAffected = db.Execute(sql, votingPeriod, transaction);
                        transaction.Commit();
                        return rowsAffected;
                    }
                    catch (System.Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        public int DeleteVotingPeriodData(int VotingPeriodId)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = "DELETE FROM [ProxyVote].[VotingPeriod] WHERE VotingPeriodId = @VotingPeriodId";

                        var rowsAffected = db.Execute(sql, new { VotingPeriodId }, transaction);
                        transaction.Commit();
                        return rowsAffected;
                    }
                    catch (System.Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
        public IEnumerable<VoteCandidate> GetAllCandidateData()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"SELECT c.*,
                            v.Title AS VotingPeriodName,
                            CASE
           WHEN GETDATE() >= [Start] AND CAST(GETDATE() AS DATE) <= CAST([End] AS DATE) AND [End] >= CAST(GETDATE() AS DATE) THEN 'true'
           ELSE 'false'
                            END AS IsActive
                    FROM [ProxyVote].[Candidate] c
                    INNER JOIN[ProxyVote].[VotingPeriod] v ON v.VotingPeriodId = c.VotingPeriodId
                    ORDER BY c.VotingPeriodId DESC";

                return db.Query<VoteCandidate>(sql);
            }
        }
        public VoteCandidate GetCandidateById(int candidateId)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"SELECT * FROM [ProxyVote].[Candidate] WHERE CandidateId = @CandidateId";
                return db.QueryFirstOrDefault<VoteCandidate>(sql, new { CandidateId = candidateId });
            }
        }
        public int GetMemberVoteCountByMemberIdAndVotePeriodId(int memberId, int votingPeriodId)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"SELECT COUNT(*) 
                    FROM [ProxyVote].[MemberCandidateBallot] AS mcb 
                    INNER JOIN [ProxyVote].[Candidate] AS c ON mcb.CandidateId = c.CandidateId 
                    WHERE mcb.MemberId = @MemberId AND mcb.VotingPeriodId = @VotingPeriodId";
                return db.QueryFirstOrDefault<int>(sql, new { MemberId = memberId, VotingPeriodId = votingPeriodId });
            }
        }


        public int CreateCandidateData(VoteCandidate voteCandidate)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {

                        var sql = @"
                        IF NOT EXISTS (SELECT 1 FROM [ProxyVote].[Candidate] WHERE Name = @Name AND VotingPeriodId=@VotingPeriodId)
                        BEGIN
                          INSERT INTO [ProxyVote].[Candidate] (VotingPeriodId , Name, ImagePath, [Content]) 
                            VALUES (@VotingPeriodId, @Name, @ImagePath, @Content);
                            SELECT SCOPE_IDENTITY();
                        END
                        ELSE
                        BEGIN
                            SELECT -1;
                        END";

                        var id = db.QueryFirstOrDefault<int>(sql, voteCandidate, transaction);
                        transaction.Commit();
                        return id;
                    }
                    catch (System.Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        public int UpdateCandidateData(VoteCandidate voteCandidate)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {                    

                        var sql = @"UPDATE [ProxyVote].[Candidate]
                            SET VotingPeriodId = @VotingPeriodId, Name = @Name, ";

                        if (voteCandidate.ImagePath != null && voteCandidate.ImagePath != string.Empty)
                        {
                            sql += " ImagePath = @ImagePath, ";
                        }

                        sql += "  [Content] = @Content WHERE CandidateId = @CandidateId";

                        var rowsAffected = db.Execute(sql, voteCandidate, transaction);
                        transaction.Commit();
                        return rowsAffected;
                    }
                    catch (System.Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
        public int DeleteCandidateData(int CandidateId)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = "DELETE FROM [ProxyVote].[Candidate] WHERE CandidateId = @CandidateId";
                        var rowsAffected = db.Execute(sql, new { CandidateId }, transaction);
                        transaction.Commit();
                        return rowsAffected;

                    }
                    catch (System.Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
        //public IEnumerable<VoteMember> GetAllVotingMemberData(int page, int pageSize, int VotingPeriodId, string IsEmail)
        //{
        //    int ofsetItem = (page - 1) * pageSize;

        //    using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
        //    {
        //        // var sql = " select *, COUNT(*) OVER() AS TotalCount from [ProxyVote].[Member] WHERE 1 = 1";
        //        // var sql = " select m.MemberId, m.PIN,m.MemberNumber,m.FullName,m.EmailAddress,m.VotingPeriodId,v.Title AS VotingPeriod, COUNT(*) OVER() AS TotalCount from \r\n[ProxyVote].[Member] AS m INNER JOIN [ProxyVote].[VotingPeriod] v ON v.VotingPeriodId=m.VotingPeriodId  WHERE 1 = 1";

        //        var sql = @"SELECT m.MemberId, 
        //                   m.PIN,
        //                   m.MemberNumber,
        //                   m.FullName,
        //                   m.EmailAddress,
        //                   m.VotingPeriodId,
        //                   v.Title AS VotingPeriod,
        //                   COUNT(*) OVER() AS TotalCount,
        //                   CASE
        //                        WHEN GETDATE() BETWEEN v.[Start] AND v.[End] THEN 'true'
        //                        ELSE 'false'
        //                   END AS IsActive
        //            FROM[ProxyVote].[Member] AS m
        //            INNER JOIN[ProxyVote].[VotingPeriod] v ON v.VotingPeriodId = m.VotingPeriodId
        //            WHERE 1 = 1 ";

        //        if (VotingPeriodId > 0)
        //        {
        //            sql += " AND m.VotingPeriodId = @VotingPeriodId";
        //        }


        //        sql += " ORDER BY m.MemberId OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY ; ";

        //        var parameters = new DynamicParameters();
        //        parameters.Add("@Offset", ofsetItem);
        //        parameters.Add("@PageSize", pageSize);

        //        if (VotingPeriodId > 0)
        //        {
        //            parameters.Add("@VotingPeriodId", VotingPeriodId);
        //        }

        //        return db.Query<VoteMember>(sql, parameters);

        //    }
        //}

        public IEnumerable<VoteMember> GetAllVotingMemberData(int page, int pageSize, int VotingPeriodId, string IsEmail)
        {
            int ofsetItem = (page - 1) * pageSize;

            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"SELECT m.MemberId, 
                           m.PIN,
                           m.MemberNumber,
                           m.FullName,
                           m.EmailAddress,
                           m.VotingPeriodId,
                           v.Title AS VotingPeriod,
                           COUNT(*) OVER() AS TotalCount,
                           CASE
           WHEN GETDATE() >= [Start] AND CAST(GETDATE() AS DATE) <= CAST([End] AS DATE) AND [End] >= CAST(GETDATE() AS DATE) THEN 'true'
           ELSE 'false'
                           END AS IsActive
                    FROM[ProxyVote].[Member] AS m
                    INNER JOIN[ProxyVote].[VotingPeriod] v ON v.VotingPeriodId = m.VotingPeriodId
                    WHERE 1 = 1 ";

                if (VotingPeriodId == 0)
                {
                    // Condition 1: Get the latest voting period data
                    sql += @"AND v.VotingPeriodId = (SELECT TOP 1 VotingPeriodId FROM ProxyVote.VotingPeriod ORDER BY VotingPeriodId DESC) ";

                    // Check if the latest voting period has no member data
                    if (!db.Query("SELECT TOP 1 1 FROM ProxyVote.Member WHERE VotingPeriodId = (SELECT TOP 1 VotingPeriodId FROM ProxyVote.VotingPeriod ORDER BY VotingPeriodId DESC)").Any())
                    {
                        // Condition 2: Get all member data if the latest voting period has no members
                        sql = @"SELECT m.MemberId, 
                                m.PIN,
                                m.MemberNumber,
                                m.FullName,
                                m.EmailAddress,
                                m.VotingPeriodId,
                                v.Title AS VotingPeriod,
                                COUNT(*) OVER() AS TotalCount,
                               CASE
           WHEN GETDATE() >= [Start] AND CAST(GETDATE() AS DATE) <= CAST([End] AS DATE) AND [End] >= CAST(GETDATE() AS DATE) THEN 'true'
           ELSE 'false'
                                END AS IsActive
                        FROM[ProxyVote].[Member] AS m
                        INNER JOIN[ProxyVote].[VotingPeriod] v ON v.VotingPeriodId = m.VotingPeriodId ";
                    }
                }
                else if (VotingPeriodId == 99999999)
                {
                    // Condition 4: Get All member data 
                    sql += " ";
                }
                else if (VotingPeriodId > 0)
                {
                    // Condition 3: Get data for a specific VotingPeriodId
                    sql += " AND v.VotingPeriodId = @VotingPeriodId";
                }

                // Additional condition based on IsEmail parameter
                if (IsEmail.ToLower() == "all")
                {
                    // Get all email or non-email members
                    // No additional condition needed
                }
                else if (IsEmail.ToLower() == "hasemail")
                {
                    // Get members where email field has email
                    sql += " AND m.EmailAddress IS NOT NULL AND m.EmailAddress != ''";
                }
                else if (IsEmail.ToLower() == "emptyemail")
                {
                    // Get members where email field is null or empty
                    sql += " AND (m.EmailAddress IS NULL OR m.EmailAddress = '')";
                }
                // Check if pageSize is 0 to retrieve all data
                if (pageSize == 0)
                {
                    sql = sql.Replace("OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY", "");
                }
                else
                {
                    sql += " ORDER BY MemberId OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY ; ";
                }

                var parameters = new DynamicParameters();
                parameters.Add("@Offset", ofsetItem);
                parameters.Add("@PageSize", pageSize);

                if (VotingPeriodId > 0)
                {
                    parameters.Add("@VotingPeriodId", VotingPeriodId);
                }

                return db.Query<VoteMember>(sql, parameters);
            }
        }

        public IEnumerable<VoteReport> GetVoteCountReportForResult(string voatingPeriodId = "")
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                if (string.IsNullOrEmpty(voatingPeriodId))
                {
                    voatingPeriodId = "(Select top 1 VotingPeriodId from [ProxyVote].[VotingPeriod] Order by 1 desc)";
                }
                var sql = @"SELECT 
                            c.[Name] AS CandidateName,
                            SUM(CASE WHEN b.Ballot='For' THEN 1 ELSE 0 END) AS VoteFor, 
                            SUM(CASE WHEN b.Ballot='Against' THEN 1 ELSE 0 END) AS VoteAgainst ,
SUM(CASE WHEN b.Ballot='Abstain' THEN 1 ELSE 0 END) AS VoteAbstain ,
COUNT(*) AS TotalVotes
                        FROM  
                            [ProxyVote].[MemberCandidateBallot] b 
                        INNER JOIN 
                            [ProxyVote].[Candidate]  c ON b.CandidateId = c.CandidateId
                        Where b.VotingPeriodId= " + voatingPeriodId + "GROUP BY c.[Name];";

                return db.Query<VoteReport>(sql);
            }
        }
        public int UpdateMemberData(VoteMember voteMember)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"UPDATE [ProxyVote].[Member]
                            SET MemberNumber = @MemberNumber, 
                                PIN = @PIN, 
                                VotingPeriodId = @VotingPeriodId, 
                                EmailAddress = @EmailAddress, 
                                FullName = @FullName,
                                IsEmailUpdated = @IsEmailUpdated
                            WHERE MemberId = @MemberId";

                        // Check if the email address is updated, then set IsEmailUpdated to true
                        voteMember.IsEmailUpdated = !string.IsNullOrEmpty(voteMember.EmailAddress);

                        var rowsAffected = db.Execute(sql, voteMember, transaction);
                        transaction.Commit();
                        return rowsAffected;
                    }
                    catch (System.Exception ex)
                    {
                        transaction.Rollback();
                        // Log or handle the exception as needed
                        // You might want to re-throw the exception here depending on your requirements
                        return 0; // Return 0 to indicate failure
                    }
                }
            }
        }

        public int DeleteMemberData(int MemberId)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = "DELETE FROM [ProxyVote].[Member] WHERE MemberId = @MemberId";
                        var rowsAffected = db.Execute(sql, new { MemberId }, transaction);
                        transaction.Commit();
                        return rowsAffected;

                    }
                    catch (System.Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        public object GetTotalVoteCountDetailsForResult(string voatingPeriodId = "")
        {
            if (string.IsNullOrEmpty(voatingPeriodId))
            {
                voatingPeriodId = "(SELECT TOP 1 VotingPeriodId FROM [ProxyVote].VotingPeriod ORDER BY 1 DESC)";
            }

            var sql = @"
                SELECT 
                    (SELECT COUNT(*) FROM [ProxyVote].[Member] WHERE Enabled = 0 AND VotingPeriodId = " + voatingPeriodId + @") AS TotalEnabledMembers,
                    (SELECT COUNT(*) FROM [ProxyVote].[Member] WHERE Enabled = 1 AND VotingPeriodId = " + voatingPeriodId + @") AS TotalDisabledMembers,
                    (SELECT COUNT(*) FROM [ProxyVote].[Member] WHERE VotingPeriodId = " + voatingPeriodId + @") AS TotalMembersInVotingPeriod,
                    vp.VotingPeriodId,
                    vp.Title
                FROM 
                    [ProxyVote].votingPeriod vp
                WHERE 
                    vp.VotingPeriodId = " + voatingPeriodId + ";";

            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                return db.Query(sql).FirstOrDefault();
            }
        }
        public bool GetCandidateVoteBallotStatus(string votingPeriodId, string memberId)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {

                var sql = $@"SELECT COUNT(*) 
                        FROM [ProxyVote].[MemberCandidateBallot] AS mcb 
                        INNER JOIN [ProxyVote].[Candidate] AS c ON mcb.CandidateId = c.CandidateId 
                        WHERE mcb.MemberId = '{memberId}' AND mcb.VotingPeriodId = '{votingPeriodId}'";

                var rowCount = db.QueryFirstOrDefault<int>(sql);

                return rowCount > 0;


            }
        }
        public string GetMemberEmailByMemberNumberAndPIN(string MemberNumber, string PIN)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"SELECT TOP 1 EmailAddress FROM [ProxyVote].[Member] WHERE MemberNumber = @MemberNumber AND PIN = @PIN AND VotingPeriodId = @VotingPeriodId";
                var query_Vp = "SELECT TOP 1 VotingPeriodId  FROM ProxyVote.VotingPeriod WHERE [Start] <= GETDATE() AND [End] >=  DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 1) - 1 ORDER BY VotingPeriodId DESC";

                var VotingPeriodId = db.QueryFirstOrDefault<string>(query_Vp);

                var parameters = new { MemberNumber, PIN, VotingPeriodId };
                var email = db.QueryFirstOrDefault<string>(sql, parameters);
                if (string.IsNullOrEmpty(email))
                {
                    return null;
                }

                return email;
            }
        }
        public object GetMemberInfoByMemberNumber(string MemberNumber)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"
            SELECT TOP 1 EmailAddress, PIN, FullName
            FROM [ProxyVote].[Member]
            WHERE MemberNumber = @MemberNumber
            AND VotingPeriodId = (
                SELECT TOP 1 VotingPeriodId
                FROM ProxyVote.VotingPeriod
                WHERE [Start] <= CAST(GETDATE() AS DATE)
                AND [End] >= CAST(GETDATE() AS DATE)
                ORDER BY VotingPeriodId DESC
            )
            ORDER BY MemberId DESC;";

                var parameters = new { MemberNumber };
                return db.Query(sql, parameters).FirstOrDefault();
            }
        }

        public IEnumerable<VoteCandidate> GetAllLatestVotingPeriodCandidateData()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"select * FROM 
[ProxyVote].[Candidate] where  VotingPeriodId = (SELECT TOP 1 VotingPeriodId FROM ProxyVote.VotingPeriod ORDER BY VotingPeriodId DESC)";

                return db.Query<VoteCandidate>(sql);
            }
        }
        public int InsertVotingMember(VoteMember voteMember)
        {

            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    //try
                    //{

                    var sql = @"IF NOT EXISTS (SELECT 1 FROM MemberId FROM [ProxyVote].[Member] WHERE MemberNumber = @MemberNumber AND PIN = @PIN AND VotingPeriodId = @VotingPeriodId)
                        BEGIN
                            INSERT INTO [ProxyVote].[Member] 
                            (
                                [MemberNumber],
                                [PIN],
                                [VotingPeriodId],
                                [EmailAddress],
                                [FullName]
                            )
                            VALUES 
                            (
                                @MemberNumber,
                                @PIN,
                                @VotingPeriodId,
                                @EmailAddress,
                                @FullName
                            );
                            SELECT SCOPE_IDENTITY();
                        END
                        ELSE
                        BEGIN
                            SELECT -1;
                        END";


                    var id = db.QueryFirstOrDefault<int>(sql, voteMember, transaction);
                    transaction.Commit();
                    return id;

                    //}
                    //catch (System.Exception ex)
                    //{
                    //    transaction.Rollback();
                    //    return 0;
                    //}
                }
            }
        }
        public IEnumerable<VotingMemberCSV> GetAllFilterVotingMemberData(int page, int pageSize, int VotingPeriodId, string IsEmail)
        {
            int ofsetItem = (page - 1) * pageSize;

            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"SELECT m.MemberId, 
                           m.MemberNumber,
                           m.PIN,
                           m.MarketingCode,
                           m.Salutation,
                           m.Prefix,
                           m.ProperFirstName,
                           m.InsuredFirstName,
                           m.MiddleName,
                           m.InsuredLastName,
                           m.Suffix,
                           m.ServiceStatus,
                           m.MailingAddressLine1,
                           m.MailingAddressLine2,
                           m.MailingCityName,
                           m.MailingStateAbbreviation,
                           m.MailingZip,
                           m.MailingCountry,
                           m.VotingPeriodId,
                           m.EmailAddress,
                           m.ClientType,
                           m.MailingCountyName,
                           m.MembershipDate,
                           m.YearsAsMember,
                           m.Gender,
                           v.Title AS VotingPeriod,
                           COUNT(*) OVER() AS TotalCount,
                           CASE
           WHEN GETDATE() >= [Start] AND CAST(GETDATE() AS DATE) <= CAST([End] AS DATE) AND [End] >= CAST(GETDATE() AS DATE) THEN 'true'
           ELSE 'false'
                           END AS IsActive
                    FROM[ProxyVote].[Member] AS m
                    INNER JOIN[ProxyVote].[VotingPeriod] v ON v.VotingPeriodId = m.VotingPeriodId
                    WHERE 1 = 1 ";

                if (VotingPeriodId == 0)
                {
                    // Condition 1: Get the latest voting period data
                    sql += @"AND v.VotingPeriodId = (SELECT TOP 1 VotingPeriodId FROM ProxyVote.VotingPeriod ORDER BY VotingPeriodId DESC) ";

                    // Check if the latest voting period has no member data
                    if (!db.Query("SELECT TOP 1 1 FROM ProxyVote.Member WHERE VotingPeriodId = (SELECT TOP 1 VotingPeriodId FROM ProxyVote.VotingPeriod ORDER BY VotingPeriodId DESC)").Any())
                    {
                        // Condition 2: Get all member data if the latest voting period has no members
                        sql = @"SELECT m.MemberId, 
                                m.MemberNumber,
                                m.PIN,
                                m.MarketingCode,
                                m.Salutation,
                                m.Prefix,
                                m.ProperFirstName,
                                m.InsuredFirstName,
                                m.MiddleName,
                                m.InsuredLastName,
                                m.Suffix,
                                m.ServiceStatus,
                                m.MailingAddressLine1,
                                m.MailingAddressLine2,
                                m.MailingCityName,
                                m.MailingStateAbbreviation,
                                m.MailingZip,
                                m.MailingCountry,
                                m.VotingPeriodId,
                                m.EmailAddress,
                                m.ClientType,
                                m.MailingCountyName,
                                m.MembershipDate,
                                m.YearsAsMember,
                                m.Gender,
                                v.Title AS VotingPeriod,
                                COUNT(*) OVER() AS TotalCount,
                               CASE
           WHEN GETDATE() >= [Start] AND CAST(GETDATE() AS DATE) <= CAST([End] AS DATE) AND [End] >= CAST(GETDATE() AS DATE) THEN 'true'
           ELSE 'false'
                                END AS IsActive
                        FROM[ProxyVote].[Member] AS m
                        INNER JOIN[ProxyVote].[VotingPeriod] v ON v.VotingPeriodId = m.VotingPeriodId ";
                    }
                }
                else if (VotingPeriodId == 99999999)
                {
                    // Condition 4: Get All member data 
                    sql += " ";
                }
                else if (VotingPeriodId > 0)
                {
                    // Condition 3: Get data for a specific VotingPeriodId
                    sql += " AND v.VotingPeriodId = @VotingPeriodId";
                }

                // Additional condition based on IsEmail parameter
                if (IsEmail.ToLower() == "all")
                {
                    // Get all email or non-email members
                    // No additional condition needed
                }
                else if (IsEmail.ToLower() == "hasemail")
                {
                    // Get members where email field has email
                    sql += " AND m.EmailAddress IS NOT NULL AND m.EmailAddress != ''";
                }
                else if (IsEmail.ToLower() == "emptyemail")
                {
                    // Get members where email field is null or empty
                    sql += " AND (m.EmailAddress IS NULL OR m.EmailAddress = '')";
                }
                // Check if pageSize is 0 to retrieve all data
                if (pageSize == 0)
                {
                    sql = sql.Replace("OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY", "");
                }
                else
                {
                    sql += " ORDER BY MemberId OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY ; ";
                }

                var parameters = new DynamicParameters();
                parameters.Add("@Offset", ofsetItem);
                parameters.Add("@PageSize", pageSize);

                if (VotingPeriodId > 0)
                {
                    parameters.Add("@VotingPeriodId", VotingPeriodId);
                }

                return db.Query<VotingMemberCSV>(sql, parameters);
            }
        }

        // Temp for Sync Moosend link AFIDB [dbo].[AFIVoteMemberMoosend]
        public int SubmitMoosendMemberVote(ProxyVoteMemberMoosend entity)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {

                        //  Check if the record already exists
                        var existingRecord = db.QueryFirstOrDefault<ProxyVoteMemberMoosend>("SELECT Top 1 * FROM [dbo].[AFIVoteMemberMoosend] WHERE MemberNumber = @MemberNumber AND VotingPeriodId = @VotingPeriodId Order by 1 DESC", new
                        {
                            entity.MemberNumber,
                            entity.VotingPeriodId
                        }, transaction);

                        if (existingRecord != null)
                        {
                            return -1;

                        }

                        var sql = @"
                    INSERT INTO [dbo].[AFIVoteMemberMoosend]
                           ([VotingPeriodId]
                           ,[MemberNumber]
                           ,[PINNumber]
                           ,[MarketingCode]
                           ,[Salutation]
                           ,[RankAbbreviation]
                           ,[ProperFirstName]
                           ,[FirstName]
                            ,[MiddleName]
                           ,[LastName]
                           ,[Suffix]
                           ,[MilitaryStatus]
                           ,[AddressLine1]
                           ,[AddressLine2]
                           ,[City]
                           ,[State]
                           ,[PostalCode]
                           ,[Country]
                           ,[Email]
                           ,[ClientType]
                           ,[MailingCountyName]
                           ,[MembershipDate]
                           ,[YearsAsMember]
                           ,[Gender]
                           ,[CreateDate]
                           ,[IsSynced])
                     VALUES
                           (@VotingPeriodId
                           ,@MemberNumber
                           ,@PINNumber
                           ,@MarketingCode
                           ,@Salutation
                           ,@RankAbbreviation
                           ,@ProperFirstName
                           ,@FirstName
                           ,@MiddleName
                           ,@LastName
                           ,@Suffix
                           ,@MilitaryStatus
                           ,@AddressLine1
                           ,@AddressLine2
                           ,@City
                           ,@State
                           ,@PostalCode
                           ,@Country
                           ,@Email
                           ,@ClientType
                           ,@MailingCountyName
                           ,@MembershipDate
                           ,@YearsAsMember
                           ,@Gender
                           ,@CreateDate
                           ,@IsSynced); ";

                        // Execute the SQL query using Dapper
                        var newId = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: create Member " + sw.Elapsed, "stopwatch");

                        return newId;
                    }
                    catch (System.Exception ex)
                    {
                        if (ex is SqlException sqlEx)
                        {
                            foreach (SqlError error in sqlEx.Errors)
                            {
                                if (error.Number == 8152) // Error number for string or binary data would be truncated
                                {
                                    // Extracting the column name from the error message
                                    string errorMessage = error.Message;
                                    int startIndex = errorMessage.IndexOf("'");
                                    int endIndex = errorMessage.IndexOf("'", startIndex + 1);
                                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                                    {
                                        string columnName = errorMessage.Substring(startIndex + 1, endIndex - startIndex - 1);
                                        Log.Error($"{nameof(QuoteRepository)}: Error while attempting to insert Member. Truncated column: {columnName}", ex, this);
                                    }
                                    else
                                    {
                                        Log.Error($"{nameof(QuoteRepository)}: Error while attempting to insert Member. Unable to determine the truncated column.", ex, this);
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Log.Error($"{nameof(QuoteRepository)}: Error while attempting to insert Member.", ex, this);
                        }


                        // Log.Error($"{nameof(QuoteRepository)}: Error while attempting to insert Member.", ex, this);
                        transaction.Rollback();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: create Member" + sw.Elapsed, "stopwatch");
                        return 0;
                    }
                }
            }
        }

        public IEnumerable<VotingMemberCSV> GetTotalMemberByVoting(int VotingId , string memberVote)
        {
            
            // string voatingPeriodId = "(SELECT TOP 1 VotingPeriodId FROM [ProxyVote].VotingPeriod ORDER BY VotingPeriodId DESC)";

            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                int votingPeriodId;
                if(VotingId == 0)
                {
                    // Execute subquery to get the latest VotingPeriodId
                    string subQuery = "SELECT TOP 1 VotingPeriodId FROM [ProxyVote].VotingPeriod ORDER BY VotingPeriodId DESC";
                    votingPeriodId = db.QueryFirstOrDefault<int>(subQuery);
                }
                else
                {
                    votingPeriodId = VotingId;
                }
                

                var sql = @"SELECT m.MemberId, 
                           m.MemberNumber,
                           m.PIN,
                           m.MarketingCode,
                           m.Salutation,
                           m.Prefix,
                           m.ProperFirstName,
                           m.InsuredFirstName,
                           m.MiddleName,
                           m.InsuredLastName,
                           m.Suffix,
                           m.ServiceStatus,
                           m.MailingAddressLine1,
                           m.MailingAddressLine2,
                           m.MailingCityName,
                           m.MailingStateAbbreviation,
                           m.MailingZip,
                           m.MailingCountry,
                           m.VotingPeriodId,
                           m.EmailAddress,
                           m.ClientType,
                           m.MailingCountyName,
                           m.MembershipDate,
                           m.YearsAsMember,
                           m.Gender,
                           COUNT(*) OVER() AS TotalCount 

                    FROM[ProxyVote].[Member] AS m
                    WHERE  1 = 1 ";

                
                // Additional condition based on IsEmail parameter
                if (memberVote.ToLower() == "all")
                {
                    // Get all email or non-email members
                    // No additional condition needed
                }
                else if (memberVote.ToLower() == "voted")
                {
                    // Get members where email field has email
                    sql += " AND m.[Enabled] = 0 ";
                }
                else if (memberVote.ToLower() == "notvoted")
                {
                    // Get members where email field is null or empty
                    sql += " AND m.[Enabled] = 1 ";
                }

                if (votingPeriodId == 99999999)
                {
                   // get all voting period member
                }
                else
                {
                    sql += $" AND m.VotingPeriodId = {votingPeriodId} ";
                }
              

                //var parameters = new DynamicParameters();
                //parameters.Add("@VotingPeriodId", votingPeriodId);
                
               // return db.Query<VotingMemberCSV>(sql, parameters);

                return db.Query<VotingMemberCSV>(sql);
            }
        }

        //public async Task InsertMemberVotesAsync(IEnumerable<ProxyVoteMember> members)
        //{
        //    try
        //    {
        //        using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
        //        {
        //            await db.OpenAsync();

        //            foreach (var member in members)
        //            {
        //                using (var transaction = db.BeginTransaction())
        //                {
        //                    var sql = @"
        //                INSERT INTO [ProxyVote].[Member]
        //                (
        //                    [MemberNumber],
        //                    [PIN],
        //                    [VotingPeriodId],
        //                    [Enabled],
        //                    [EmailAddress],
        //                    [FullName],
        //                    [ResidentialOccupied],
        //                    [ResidentialDwelling],
        //                    [Renters],
        //                    [Flood],
        //                    [Life],
        //                    [PersonalLiabilityRenters],
        //                    [PersonalLiabilityCatastrophy],
        //                    [Auto],
        //                    [RV],
        //                    [Watercraft],
        //                    [Motorcycle],
        //                    [Supplemental],
        //                    [AnnualReport],
        //                    [StatutoryFinancialStatements],
        //                    [MobileHome],
        //                    [PetHealth],
        //                    [Business],
        //                    [LongTermCare],
        //                    [MailFinancials],
        //                    [EmailFinancials],
        //                    [IsEmailUpdated],
        //                    [Prefix],
        //                    [Salutation],
        //                    [InsuredFirstName],
        //                    [InsuredLastName],
        //                    [ClientType],
        //                    [ServiceStatus],
        //                    [MailingAddressLine1],
        //                    [MailingAddressLine2],
        //                    [MailingCityName],
        //                    [MailingCountyName],
        //                    [MailingStateAbbreviation],
        //                    [MailingZip],
        //                    [MailingCountry],
        //                    [MembershipDate],
        //                    [YearsAsMember],
        //                    [Gender],
        //                    [Deceased],
        //                    [MarketingCode],
        //                    [ProperFirstName],
        //                    [MiddleName],
        //                    [Suffix],
        //                    [CreateDate]
        //                )
        //                VALUES
        //                (
        //                    @MemberNumber,
        //                    @PIN,
        //                    @VotingPeriodId,
        //                    @Enabled,
        //                    @EmailAddress,
        //                    @FullName,
        //                    @ResidentialOccupied,
        //                    @ResidentialDwelling,
        //                    @Renters,
        //                    @Flood,
        //                    @Life,
        //                    @PersonalLiabilityRenters,
        //                    @PersonalLiabilityCatastrophy,
        //                    @Auto,
        //                    @RV,
        //                    @Watercraft,
        //                    @Motorcycle,
        //                    @Supplemental,
        //                    @AnnualReport,
        //                    @StatutoryFinancialStatements,
        //                    @MobileHome,
        //                    @PetHealth,
        //                    @Business,
        //                    @LongTermCare,
        //                    @MailFinancials,
        //                    @EmailFinancials,
        //                    @IsEmailUpdated,
        //                    @Prefix,
        //                    @Salutation,
        //                    @InsuredFirstName,
        //                    @InsuredLastName,
        //                    @ClientType,
        //                    @ServiceStatus,
        //                    @MailingAddressLine1,
        //                    @MailingAddressLine2,
        //                    @MailingCityName,
        //                    @MailingCountyName,
        //                    @MailingStateAbbreviation,
        //                    @MailingZip,
        //                    @MailingCountry,
        //                    @MembershipDate,
        //                    @YearsAsMember,
        //                    @Gender,
        //                    @Deceased,
        //                    @MarketingCode,
        //                    @ProperFirstName,
        //                    @MiddleName,
        //                    @Suffix,
        //                    @CreateDate
        //                );";

        //                    var id = await db.ExecuteAsync(sql, member, transaction);
        //                    transaction.Commit();

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


    }
}