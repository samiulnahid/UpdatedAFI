using Newtonsoft.Json;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.Web.UI.Sheer;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.ExperienceForms.Samples.SubmitActions
{

    public class SitecoreSend : SubmitActionBase<string>
    {
        private readonly Uri baseAddress = new Uri("https://api.moosend.com/v3/");
        private readonly string emailListID = "da0ab24e-c2ee-4ad0-98ea-11f5ef46b7d4"; // Add email list ID here, refer above images
        private readonly string SitecoreSendApiKey = "5397ed3f-a438-4b1c-a39a-52025ff82383"; // Add sitecore send api key here, refer above images

        public SitecoreSend(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, "formSubmitContext");
            var txtFirstName = _getValue(formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("FirstName")));
            var txtLastName = _getValue(formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("LastName")));
            var txtEmail = _getValue(formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Email")));
            var txtMobile = _getValue(formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Mobile")));
            var txtMessage = _getValue(formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Message")));

            var sitecoreSendModel = new SitecoreSendModel
            {
                Name = txtFirstName + " " + txtLastName,
                Email = txtEmail,
                Mobile = txtMobile,
                Comments = txtMessage
            };
            Task<bool> executePost = Task.Run(async () => await ExecuteCall(JsonConvert.SerializeObject(sitecoreSendModel)));
            return executePost.Result;
        }

        private async Task<bool> ExecuteCall(string data)
        {
            try
            {
                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                    using (var content = new StringContent(data, Encoding.Default, "application/json"))
                    {
                        using (var response = await httpClient.PostAsync($"subscribers/{emailListID}/subscribe.json?apiKey={SitecoreSendApiKey}", content))
                        {
                            string response_ = await response.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(response_))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message.ToString(), "ExecuteCall");
                return false;
            }
        }

        public string GetFieldValue(IViewModel field)
        {
            if (field != null && field is ListViewModel)
            {
                PropertyInfo property = field.GetType().GetProperty("Value");
                string str = (object)property != null ? property.GetValue(field, (object[])null)?.ToString() : (string)null;
                return str;
            }
            else
            {
                return null;
            }
            
        }
        public string _getValue(IViewModel field)
        {
            var property = field.GetType().GetProperty("Value");
            var data = (object)property != null ? property.GetValue(field, (object[])null)?.ToString() : (string)null;
            return data.ToString();
        }
    }
    public partial class SitecoreSendModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Comments { get; set; }
    }
}
