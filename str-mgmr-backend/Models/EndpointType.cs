using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace str_mgmr_backend.Models
{
    /// <summary>
    /// the class contains the type of an endpoint
    /// </summary>
    public enum EndpointType
    {
        LEFTBOTTOMBEHIND,
        LEFTBOTTOMINFRONT,
        RIGHTBOTTOMBEHIND,
        RIGHTBOTTOMINFRONT,
        LEFTTOPBEHIND,
        LEFTTOPINFRONT,
        RIGHTTOPBEHIND,
        RIGHTTOPINFRONT
    }
}