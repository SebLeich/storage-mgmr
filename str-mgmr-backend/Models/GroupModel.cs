using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storage.mgr.backend.Models
{
    /// <summary>
    /// the class contains an order group
    /// </summary>
    public class GroupModel
    {
        /// <summary>
        /// the identifier for the group
        /// </summary>
        public int _Id { get; set; }
        /// <summary>
        /// the color fpor the group
        /// </summary>
        public string _Color { get; set; }
    }
}
