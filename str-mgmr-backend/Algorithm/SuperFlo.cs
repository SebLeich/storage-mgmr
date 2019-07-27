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
    /// <summary>
    /// the class contains the super flo algorithm
    /// </summary>
    public class SuperFlo : GenericAlgorithm
    {
        /// <summary>
        /// the flag controls whether the response contains process information
        /// </summary>
        public bool loadSequenceInfos { get; set; } = true;

        /// <summary>
        /// the constructor creates a new instance of the super flo's algorithm
        /// </summary>
        public SuperFlo()
        {
            _First = ContainerPositionAxis.Y;
            _Second = ContainerPositionAxis.Z;
            _Third = ContainerPositionAxis.X;
        }

        /// <summary>
        /// the method calculates the algorithm's solution according to the heuristic
        /// </summary>
        /// <param name="_Input"></param>
        /// <returns></returns>
        public override SolutionModel calculate(DataInput _Input)
        {
            SolutionModel _Model = new SolutionModel();
            ContainerModel _Container = new ContainerModel()
            {
                _Height = _Input._ContainerHeight,
                _Width = _Input._ContainerWidth
            };
            _Model._Container = _Container;
            _Model._Algorithm = "SuperFlo";
            _Model._Groups = AnalyzeGroups(_Input._Orders);
            _Model._Groups.OrderBy(x => x._Id);
            Room _R = new Room((int) _Input._ContainerHeight, (int) _Input._ContainerWidth);
            int seqNr = 1;
            foreach(GroupModel g in _Model._Groups)
            {
                List<OrderModel> _Orders = _Input._Orders.Where(x => x._Group == g._Id).OrderByDescending(x => x.area).ToList();
                foreach (OrderModel _O in _Orders)
                {
                    for (var i = 0; i < _O._Quantity; i++)
                    {
                        int start = _R._Positions.Max(x => x.index);
                        _R.AddOrder(_O, start, seqNr, loadSequenceInfos);
                        seqNr++;
                    }
                }
            }
            _Model._Empty = _R._Positions;
            _Model._Container._Goods = _R._Goods;
            _Model._Container._Length = _R.Length;
            _Model._Steps = _R._Steps;
            return _Model;
        }
    }
}
