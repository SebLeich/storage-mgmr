using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storage.mgr.backend.Models
{
    /// <summary>
    /// the class contains a container, in which the goods are placed
    /// </summary>
    public class ContainerModel
    {
        /// <summary>
        /// the width of a container
        /// </summary>
        public double _Width { get; set; }
        /// <summary>
        /// the height of a container
        /// </summary>
        public double _Height { get; set; }
        /// <summary>
        /// the length of a container
        /// </summary>
        public double _Length { get; set; } = 0;
        /// <summary>
        /// a set of goods that are contained in the container
        /// </summary>
        public List<GoodModel> _Goods { get; set; } 
        /// <summary>
        /// the constructor creates a new instance of a container
        /// </summary>
        public ContainerModel()
        {
            _Goods = new List<GoodModel>();
        }
        /// <summary>
        /// the method adds a good to the container model
        /// </summary>
        public void AddGood(GoodModel _Good)
        {
            _Goods.Add(_Good);
            double _Endpoint = _Good._Z + _Good._Length;
            if(_Endpoint > _Length)
            {
                _Length = _Endpoint;
            }
        }
        /// <summary>
        /// the method returns if element1 can be stacked next to element2
        /// </summary>
        /// <returns></returns>
        public bool CanStackNext(OrderModel _New, GoodModel _Last)
        {
            if (_Last == null)
            {
                return false;
            }
            if (!_New._Stack)
            {
                return false;
            }
            if (!_Last._Stack)
            {
                return false;
            }
            if ((_Last._X + _Last._Width + _New._Width) <= _Width)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// the method returns if element1 can be stacked on element2
        /// </summary>
        /// <returns></returns>
        public bool CanStackOn(OrderModel _New, GoodModel _Last)
        {
            if(_Last == null)
            {
                return false;
            }
            if (!_New._Stack)
            {
                return false;
            }
            if (!_Last._Stack)
            {
                return false;
            }
            if((_Last._Y + _Last._Height + _New._Height) <= _Height)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// updated 27.04.2019
        /// the method returns the last element that was added to the container
        /// </summary>
        /// <returns></returns>
        public GoodModel GetLastElement()
        {
            if(_Goods.Count > 0)
            {
                return _Goods.Last();
            }
            return null;
        }
        /// <summary>
        /// the method returns the next free container position according to the algorithm's preferences
        /// </summary>
        /// <returns></returns>
        public ContainerPosition GetPositionsInContainer(double _Width, double _Length, double _Height)
        {
            if(_Goods.Count == 0)
            {
                if(this._Height >= _Height && this._Width >= _Width)
                {
                    return new ContainerPosition() { _X = 0.0, _Y = 0.0, _Z = 0.0 };
                } else
                {
                    return null;
                }
            }

            return null;
        }
    }
}
