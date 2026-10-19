using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NycSubway.Core.Services.Station;
using NycSubway.WebApi.Models;
using System.Collections.Generic;
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
        private readonly IStationService _service;

        public StationController(IStationService service)
        {
            _service = service;
        }

        /// <summary>
        /// NYC Station Entrances
        /// </summary>
        /// <returns>List of all NYC station entrances</returns>
        [HttpGet("station-entrances")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StationEntranceModel>))]
        public async Task<IActionResult> GetStationEntrances()
        {
            var entrances = await _service.GetStationEntrancesAsync();
            return Ok(entrances);
        }

        /// <summary>
        /// Get the distance between two station entrances
        /// </summary>
        /// <param name="request">The two station entrances</param>
        /// <returns>Distance calculation result</returns>
        [HttpPost("station-distance")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeoDistance))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetStationDistance([FromBody] StationDistanceRequest request)
        {
            if (request?.Entrance1?.Point == null || request?.Entrance2?.Point == null)
            {
                return BadRequest("Both entrances and their coordinates are required.");
            }

            return Ok(_service.GetStationDistance(request.Entrance1, request.Entrance2));
        }
    }
}
