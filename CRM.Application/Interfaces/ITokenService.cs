using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);
        string GenerateRefreshToken();
        ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token, IConfiguration _config);

        Task<string> AcquireTokenByUsernamePasswordAsync(string email, string password, string clienteId, string _tenantId, string apiScope);
    }
}
