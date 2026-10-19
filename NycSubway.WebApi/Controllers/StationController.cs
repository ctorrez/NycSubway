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

        /// <summary>
        /// Find the nearest subway entrances to a given location within a specified radius
        /// </summary>
        /// <param name="latitude">User latitude (e.g. 40.7527)</param>
        /// <param name="longitude">User longitude (e.g. -73.9772)</param>
        /// <param name="radiusMiles">Search radius in miles (default: 0.5)</param>
        /// <param name="limit">Max number of results to return (default: 10, max: 50)</param>
        /// <returns>List of nearby station entrances ordered by distance ascending</returns>
        [HttpGet("nearby")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NearbyStationEntranceModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetNearbyStations(
            [FromQuery] double latitude,
            [FromQuery] double longitude,
            [FromQuery] double radiusMiles = 0.5,
            [FromQuery] int limit = 10)
        {
            if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
            {
                return BadRequest("Coordinates out of range. Latitude must be between -90 and 90, longitude between -180 and 180.");
            }

            if (radiusMiles <= 0 || radiusMiles > 50)
            {
                return BadRequest("Radius must be greater than 0 and at most 50 miles.");
            }

            if (limit <= 0) limit = 10;
            if (limit > 50) limit = 50;

            var results = await _service.GetNearbyStationEntrancesAsync(
                new GeoPoint(latitude, longitude), radiusMiles, limit);

            return Ok(results);
        }
    }
}
