using storage.mgr.backend.Algorithm;
using storage.mgr.backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storagemanager.backend.Models
{
    /// <summary>
    /// the class contains the user's input to calculate
    /// </summary>
    public class DataInput
    {
        /// <summary>
        /// the constructor generates a new instance of a data input
        /// </summary>
        public DataInput(){

        }
        /// <summary>
        /// the current container's height
        /// </summary>
        public double _ContainerHeight { get; set; }
        /// <summary>
        /// the current container's width
        /// </summary>
        public double _ContainerWidth { get; set; }
        /// <summary>
        /// contains all orders in the container
        /// </summary>
        public List<OrderModel> _Orders { get; set; }
        /// <summary>
        /// the algorithm selected for calculation
        /// </summary>
        public Algorithm _Algorithm { get; set; } = Algorithm.SuperFlo;

        /// <summary>
        /// the method checks whether the user's input is in a valid format or not
        /// </summary>
        /// <returns></returns>
        public bool isValid()
        {
            if(_ContainerHeight <= 0)
            {
                return false;
            }
            if(_ContainerWidth <= 0)
            {
                return false;
            }
            if(_Orders.Count <= 0)
            {
                return false;
            }
            return true;
        }
    }
}
