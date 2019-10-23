using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace str_mgmr_backend.Models
{
    /// <summary>
    /// a sequence step contains the information of a done algorithm process
    /// </summary>
    public class SequenceStep
    {
        /// <summary>
        /// the step number
        /// </summary>
        public int _SequenceNumber { get; set; }
        /// <summary>
        /// a list of collected process information
        /// </summary>
        public List<string> _Messages { get; set; }
        /// <summary>
        /// a list of positions created by the algorithm's process
        /// </summary>
        public List<Position> _Positions { get; set; }
        /// <summary>
        /// a list of group restricted positions
        /// </summary>
        public List<Position> _RecursiveGroupRestricted { get; set; }
        /// <summary>
        /// the constructor creates a new instance of a sequence step
        /// </summary>
        /// <param name="_SeqNr">number of the step</param>
        public SequenceStep(int _SeqNr, List<string> _Messages, List<Position> _Positions, List<Position> _GroupRestrictions)
        {
            _SequenceNumber = _SeqNr;
            this._Messages = _Messages;
            this._Positions = _Positions;
            _RecursiveGroupRestricted = _GroupRestrictions;
        }
    }
}