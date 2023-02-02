using DataAccess_EF.Data;
using System.IdentityModel.Tokens.Jwt;

namespace DataAccess_EF.Services
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user);
    }
}
