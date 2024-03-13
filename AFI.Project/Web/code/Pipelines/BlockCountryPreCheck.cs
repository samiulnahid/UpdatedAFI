using AFI.Project.Web.Models;
using Newtonsoft.Json;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Web;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Web.Mvc;
using AFI.Foundation.Helper.Models;
using Org.BouncyCastle.Utilities.Net;
using static Sitecore.Configuration.Settings;
using AFI.Foundation.Helper.Repositories;
using Sitecore.Publishing.Explanations;
using System.Collections.Generic;
using System.Threading;

namespace AFI.Project.Web.Pipelines
{
    public class BlockCountryPreCheck : HttpRequestProcessor
    {
        Repository repository = new Repository();
        public override void Process(HttpRequestArgs args)
        {
            try
            {
                string countryName = "";
                string ipAddress = GetIPAddress();
                Log.Info("Mehedi> ipAddress > " + ipAddress.Split(':').FirstOrDefault().Trim(), "");
               // ipAddress = "103.158.2.253";
                // Check Existing
                IpLog ipLog = repository.GetLastIPAddressInfoByIP(ipAddress.Split(':').FirstOrDefault().Trim());
                if (ipLog == null)
                {
                    Log.Info("Mehedi> ipAddress > New", "");
                    #region ipinfo

                    countryName = GetUserCountryByIp(ipAddress.Split(':').FirstOrDefault().Trim());
                    Log.Info("Mehedi> ipAddress > New country " + countryName, "");
                    #endregion



                }
                else
                {
                    Log.Info("Mehedi> ipAddress > Existing Country " + ipLog.Country, "");
                    // IP log already exists, insert the existing one with current time
                    countryName = ipLog.Country;
                    ipLog.AddedDate = DateTime.Now;
                    ipLog.Id = 0;
                    repository.InsertIPAddressInfo(ipLog);
                }

                #region Block Countries
                if (!string.IsNullOrEmpty(countryName))
                {
                    string BlockCountryList = ConfigurationManager.AppSettings["BlockCountryList"];// "BD,PK,EG,DZ";
                    Log.Info("Mehedi> County > " + countryName, "");
                    if (BlockCountryList.ToLower().Contains(countryName.ToLower()))
                    {
                        HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings["DeniedTemplate"],false);//"/access-denied.html"
                        args.AbortPipeline();
                        return;
                    }
                }
                else
                {
                    Log.Info("Mehedi> County > empty ", "");
                }
                #endregion

            }
            catch (ThreadAbortException ex)
            {
                Log.Info("Mehedi> ipAddress > ThreadAbortException: " + ex.Message, "");
            }
            catch (Exception ex)
            {
                Log.Info("Mehedi> ipAddress > Exception: " + ex.Message, "");

            }


        }



        private string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        private string GetUserCountryByIp(string ip)
        {
            CustomIpInfo ipInfo = new CustomIpInfo();

            IpLog ipLog = new IpLog();
            ipLog.IP = ip;
            ipLog.AddedDate = DateTime.Now;

            try
            {
                string APIKey = ConfigurationManager.AppSettings["IpInfoAPIKey"];
                string ipURL = ConfigurationManager.AppSettings["IpInfoAPIURL"];
                string url = string.Format("{0}?apiKey={1}&ip={2}", ipURL, APIKey, ip);

                //string url = "http://ipinfo.io/" + ip;
                Uri uri = new Uri(url);
                Log.Info("Mehedi>> url>>" + url, "");
                string info = new WebClient().DownloadString(uri);
                Log.Info("Mehedi>> info>>" + info, "");
                ipInfo = JsonConvert.DeserializeObject<CustomIpInfo>(info);
                Log.Info("Mehedi>> ipInfo>>" + (ipInfo != null), "");
                Log.Info("Mehedi>> ipInfo.Country>>" + ipInfo.Country, "");

                ipLog.Country = ipInfo.Country_code2;
                ipLog.City = ipInfo.City;
                ipLog.PostalCode = ipInfo.Postal;
                ipLog.State = ipInfo.State_prov;

            }
            catch (Exception ex)
            {
                ipInfo.Country = null;
                Log.Info("Mehedi>> ex.Message>>" + ex.Message, "");
            }

            repository.InsertIPAddressInfo(ipLog);

            return ipInfo.Country_code2;
        }
    }


}