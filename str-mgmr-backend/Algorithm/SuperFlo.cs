using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using storage.mgr.backend.Models;
using storagemanager.backend.Models;
using str_mgmr_backend.Models;

namespace storage.mgr.backend.Algorithm
{
    public class SuperFlo : GenericAlgorithm
    {
        public SuperFlo()
        {
            _First = ContainerPositionAxis.Y;
            _Second = ContainerPositionAxis.Z;
            _Third = ContainerPositionAxis.X;
        }

        public override SolutionModel calculate(DataInput _Input)
        {
            SolutionModel _Model = new SolutionModel();
            ContainerModel _Container = new ContainerModel()
            {
                _Height = _Input._ContainerHeight,
                _Width = _Input._ContainerWidth
            };
            _Model._Container = _Container;
            _Model._Algorithm = this;
            _Model._Groups = AnalyzeGroups(_Input._Orders);
            _Model._Groups.OrderBy(x => x._Id);
            Room _R = new Room((int) _Input._ContainerHeight, (int) _Input._ContainerWidth);
            foreach(OrderModel _O in _Input._Orders)
            {
                for (var i = 0; i < _O._Quantity; i++)
                {
                    int start = _R._Positions.Max(x => x.index);
                    _R.AddOrder(_O, start);
                }
            }
            _Model._Empty = _R._Positions;
            _Model._Container._Goods = _R._Goods;
            _Model._Container._Length = _R.Length;
            _Model._Steps.AddRange(_R._Steps);
            return _Model;
        }
    }
}
