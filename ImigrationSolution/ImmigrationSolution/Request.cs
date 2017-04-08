using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrationSolution
{
    public class Request
    {
        public enum Method
        {
            Post,
            Get
        }

        public Method Type { get; set; }
        public string Address { get; set; }
        public AppWebProxy WebProxy { get; set; }
    }
}
