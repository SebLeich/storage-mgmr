using storage.mgr.backend.Models;
using str_mgmr_backend.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace str_mgmr_backend.Models
{
    public class Possibilities
    {
        public List<Position> _NonR = new List<Position>();
        public List<Position> _R = new List<Position>();

        public int Count { get { return _NonR.Count + _R.Count;  } }

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

        public Position Min()
        {
            List<Position> _All = _NonR;
            _All.AddRange(_R);
            Position _Output = null;
            foreach(Position position in _All)
            {
                if(_Output == null)
                {
                    _Output = position;
                } else
                {
                    if(position._Z < _Output._Z)
                    {
                        _Output = position;
                    }
                }
            }
            return _Output;
        }
    }

    public class Room
    {
        public List<Position> _Positions;
        public List<GoodModel> _Goods { get; }
        public List<Step> _Steps = new List<Step>();
        public int Length {
            get
            {
                return _Goods.Max(x => (int) x._Z + (int) x._Length);
            }
        }

        public Room(int height, int width)
        {
            _Positions = new List<Position>() { new Position() { _X = 0, _Y = 0, _Z = 0, _H = height, _L = null, _W = width } };
            _Goods = new List<GoodModel>();
        }

        public bool AddOrder(OrderModel _Order, int start)
        {
            Possibilities _P = new Possibilities();
            _P._NonR = _Positions.Where(
                x => x._H >= _Order._Height
                && (x._L == null || x._L >= _Order._Length)
                && x._W >= _Order._Width 
                && (_Order._Stack || x._Y == 0)
                && (x._GroupRestrictionBy == null || x._GroupRestrictionBy <= _Order._Group)
            ).ToList();
            if (_Order._Rotate)
            {
                _P._R = _Positions.Where(
                    x => x._H >= _Order._Height 
                    && (x._L == null || x._L >= _Order._Width) 
                    && x._W >= _Order._Length 
                    && (_Order._Stack || x._Y == 0)
                    && (x._GroupRestrictionBy == null || x._GroupRestrictionBy <= _Order._Group)
                ).ToList();
            }
            if(_P.Count == 0)
            {
                return false;
            } else
            {
                Position Pos = _P.Min();
                if (Pos._Z == 2004)
                {
                    _Steps.Add(new Step()
                    {
                        desc = Pos.ToString()
                    });
                }
                PositionResponse Resp = Pos.Put(_Order, start);
                /*
                _Steps.Add(new Step() {
                    desc = _Order.ToString() + " wurde an Position " + Pos.ToString() + " gesetzt"
                });
                */
                _Positions.Remove(Pos);
                /*
                _Steps.Add(new Step()
                {
                    desc = Pos.ToString() + " wurde entfernt"
                });
                */
                foreach(Position _Check in _Positions.Where(x => x._Z < Pos._Z && (x._GroupRestrictionBy == null || x._GroupRestrictionBy < _Order._Group)).ToList())
                {
                    if (_Check._Z == 2004)
                    {
                        _Steps.Add(new Step()
                        {
                            desc = "Restriktion (" + _Check._GroupRestrictionBy + ") gesetzt auf (" + _Order._Group + ") auf " + _Check.ToString() + ", " + _Check._R + ", " + _Check._T
                        });
                    }
                    /*
                    if (
                        (((_Check._X >= Resp.Putted._X) && (_Check._X <= Resp.Putted._R)) || ((_Check._R >= Resp.Putted._X) && (_Check._R <= Resp.Putted._R))
                        || ((Resp.Putted._X >= _Check._X) && (Resp.Putted._X <= _Check._R)) || ((Resp.Putted._R >= _Check._X) && (Resp.Putted._R <= _Check._R)))
                        && (((_Check._Y >= Resp.Putted._Y) && (_Check._Y <= Resp.Putted._T)) || ((_Check._T >= Resp.Putted._Y) && (_Check._T <= Resp.Putted._T))
                        || ((Resp.Putted._Y >= _Check._Y) && (_Check._Y <= Resp.Putted._T)) || ((_Check._T >= Resp.Putted._Y) && (_Check._T <= Resp.Putted._T)))
                    )
                    */
                    if (!(Resp.Putted._X > _Check._R || _Check._X > Resp.Putted._R || Resp.Putted._Y < _Check._Y || _Check._Y < Resp.Putted._Y))
                    {
                        if(_Check._Z == 2004)
                        {
                            _Steps.Add(new Step()
                            {
                                desc = "Restriktion (" + _Check._GroupRestrictionBy + ") gesetzt auf (" + _Order._Group + ") auf " + _Check.ToString() + ", " + _Check._R + ", " + _Check._T
                            });
                        }
                        
                        _Check._GroupRestrictionBy = _Order._Group;
                    }
                }

                foreach (Position pos in Resp.NewPos)
                {
                    _Positions.Add(pos);
                    //_Steps.Add(new Step() { desc = "Leere Position '" + pos.ToString() + "' hinzugefügt" });
                }
                _Goods.Add(Resp.Putted);
                return true;
            }
        }
    }
}