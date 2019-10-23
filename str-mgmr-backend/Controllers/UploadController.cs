using System;
using System.Web.Http;
using System.Web.Http.Cors;
using storagemanager.backend.Models;
using storagemanager.backend.Repositories;
using str_mgmr_backend.Core;
using wista.backend.Controllers.Socket;

namespace storagemanager.backend.Controllers
{
    /// <summary>
    /// the endpoint allows the user's to upload input data for the calculation
    /// </summary>
    [RoutePrefix("api/upload")]
    public class UploadController : ApiController
    {

        /// <summary>
        /// the repository contains the methods to calculate the solutionss
        /// </summary>
        private CalculationRepository repo;

        /// <summary>
        /// the constructor creates a new instance of the controller
        /// </summary>
        public UploadController()
        {
            repo = new CalculationRepository();
        }

        /// <summary>
        /// the endpoint allows the clients to upload a given data sheet
        /// </summary>
        /// <param name="_Input">data input (from csv)</param>
        /// <returns>solution</returns>
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult UploadUserInput([FromBody] DataInput input)
        {
            Guid responseId = Hubproxy._Instance.RegisterTask();
            repo.StartCalculation(input, responseId);
            return Ok();
        }
    }
}
