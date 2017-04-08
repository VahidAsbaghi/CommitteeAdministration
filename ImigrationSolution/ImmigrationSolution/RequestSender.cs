using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace ImmigrationSolution
{
    public  class RequestSender
    {
        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        bool settingsReturn, refreshReturn;
        public static volatile BlockingCollection<Request> SuccessfullList=new BlockingCollection<Request>();
        public static volatile int ResponseNumber = 0;
        private string usersAheadOfYou = "usersInLineAheadOfYou";
        public QueueResponse Send(Request request,int index,int repeatCount)
        {
            var webRequest = WebRequest.CreateHttp(request.Address);
           // webRequest.Timeout = 20000;
            //AppWebProxies webProxies=new AppWebProxies();
           // webProxies.WebProxies.Add(new AppWebProxy());
            WebProxy proxy = SetWebProxy(request.WebProxy);
            webRequest.AllowAutoRedirect = true;
            webRequest.Proxy = proxy;
            webRequest.Method =request.Type.ToString();
            int queueNumber = -1;
            try
            {
                
                var response = webRequest.GetResponse();
                // Get the stream containing content returned by the server.  
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                if (dataStream != null)
                {
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.  
                    string responseFromServer = reader.ReadToEnd();
                    queueNumber = CatchQueueNumber(responseFromServer);
                    // Clean up the streams and the response.  
                    reader.Close();
                }
                response.Close();
                SuccessfullList.Add(request);
                return new QueueResponse() {QueueNumber = queueNumber, WebResponse = response,ResponseNumber = Interlocked.Increment(ref ResponseNumber)};
            }
            //catch (WebException exception)
            //{
            //    if (exception.Status==WebExceptionStatus.Timeout)
            //    {
            //        if (repeatCount>3)
            //        {
            //            return null;
            //        }
            //        repeatCount = repeatCount + 1;
            //        var random=new Random(DateTime.Now.Millisecond);
            //        var newRequest = request;
            //        if (SuccessfullList.Count>0)
            //        {
            //            newRequest = SuccessfullList.ToList()[random.Next(0, SuccessfullList.Count)]; //new Request() {Address = request.Address,Type = request.Type,WebProxy = new AppWebProxy("127.0.0.1",8580)}; //
            //        }
                    
            //        return Send(newRequest,index,repeatCount);
            //    }
            //    return null;
            //}
            catch (Exception e)
            {
                if (repeatCount > 1)
                {
                    return null;
                }
                repeatCount = repeatCount + 1;
                var random = new Random(DateTime.Now.Millisecond);
                var newRequest = request;
                if (SuccessfullList.Count > 0)
                {
                    newRequest = SuccessfullList.ToList()[random.Next(0, SuccessfullList.Count)]; //new Request() {Address = request.Address,Type = request.Type,WebProxy = new AppWebProxy("127.0.0.1",8580)}; //
                }

                return Send(newRequest, index, repeatCount);

            }
            
            
        }
        public  Task<WebResponse> SendAsync(Request request)
        {
            return Task<WebResponse>.Factory.StartNew(() =>
            {
                var webRequest = WebRequest.CreateHttp(request.Address);
                webRequest.Method = request.Type.ToString();
                var response = webRequest.GetResponse();
                return response;
            });
        }

        private int CatchQueueNumber(string responseFromServer)
        {

            var index=responseFromServer.IndexOf(usersAheadOfYou, StringComparison.Ordinal)+usersAheadOfYou.Length+3;

            return int.Parse(responseFromServer[index].ToString());
        }
        private WebProxy SetWebProxy(AppWebProxy webProxy)
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            registry.SetValue("ProxyEnable", 1);
            registry.SetValue("ProxyServer", webProxy.ProxyAddress + ":" + webProxy.PortNumber);
            settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
            return new WebProxy(webProxy.ProxyAddress, webProxy.PortNumber) { BypassProxyOnLocal = false };
        }

    }

    public class QueueResponse
    {
        public WebResponse WebResponse { get; set; }
        public int QueueNumber { get; set; }
        public int ResponseNumber { get; set; }
    }
}
