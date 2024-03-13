using System;
using System.Linq;
using System.Text;
using System.IO;
using Sitecore.Data.Items;
using System.Configuration;
using System.Data.SqlClient;

namespace AFI.Feature.Prospect.Job
{
    public class CleanTempProspectTableData
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
                        string sql = $"DELETE FROM [dbo].[AFI_Marketing_Prospect_Temp] WHERE[DateCreated] < '{deletedTempDate}' ";

                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        Sitecore.Diagnostics.Log.Info("Clean Temp Prospect >> Successfully Deleted Prospect Temp Data from "+ dayLength + " Days Ago", this);

                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Info("Clean Temp Prospect >> Exception " + ex, this);

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