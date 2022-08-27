using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NycSubway.WebApi.Models;
using NycSubway.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NycSubway.WebApi.Controllers
{
    /// <summary>
    /// Provides NYC station data
    /// </summary>
    [ApiController]
    [Route("station")]
    public class StationController : ControllerBase
    {
        private StationService service;

        public StationController()
        {
            service = new StationService();
        }

        /// <summary>
        /// NYC Station Entrances
        /// </summary>
        /// <returns></returns>
        [HttpGet("station-entrances")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StationEntrance>))]
        public IActionResult GetStationEntrances()
        {
            return Ok(service.GetStationEntrances());
        }
    }
}
