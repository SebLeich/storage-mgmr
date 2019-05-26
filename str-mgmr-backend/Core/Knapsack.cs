using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace str_mgmr_backend.Core
{
    public class Item
    {
        public int Id { get; set; }
        public int Value { get; set; }
    }

    public class Combination
    {
        public List<Item> items { get; set; }

        public int capacity {
            get
            {
                return items.Sum(x => x.Value);
            }
        }
        public Combination(Item _I)
        {
            items = new List<Item>() { _I };
        }
        public Combination(List<Item> _I)
        {
            items = _I;
        }
        public void AddUnderRestriction(Item _I, int cap)
        {
            if((capacity + _I.Value) <= cap)
            {
                this.items.Add(_I);
            }
        }
    }

    public class Knapsack
    {
        static List<Combination> GetTopSolutions(List<Item> _Input, int count, int max)
        {
            List<Combination> _Output = new List<Combination>();
            foreach(Item _I in _Input.Where(x => x.Value <= max).ToList())
            {
                if(_I.Value == max)
                {
                    _Output.Add(new Combination(_I));
                } else
                {
                    _Output.ForEach(x => x.AddUnderRestriction(_I, max));
                    _Output.Add(new Combination(_I));
                }
            }
            return _Output.OrderByDescending(x => x.capacity).Take(count).ToList();
        }
    }
}