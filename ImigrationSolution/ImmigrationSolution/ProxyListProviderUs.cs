using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrationSolution
{
    public class ProxyListProviderUs : IProxyListProvider
    {
        private readonly string _proxyListUrl;

        public ProxyListProviderUs(string proxySiteUrl)
        {
            _proxyListUrl = proxySiteUrl != string.Empty ? proxySiteUrl : "https://www.us-proxy.org/";
            
        }

        public AppWebProxies GetProxyList(bool onlyHttps,bool onlyAnonymous)
        {
            var webContent = WebSiteReader(_proxyListUrl);
            var allProxies = GetAllProxies(webContent);
            List<ProxyInfo> localProxies;
            if (onlyHttps && onlyAnonymous)
            {
                localProxies=allProxies.Where(proxy =>  proxy.IsAnonymous).ToList(); //proxy.IsHttps &&
            }
            else
            {
                localProxies = allProxies;
            }
            return new AppWebProxies
            {
                WebProxies =
                    localProxies.Select(proxy => new AppWebProxy(proxy.ProxyAddress, proxy.PortNumber)).ToList()
            };
           
        }

        public string WebSiteReader(string webSiteAddress)
        {
            string siteContent;
            using (var client = new WebClient())
            {
                siteContent = client.DownloadString(webSiteAddress);
            }
            return siteContent;
        }

        private ProxyInfo GetProxyInfo(string lineContent)
        {
            return null;
        }

        public List<ProxyInfo> GetAllProxies(string webSiteContent)
        {
            var body = webSiteContent.Split(new[] {"tbody","tbody"},StringSplitOptions.RemoveEmptyEntries);
            var allLines = body[1].Split(new[] {"<tr>", "</tr>"}, StringSplitOptions.RemoveEmptyEntries).Where(line=>line.Length>20);
            try
            {
                return allLines.Select(line => line.Split(new[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries)).Select(separatedTokens => new ProxyInfo
                {
                    ProxyAddress = separatedTokens[0],
                    PortNumber = int.Parse(separatedTokens[1]),
                    IsAnonymous = separatedTokens[4] == "anonymous",// || separatedTokens[4]== "transparent",
                    IsHttps = separatedTokens[6] == "yes"
                }).ToList();
            }
            catch (Exception)
            {

                return null;
            }
           
        } 

    }

    public class ProxyInfo
    {
        public string ProxyAddress { get; set; }
        public int PortNumber { get; set; }
        public bool IsHttps { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
