using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NycSubway.Core.Models;
using NycSubway.Core.Services.Station;
using NycSubway.Core.Services.User;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NycSubway.WebApi.Controllers
{
    /// <summary>
    /// User Action Controller
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserDataService _service;

        public UserController(IUserDataService userDataService)
        {
            _service = userDataService;
        }

        /// <summary>
        /// Retrieve the authenticated user’s frequently used station entrances
        /// </summary>
        /// <returns>List of saved station entrances</returns>
        [HttpGet("stations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StationEntranceModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStations()
        {
            var email = User.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var userModel = await _service.GetUserReadModelAsync(email);

            if (userModel == null)
            {
                return NotFound("User profile not found.");
            }

            var stations = await _service.GetStationEntrancesAsync(userModel);

            return Ok(stations);
        }

        /// <summary>
        /// Save a station entrance to the authenticated user's frequent list
        /// </summary>
        /// <param name="entrance">Station entrance to bookmark</param>
        /// <returns>Result of the save operation</returns>
        [HttpPost("save-station")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SaveStation([FromBody] StationEntranceModel entrance)
        {
            if (entrance == null || string.IsNullOrWhiteSpace(entrance.Name))
            {
                return BadRequest(ResultResponse.FailureResponse("A valid station entrance name is required."));
            }

            var email = User.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var userModel = await _service.GetUserReadModelAsync(email);

            if (userModel == null)
            {
                return NotFound("User profile not found.");
            }

            var response = await _service.SaveStationEntranceAsync(userModel, entrance);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
