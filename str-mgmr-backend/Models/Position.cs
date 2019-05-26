using storage.mgr.backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace str_mgmr_backend.Models
{
    public class Position
    {
        public int _X { get; set; }
        public int _Y { get; set; }
        public int _Z { get; set; }
        public Nullable<int> _L { get; set; } = null;
        public int _W { get; set; }
        public int _H { get; set; }
        public int index { get; set; }
        public Nullable<int> _GroupRestrictionBy { get; set; } = null;
        public int _R { get
            {
                return _X + _W;
            }
        }
        public int _T
        {
            get
            {
                return _Y + _H;
            }
        }

        public Position()
        {

        }

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

    public class Endpoint
    {
        public int _X { get; set; }
        public int _Y { get; set; }
        public int _Z { get; set; }
        public EndpointType _Type { get; set; }
    }

    public enum EndpointType
    {
        LEFTBOTTOMBEHIND,
        LEFTBOTTOMINFRONT,
        RIGHTBOTTOMBEHIND,
        RIGHTBOTTOMINFRONT,
        LEFTTOPBEHIND,
        LEFTTOPINFRONT,
        RIGHTTOPBEHIND,
        RIGHTTOPINFRONT
    }


    public class PositionResponse
    {
        public List<Position> NewPos { get; set; }
        public GoodModel Putted { get; set; }
    }
}