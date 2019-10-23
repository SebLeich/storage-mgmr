using Newtonsoft.Json;
using storage.mgr.backend.Algorithm;
using storage.mgr.backend.Models;
using storagemanager.backend.Models;
using str_mgmr_backend.Core;
using str_mgmr_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storagemanager.backend.Repositories
{
    public class CalculationRepository
    { 
        /// <summary>
        /// the method starts the calculation with the given input data
        /// </summary>
        /// <param name="_Input"></param>
        /// <param name="callbackId"></param>
        /// <returns></returns>
        public void StartCalculation(DataInput _Input, Guid callbackId)
        {
            if (!_Input.isValid())
            {
                Hubproxy._Instance.SendError(new ErrorModel()
                {
                    desc = "Der Input ist nicht zulässig"
                }, callbackId);
            } else
            {
                Hubproxy._Instance.SendSolution(new SuperFlo().calculate(_Input), callbackId);
                Hubproxy._Instance.SendSolution(new AllInOneRow(true).calculate(_Input), callbackId);
                Hubproxy._Instance.SendSolution(new StartLeftBottom().calculate(_Input), callbackId);
            }
        }
    }
}
