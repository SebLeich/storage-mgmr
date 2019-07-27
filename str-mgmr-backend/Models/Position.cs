using storage.mgr.backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace str_mgmr_backend.Models
{
    /// <summary>
    /// the class contains a physical position
    /// </summary>
    public class Position
    {
        /// <summary>
        /// the id of the position
        /// </summary>
        public Guid _Id { get; set; }
        /// <summary>
        /// is the position sumed uped
        /// </summary>
        public bool _IsSumedUp { get; set; } = false;
        /// <summary>
        /// the x coordinate of the position
        /// </summary>
        public int _X { get; set; }
        /// <summary>
        /// the y coordinate of the position
        /// </summary>
        public int _Y { get; set; }
        /// <summary>
        /// the z coordinate of the position
        /// </summary>
        public int _Z { get; set; }
        /// <summary>
        /// the length of the position (if null the length is infinity)
        /// </summary>
        public Nullable<int> _L { get; set; } = null;
        /// <summary>
        /// the width of the position
        /// </summary>
        public int _W { get; set; }
        /// <summary>
        /// the height of the position
        /// </summary>
        public int _H { get; set; }
        /// <summary>
        /// the current algorithm's build index of the position
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// the method returns the ground area of the position
        /// </summary>
        public Nullable<int> area {
            get {
                if (_L != null) return _L * _W;
                return null;
            }
        }
        /// <summary>
        /// the current group restriction of the position
        /// </summary>
        public Nullable<int> _GroupRestrictionBy { get; set; } = null;
        /// <summary>
        /// the current right coordinate of the position
        /// </summary>
        public int _R { get
            {
                return _X + _W;
            }
        }
        /// <summary>
        /// the current top coordinate of the position
        /// </summary>
        public int _T
        {
            get
            {
                return _Y + _H;
            }
        }
        /// <summary>
        /// the current front coordinate of the position
        /// </summary>
        public Nullable<int> _F
        {
            get
            {
                if(_L != null) return _Z + _L.Value;
                return null;
            }
        }
        /// <summary>
        /// is the position recognized by a rotated order
        /// </summary>
        public bool IsRotated { get; set; } = false;
        /// <summary>
        /// the constructor creates a new instance of a position
        /// </summary>
        public Position() {
            _Id = Guid.NewGuid();
        }

        /// <summary>
        /// the method sets an order to the current instance and returns the good model with the updated coordinates and a set of new positions
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="Start"></param>
        /// <returns></returns>
        public PositionResponse Put(OrderModel Input, int Start)
        {

            int _G = Input._Group;

            if (_GroupRestrictionBy.HasValue && _GroupRestrictionBy < _G) _G = _GroupRestrictionBy.Value;

            List<Position> Output = new List<Position>();
            int DiffX = _W - Input._Width;
            int DiffY = _H - Input._Height;
            Nullable<int> DiffZ = null;

            if(_L != null)
            {
                DiffZ = _L - Input._Length;
            }
            if(DiffY > 0 && Input._Stack)
            {
                Start++;
                Output.Add(new Position()
                {
                    _X = _X,
                    _Y = _Y + Input._Height,
                    _Z = _Z,
                    _L = Input._Length,
                    _W = Input._Width,
                    _H = DiffY,
                    index = Start,
                    _GroupRestrictionBy = Input._Group
                });
            }
            if(DiffX > 0)
            {
                Start++;
                Output.Add(new Position()
                {
                    _X = _X + Input._Width,
                    _Y = _Y,
                    _Z = _Z,
                    _L = Input._Length,
                    _W = DiffX,
                    _H = _H,
                    index = Start,
                    _GroupRestrictionBy = _GroupRestrictionBy
                });
            }
            if (DiffZ != null)
            {
                if (DiffZ > 0)
                {
                    Start++;
                    Output.Add(new Position()
                    {
                        _X = _X,
                        _Y = _Y,
                        _Z = _Z + Input._Length,
                        _L = DiffZ,
                        _W = _W,
                        _H = _H,
                        index = Start,
                        _GroupRestrictionBy = _GroupRestrictionBy
                    });
                }
            }
            else if (_Y == 0 && _X == 0)
            {
                Start++;
                Output.Add(new Position()
                {
                    _X = _X,
                    _Y = _Y,
                    _Z = _Z + Input._Length,
                    _L = null,
                    _W = _W,
                    _H = _H,
                    index = Start,
                    _GroupRestrictionBy = null
                });
            }
            return new PositionResponse()
            {
                NewPos = Output,
                Putted = new GoodModel()
                {
                    _Desc = Input._Description,
                    _Group = Input._Group,
                    _X = this._X,
                    _Y = this._Y,
                    _Z = this._Z,
                    _Rotate = Input._Rotate,
                    _Stack = Input._Stack,
                    _Height = Input._Height,
                    _Width = Input._Width,
                    _Length = Input._Length
                }
            };
        }
        /// <summary>
        /// the method sets an order to the current instance and returns the good model with the updated coordinates and a set of new positions and returns sequence information
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="Start"></param>
        /// <param name="seqnr"></param>
        /// <returns></returns>
        public PositionResponse Put(OrderModel Input, int Start, int seqnr)
        {

            int _G = Input._Group;
            if (_GroupRestrictionBy.HasValue && _GroupRestrictionBy < _G) _G = _GroupRestrictionBy.Value;
            Nullable<int> DiffZ = null;
            int DiffY = _H - Input._Height;
            int DiffX = _W - Input._Width;
            if (_L != null)
            {
                DiffZ = _L - Input._Length;
            }
            if (IsRotated)
            {
                DiffX = _W - Input._Length;
                if (_L != null)
                {
                    DiffZ = _L - Input._Width;
                }
            }
            List<Position> Output = new List<Position>();
            List<string> _Steps = new List<string>();
            if (DiffY > 0 && Input._Stack)
            {
                Start++;
                Position p;
                if (IsRotated)
                {
                    p = new Position()
                    {
                        _X = _X,
                        _Y = _Y + Input._Height,
                        _Z = _Z,
                        _L = Input._Width,
                        _W = Input._Length,
                        _H = DiffY,
                        index = Start,
                        _GroupRestrictionBy = Input._Group,
                    };
                } else
                {
                    p = new Position()
                    {
                        _X = _X,
                        _Y = _Y + Input._Height,
                        _Z = _Z,
                        _L = Input._Length,
                        _W = Input._Width,
                        _H = DiffY,
                        index = Start,
                        _GroupRestrictionBy = Input._Group,
                    };
                }
                Output.Add(p);
                _Steps.Add("Neue Position (" + p._Id + ") an (x: " + p._X + ", y: " + p._Y + ", z: " + p._Z + ") mit der Dimension (Breite: " + p._W + ", Höhe: " + p._H + ", Länge: " + p._L + ")");
            }
            if (DiffX > 0)
            {
                Start++;
                Position p;
                if (IsRotated)
                {
                    p = new Position()
                    {
                        _X = _X + Input._Length,
                        _Y = _Y,
                        _Z = _Z,
                        _L = Input._Width,
                        _W = DiffX,
                        _H = _H,
                        index = Start,
                        _GroupRestrictionBy = _GroupRestrictionBy
                    };
                } else
                {
                    p = new Position()
                    {
                        _X = _X + Input._Width,
                        _Y = _Y,
                        _Z = _Z,
                        _L = Input._Length,
                        _W = DiffX,
                        _H = _H,
                        index = Start,
                        _GroupRestrictionBy = _GroupRestrictionBy
                    };
                }
                Output.Add(p);
                _Steps.Add("Neue Position (" + p._Id + ") an (x: " + p._X + ", y: " + p._Y + ", z: " + p._Z + ") mit der Dimension (Breite: " + p._W + ", Höhe: " + p._H + ", Länge: " + p._L + ")");
            }
            if (DiffZ != null)
            {
                if (DiffZ > 0)
                {
                    Position p;
                    Start++;
                    if (IsRotated)
                    {
                        p = new Position()
                        {
                            _X = _X,
                            _Y = _Y,
                            _Z = _Z + Input._Width,
                            _L = DiffZ,
                            _W = _W,
                            _H = _H,
                            index = Start,
                            _GroupRestrictionBy = _GroupRestrictionBy
                        };
                    } else
                    {
                        p = new Position()
                        {
                            _X = _X,
                            _Y = _Y,
                            _Z = _Z + Input._Length,
                            _L = DiffZ,
                            _W = _W,
                            _H = _H,
                            index = Start,
                            _GroupRestrictionBy = _GroupRestrictionBy
                        };
                    }
                    Output.Add(p);
                    _Steps.Add("Neue Position an (x: " + p._X + ", y: " + p._Y + ", z: " + p._Z + ") mit der Dimension (Breite: " + p._W + ", Höhe: " + p._H + ", Länge: " + p._L + ")");
                }
            }
            else if (_Y == 0 && _X == 0)
            {
                Position p;
                Start++;
                if (IsRotated)
                {
                    p = new Position()
                    {
                        _X = _X,
                        _Y = _Y,
                        _Z = _Z + Input._Width,
                        _L = null,
                        _W = _W,
                        _H = _H,
                        index = Start,
                        _GroupRestrictionBy = null
                    };
                } else
                {
                    p = new Position()
                    {
                        _X = _X,
                        _Y = _Y,
                        _Z = _Z + Input._Length,
                        _L = null,
                        _W = _W,
                        _H = _H,
                        index = Start,
                        _GroupRestrictionBy = null
                    };
                }
                Output.Add(p);
                _Steps.Add("Neue Position (" + p._Id + ") an (x: " + p._X + ", y: " + p._Y + ", z: " + p._Z + ") mit der Dimension (Breite: " + p._W + ", Höhe: " + p._H + ", Länge: " + p._L + ")");
            }
            return new PositionResponse()
            {
                NewPos = Output,
                Putted = new GoodModel()
                {
                    _Desc = Input._Description,
                    _Group = Input._Group,
                    _X = this._X,
                    _Y = this._Y,
                    _Z = this._Z,
                    _Rotate = Input._Rotate,
                    _IsRotated = IsRotated,
                    _Stack = Input._Stack,
                    _Height = Input._Height,
                    _Width = Input._Width,
                    _Length = Input._Length
                },
                Sequence = _Steps
            };
        }
        /// <summary>
        /// the method returns the front coordinate depending on a given order and the rotation state
        /// </summary>
        /// <param name="_Order"></param>
        /// <returns></returns>
        public int GetFrontCoordinate(OrderModel _Order)
        {
            if (IsRotated)
            {
                return (_Z + _Order._Width);
            } else
            {
                return (_Z + _Order._Length);
            }
        }
        public int GetLeftCoordinate(OrderModel _Order)
        {
            if (IsRotated)
            {
                return (_X + _Order._Length);
            }
            else
            {
                return (_X + _Order._Width);
            }
        }
        public List<Endpoint> GetFront()
        {
            return new List<Endpoint>()
            {
                new Endpoint()
                {
                    _X = _X, _Y = _Y, _Z = (_Z + _L.GetValueOrDefault()), _Type = EndpointType.LEFTBOTTOMINFRONT
                },
                new Endpoint()
                {
                    _X = _X, _Y = (_Y + _H), _Z = (_Z + _L.GetValueOrDefault()), _Type = EndpointType.LEFTTOPINFRONT
                },
                new Endpoint()
                {
                    _X = (_X + _W), _Y = _Y, _Z = (_Z + _L.GetValueOrDefault()), _Type = EndpointType.RIGHTBOTTOMINFRONT
                },
                new Endpoint()
                {
                    _X = (_X + _W), _Y = (_Y + _H), _Z = (_Z + _L.GetValueOrDefault()), _Type = EndpointType.RIGHTTOPINFRONT
                }
            };
        }

        public List<Endpoint> GetBack()
        {
            return new List<Endpoint>()
            {
                new Endpoint()
                {
                    _X = _X, _Y = _Y, _Z = _Z, _Type = EndpointType.LEFTBOTTOMBEHIND
                },
                new Endpoint()
                {
                    _X = (_X + _W), _Y = _Y, _Z = _Z, _Type = EndpointType.RIGHTBOTTOMBEHIND
                },
                new Endpoint()
                {
                    _X = _X, _Y = (_Y + _H), _Z = _Z, _Type = EndpointType.LEFTTOPBEHIND
                },
                new Endpoint()
                {
                    _X = (_X + _W), _Y = (_Y + _H), _Z = _Z, _Type = EndpointType.RIGHTTOPBEHIND
                }
            };
        }

        public List<Endpoint> GetEndpoints()
        {
            return new List<Endpoint>()
            {
                new Endpoint()
                {
                    _X = _X, _Y = _Y, _Z = _Z, _Type = EndpointType.LEFTBOTTOMBEHIND
                },
                new Endpoint()
                {
                    _X = (_X + _W), _Y = _Y, _Z = _Z, _Type = EndpointType.RIGHTBOTTOMBEHIND
                },
                new Endpoint()
                {
                    _X = _X, _Y = (_Y + _H), _Z = _Z, _Type = EndpointType.LEFTTOPBEHIND
                },
                new Endpoint()
                {
                    _X = _X, _Y = _Y, _Z = (_Z + _L.GetValueOrDefault()), _Type = EndpointType.LEFTBOTTOMINFRONT
                },
                new Endpoint()
                {
                    _X = (_X + _W), _Y = (_Y + _H), _Z = _Z, _Type = EndpointType.RIGHTTOPBEHIND
                },
                new Endpoint()
                {
                    _X = _X, _Y = (_Y + _H), _Z = (_Z + _L.GetValueOrDefault()), _Type = EndpointType.LEFTTOPINFRONT
                },
                new Endpoint()
                {
                    _X = (_X + _W), _Y = _Y, _Z = (_Z + _L.GetValueOrDefault()), _Type = EndpointType.RIGHTBOTTOMINFRONT
                },
                new Endpoint()
                {
                    _X = (_X + _W), _Y = (_Y + _H), _Z = (_Z + _L.GetValueOrDefault()), _Type = EndpointType.RIGHTTOPINFRONT
                }
            };
        }

        /// <summary>
        /// the method merges two sets of positions and returns 
        /// </summary>
        /// <returns></returns>
        public static PositionMergeResponse MergePositions(List<Position> positions, List<Guid> watchedPos)
        {
            PositionMergeResponse output = new PositionMergeResponse();
            List<Position> newPositions = new List<Position>();
            List<Guid> kick = new List<Guid>();
            foreach(Position p in positions)
            {
                Position behind = positions.Where(x => x._Y == p._Y && x._H == p._H && x._X == p._X && x._W == p._W).FirstOrDefault();
                if(behind != null)
                {
                    Nullable<int> l = null;
                    if(p._L != null && behind._L != null)
                    {
                        l = p._L + behind._L;
                    }
                    Nullable<int> r = null;
                    if (behind._GroupRestrictionBy != null)
                    {
                        r = behind._GroupRestrictionBy;
                    }
                    if (p._GroupRestrictionBy != null)
                    {
                        if(r == null || r > p._GroupRestrictionBy)
                        r = p._GroupRestrictionBy;
                    }
                    kick.Add(p._Id);
                    kick.Add(behind._Id);
                    Position nP = new Position()
                    {
                        _H = p._H,
                        _W = p._W,
                        _Y = p._Y,
                        _X = p._X,
                        _Z = behind._Z,
                        _L = l,
                        _GroupRestrictionBy = r
                    };
                    if (watchedPos.Contains(p._Id))
                    {
                        output.Kicked.Add(p._Id);
                        if (watchedPos.Contains(behind._Id))
                        {
                            output.Kicked.Add(behind._Id);
                        }
                        output.Replaced.Add(nP._Id);
                    }
                    else if (watchedPos.Contains(behind._Id))
                    {
                        output.Kicked.Add(behind._Id);
                        output.Replaced.Add(nP._Id);
                    }
                    newPositions.Add(nP);
                }
            }
            foreach (Guid k in kick)
            {
                positions.Remove(positions.Find(x => x._Id == k));
            }
            output.Merged = positions.Concat(newPositions).ToList();
            return output;
        }

        public bool IsOverlapping(Position position)
        {
            foreach(Endpoint point in position.GetEndpoints())
            {
                if (this.IsOverlapping(point))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// the method checks if the given position (x, y, z) is overlapping the instances sphere
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="z">z</param>
        /// <returns></returns>
        public bool IsOverlapping(Endpoint point)
        {
            if(_X < point._X && (_X + _W) > point._X && _Y < point._Y && (_Y + _H) > point._Y && _Z < point._Z && (_L == null || (_Z + _L.GetValueOrDefault()) > point._Z))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// the method substracts a given position from the current instance
        /// </summary>
        /// <param name="pos"></param>
        /// <returns>bool, if false the instance is invalid</returns>
        public bool Substract(Position pos)
        {
            _H -= pos._H;
            if(_L != null && pos._L != null)
            {
                _L -= pos._L;
            }
            _W -= pos._W;
            if (_H > 0 && _W > 0)
            {
                return true;
            }
            return false;
        }

        public string ToString()
        {
            return "(" + this._X + ", " + this._Y + ", " + this._Z + "), (" + this._W + ", " + this._H + ", " + this._L + ")";
        }
    }
    /// <summary>
    /// the class binds the information of a position placement
    /// </summary>
    public class PositionResponse
    {
        public List<Position> NewPos { get; set; }
        public GoodModel Putted { get; set; }
        public List<string> Sequence { get; set; }
        /// <summary>
        /// the constructor creates a new instance of a position response
        /// </summary>
        public PositionResponse()
        {
            Sequence = new List<string>();
        }
    }
    /// <summary>
    /// the class contains the result of merged positions - physical and virtual
    /// </summary>
    public class PositionMergeResponse
    {
        public List<Position> Merged { get; set; }
        public List<VirtualPosition> VirtualPositions { get; set; }
        public List<Guid> Kicked { get; set; }
        public List<Guid> Replaced { get; set; }

        public PositionMergeResponse()
        {
            Merged = new List<Position>();
            VirtualPositions = new List<VirtualPosition>();
            Kicked = new List<Guid>();
            Replaced = new List<Guid>();
        }
    }
}