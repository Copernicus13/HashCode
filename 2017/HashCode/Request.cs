using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode
{
    public class Request
    {
        public int RequestId { get; set; }
        public int RequestsCount { get; set; }
        public Video Video { get; set; }
        public Endpoint Endpoint { get; set; }
    }
}
