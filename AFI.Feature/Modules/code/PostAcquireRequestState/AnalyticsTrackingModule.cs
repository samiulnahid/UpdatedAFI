using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace AFI.Feature.Modules.PostAcquireRequestState
{
    public class AnalyticsTrackingModule : IHttpModule
    {
        private readonly string AnalyticsQueryStringKey = "analytics:querystring";
        private readonly string AnalyticsRefererKey = "analytics:referer";

        public void Init(HttpApplication context)
        {
            context.PostAcquireRequestState += new EventHandler(RequestHandler);
        }

        private void RequestHandler(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            if (Sitecore.Context.Site?.Name.ToLowerInvariant() == "shell" || app.Request.Url.LocalPath.Trim('/').ToLower().StartsWith("sitecore"))
                return;
            if (app.Context.Handler is IRequiresSessionState)
            {
                var session = app.Session;
                NameValueCollection queryString = app.Request.QueryString;
                try
                {
                    if (IsValid(queryString))
                    {
                        session[AnalyticsQueryStringKey] = queryString;
                    }
                }
                catch (Exception ex)
                {
                    session[AnalyticsQueryStringKey] = queryString;

                }
               

                if (app.Request.UrlReferrer != null)
                {
                    session[AnalyticsRefererKey] = session[AnalyticsRefererKey] ?? app.Request.UrlReferrer;
                }
            }
        }

        private bool IsValid(NameValueCollection queryString)
        {
            return queryString.HasKeys() && queryString.AllKeys.Any(key => string.Equals("ResponseType", key, StringComparison.InvariantCultureIgnoreCase) || key.StartsWith("utm_"));
        }

        public void Dispose()
        {
            //
        }
    }
}
