using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storage.mgr.backend.Models
{
    /// <summary>
    /// the class contains the model of an order
    /// </summary>
    public class OrderModel
    {
        /// <summary>
        /// the id of the order
        /// </summary>
        public int _Id { get; set; }
        /// <summary>
        /// the description of the good
        /// </summary>
        public string _Description { get; set; }
        /// <summary>
        /// the quantity of the order
        /// </summary>
        public int _Quantity { get; set; }
        /// <summary>
        /// the length of the good
        /// </summary>
        public int _Length { get; set; }
        /// <summary>
        /// the width of the good
        /// </summary>
        public int _Width { get; set; }
        /// <summary>
        /// the height of the good
        /// </summary>
        public int _Height { get; set; }
        /// <summary>
        /// is the good rotateable?
        /// </summary>
        public bool _Rotate { get; set; }
        /// <summary>
        /// is the good stackable?
        /// </summary>
        public bool _Stack { get; set; }
        /// <summary>
        /// the id of the group
        /// </summary>
        public int _Group { get; set; }

        /// <summary>
        /// the method returns the biggest possible width of the good
        /// </summary>
        /// <returns></returns>
        public double GetBiggestWidth()
        {
            if(_Rotate && _Length > _Width)
            {
                return _Length;
            }
            return _Width;
        }

        public string ToString()
        {
            return _Description + ", Maß (" + _Width + ", " + _Height + ", " + _Length + ")";
        }
    }
}
