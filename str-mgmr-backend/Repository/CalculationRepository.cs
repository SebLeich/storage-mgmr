﻿using Newtonsoft.Json;
using storage.mgr.backend.Algorithm;
using storage.mgr.backend.Models;
using storagemanager.backend.Models;
using str_mgmr_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storagemanager.backend.Repositories
{
    public class CalculationRepository
    { 
        public SolutionModel StartCalculation(DataInput _Input)
        {
            if (!_Input.isValid())
            {
                return null;
            }
            switch (_Input._Algorithm)
            {
                case Algorithm.AllInOneRow:
                    return new AllInOneRow(true).calculate(_Input);
                case Algorithm.StartLeftBottom:
                    return new StartLeftBottom().calculate(_Input);
                case Algorithm.SuperFlo:
                    return new SuperFlo().calculate(_Input);
            }
            return null;
        }

        public List<CalculationOutput> OptimizeDimension(double _Max, List<CalculationInput> _Entities)
        {
            List<CalculationOutput> _Output = new List<CalculationOutput>();
            double total = 0.0;
            int index = 0;
            while(total <= _Max && index < _Entities.Count)
            {
                total += _Entities[index]._Value * _Entities[index]._Count;
                index++;
            }
            if(total > _Max)
            {
                _Output.Add(new CalculationOutput()
                {
                    _Mod = _Max - total,
                    _Codes = _Entities.Select(x => x._Code).ToList()
                });
                return _Output;
            }
            foreach(CalculationInput _Input in _Entities)
            {

            }
            return _Output;
        }
    }
}
