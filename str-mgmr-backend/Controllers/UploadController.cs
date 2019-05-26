using System;
using System.Web.Http;
using System.Web.Http.Cors;
using storagemanager.backend.Models;
using storagemanager.backend.Repositories;

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
        private CalculationRepository _Repo;

        /// <summary>
        /// the constructor creates a new instance of the controller
        /// </summary>
        public UploadController()
        {
            _Repo = new CalculationRepository();
        }

        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult UploadUserInput([FromBody] DataInput _Input)
        {
            return Ok(_Repo.StartCalculation(_Input));
        }
    }
}
