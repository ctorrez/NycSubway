using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NycSubway.Core.Services.Station;
using NycSubway.Core.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace NycSubway.WebApi.Controllers
{
    /// <summary>
    /// User Action Controller
    /// </summary>
    [Authorize()]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserDataService service;

        public UserController(IUserDataService userDataService)
        {
            service = userDataService;
        }

        /// <summary>
        /// Retrieve a user’s frequently used station entrances
        /// </summary>
        /// <returns></returns>
        [HttpGet("stations")]
        public IActionResult GetStations()
        {
            var user = HttpContext.User;

            if (user == null)
            {
                return BadRequest("Bad Request");
            }

            var email = user.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var userModel = service.GetUserReadModel(email);

            var stations = service.GetStationEntrances(userModel);

            return Ok(stations);
        }

        /// <summary>
        /// Save a user’s frequently used station entrances
        /// </summary>
        /// <param name="entrance"></param>
        /// <returns></returns>
        [HttpPost("save-station")]
        public IActionResult SaveStation(StationEntrance entrance)
        {
            var user = HttpContext.User;

            if (user == null)
            {
                return BadRequest();
            }

            var email = user.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var userModel = service.GetUserReadModel(email);

            var response = service.SaveStationEntrance(userModel, entrance);

            return Ok(response);
        }
    }
}
