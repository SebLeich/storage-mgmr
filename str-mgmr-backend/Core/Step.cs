using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace str_mgmr_backend.Core
{
    public class Step
    {
        public string desc { get; set; }

        public DateTime ts { get; set; }

        public Step()
        {
            ts = DateTime.Now;
        }
    }
}