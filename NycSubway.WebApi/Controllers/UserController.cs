using Microsoft.AspNetCore.Mvc;
using NycSubway.Core.Services.Station;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NycSubway.WebApi.Controllers
{
    /// <summary>
    /// User Action Controller
    /// </summary>
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        public UserController()
        {

        }

        /// <summary>
        /// Retrieve a user’s frequently used station entrances
        /// </summary>
        /// <returns></returns>
        [HttpGet("stations")]
        public IActionResult GetStations()
        {
            return Ok();
        }

        /// <summary>
        /// Save a user’s frequently used station entrances
        /// </summary>
        /// <param name="entrance"></param>
        /// <returns></returns>
        [HttpPost("save-station")]
        public IActionResult SaveStation(StationEntrance entrance)
        {
            return Ok();
        }
    }
}
