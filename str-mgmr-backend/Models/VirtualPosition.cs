using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace str_mgmr_backend.Models
{
    /// <summary>
    /// the class contains an virtual position containing a set of physical positions
    /// </summary>
    public class VirtualPosition : Position
    {
        /// <summary>
        /// the positions, that are included in the virtual range
        /// </summary>
        public List<Guid> _Positions;

        /// <summary>
        /// the constructor creates a new instance of a virtual position
        /// </summary>
        public VirtualPosition()
        {
            _Positions = new List<Guid>();
        }

        /// <summary>
        /// the method converts the instance to a string
        /// </summary>
        /// <returns>key information</returns>
        public new string ToString()
        {
            string s = "Virtuelle Position mit " + _Positions.Count + " Positionen: ";
            foreach(Guid p in _Positions) s += p + ",";
            return s;
        }
    }
}