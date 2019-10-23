using storage.mgr.backend.Models;
using storagemanager.backend.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace storage.mgr.backend.Algorithm
{
    abstract public class GenericAlgorithm
    {
        private static readonly Random _Rand = new Random();

        public ContainerPositionAxis _First;

        public ContainerPositionAxis _Second;

        public ContainerPositionAxis _Third;

        abstract public SolutionModel calculate(DataInput _Input);

        public DateTime _ProcedureCall;

        public DateTime _ProcedureEnd;

        /// <summary>
        /// the method analysis the datasets to extract all groups and set a random color to them
        /// </summary>
        /// <param name="_Orders"></param>
        /// <returns></returns>
        public List<GroupModel> AnalyzeGroups(List<OrderModel> _Orders)
        {
            List<GroupModel> _Output = new List<GroupModel>();
            foreach(OrderModel _Model in _Orders)
            {
                if(_Output.Find(x => x._Id == _Model._Group) == null)
                {
                    _Output.Add(new GroupModel()
                    {
                        _Id = _Model._Group,
                        _Color = String.Format("#{0:X6}", _Rand.Next(0x1000000))
                    });
                }
            }
            return _Output;
        }
    }
    /// <summary>
    /// the enumeration contains all available algorithms
    /// </summary>
    public enum Algorithm
    {
        AllInOneRow, StartLeftBottom, SuperFlo
    }
}
