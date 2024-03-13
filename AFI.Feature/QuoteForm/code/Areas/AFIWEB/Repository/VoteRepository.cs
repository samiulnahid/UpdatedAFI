using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.ExperienceForms.Samples.SubmitActions;
using System.Diagnostics;
using Sitecore.Diagnostics;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Repository
{
    //public interface IVoteRepository
    //{
    //    ProxyVoteMember GetLoginInfo(string memberNumber, string pinNumber, int VotingPeriodId);
    //    IEnumerable<ProxyVoteMemberCandidateBallot> GetAllCastBallots(int memberId, int votingPeriodId);
    //    int Create(ProxyVoteMemberCandidateBallot entity);
    //}


    public class VoteRepository 
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public VoteRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public ProxyVoteMember GetLoginInfo(string memberNumber, string pinNumber, int votingPeriodId)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sqlLogin = @"Select top 1* from ProxyVote.Member Where MemberNumber='" + memberNumber + "' and PIN='" + pinNumber + "' and VotingPeriodId=" + votingPeriodId;

                return db.QueryFirstOrDefault<ProxyVoteMember>(sqlLogin);
            }
        }
        public IEnumerable<ProxyVoteMemberCandidateBallot> GetAllCastBallots(int memberId, int votingPeriodId)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = @"Select mcb.MemberId,mcb.CandidateId,mcb.VotingPeriodId,mcb.Ballot,mcb.DateTimeCast from [ProxyVote].[MemberCandidateBallot] AS mcb inner join [ProxyVote].[Candidate] AS c ON mcb.CandidateId = c.CandidateId 
Where mcb.MemberId = " + memberId + " And mcb.VotingPeriodId=" + votingPeriodId;
                return db.Query<ProxyVoteMemberCandidateBallot>(sql);
            }
        }
        public int Create(ProxyVoteMemberCandidateBallot entity)
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
                        var sql = "insert into dbo.[ProxyVote].[MemberCandidateBallot] ([MemberId], [CandidateId], [VotingPeriodId], [Ballot], [DateTimeCast], [changeset]) values (@MemberId, @CandidateId, @VotingPeriodId, @Ballot, @DateTimeCast, @changeset); select scope_identity();";
                        var id = db.QueryFirstOrDefault<int>(sql, entity, transaction);
                        transaction.Commit();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: create Ballot " + sw.Elapsed, "stopwatch");
                        return id;
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(VoteRepository)}: Error while attempting to insert Vote.", ex, this);
                        transaction.Rollback();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: create vote " + sw.Elapsed, "stopwatch");
                        return 0;
                    }
                }
            }
        }
    }
}