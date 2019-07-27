using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace str_mgmr_backend.Models
{
    public class Endpoint
    {
        public int _X { get; set; }
        public int _Y { get; set; }
        public int _Z { get; set; }
        public EndpointType _Type { get; set; }
    }
}