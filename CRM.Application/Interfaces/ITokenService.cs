using AutoMapper;
using CRM.Application.DTOs;
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
        Task<string> CreateUser(RegisterModelDTO userModel, string clienteId, string _tenantId, string _instance, string _domain, string apiScope, string clientSecret, string policyName);
        Task<string> AcquireTokenByUsernamePasswordAsyncB2c(string clienteId, string _tenantName, string policyName, string apiScope, string username, string password);
    }
}
