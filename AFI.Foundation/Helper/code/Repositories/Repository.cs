using AFI.Foundation.Helper.Models;
using Sitecore.Diagnostics;
using Sitecore.Drawing.Exif;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AFI.Foundation.Helper.Repositories
{

    public class Repository
    {
        private readonly string connectionString;

        public Repository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["AFIDB"].ConnectionString;
        }

        #region AFI_IP_Log
        public void InsertIPAddressInfo(IpLog info)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO AFI_IP_Log (IP, Country, City, State, PostalCode, AddedDate)
                             VALUES (@IP, @Country, @City, @State, @PostalCode, @AddedDate)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IP", info.IP);
                    command.Parameters.AddWithValue("@Country", (object)info.Country ?? DBNull.Value);
                    command.Parameters.AddWithValue("@City", (object)info.City ?? DBNull.Value);
                    command.Parameters.AddWithValue("@State", (object)info.State ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PostalCode", (object)info.PostalCode ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AddedDate", info.AddedDate);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<IpLog> GetIPAddressInfoCountryWiseCount(string country = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            List<IpLog> ipAddressInfos = new List<IpLog>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Country, COUNT(*) AS TotalCount FROM AFI_IP_Log WHERE 1 = 1";

                if (!string.IsNullOrEmpty(country))
                {
                    query += " AND Country = @Country";
                }

                if (fromDate != null)
                {
                    query += " AND CONVERT(date, AddedDate) >= @FromDate"; // Convert AddedDate to date only
                }

                if (toDate != null)
                {
                    query += " AND CONVERT(date, AddedDate) <= @ToDate"; // Convert AddedDate to date only
                }

                query += " GROUP BY Country ORDER BY TotalCount DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(country))
                    {
                        command.Parameters.AddWithValue("@Country", country);
                    }

                    if (fromDate != null)
                    {
                        command.Parameters.AddWithValue("@FromDate", fromDate.Value.Date); // Use Date property to get only the date
                    }

                    if (toDate != null)
                    {
                        command.Parameters.AddWithValue("@ToDate", toDate.Value.Date); // Use Date property to get only the date
                    }

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        IpLog data = new IpLog();
                        data.Country = reader["Country"].ToString();
                        data.TotalCount = reader["TotalCount"].ToString();
                        ipAddressInfos.Add(data);
                    }
                }
            }

            return ipAddressInfos;
        }


        public List<IpLog> GetIPAddressInfoCountryWiseCount()
        {
            List<IpLog> ipAddressInfos = new List<IpLog>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Country, COUNT(*) AS TotalCount FROM AFI_IP_Log GROUP BY Country Order By TotalCount DESC;";
               
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        IpLog data = new IpLog();
                        data.Country = reader["Country"].ToString();
                        data.TotalCount = reader["TotalCount"].ToString();
                        ipAddressInfos.Add(data);
                    }
                }
            }

            return ipAddressInfos;
        }
        public List<IpLog> GetAllIPAddressInfo(string country = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            List<IpLog> ipAddressInfos = new List<IpLog>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM AFI_IP_Log WHERE 1 = 1";

                if (!string.IsNullOrEmpty(country))
                {
                    query += " AND Country = @Country";
                }

                if (fromDate != null)
                {
                    query += " AND CONVERT(date, AddedDate) >= @FromDate"; // Convert AddedDate to date only
                }

                if (toDate != null)
                {
                    query += " AND CONVERT(date, AddedDate) <= @ToDate"; // Convert AddedDate to date only
                }

                query += " ORDER BY 1 DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(country))
                    {
                        command.Parameters.AddWithValue("@Country", country);
                    }

                    if (fromDate != null)
                    {
                        command.Parameters.AddWithValue("@FromDate", fromDate.Value.Date); // Use Date property to get only the date
                    }

                    if (toDate != null)
                    {
                        command.Parameters.AddWithValue("@ToDate", toDate.Value.Date); // Use Date property to get only the date
                    }


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        IpLog data = RepositoryHelper.MapReaderToObject<IpLog>(reader);
                        ipAddressInfos.Add(data);
                    }
                }
            }

            return ipAddressInfos;
        }

        public List<IpLog> GetAllIPAddressInfo(string country)
        {
            List<IpLog> ipAddressInfos = new List<IpLog>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM AFI_IP_Log";

                // If country is provided, add a WHERE clause to filter by country
                if (!string.IsNullOrEmpty(country))
                {
                    query += " WHERE Country = @Country";
                }

                query += " ORDER BY AddedDate DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // If country is provided, set the parameter value
                    if (!string.IsNullOrEmpty(country))
                    {
                        command.Parameters.AddWithValue("@Country", country);
                    }

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        IpLog data = RepositoryHelper.MapReaderToObject<IpLog>(reader);
                        ipAddressInfos.Add(data);
                    }
                }
            }

            return ipAddressInfos;
        }


        public List<IpLog> GetAllIPAddressInfo()
        {
            List<IpLog> ipAddressInfos = new List<IpLog>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM AFI_IP_Log Order by AddedDate Desc";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        IpLog data = RepositoryHelper.MapReaderToObject<IpLog>(reader);
                        ipAddressInfos.Add(data);
                    }
                }
            }

            return ipAddressInfos;
        }

        public List<IpLog> GetIPAddressInfoByDateRange(DateTime? startDate, DateTime? endDate)
        {
            List<IpLog> ipAddressInfos = new List<IpLog>();

            if (!startDate.HasValue)
            {
                startDate = DateTime.Today.AddYears(-5); // Set startDate to 5 years ago from today
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Today; // Set endDate to today
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM AFI_IP_Log WHERE AddedDate BETWEEN @StartDate AND @EndDate";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        IpLog data = RepositoryHelper.MapReaderToObject<IpLog>(reader);
                        ipAddressInfos.Add(data);
                    }
                }
            }

            return ipAddressInfos;
        }


        public List<IpLog> GetIPAddressInfoByCountry(string country)
        {
            List<IpLog> ipAddressInfos = new List<IpLog>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM AFI_IP_Log WHERE Country = @Country";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Country", country);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        IpLog data = RepositoryHelper.MapReaderToObject<IpLog>(reader);
                        ipAddressInfos.Add(data);
                    }
                }
            }

            return ipAddressInfos;
        }
        public IpLog GetLastIPAddressInfoByIP(string ip)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                try
                {
                    var sql = "SELECT TOP 1 * FROM AFI_IP_Log WHERE IP = @IP ORDER BY 1 DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, db))
                    {
                        cmd.Parameters.AddWithValue("@IP", ip);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return RepositoryHelper.MapReaderToObject<IpLog>(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while attempting to get Email List data by ip {ip}.", ex, this);
                }
                finally
                {
                    db.Close();
                }
            }

            return null; // Return null if not found
        }

        public List<IpLog> GetUniqueIPAddressCount(string country = null, DateTime? fromDate = null, DateTime? toDate = null)
        {

            List<IpLog> ipAddressInfos = new List<IpLog>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT IP,Country,City,State, PostalCode, COUNT(*) AS TotalCount FROM AFI_IP_Log WHERE 1 = 1";

                if (!string.IsNullOrEmpty(country))
                {
                    query += " AND Country = @Country";
                }

                if (fromDate != null)
                {
                    query += " AND CONVERT(date, AddedDate) >= @FromDate"; // Convert AddedDate to date only
                }

                if (toDate != null)
                {
                    query += " AND CONVERT(date, AddedDate) <= @ToDate"; // Convert AddedDate to date only
                }

                query += " GROUP BY IP,Country,City,State, PostalCode Order By TotalCount DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(country))
                    {
                        command.Parameters.AddWithValue("@Country", country);
                    }

                    if (fromDate != null)
                    {
                        command.Parameters.AddWithValue("@FromDate", fromDate.Value.Date); // Use Date property to get only the date
                    }

                    if (toDate != null)
                    {
                        command.Parameters.AddWithValue("@ToDate", toDate.Value.Date); // Use Date property to get only the date
                    }


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        IpLog data = new IpLog();
                        data.IP = reader["IP"].ToString();
                        data.Country = reader["Country"].ToString();
                        data.City = reader["City"].ToString();
                        data.State = reader["State"].ToString();
                        data.PostalCode = reader["PostalCode"].ToString();
                        data.TotalCount = reader["TotalCount"].ToString();
                        ipAddressInfos.Add(data);
                    }
                }
            }

            return ipAddressInfos;


        }
        #endregion
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
                    // Skip properties with NotMapped attribute
                    if (property.GetCustomAttributes(typeof(NotMappedAttribute), false).Any())
                    {
                        continue;
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                    {
                        object value = reader[property.Name];
                        property.SetValue(obj, value);
                    }
                }

                return obj;
            }

        }

    }
}
