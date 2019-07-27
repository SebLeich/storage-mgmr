﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storage.mgr.backend.Models
{
    /// <summary>
    /// the class contains a good, that is located in a container
    /// </summary>
    public class GoodModel
    {
        /// <summary>
        /// the description of the good
        /// </summary>
        public string _Desc { get; set; }
        /// <summary>
        /// the height of the good
        /// </summary>
        public double _Height { get; set; }
        /// <summary>
        /// the width of the good
        /// </summary>
        public double _Width { get; set; }
        /// <summary>
        /// the length of the good in the container
        /// </summary>
        public double _Length { get; set; }
        /// <summary>
        /// the x position of the good in the container
        /// </summary>
        public double _X { get; set; }
        /// <summary>
        /// the y position of the good in the container
        /// </summary>
        public double _Y { get; set; }
        /// <summary>
        /// the z position of the good in the container
        /// </summary>
        public double _Z { get; set; }
        /// <summary>
        /// the group id
        /// </summary>
        public int _Group { get; set; }
        /// <summary>
        /// is stackable
        /// </summary>
        public bool _Stack { get; set; } = false;
        /// <summary>
        /// is rotatable
        /// </summary>
        public bool _Rotate { get; set; } = false;
        /// <summary>
        /// is the good rotated
        /// </summary>
        public bool _IsRotated { get; set; } = false;
        /// <summary>
        /// the goods sequence number of order
        /// </summary>
        public int _SequenceNr { get; set; } = 0;
        /// <summary>
        /// the calculated right position of the good
        /// </summary>
        public double _R
        {
            get
            {
                if (_IsRotated)
                {
                    return _X + _Length;
                } else
                {
                    return _X + _Width;
                }
            }
        }
        /// <summary>
        /// the calculated top position of the good
        /// </summary>
        public double _T
        {
            get
            {
                return _Y + _Height;
            }
        }
        /// <summary>
        /// the method returns the front position of the good
        /// </summary>
        public double _F
        {
            get
            {
                if (_IsRotated)
                {
                    return _Z + _Width;
                } else
                {
                    return _Z + _Length;
                }
            }
        }
    }
}
