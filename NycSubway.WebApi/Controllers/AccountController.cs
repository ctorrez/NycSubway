using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NycSubway.Core.Models;
using NycSubway.Core.Services.Identity;
using System.Threading.Tasks;

namespace NycSubway.WebApi.Controllers
{
    /// <summary>
    /// Authentication Endpoint
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private ITokenService _tokenService;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost(nameof(Login))]
        public async Task<ActionResult<UserData>> Login(LoginData loginData)
        {
            var user = await _userManager.FindByEmailAsync(loginData.Email);

            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginData.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var dto = new UserData()
            {
                Email = user.Email,
            };

            dto.Token = _tokenService.CreateToken(dto);

            return dto;
        }
    }
}
