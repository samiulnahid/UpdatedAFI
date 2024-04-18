using Newtonsoft.Json;
using System.Net.Http;
using System;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;
using AFI.Foundation.Helper.Models;
using System.Threading.Tasks;
using AFI.Foundation.Helper.Repositories;
using System.Text;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper;
using System.Reflection;
using Sitecore.Data;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using Sitecore.Mvc.Extensions;
using Sitecore.Data.Items;
using Sitecore.Tasks;



namespace AFI.Feature.Quote.Job
{
    public class CreateCsvAFIIpLog
    {
        Repository repositoryHelper = new Repository();

        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {

            Sitecore.Diagnostics.Log.Info("IP Log task is being run!", this);


            string ipLogFolderPath = Sitecore.IO.FileUtil.MapPath("/upload/IpLog");
            string backupIpLogFolderPath = Sitecore.IO.FileUtil.MapPath("/upload/BackupIpLog");

            if (!Directory.Exists(ipLogFolderPath))
            {
                Directory.CreateDirectory(ipLogFolderPath);
            }
            if (!Directory.Exists(backupIpLogFolderPath))
            {
                Directory.CreateDirectory(backupIpLogFolderPath);
            }

            if (Directory.Exists(ipLogFolderPath))
            {
                try
                {
                    Sitecore.Diagnostics.Log.Info("IP Log >> Directory ok", this);
                    // Move csv file IpLog Folder to BackupIpLog Folder
                    var csvFiles = Directory.GetFiles(ipLogFolderPath, "*.csv");
                    if (ipLogFolderPath != null)
                    {
                        foreach (var csvFile in csvFiles)
                        {
                            string fileName = Path.GetFileName(csvFile);

                            string destinationPath = Path.Combine(backupIpLogFolderPath, fileName);

                            File.Move(csvFile, destinationPath);
                            Console.WriteLine($"Moved {fileName} to BackupIpLog folder.");
                        }
                    }
                    Sitecore.Diagnostics.Log.Info("IP Log >> Moved BackupIpLog", this);

                    string country = null;
                    DateTime? fromDate = null;
                    DateTime? toDate = null;


                    //Get data for Generate CSV data
                    var data = repositoryHelper.GetAllIPAddressInfo(country, fromDate, toDate).Select(d => new { d.IP, d.Country, d.City, d.State, d.PostalCode, d.FormattedAddedDate });
                    Sitecore.Diagnostics.Log.Info("IP Log >> GetAllIPAddressInfo", this);
                    // Generate CSV data
                    var csvData = new StringBuilder();
                    using (var stringWriter = new StringWriter(csvData))
                    using (var csvWriter = new CsvWriter(stringWriter, System.Globalization.CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(data);
                    }
                   

                    DateTime dateTime = DateTime.Now; 

                    string formattedDate = dateTime.ToString("dd-MM-yyyy");

                    var CsvfileName = $"{formattedDate}-IPLogs.csv";
                    var filePath = Path.Combine(ipLogFolderPath, CsvfileName);

                    System.IO.File.WriteAllText(filePath, csvData.ToString());

                    Sitecore.Diagnostics.Log.Info("IP Log >> GetAllIPAddressInfo csv done", this);

                    // Get Count Data
                    var countData = repositoryHelper.GetUniqueIPAddressCount(country, fromDate, toDate).Select(d => new { d.IP, d.Country, d.City, d.State, d.PostalCode, d.TotalCount });
                    Sitecore.Diagnostics.Log.Info("IP Log >> GetUniqueIPAddressCount", this);
                    var csvCountData = new StringBuilder();
                    using (var stringWriter = new StringWriter(csvCountData))
                    using (var csvWriter = new CsvWriter(stringWriter, System.Globalization.CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(countData);
                    }
                    var CsvCountfileName = $"{formattedDate}-UniqueIPLogs.csv";
                    var CountfilePath = Path.Combine(ipLogFolderPath, CsvCountfileName);

                    System.IO.File.WriteAllText(CountfilePath, csvCountData.ToString());
                    Sitecore.Diagnostics.Log.Info("IP Log >> GetUniqueIPAddressCount csv done", this);

                }
                catch (Exception ex)
                {

                    Sitecore.Diagnostics.Log.Info("IP Log >> Exception"+ex.Message, this);
                }

            }
            else
            {
                Sitecore.Diagnostics.Log.Info("IP Log >> folder does not exist" , this);
            }
            

        }       
       
       
    }
}