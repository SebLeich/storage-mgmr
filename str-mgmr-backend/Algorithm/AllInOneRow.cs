using storage.mgr.backend.Models;
using storagemanager.backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storage.mgr.backend.Algorithm
{
    /// <summary>
    /// the algorihm s
    /// </summary>
    public class AllInOneRow : GenericAlgorithm
    {
        /// <summary>
        /// the flag stores the stack flag
        /// </summary>
        public bool _StackSame { get; set; } = true;
        /// <summary>
        /// the constructor creates a new instance of the algorihm
        /// </summary>
        /// <param name="_StackSame">enable stackable</param>
        public AllInOneRow(bool _StackSame)
        {
            _First = ContainerPositionAxis.Y;
            _Second = ContainerPositionAxis.Z;
            _Third = ContainerPositionAxis.X;
            this._StackSame = _StackSame;
        }
        /// <summary>
        /// the method calculates the solution for the given data input
        /// </summary>
        /// <param name="_Input">user's input</param>
        /// <returns>solution</returns>
        public override SolutionModel calculate(DataInput _Input)
        {
            SolutionModel _Model = new SolutionModel();
            _Model._ProcedureCall = DateTime.Now;
            ContainerModel _Container = new ContainerModel()
            {
                _Height = _Input._ContainerHeight,
                _Width = _Input._ContainerWidth
            };
            _Model._Container = _Container;
            _Model._Algorithm = "AllInOneRow";
            _Model._Groups = AnalyzeGroups(_Input._Orders);
            foreach (GroupModel _Group in _Model._Groups)
            {
                List<OrderModel> _OrdersForGroup = _Input._Orders.Where(x => x._Group == _Group._Id).ToList();
                _OrdersForGroup.OrderByDescending(x => x.GetBiggestWidth());
                foreach (OrderModel _C in _OrdersForGroup)
                {
                    for(var i = 0; i < _C._Quantity; i++)
                    {
                        if (_StackSame)
                        {
                            double _Z = 0.0;
                            double _Y = 0.0;
                            GoodModel _Last = _Container.GetLastElement();
                            if(_Last != null && _Last._Desc == _C._Description && _StackSame && _Container.CanStackOn(_C, _Last))
                            {
                                _Y = _Last._Y + _Last._Height;
                                _Z = _Last._Z;
                            } else
                            {
                                _Z = _Container._Length;
                            }
                            GoodModel _Good = new GoodModel();
                            _Good._Desc = _C._Description;
                            _Good._Rotate = _C._Rotate;
                            _Good._Stack = _C._Stack;
                            _Good._Group = _Group._Id;
                            _Good._Height = _C._Height;
                            _Good._Length = _C._Length;
                            _Good._Width = _C._Width;
                            _Good._X = 0;
                            _Good._Y = _Y;
                            _Good._Z = _Z;
                            _Container.AddGood(_Good);
                        } else
                        {
                            GoodModel _Good = new GoodModel();
                            _Good._Desc = _C._Description;
                            _Good._Group = _Group._Id;
                            _Good._Height = _C._Height;
                            _Good._Length = _C._Length;
                            _Good._Width = _C._Width;
                            _Good._X = 0;
                            _Good._Y = 0;
                            _Good._Z = _Container._Length;
                            _Container.AddGood(_Good);
                        }
                    }
                }
            }
            _Model._ProcedureEnd = DateTime.Now;
            return _Model;
        }
    }
}

