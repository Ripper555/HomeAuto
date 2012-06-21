using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Principal;
using System.Web.Security;
using System.Configuration;

namespace HomeAuto
{
    public class Authenticator : IHttpModule
    {
         
        public void AuthenticateRequest(object source, EventArgs eventArgs)
        {
            var app = (HttpApplication)source;

            var url = app.Request.RawUrl.ToLower();

            AuthenticateServicesRequest(app);
        }

        private void AuthenticateServicesRequest(HttpApplication app)
        {
            //the Authorization header is checked if present
            string authStr = app.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authStr))
            {
                authStr = authStr.Trim();
                if (authStr.IndexOf("Basic", 0) != 0)
                {
                    //header not correct - deny request
                    DenyRequest(app);
                    return;
                }

                string encodedCredentials = authStr.Substring(6);
                byte[] decodedBytes = Convert.FromBase64String(encodedCredentials);
                string s = Encoding.UTF8.GetString(decodedBytes);
                string[] userPass = s.Split(new char[] { ':' });
                string username = userPass[0];
                string password = userPass[1];

                if (username == ConfigurationManager.AppSettings["username"] && password == ConfigurationManager.AppSettings["password"])
                {
                    // Allow Request
                    return;
                }
                else
                {
                    // Deny Request
                    DenyRequest(app);
                    return;
                }
            }
            else
            {
                // no a Authorization header, request authentication
                RequestAuthentication(app);
            }
        }

        private void RequestAuthentication(HttpApplication app)
        {
            app.Response.Clear();
            app.Response.StatusCode = 401;
            app.Response.StatusDescription = "Unauthorized";
            app.Response.AddHeader("WWW-Authenticate", "Basic Realm=\"HomeAuto\"");
            app.Response.Write("Unauthorized");
            app.Response.End();
        }

        private void DenyRequest(HttpApplication app)
        {
            app.Response.Clear();
            app.Response.StatusCode = 401;
            app.Response.StatusDescription = "Access Denied";
            app.Response.Write("Access Denied");
            app.Response.End();
        }

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(AuthenticateRequest);
        }
    }
}