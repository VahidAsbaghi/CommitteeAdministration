using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrationSolution
{
    public class ProxyListProviderHidymy:IProxyListProvider
    {
        private readonly List<string> _proxyListUrl=new List<string>();

        private List<int> subAddressList = new List<int>()
        {
            0,
            64,
            128,
            192,
            256,
            256 + 64,
            256 + 128,
            256 + 192,
            512,
            512 + 64,
            512 + 128,
            512 + 192,
            512 + 256
        };
         
        private readonly string _proxyListBaseUrl= "https://hidemy.name/en/proxy-list/?start={0}#list";

        public ProxyListProviderHidymy(string proxySiteUrl)
        {
            foreach (var subAddress in subAddressList)
            {
                _proxyListUrl.Add(string.Format(_proxyListBaseUrl,subAddress));
            }
        }

        public AppWebProxies GetProxyList(bool onlyHttps, bool onlyAnonymous)
        {
            var webProxies=new AppWebProxies();
            foreach (var proxyAddress in _proxyListUrl)
            {
                var webContent = WebSiteReader(proxyAddress);
                var allProxies = GetAllProxies(webContent);
                List<ProxyInfo> localProxies;
                if (onlyHttps && onlyAnonymous)
                {
                    localProxies = allProxies.Where(proxy => proxy.IsAnonymous).ToList(); //proxy.IsHttps &&
                }
                else
                {
                    localProxies = allProxies;
                }
                foreach (var localProxy in localProxies)
                {
                    webProxies.WebProxies.Add(new AppWebProxy(localProxy.ProxyAddress,localProxy.PortNumber));
                }
            }
            return webProxies;
        }

        public string WebSiteReader(string siteAddress)
        {
            string siteContent;
            using (var client = new WebClient())
            {
                siteContent = client.DownloadString(siteAddress);
            }
            return siteContent;
        }

        public List<ProxyInfo> GetAllProxies(string webSiteContent)
        {
            var body = webSiteContent.Split(new[] { "tbody", "tbody" }, StringSplitOptions.RemoveEmptyEntries);
            var allLines = body[1].Split(new[] { "<tr>", "</tr>" }, StringSplitOptions.RemoveEmptyEntries).Where(line => line.Length > 20);
            try
            {
                return allLines.Select(line => line.Split(new[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries)).Select(separatedTokens => new ProxyInfo
                {
                    ProxyAddress = separatedTokens[0],
                    PortNumber = int.Parse(separatedTokens[1]),
                    IsAnonymous = separatedTokens[5] == "High" &&int.Parse(separatedTokens[3].Split(new [] {"<p>","</p>"},StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ms",""))<1000,
                    IsHttps = true
                }).ToList();
            }
            catch (Exception)
            {

                return null;
            }

        }
    }
}
