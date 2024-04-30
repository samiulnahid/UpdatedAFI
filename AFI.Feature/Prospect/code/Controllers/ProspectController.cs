using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFI.Feature.Prospect.Repositories;
using AFI.Feature.Prospect.Models;
using CsvHelper;
using System.Reflection;
using System.Globalization;
using System.Data.SqlClient;
using Newtonsoft.Json;
using AFI.Feature.SitecoreSend.Repositories;
using AFI.Feature.SitecoreSend.Models;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AFI.Feature.Prospect.Controllers
{
    public class ProspectController : Controller
    {
        private readonly ISuspectMarketingRepository _prospectMarketingRepository;
        private readonly ISuspectMarketingRepositoryTemp _tempProspectRepository;
        AFIMoosendRepository repository = new AFIMoosendRepository();

        public ProspectController(ISuspectMarketingRepository prospectMarketingRepository, ISuspectMarketingRepositoryTemp tempProspectRepository)
        {
            _prospectMarketingRepository = prospectMarketingRepository;
            _tempProspectRepository = tempProspectRepository;
        }

        [HttpPost]
        public JsonResult CsvMarketingProspect(HttpPostedFileBase file, string email)
        {

            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string fileExtension = Path.GetExtension(file.FileName);
                    if (fileExtension.ToLower() == ".csv")
                    {
                        List<SuspectMarketing> prospectMarketingList = ProcessCsvFile(file.InputStream);
                        var total = 0;
                        foreach (SuspectMarketing item in prospectMarketingList)
                        {
                            var count = _prospectMarketingRepository.Create(item);
                            total = total + count;
                        }
                        if (total > 0)
                        {
                            var response = new { Success = true, Message = $"Successfully Insert {total} Item" };
                            return Json(response);
                        }
                        else
                        {
                            var response = new { Success = false, Message = $"CSV file Empty" };
                            return Json(response);
                        }
                    }
                    else
                    {
                        var response = new { Success = false, Message = $"File is not a CSV file. Please enter CSV file" };
                        return Json(response);
                    }

                }
                catch (Exception ex)
                {
                    var response = new { Success = false, Message = $"Error: + {ex.Message}" };
                    return Json(response);
                }
            }
            else
            {
                var response = new { Success = false, Message = $"Enter CSV file" };
                return Json(response);
            }

        }

        private List<SuspectMarketing> ProcessCsvFile(Stream stream)
        {
            List<SuspectMarketing> prospectMarketingList = new List<SuspectMarketing>();

            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    SuspectMarketing prospectMarketing = new SuspectMarketing
                    {
                        FirstName = GetFieldOrDefault<string>(csv, "FirstName"),
                        LastName = GetFieldOrDefault<string>(csv, "LastName"),
                        Email = GetFieldOrDefault<string>(csv, "Email"),
                        Phone = GetFieldOrDefault<string>(csv, "Phone"),
                        Address = GetFieldOrDefault<string>(csv, "Address"),
                        City = GetFieldOrDefault<string>(csv, "City"),
                        State = GetFieldOrDefault<string>(csv, "State"),
                        ZipCode = GetFieldOrDefault<string>(csv, "ZipCode"),
                        Country = GetFieldOrDefault<string>(csv, "Country"),
                        DateOfBirth = GetFieldOrDefault<DateTime?>(csv, "DateOfBirth"),
                        Occupation = GetFieldOrDefault<string>(csv, "Occupation"),
                        PreferredCoverage = GetFieldOrDefault<string>(csv, "PreferredCoverage"),
                        LeadSource = GetFieldOrDefault<string>(csv, "LeadSource"),
                        LeadStatus = GetFieldOrDefault<string>(csv, "LeadStatus"),
                        LeadOwner = GetFieldOrDefault<string>(csv, "LeadOwner"),
                        LeadScore = GetFieldOrDefault<int>(csv, "LeadScore")
                    };

                    prospectMarketingList.Add(prospectMarketing);
                }
            }

            return prospectMarketingList;
        }

        private T GetFieldOrDefault<T>(CsvReader csv, string fieldName)
        {
            try
            {
                return csv.GetField<T>(fieldName);
            }
            catch (CsvHelper.MissingFieldException)
            {
                return default(T); ;
            }
        }

        [HttpPost]
        public JsonResult XlsMarketingProspect(HttpPostedFileBase file, string email)
        {

            if (file != null && file.ContentLength > 0)
            {
                try
                {

                    var response = new { Success = false, Message = $"Enter xls file" };
                    return Json(response);
                }
                catch (Exception ex)
                {
                    var response = new { Success = false, Message = $"Error:  {ex.Message}" };
                    return Json(response);
                }
            }
            else
            {
                var response = new { Success = false, Message = $"Enter xls file" };
                return Json(response);
            }

        }

        [HttpPost]
        public JsonResult SqlMarketingProspect(ObjectModel data)
        {

            try
            {
                string connectionString = $"Server={data.DataSource};Database={data.DbName};User Id={data.UserId};Password={data.Password};";

                List<SuspectMarketing> marketingList = new List<SuspectMarketing>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = $"{data.Query}";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        // Execute the query
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Read data from the reader
                            while (reader.Read())
                            {
                                SuspectMarketing item = RepositoryHelper.MapReaderToObject<SuspectMarketing>(reader);
                                marketingList.Add(item);
                            }
                        }
                    }
                }
                int total = 0;
                foreach (SuspectMarketing item in marketingList)
                {
                    int count = _prospectMarketingRepository.Create(item);
                    total = total + count;
                }
                if (total > 0)
                {
                    var response = new { Success = true, Message = $"Successfully Insert {total} Item" };
                    return Json(response);
                }
                else
                {
                    var response = new { Success = false, Message = $"Insert Unsuccessful" };
                    return Json(response);
                }

            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error: + {ex.Message}" };
                return Json(response);
            }
        }



        public JsonResult GetTempProspectData(int page = 1, int pageSize = 50, string leadStatus = null, string syncFilter = null, string coverage = null, string coverageType = null)
        {
            try
            {
                var data = _tempProspectRepository.GetAllForMarketing(page, pageSize, leadStatus, syncFilter, coverage, coverageType);

                var totalCount = data.FirstOrDefault()?.TotalCount ?? 0;
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                SuspectTempPagination finalData = new SuspectTempPagination
                {
                    CurrentPage = page,
                    TotalItem = totalCount,
                    TotalPages = totalPages,
                    ProspectTempList = data
                };
                if (finalData != null && data.Count() > 0)
                {
                    string finalJson = JsonConvert.SerializeObject(finalData);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"No Data Found!" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                var response = new { Success = false, Message = $"Error Get Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }

            // return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SyncTempApprovedtoProspect(string ids)
        {
            try
            {
                var data = _tempProspectRepository.SyncTempApprovedtoProspect(ids);

                if (data > 0)
                {
                    var response = new { Success = true, Message = $"Successfully Send to Suspect Table" };
                    return Json(response);
                }
                else
                {
                    var response = new { Success = false, Message = $"Error : Insert Unsuccessfull " };
                    return Json(response);
                }

            }
            catch (Exception ex)
            {

                var response = new { Success = false, Message = $"Error Insert Item Message " + ex.InnerException };
                return Json(response);
            }
        }

        [HttpGet]
        public JsonResult GetProspectData(int page = 1, int pageSize = 50, string leadStatus = null, string syncFilter = null, string coverage = null)
        {
            try
            {
                var data = _prospectMarketingRepository.GetAllFromProspectMarketing(page, pageSize, leadStatus, syncFilter, coverage);

                var totalCount = data.FirstOrDefault()?.TotalCount ?? 0;
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                SuspectTempPagination finalData = new SuspectTempPagination
                {
                    CurrentPage = page,
                    TotalItem = totalCount,
                    TotalPages = totalPages,
                    ProspectList = data
                };
                if (finalData != null && data.Count() > 0)
                {
                    string finalJson = JsonConvert.SerializeObject(finalData);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"No Data Found!" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                var response = new { Success = false, Message = $"Error Get Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }

            // return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SyncProspectToListSubscriber(string ids)
        {
            try
            {
                List<SuspectMarketing> dataList = _prospectMarketingRepository.GetProspectsByIds(ids);
                int UpdateDataCount;
                if (dataList.Count() > 0)
                {
                    foreach (SuspectMarketing data in dataList)
                    {

                        JObject jsonData = JObject.FromObject(new
                        {
                            FirstName = data.FirstName,
                            LastName = data.LastName,
                            Address = data.Address,
                            City = data.City,
                            State = data.State,
                            ZipCode = data.ZipCode,
                            Phone=data.Phone,
                            Country = data.Country,
                            DateOfBirth = data.DateOfBirth,
                            Occupation = data.LastName,
                            PreferredCoverage = data.PreferredCoverage,
                            LeadSource = data.LeadScore,
                            LeadStatus = data.State,
                            LeadOwner = data.LeadOwner,
                            LeadScore = data.LeadScore,
                            ProspectCreated = data.DateCreated

                        });

                        string mobile = data.Phone ?? string.Empty;
                        string name = (data.FirstName + " " + data.LastName) ?? string.Empty;
                        string leadSource = data.LeadSource ?? string.Empty;

                        JObject json = JObject.FromObject(new
                        {
                            Email = data.Email,
                            Name = name,
                            CustomFields = jsonData

                        });
                        string jsonBody = JsonConvert.SerializeObject(json);

                        var listSubscriber = new MoosendListSubscriber
                        {
                            SendListId = string.Empty,
                            ListName = data.PreferredCoverage,
                            Name = name,
                            Email = data.Email,
                            Mobile = mobile,
                            JsonBody = jsonBody,
                            SubscriberId = string.Empty,
                            Source = leadSource,
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow,
                            IsSynced = false,
                            SyncedTime = null

                        };
                        repository.ListSubscriberInsert(listSubscriber);
                    }

                    UpdateDataCount = _prospectMarketingRepository.UpdateIsSyncedFromProspect(ids);
                    if (UpdateDataCount > 0)
                    {
                        var response = new { Success = true, Message = $"Successfully  Send to Sync Table" };
                        return Json(response);
                    }
                    else
                    {
                        var response = new { Success = false, Message = $"Error : Suspect Table Updated Unsuccessfull ! " };
                        return Json(response);
                    }
                }
                else
                {
                    var response = new { Success = false, Message = $" Error : Insert Unsuccessfull. No Data Found in the Database. " };
                    return Json(response);
                }

            }
            catch (Exception ex)
            {

                var response = new { Success = false, Message = $"Error Insert Item Message " + ex.InnerException };
                return Json(response);
            }
        }

        public FileContentResult DownloadSuspectTempCSV(string leadStatus = null, string syncFilter = null, string coverage = null, string coverageType = null)
        {
            try
            {

            // Sample data
            IEnumerable<SuspectMarketingTemp> modelList = new List<SuspectMarketingTemp>();

                modelList = _tempProspectRepository.GetAllForDownloadTempSuspect(leadStatus, syncFilter, coverage, coverageType);
                if (modelList.Count() > 0)
                {
                    // Convert list to CSV string
                    string csvData = ConvertToCSV(modelList);

                    byte[] bytes = Encoding.UTF8.GetBytes(csvData);
                    Response.Headers.Add("Content-Disposition", "attachment; filename=SuspectTemp.csv");
                    return File(bytes, "text/csv");

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                byte[] errorBytes = Encoding.UTF8.GetBytes($"Error: {ex.Message}");
                Response.Headers.Add("Content-Disposition", "attachment; filename=Error_SuspectTemp.csv");
                return File(errorBytes, "text/csv");
            }
        }

        public FileContentResult DownloadSuspectCSV(string leadStatus = null, string syncFilter = null, string coverage = null)
        {
            // Sample data
            try
            {
                IEnumerable<SuspectMarketing> modelList = new List<SuspectMarketing>();

                modelList = _prospectMarketingRepository.GetAllForDownloadSuspect(leadStatus, syncFilter, coverage);

                if (modelList.Count() > 0)
                {
                    // Convert list to CSV string
                    string csvData = ConvertToCSV(modelList);

                    byte[] bytes = Encoding.UTF8.GetBytes(csvData);
                    Response.Headers.Add("Content-Disposition", "attachment; filename=Suspect.csv");
                    return File(bytes, "text/csv");

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                byte[] errorBytes = Encoding.UTF8.GetBytes($"Error: {ex.Message}");
                Response.Headers.Add("Content-Disposition", "attachment; filename=Error_Suspect.csv");
                return File(errorBytes, "text/csv");
            }
        }
        private static string ConvertToCSV<T>(IEnumerable<T> dataList)
        {
            var sb = new StringBuilder();
            var properties = typeof(T).GetProperties()
                                       .Where(p => p.Name != "ID" &&  p.Name != "EntityID"  && p.Name != "TotalCount"); // Exclude Id and UpdatedTime

            // Write headers
            sb.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // Write data
            foreach (var data in dataList)
            {
                var values = new List<string>();
                foreach (var property in properties)
                {
                    var value = property.GetValue(data);
                    if (value is DateTime)
                    {
                        // Format DateTime values
                        values.Add($"\"{((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")}\"");
                    }
                    else
                    {
                        values.Add($"\"{value}\"");
                    }
                }
                sb.AppendLine(string.Join(",", values));
            }

            return sb.ToString();
        }

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