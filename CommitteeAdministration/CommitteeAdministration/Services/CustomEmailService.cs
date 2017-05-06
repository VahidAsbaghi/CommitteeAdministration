using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommitteeAdministration.Models;
using CommitteeAdministration.Services.Contract;
using RestSharp;
using RestSharp.Authenticators;

namespace CommitteeAdministration.Services
{
    public class CustomEmailService :ICustomEmailService
    {
        public string _apiKey = "key-da017bf3eaf2db5ac9fadecdbf2f39ce";
        public string _domainName = "asbaghi.ir";
        public async Task<IRestResponse> SendEmail(RecipientUser recipientUser,string subject, string bodyMessage, string actionTitle, string actionUrl)
        {

            var client = new RestClient
            {
                BaseUrl = new Uri("https://api.mailgun.net/v3"),
                Authenticator = new HttpBasicAuthenticator("api",
                    "key-da017bf3eaf2db5ac9fadecdbf2f39ce")
            };
            var request = new RestRequest();    
            
            request.Resource = "{domain}/messages";
            request.AddParameter("from", " ما هستیم<info@vahid.ir>");
            request.AddParameter("to", recipientUser.Email);
            request.AddParameter("subject", subject);
            var path = System.Web.HttpContext.Current.Request.MapPath("~\\EmailTemplates\\template.html");
            var template = File.ReadAllText(path);
            request.AddParameter("html", FormatEmail(template, bodyMessage, actionTitle, actionUrl));
            request.AddParameter("recipient-variables", RecipientJsonBuider(recipientUser));

            request.Method = Method.POST;
            // client.Execute(request);
            return await Task.Run(() => client.Execute(request));

        }
        private string FormatEmail(string emailTemplate, string description, string actionTitle, string actionUrl)
        {
            var emailFormatted = string.Format(emailTemplate, description, actionTitle, actionUrl);
            return emailFormatted;
        }
        private string RecipientJsonBuider(List<RecipientUser> ricipientUsers)
        {
            string json = null;
            json = "{ ";
            var last = ricipientUsers.Last();
            foreach (var item in ricipientUsers)
            {
                json += "\"" + item.Email + "\" : {\"first\" : \"" + item.FirstName + "\", \"last\" : \"" + item.LastName + "\" }";
                if (!item.Equals(last))
                    json += ",";
            }
            json += "}";
            return json;
        }

        private string RecipientJsonBuider(RecipientUser ricipientUser)
        {
            string json = null;
            json = "{ ";
            json += "\"" + ricipientUser.Email + "\" : {\"first\" : \"" + ricipientUser.FirstName + "\", \"last\" : \"" + ricipientUser.LastName + "\" }";
            json += "}";
            return json;
        }
    }
  
}