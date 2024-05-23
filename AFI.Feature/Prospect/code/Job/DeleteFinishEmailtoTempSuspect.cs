using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Sitecore.Data.Items;
using AFI.Feature.Prospect.Models;
using Dapper;
using Sitecore.Diagnostics;
using AFI.Feature.Prospect.Repositories;

namespace AFI.Feature.Prospect.Job
{
    public class DeleteFinishEmailtoTempSuspect
    {
        private static readonly string AFIConnectionString = ConfigurationManager.ConnectionStrings["AFIDB"].ConnectionString;
        private static readonly string MConnectionString = ConfigurationManager.ConnectionStrings["master"].ConnectionString;

        ISuspectMarketingRepositoryTemp _tempSuspect;
        public DeleteFinishEmailtoTempSuspect(ISuspectMarketingRepositoryTemp tempSuspect)
        {
            _tempSuspect = tempSuspect;
        }

        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            Sitecore.Diagnostics.Log.Info("Delete Mails from Temp Suspect Sitecore scheduled task is being run!", this);

            var tampSuspectList = _tempSuspect.GetAllForMarketing();

            foreach (var tempSuspect in tampSuspectList)
            {
                
            }




        }

    
    }
}