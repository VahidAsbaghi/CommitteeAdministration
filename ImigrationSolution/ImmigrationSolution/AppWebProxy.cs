using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ImmigrationSolution
{
    public  class AppWebProxy
    {
        public  string ProxyAddress { get; private set; }
        public  int PortNumber { get;private set; }
        
        public AppWebProxy(string address,int port)
        {
            ProxyAddress = address;
            PortNumber = port;
            
        }


    }

    public class AppWebProxies
    {
        public List<AppWebProxy> WebProxies { get; set; }
    }
}
