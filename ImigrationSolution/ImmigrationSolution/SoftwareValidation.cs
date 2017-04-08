using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImmigrationSolution
{
    public class SoftwareValidation
    {
        private const string BaseUrl = @"http://localhost:9080";//@"http://localhost:9080"; //@"http://mivatejarat.com"

        public   void SendMacAddress(string macAddress1)
        {
            //using (var client = new WebClient())
            //{

            //    client.BaseAddress = BaseUrl;
            //    //client.DefaultRequestHeaders.Accept.Clear();
            //    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text"));
            //    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            //   // StringContent content = new StringContent(macAddress,Encoding.Unicode, "application/x-www-form-urlencoded");
            //    // HTTP POST
            //    var response =  client.UploadString("api/DeviceInfo","POST", macAddress);

            //}
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //var content = new FormUrlEncodedContent(new[]
                //{
                //    new KeyValuePair<string, string>("macAddress=", macAddress1)
                //});
                var content =new StringContent(JsonConvert.SerializeObject( new DeviceInfoDTO() {DeviceInfo = macAddress1}),Encoding.UTF8,"application/json");
                var result = client.PostAsync("/api/DeviceInfo", content).Result;

            }
            //string URI = "http://localhost:9080/api/DeviceInfo/";
            //string myParameters = "macAddress="+macAddress1;

            //using (WebClient wc = new WebClient())
            //{
            //    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            //    string HtmlResult = wc.UploadString(URI, myParameters);
            //}
        }

        public bool CheckValidation(string macAddress)
        {
            WebRequest req = WebRequest.Create(BaseUrl+"/api/Validation?macAddress=" + macAddress);
            req.Method = "GET";
            //req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("username:password"));
            //req.Credentials = new NetworkCredential("username", "password");
            HttpWebResponse resp=null;
            try
            {
                 resp = req.GetResponse() as HttpWebResponse;
            }
            catch (WebException exception)
            {
                
            }
            if (resp != null && resp.StatusCode == HttpStatusCode.Accepted)
            {
                return true;
            }
            
            return false;
        }

    }

    public class DeviceInfoDTO
    {
        public string DeviceInfo { get; set; }
    } 
}
