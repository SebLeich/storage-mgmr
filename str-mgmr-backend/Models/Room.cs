using storage.mgr.backend.Models;
using str_mgmr_backend.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace str_mgmr_backend.Models
{
    /// <summary>
    /// the class is used for minimum calculation
    /// </summary>
    public class MinObject
    {
        /// <summary>
        /// currently best found position
        /// </summary>
        public Position selected { get; set; } = null;
        /// <summary>
        /// all criteria, that are relevant for comparison
        /// </summary>
        public Dictionary<string, double> criteria { get; set; }
        /// <summary>
        /// the constructor creates a new instance of a min object
        /// </summary>
        public MinObject()
        {
            criteria = new Dictionary<string, double>();
        }
    }
    /// <summary>
    /// the class represents a helper class for the sweep algorithm
    /// </summary>
    public class SweepHelper
    {
        /// <summary>
        /// the value of the piece
        /// </summary>
        public Nullable<int> value { get; set; }
        /// <summary>
        /// is the helper a left/right | front/back
        /// </summary>
        public bool isFront { get; set; }
        /// <summary>
        /// the position the helper is referencing
        /// </summary>
        public Position reference { get; set; }
    }
    /// <summary>
    /// a possible positions
    /// </summary>
    public class Possibilities
    {
        /// <summary>
        /// list of all (not rotated, physical) positions
        /// </summary>
        public List<Position> _NonR = new List<Position>();
        /// <summary>
        /// list of rotated physical positions
        /// </summary>
        public List<Position> _R = new List<Position>();
        /// <summary>
        /// list of virtual positions
        /// </summary>
        public List<VirtualPosition> _V = new List<VirtualPosition>();

        public int Count { get { return _NonR.Count + _R.Count + _V.Count;  } }

        /// <summary>
        /// the method returns the first position of the possibilities
        /// </summary>
        /// <returns></returns>
        public Position First()
        {
            if(_NonR.Count > 0)
            {
                return _NonR.First();
            } else
            {
                return _R.FirstOrDefault();
            }
        }

        /// <summary>
        /// the method creates all virtual positions
        /// </summary>
        /// <returns></returns>
        public static List<VirtualPosition> MergePositions(List<Position> positions, OrderModel order)
        {
            List<VirtualPosition> _Output = new List<VirtualPosition>();
            foreach (int vert in positions.Select(x => x._Y).Distinct().ToList())
            {
                List<Position> pos = positions.Where(x => x._Y == vert).ToList();
                foreach (IGrouping<int, Position> gs in pos.GroupBy(x => x._X).ToList())
                {
                    List<Position> sameX = new List<Position>();
                    foreach (Position p in gs) sameX.Add(p);
                    if(sameX.Count > 1)
                    {
                        foreach (IGrouping<int, Position> gsr in sameX.GroupBy(x => x._R).ToList())
                        {
                            List<Position> sameXAndR = new List<Position>();
                            foreach (Position p in gsr) sameXAndR.Add(p);
                            if (sameXAndR.Count > 1)
                            {
                                List<SweepHelper> sweeps = new List<SweepHelper>();
                                foreach (Position p in sameXAndR)
                                {
                                    sweeps.Add(new SweepHelper()
                                    {
                                        isFront = false,
                                        reference = p,
                                        value = p._Z
                                    });
                                    sweeps.Add(new SweepHelper()
                                    {
                                        isFront = true,
                                        reference = p,
                                        value = p._F
                                    });
                                }
                                sweeps.Sort((a, b) =>
                                {
                                    if (a.value.HasValue && b.value.HasValue)
                                    {
                                        if (a.value > b.value) return 1;
                                        else if (a.value < b.value) return -1;
                                        else
                                        {
                                            if (a.isFront) return 1;
                                            return -1;
                                        }
                                    }
                                    if (a.value.HasValue && !b.value.HasValue) return -1;
                                    else if (!a.value.HasValue && b.value.HasValue) return 1;
                                    else
                                    {
                                        if (a.isFront) return 1;
                                        return -1;
                                    }
                                });
                                int L = 0;
                                int R = 0;
                                List<Position> swPos = new List<Position>();
                                foreach(SweepHelper s in sweeps)
                                {
                                    if (!swPos.Contains(s.reference)) swPos.Add(s.reference);
                                    if (s.isFront) R++;
                                    else L++;
                                    if(L-R == 0)
                                    {
                                        if(swPos.Count > 1)
                                        {
                                            _Output.Add(new VirtualPosition()
                                            {
                                                _Positions = swPos.Select(x => x._Id).ToList()
                                            });
                                        }
                                        swPos.Clear();
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (IGrouping<int, Position> gs in pos.GroupBy(x => x._Z).ToList())
                {
                    List<Position> sameZ = new List<Position>();
                    foreach (Position p in gs)
                    {
                        if(p._L != null) sameZ.Add(p);
                    }
                    if (sameZ.Count > 1)
                    {
                        foreach (IGrouping<Nullable<int>, Position> gsr in sameZ.GroupBy(x => x._F).ToList())
                        {
                            List<Position> sameZAndF = new List<Position>();
                            foreach (Position p in gsr) sameZAndF.Add(p);
                            if (sameZAndF.Count > 1)
                            {
                                List<SweepHelper> sweeps = new List<SweepHelper>();
                                foreach (Position p in sameZAndF)
                                {
                                    sweeps.Add(new SweepHelper()
                                    {
                                        isFront = false,
                                        reference = p,
                                        value = p._X
                                    });
                                    sweeps.Add(new SweepHelper()
                                    {
                                        isFront = true,
                                        reference = p,
                                        value = p._R
                                    });
                                }
                                sweeps.Sort((a, b) =>
                                {
                                    if (a.value.HasValue && b.value.HasValue)
                                    {
                                        if (a.value > b.value) return 1;
                                        else if (a.value < b.value) return -1;
                                        else
                                        {
                                            if (a.isFront) return 1;
                                            return -1;
                                        }
                                    }
                                    if (a.value.HasValue && !b.value.HasValue) return -1;
                                    else if (!a.value.HasValue && b.value.HasValue) return 1;
                                    else
                                    {
                                        if (a.isFront) return 1;
                                        return -1;
                                    }
                                });
                                int L = 0;
                                int R = 0;
                                List<Position> swPos = new List<Position>();
                                foreach (SweepHelper s in sweeps)
                                {
                                    if (!swPos.Contains(s.reference)) swPos.Add(s.reference);
                                    if (s.isFront) R++;
                                    else L++;
                                    if (L - R == 0)
                                    {
                                        if (swPos.Count > 1)
                                        {
                                            _Output.Add(new VirtualPosition()
                                            {
                                                _Positions = swPos.Select(x => x._Id).ToList()
                                            });
                                        }
                                        swPos.Clear();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return _Output;
        }

        /// <summary>
        /// the method returns the position with the lowest z-coordinate (start)
        /// </summary>
        /// <returns></returns>
        public Position Min(OrderModel _Order)
        {
            Position _Output = null;
            foreach(Position position in _NonR)
            {
                if(_Output == null)
                {
                    _Output = position;
                } else
                {
                    int f1 = position.GetFrontCoordinate(_Order); // front of current position
                    int f2 = _Output.GetFrontCoordinate(_Order); // front of found position
                    if (f1 < f2)
                    {
                        _Output = position;
                    } else if (f1 == f2)
                    {
                        if (position.GetLeftCoordinate(_Order) < _Output.GetLeftCoordinate(_Order))
                        {
                            _Output = position;
                        }
                    }
                }
            }
            foreach (Position position in _R)
            {
                if (_Output == null)
                {
                    _Output = position;
                }
                else
                {
                    int f1 = position.GetFrontCoordinate(_Order); // front of current position
                    int f2 = _Output.GetFrontCoordinate(_Order); // front of found position
                    if (f1 < f2)
                    {
                        _Output = position;
                    }
                    else if (f1 == f2)
                    {
                        if (position.GetLeftCoordinate(_Order) < _Output.GetLeftCoordinate(_Order))
                        {
                            _Output = position;
                        }
                    }
                }
            }
            return _Output;
        }

        /// <summary>
        /// the method returns the position with the smallest area
        /// </summary>
        /// <returns></returns>
        public Position Min2(OrderModel _Order)
        {
            Position _Output = null;
            foreach(Position p in _R)
            {
                if (_Output == null) _Output = p;
                else
                {
                    if(p.area != null)
                    {
                        if (_Output.area == null) _Output = p;
                        else
                        {
                            if (_Output.area > p.area) _Output = p;
                        }
                    }
                }
            }
            foreach (Position p in _NonR)
            {
                if (_Output == null) _Output = p;
                else
                {
                    if (p.area != null)
                    {
                        if (_Output.area == null) _Output = p;
                        else
                        {
                            if (_Output.area > p.area) _Output = p;
                        }
                    }
                }
            }
            return _Output;
        }

        /// <summary>
        /// the method returns the position with the smallest number of divisors
        /// </summary>
        /// <returns></returns>
        public Position Min3(OrderModel _Order)
        {
            MinObject r = new MinObject();
            foreach(Position p in _R)
            {
                if (r.selected == null)
                {
                    r.selected = p;
                    r.criteria.Add("rest", Math.Floor((double) p._W / _Order._Length));
                    r.criteria.Add("front", p._Z + _Order._Width);
                }
                else
                {
                    var m = Math.Floor((double) p._W / _Order._Length);
                    if (m < r.criteria["rest"])
                    {
                        r.selected = p;
                        r.criteria["rest"] = m;
                        r.criteria["front"] = p._Z + _Order._Width;
                    } else if(m == r.criteria["rest"] && p._Z + _Order._Width < r.criteria["front"])
                    {
                        r.selected = p;
                        r.criteria["rest"] = m;
                        r.criteria["front"] = p._Z + _Order._Width;
                    }
                }
            }
            foreach (Position p in _NonR)
            {
                if (r.selected == null)
                {
                    r.selected = p;
                    r.criteria.Add("rest", Math.Floor((double)p._W / _Order._Width));
                    r.criteria.Add("front", p._Z + _Order._Length);
                }
                else
                {
                    var m = Math.Floor((double)p._W / _Order._Width);
                    if (m < r.criteria["rest"])
                    {
                        r.selected = p;
                        r.criteria["rest"] = m;
                        r.criteria["front"] = p._Z + _Order._Length;
                    }
                    else if (m == r.criteria["rest"] && p._Z + _Order._Length < r.criteria["front"])
                    {
                        r.selected = p;
                        r.criteria["rest"] = m;
                        r.criteria["front"] = p._Z + _Order._Length;
                    }
                }
            }
            return r.selected;
        }

        /// <summary>
        /// the method returns the position with the smallest modulo
        /// </summary>
        /// <returns></returns>
        public Position Min4(OrderModel _Order)
        {
            KeyValuePair<int, Position> r = new KeyValuePair<int, Position>();
            foreach (Position p in _R)
            {
                if (r.Equals(new KeyValuePair<int, Position>())) r = new KeyValuePair<int, Position>(p._W % _Order._Width, p);
                else
                {
                    var m = p._W % _Order._Width;
                    if (m < r.Key) r = new KeyValuePair<int, Position>(m, p);
                }
            }
            foreach (Position p in _NonR)
            {
                if (r.Equals(new KeyValuePair<int, Position>())) r = new KeyValuePair<int, Position>(p._W % _Order._Width, p);
                else
                {
                    var m = p._W % _Order._Width;
                    if (m < r.Key) r = new KeyValuePair<int, Position>(m, p);
                }
            }
            return r.Value;
        }

        /// <summary>
        /// the method returns the position with a combined minimum calculation
        /// </summary>
        /// <returns></returns>
        public Position MinCombined(OrderModel _Order)
        {
            Position min = Min(_Order);
            Position min3 = Min3(_Order);
            if (min._Z < min3._Z) return min;
            return min3;
        }
    }

    public class Room
    {
        /// <summary>
        /// the positions that are recently in the puffer
        /// </summary>
        public List<Position> _Positions;
        /// <summary>
        /// all goods that are placed
        /// </summary>
        public List<GoodModel> _Goods { get; }
        /// <summary>
        /// the list contains all steps of the sequence
        /// </summary>
        public List<SequenceStep> _Steps = new List<SequenceStep>();
        /// <summary>
        /// the total height of the container
        /// </summary>
        public int height { get; set; }
        /// <summary>
        /// the total width of the container
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// the method returns the length of the room
        /// </summary>
        public int Length {
            get
            {
                return _Goods.Max(x => (int) x._F);
            }
        }

        public Room(int height, int width)
        {
            _Positions = new List<Position>() { new Position() { _X = 0, _Y = 0, _Z = 0, _H = height, _L = null, _W = width } };
            _Goods = new List<GoodModel>();
            this.height = height;
            this.width = width;
        }

        public bool AddOrder(OrderModel _Order, int start, int sequnr, bool includeSequenceInfo)
        {
            Possibilities _P = new Possibilities();
            _P._NonR = _Positions.Where(
                x => x._H >= _Order._Height
                && (x._L == null || x._L >= _Order._Length)
                && x._W >= _Order._Width 
                && (_Order._Stack || x._Y == 0)
                && (x._GroupRestrictionBy == null || x._GroupRestrictionBy <= _Order._Group) // HIER UNBEDINGT RESTRICT < O.GROUP !!!
            ).ToList();
            if (_Order._Rotate)
            {
                _P._R = _Positions.Where(
                    x => x._H >= _Order._Height 
                    && (x._L == null || x._L >= _Order._Width) 
                    && x._W >= _Order._Length 
                    && (_Order._Stack || x._Y == 0)
                    && (x._GroupRestrictionBy == null || x._GroupRestrictionBy <= _Order._Group) // HIER UNBEDINGT RESTRICT < O.GROUP !!!
                ).ToList();
                foreach(Position _Pos in _P._R)
                {
                    _Pos.IsRotated = true;
                }
            }
            if(_P.Count == 0)
            {
                return false;
            } else
            {
                Position Pos = _P.MinCombined(_Order);
                PositionResponse Resp;
                if (includeSequenceInfo)
                {
                    Resp = Pos.Put(_Order, start, sequnr);
                } else
                {
                    Resp = Pos.Put(_Order, start);
                }
                _Positions.Remove(Pos);
                List<Position> _RecursiveGroupRestricted = new List<Position>();
                foreach(Position _Check in _Positions.Where(x => x._Z < Resp.Putted._Z && (x._GroupRestrictionBy == null || x._GroupRestrictionBy < _Order._Group)).ToList())
                {
                    if (Resp.Putted.GetPosition().IsOverlapping(_Check))
                    {
                        _Check._GroupRestrictionBy = Resp.Putted._Group;
                        _RecursiveGroupRestricted.Add(_Check);
                    }
                }
                _Positions.AddRange(Resp.NewPos);
                foreach (VirtualPosition vp in Possibilities.MergePositions(_Positions, _Order))
                {
                    List<Position> positionObjects = new List<Position>();
                    Nullable<int> gRB = null;
                    foreach (Guid id in vp._Positions)
                    {
                        Position p = _Positions.Find(x => x._Id == id);
                        if(p != null)
                        {
                            positionObjects.Add(p);
                            if (!gRB.HasValue)
                            {
                                if (p._GroupRestrictionBy.HasValue) gRB = p._GroupRestrictionBy;
                            }
                            else
                            {
                                if (p._GroupRestrictionBy.HasValue)
                                {
                                    if (p._GroupRestrictionBy.Value < gRB.Value) gRB = p._GroupRestrictionBy;
                                }
                            }
                            _Positions.Remove(p);
                            if (Resp.NewPos.Contains(p)) Resp.NewPos.Remove(p);
                        }
                    }
                    int X = positionObjects.Min(x => x._X);
                    int Y = positionObjects.Min(x => x._Y);
                    int Z = positionObjects.Min(x => x._Z);
                    Position newPos = new Position()
                    {
                        _GroupRestrictionBy = gRB,
                        _X = X,
                        _Y = Y,
                        _Z = Z,
                        _H = positionObjects.Max(x => x._T) - Y,
                        _W = positionObjects.Max(x => x._R) - X,
                        _L = positionObjects.Max(x => x._F) - Z,
                        _IsSumedUp = true
                    };
                    _Positions.Add(newPos);
                    Resp.Sequence.Add("Vereinfachung von " + positionObjects.Count + " Positionen zu einer Position mit der Id " + newPos);
                    Resp.NewPos.Add(newPos);
                }
                Resp.Putted._SequenceNr = sequnr;
                _Goods.Add(Resp.Putted);
                if (Resp.Sequence != null)
                {
                    _Steps.Add(new SequenceStep(sequnr, Resp.Sequence, Resp.NewPos, _RecursiveGroupRestricted));
                }
                return true;
            }
        }
    }
}