using NycSubway.Core.Models;

namespace NycSubway.Core.Services.Identity
{
    public interface ITokenService
    {
        string CreateToken(UserData user);
    }
}
