using System.Collections.Generic;

namespace ImmigrationSolution
{
    public interface IProxyListProvider
    {
        AppWebProxies GetProxyList(bool onlyHttps, bool onlyAnonymous);
        string WebSiteReader(string siteAddress);
        List<ProxyInfo> GetAllProxies(string webSiteContent);
    }
}