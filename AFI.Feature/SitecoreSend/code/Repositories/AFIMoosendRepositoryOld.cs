using AFI.Feature.SitecoreSend.Models;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AFI.Feature.SitecoreSend.Repositories
{
    public class AFIMoosendRepositoryOld
    {

        private readonly string AFIConnectionString;

        public AFIMoosendRepositoryOld()
        {
            AFIConnectionString = ConfigurationManager.ConnectionStrings["AFIDB"].ConnectionString;
        }

        #region Email List Subscribers
        public int InsertEmailListData(EmailListData data)
        {
            int insertedId = 0; // Initialize the variable to hold the inserted ID
            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "INSERT INTO [AFIMooSend_EmailList] ([ListName],[ListId],[SecurityKey],[CreatedBy],[CreatedDate]) OUTPUT INSERTED.Id VALUES(@ListName, @ListId, @SecurityKey, @CreatedBy, @CreatedDate)";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@ListName", data.ListName);
                        cmd.Parameters.AddWithValue("@ListId", data.ListId);
                        cmd.Parameters.AddWithValue("@SecurityKey", data.SecurityKey);
                        cmd.Parameters.AddWithValue("@CreatedBy", data.CreatedBy);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                        // Execute the query and retrieve the inserted ID
                        insertedId = (int)cmd.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error while attempting to insert EmailListData.", ex, this);
                    // You may want to handle or log the exception accordingly
                }
                finally
                {
                    db.Close();
                }
            }
            return insertedId; // Return the inserted ID
        }

       
        public List<EmailListData> GetAllEmailListData()
        {
            List<EmailListData> emailListDataList = new List<EmailListData>();

            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "SELECT * FROM [AFIMooSend_EmailList]";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Map each row from the reader to MoosendListSubscriber and add to subscribers list
                        while (reader.Read())
                        {
                            EmailListData data = RepositoryHelper.MapReaderToObject<EmailListData>(reader);
                            emailListDataList.Add(data);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error while attempting to get all Email List data.", ex, this);
                }
                finally
                {
                    db.Close();
                }
            }

            return emailListDataList;
        }



        public EmailListData GetEmailListDataById(int id)
        {
            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "SELECT * FROM [AFIMooSend_EmailList] WHERE Id = @Id";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return RepositoryHelper.MapReaderToObject<EmailListData>(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while attempting to get Email List data by Id {id}.", ex, this);
                }
                finally
                {
                    db.Close();
                }
            }

            return null; // Return null if not found
        }

        public EmailListData GetEmailListDataBylistName(string listName)
        {
            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "SELECT * FROM [AFIMooSend_EmailList] WHERE ListName = @ListName";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@ListName", listName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return RepositoryHelper.MapReaderToObject<EmailListData>(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while attempting to get Email List data by Id {listName}.", ex, this);
                }
                finally
                {
                    db.Close();
                }
            }

            return null; // Return null if not found
        }

        public bool IsListNameDuplicate(string listName)
        {
            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "SELECT COUNT(*) FROM [AFIMooSend_EmailList] WHERE ListName = @ListName";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@ListName", listName);

                        int count = (int)cmd.ExecuteScalar();

                        return count > 0; // If count is greater than 0, the list name is duplicate
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while attempting to check duplicate ListName '{listName}'.", ex, this);
                }
                finally
                {
                    db.Close();
                }
            }

            // Return true to be cautious if an exception occurs
            return true;
        }
        public void UpdateEmailListName(EmailListData data)
        {
            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "Update AFIMooSend_EmailList SET ListName = @ListName where ListId = @ListId";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@ListName", data.ListName);
                        cmd.Parameters.AddWithValue("@ListId", data.ListId);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error while attempting to insert Quote.", ex, this);
                }
                finally
                {
                    db.Close();
                }
            }
        }

        #endregion

        #region SecurityKey
        public void InsertAFISecurityKeyData(AFISecurityKeyData data)
        {
            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "INSERT INTO [AFIMooSend_SecurityKey] ([Email],[SecurityKey],[StateTime],[EndTime]) VALUES(@Email, @SecurityKey, @StateTime, @EndTime)";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@Email", data.Email);
                        cmd.Parameters.AddWithValue("@SecurityKey", data.SecurityKey);
                        cmd.Parameters.AddWithValue("@StateTime", data.StateTime);
                        cmd.Parameters.AddWithValue("@EndTime", data.EndTime);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error while attempting to insert Quote.", ex, this);
                }
                finally
                {
                    db.Close();
                }
            }
        }

        public void UpdateSecurityKey(AFISecurityKeyData data)
        {
            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "UPDATE [AFIMooSend_SecurityKey] SET [Email] = @Email, [SecurityKey] = @SecurityKey, [StateTime] = @StateTime, [EndTime] = @EndTime WHERE [Id] = @Id";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@Id", data.Id);
                        cmd.Parameters.AddWithValue("@Email", data.Email);
                        cmd.Parameters.AddWithValue("@SecurityKey", data.SecurityKey);
                        cmd.Parameters.AddWithValue("@StateTime", data.StateTime);
                        cmd.Parameters.AddWithValue("@EndTime", data.EndTime);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception as needed
                }
                finally
                {
                    db.Close();
                }
            }
        }

        public List<AFISecurityKeyData> GetAllSecurityKeyData()
        {
            List<AFISecurityKeyData> emailListDataList = new List<AFISecurityKeyData>();

            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "SELECT * FROM [AFIMooSend_SecurityKey]";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AFISecurityKeyData data = RepositoryHelper.MapReaderToObject<AFISecurityKeyData>(reader);
                            emailListDataList.Add(data);

                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error while attempting to get all Email List data.", ex, this);
                }
                finally
                {
                    db.Close();
                }
            }

            return emailListDataList;
        }

        public AFISecurityKeyData GetSecurityKeyById(int id)
        {

            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "SELECT * FROM [AFIMooSend_SecurityKey] WHERE [Id] = @Id";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return RepositoryHelper.MapReaderToObject<AFISecurityKeyData>(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception as needed
                }
                finally
                {
                    db.Close();
                }
            }

            return null;
        }

        public AFISecurityKeyData GetBySecurityKey(string securityKey)
        {

            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "SELECT * FROM [AFIMooSend_SecurityKey] WHERE [SecurityKey] = @SecurityKey";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@SecurityKey", securityKey);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return RepositoryHelper.MapReaderToObject<AFISecurityKeyData>(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception as needed
                }
                finally
                {
                    db.Close();
                }
            }

            return null;
        }
        public AFISecurityKeyData GetBSecurityKeyByEmail(string email)
        {

            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "SELECT * FROM [AFIMooSend_SecurityKey] WHERE [Email] = @email";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return RepositoryHelper.MapReaderToObject<AFISecurityKeyData>(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception as needed
                }
                finally
                {
                    db.Close();
                }
            }

            return null;
        }
        public void DeleteSecurityKey(int id)
        {
            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "DELETE FROM [AFIMooSend_SecurityKey] WHERE [Id] = @Id";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception as needed
                }
                finally
                {
                    db.Close();
                }
            }
        }


        public List<AFISecurityKeyData> GetBSecurityKeysByEmail(string email)
        {
            List<AFISecurityKeyData> securityKeys = new List<AFISecurityKeyData>();

            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "SELECT * FROM [AFIMooSend_SecurityKey] WHERE [Email] = @Email";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AFISecurityKeyData keyData = RepositoryHelper.MapReaderToObject<AFISecurityKeyData>(reader);
                                securityKeys.Add(keyData);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception as needed
                }
                finally
                {
                    db.Close();
                }
            }

            return securityKeys;
        }
        public AFISecurityKeyData GetValidSecurityKeyByEmail(string email)
        {
            List<AFISecurityKeyData> dataCollection = GetBSecurityKeysByEmail(email);

            if (dataCollection.Count > 0)
            {
                DateTime currentTime = DateTime.Now;

                // Find the first entry with valid time
                AFISecurityKeyData validKey = dataCollection.FirstOrDefault(data =>
                    currentTime >= data.StateTime && currentTime <= data.EndTime);

                return validKey;
            }

            return null;
        }

        public bool IsValidStateTimeAndEndTime(string securityKey)
        {
            AFISecurityKeyData data = GetBySecurityKey(securityKey);

            if (data != null)
            {
                DateTime currentTime = DateTime.Now;
                return currentTime >= data.StateTime && currentTime <= data.EndTime;
            }

            return false;
        }


        #endregion

        #region ListSubscriber
        public void ListSubscriberInsert(MoosendListSubscriber data)
        {
            using (SqlConnection connection = new SqlConnection(AFIConnectionString))
            {
                string query = "INSERT INTO AFIMoosend_ListSubscriber (EmailListId,SendListId, ListName, SubscriberId, Email, Name, Mobile, JsonBody, Source, CreatedBy, CreatedDate, IsSynced, SyncedTime) " +
                               "VALUES (@EmailListId,@SendListId, @ListName, @SubscriberId, @Email, @Name, @Mobile, @JsonBody, @Source, @CreatedBy, @CreatedDate, @IsSynced, @SyncedTime)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Set parameter values from the MoosendListSubscriber object
                    command.Parameters.AddWithValue("@EmailListId", data.EmailListId);
                    command.Parameters.AddWithValue("@SendListId", data.SendListId);
                    command.Parameters.AddWithValue("@ListName", data.ListName);
                    command.Parameters.AddWithValue("@SubscriberId", data.SubscriberId);
                    command.Parameters.AddWithValue("@Email", data.Email);
                    command.Parameters.AddWithValue("@Name", data.Name);
                    command.Parameters.AddWithValue("@Mobile", data.Mobile);
                    command.Parameters.AddWithValue("@JsonBody", data.JsonBody);
                    command.Parameters.AddWithValue("@Source", data.Source);
                    command.Parameters.AddWithValue("@CreatedBy", data.CreatedBy);
                    command.Parameters.AddWithValue("@CreatedDate", data.CreatedDate);
                    command.Parameters.AddWithValue("@IsSynced", data.IsSynced);
                    command.Parameters.AddWithValue("@SyncedTime", (object)data.SyncedTime ?? DBNull.Value); // Handle nullable SyncedTime

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        public void ListSubscriberUpdate(MoosendListSubscriber data)
        {
            using (SqlConnection connection = new SqlConnection(AFIConnectionString))
            {
                string query = "UPDATE AFIMoosend_ListSubscriber SET EmailListId = @EmailListId, " +
                               "SendListId = @SendListId, ListName = @ListName, " +
                               "SubscriberId = @SubscriberId, Email = @Email, " +
                               "Name = @Name, Mobile = @Mobile, JsonBody = @JsonBody, " +
                               "Source = @Source, CreatedBy = @CreatedBy, " +
                               "CreatedDate = @CreatedDate, IsSynced = @IsSynced, " +
                               "SyncedTime = @SyncedTime WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Set parameter values from the MoosendListSubscriber object
                    command.Parameters.AddWithValue("@Id", data.Id);
                    command.Parameters.AddWithValue("@EmailListId", data.EmailListId);
                    command.Parameters.AddWithValue("@SendListId", data.SendListId);
                    command.Parameters.AddWithValue("@ListName", data.ListName);
                    command.Parameters.AddWithValue("@SubscriberId", data.SubscriberId);
                    command.Parameters.AddWithValue("@Email", data.Email);
                    command.Parameters.AddWithValue("@Name", data.Name);
                    command.Parameters.AddWithValue("@Mobile", data.Mobile);
                    command.Parameters.AddWithValue("@JsonBody", data.JsonBody);
                    command.Parameters.AddWithValue("@Source", data.Source);
                    command.Parameters.AddWithValue("@CreatedBy", data.CreatedBy);
                    command.Parameters.AddWithValue("@CreatedDate", data.CreatedDate);
                    command.Parameters.AddWithValue("@IsSynced", data.IsSynced);
                    command.Parameters.AddWithValue("@SyncedTime", data.SyncedTime);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        public void DeleteListSubscriber(int Id)
        {
            using (SqlConnection connection = new SqlConnection(AFIConnectionString))
            {
                string query = "DELETE FROM AFIMoosend_ListSubscriber WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<MoosendListSubscriber> GetAllListSubscriberByListId(int emailListId)
        {
            List<MoosendListSubscriber> subscribers = new List<MoosendListSubscriber>();

            using (SqlConnection connection = new SqlConnection(AFIConnectionString))
            {
                string query = "SELECT * FROM AFIMoosend_ListSubscriber WHERE EmailListId = @EmailListId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmailListId", emailListId);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Map each row from the reader to MoosendListSubscriber and add to subscribers list
                        while (reader.Read())
                        {
                            MoosendListSubscriber subscriber = RepositoryHelper.MapReaderToObject<MoosendListSubscriber>(reader);
                            subscribers.Add(subscriber);
                        }
                    }
                }
            }

            return subscribers;
        }
        public List<MoosendListSubscriber> GetAllListSubscriber()
        {
            List<MoosendListSubscriber> subscribers = new List<MoosendListSubscriber>();

            using (SqlConnection connection = new SqlConnection(AFIConnectionString))
            {
                string query = "SELECT * FROM AFIMoosend_ListSubscriber";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MoosendListSubscriber data = RepositoryHelper.MapReaderToObject<MoosendListSubscriber>(reader);
                            subscribers.Add(data);
                        }
                    }
                }
            }

            return subscribers;
        }

        public MoosendListSubscriber GetListSubscriberById(int id)
        {
            using (SqlConnection connection = new SqlConnection(AFIConnectionString))
            {
                string query = "SELECT * FROM AFIMoosend_ListSubscriber WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return RepositoryHelper.MapReaderToObject<MoosendListSubscriber>(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<MoosendListSubscriber> GetAllListSubscriberByIsSynced(bool isSynced)
        {
            List<MoosendListSubscriber> subscribers = new List<MoosendListSubscriber>();

            using (SqlConnection connection = new SqlConnection(AFIConnectionString))
            {
                string query = "Select * From [AFIMoosend_ListSubscriber] where IsSynced = @IsSynced";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsSynced", isSynced);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MoosendListSubscriber data = RepositoryHelper.MapReaderToObject<MoosendListSubscriber>(reader);
                            subscribers.Add(data);
                        }
                    }
                }
            }

            return subscribers;
        }

        //public List<MoosendListSubscriber> GetListSubscriberlist()
        //{
        //    List<MoosendListSubscriber> subscribers = new List<MoosendListSubscriber>();

        //    using (SqlConnection connection = new SqlConnection(AFIConnectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("Select * From [AFIMoosend_ListSubscriber] where IsSynced = 0", connection);
        //        command.Parameters.AddWithValue("@Id", id);
        //        connection.Open();
        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                MoosendListSubscriber data = RepositoryHelper.MapReaderToObject<MoosendListSubscriber>(reader);
        //                subscribers.Add(data);
        //            }
        //        }
        //        return subscribers;

        //    }
        //}
        //public void UpdateSubscriberList(int subListId, int emailId, string sendListId, string subscriberId)
        //{
        //    using (SqlConnection db = new SqlConnection(AFIConnectionString))
        //    {
        //        db.Open();
        //        try
        //        {
        //            var sql = "Update [AFIMoosend_ListSubscriber] SET SendListId = @sendListId, EmailListId =@emailListId,  SubscriberId = @subscriberId, IsSynced = @IsSynced, syncedTime = @syncedTime  where Id = @subListId";

        //            using (SqlCommand cmd = new SqlCommand(sql, db))
        //            {
        //                cmd.Parameters.AddWithValue("@subListId", subListId);
        //                cmd.Parameters.AddWithValue("@emailListId", emailId);
        //                cmd.Parameters.AddWithValue("@sendListId", sendListId);
        //                cmd.Parameters.AddWithValue("@subscriberId", subscriberId);
        //                cmd.Parameters.AddWithValue("@IsSynced", true);
        //                cmd.Parameters.AddWithValue("@syncedTime", DateTime.Now);

        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error("Error while attempting to insert Quote.", ex, this);
        //        }
        //        finally
        //        {
        //            db.Close();
        //        }
        //    }
        //}


        public void InserAFIMoosendtLog(string data, Enum name, string jsonBody, string responseContext)
        {
            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "INSERT INTO [dbo].[AFIMoosend_Log] ([CreatedTime],[Logtype],[LogDescription],[RequestBody],[ResponseBody]) VALUES(@CreatedTime, @Logtype, @LogDescription,@RequestBody ,@ResponseBody)";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@CreatedTime", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Logtype", name.ToString());
                        cmd.Parameters.AddWithValue("@LogDescription", data.ToString());
                        cmd.Parameters.AddWithValue("@RequestBody", jsonBody.ToString());
                        cmd.Parameters.AddWithValue("@ResponseBody", responseContext.ToString());


                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error while attempting to insert Quote.", ex, this);
                }
                finally
                {
                    db.Close();
                }
            }
        }
        #endregion


        #region AFI Vote

        public List<ProxyVoteMember> GetAllVotingMemberData()
        {
            List<ProxyVoteMember> members = new List<ProxyVoteMember>();

            using (SqlConnection connection = new SqlConnection(AFIConnectionString))
            {
                string query = $@" SELECT m.*
                                FROM [dbo].[Member] m
                                INNER JOIN dbo.VotingPeriod v ON m.VotingPeriodId = v.VotingPeriodId
                                WHERE v.VotingPeriodId = (SELECT TOP 1 VotingPeriodId FROM dbo.VotingPeriod ORDER BY VotingPeriodId DESC)"; 

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProxyVoteMember data = RepositoryHelper.MapReaderToObject<ProxyVoteMember>(reader);
                            members.Add(data);
                        }
                    }
                }
            }

            return members;
        }
    
        public string GetVotingPeriodTitleById(int votingPeriodId)
        {
            using (SqlConnection connection = new SqlConnection(AFIConnectionString))
            {
                string query = @"SELECT Title FROM [dbo].[VotingPeriod] WHERE VotingPeriodId = @VotingPeriodId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@VotingPeriodId", votingPeriodId);

                    connection.Open();

                    var title = command.ExecuteScalar() as string;

                    return title;
                }
            }
        }
        public string GetLatestVotingPeriodTitle()
        {
            using (SqlConnection connection = new SqlConnection(AFIConnectionString))
            {
                string query = @"SELECT TOP 1 Title FROM [dbo].[VotingPeriod] ORDER BY VotingPeriodId DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    var title = command.ExecuteScalar() as string;

                    return title;
                }
            }
        }

        #endregion


        #region Common

        public static class RepositoryHelper
        {
            public static T MapReaderToObject<T>(SqlDataReader reader)
            {
                if (reader == null || !reader.HasRows)
                {
                    return default(T); // Return default value for reference types or null for value types
                }

                T obj = Activator.CreateInstance<T>(); // Create an instance of the specified type

                // Iterate over each property in the object and set its value based on the column in the reader
                foreach (PropertyInfo property in typeof(T).GetProperties())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                    {
                        object value = reader[property.Name];
                        property.SetValue(obj, value);
                    }
                }

                return obj;
            }
        }

        #endregion
      

    }


}