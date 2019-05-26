using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storage.mgr.backend.Models
{
    public class ContainerPosition
    {
        public double _X;
        public double _Y;
        public double _Z;
    }
    /// <summary>
    /// the enumeration contains the possible axis of the container position
    /// </summary>
    public enum ContainerPositionAxis
    {
        X, Y, Z
    }
}
