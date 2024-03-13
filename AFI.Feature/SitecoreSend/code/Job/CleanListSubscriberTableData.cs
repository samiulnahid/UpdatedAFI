using System;
using System.Linq;
using System.Text;
using System.IO;
using Sitecore.Data.Items;
using System.Configuration;
using System.Data.SqlClient;

namespace AFI.Feature.SitecoreSend.Job
{
    public class CleanListSubscriberTableData
    {
        
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["AFIDB"].ConnectionString;

        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {

            Sitecore.Diagnostics.Log.Info("Ip Logs >> scheduled task is being run!", this);

            var dayLength = ConfigurationManager.AppSettings["TimeLength"];
            if (dayLength != null)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {

                    try
                    {
                        DateTime deletedTempDate = DateTime.Now.AddDays(-Convert.ToInt32(dayLength)).Date;
                        string sql = $"DELETE FROM [dbo].[AFIMoosend_ListSubscriber] WHERE [CreatedDate] < '{deletedTempDate}' AND [IsSynced] = 1 ";

                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        Sitecore.Diagnostics.Log.Info("Clean ListSubscriber >> Successfully Deleted ListSubscriber Data from " + dayLength + "Days Ago", this);

                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Info("Clean ListSubscriber >> Exception " + ex, this);

                    }
                    finally
                    {
                        connection.Close();
                    }


                }
            }
           

        }
        
    }
}