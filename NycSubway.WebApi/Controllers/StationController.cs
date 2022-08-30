using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NycSubway.Core.Services.Station;
using NycSubway.WebApi.Models;
using System.Collections.Generic;

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

        public StationController(StationService service)
        {
            this.service = service;
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

        /// <summary>
        /// Get the distance between two station entrances
        /// </summary>
        /// <returns></returns>
        [HttpPost("station-distance")]
        public IActionResult GetStationDistance(StationDistanceRequest request)
        {
            return Ok(service.GetStationDistance(request.Entrance1, request.Entrance2));
        }
    }
}
