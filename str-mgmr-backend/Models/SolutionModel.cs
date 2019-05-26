using storage.mgr.backend.Algorithm;
using str_mgmr_backend.Core;
using str_mgmr_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storage.mgr.backend.Models
{
    /// <summary>
    /// updated 26.04.2019
    /// the class contains the model of a calculated solution
    /// </summary>
    public class SolutionModel
    {
        /// <summary>
        /// the container with the calculated length
        /// </summary>
        public ContainerModel _Container { get; set; }
        /// <summary>
        /// the groups, that where detected while analyzing
        /// </summary>
        public List<GroupModel> _Groups { get; set; }
        /// <summary>
        /// the used algorithm for the calculation
        /// </summary>
        public GenericAlgorithm _Algorithm { get; set; }
        /// <summary>
        /// the list of empty places
        /// </summary>
        public List<Position> _Empty { get; set; }

        public List<Step> _Steps { get; set; }

        public SolutionModel()
        {
            _Steps = new List<Step>();
        }
    }
}
