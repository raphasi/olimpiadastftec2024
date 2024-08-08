using CRM.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class TokenService : ITokenService
    {
        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            var key = _config["Jwt:Key"]! ?? throw new InvalidOperationException($"{nameof(GenerateAccessToken)}");

            var privateKey = Encoding.UTF8.GetBytes(key);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Audience = _config["Jwt:Audience"]!,
                Issuer = _config["Jwt:Issuer"]!,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return token;
        }

        public string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[128];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(secureRandomBytes);
            var refreshToken = Convert.ToBase64String(secureRandomBytes);
            return refreshToken;
        }
        public ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token, IConfiguration _config)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                //valores validos
                IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)),
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token Inválido");
            }
            return principal;
        }

        public async Task<string> AcquireTokenByUsernamePasswordAsync(string email, string password, string clienteId, string _tenantId, string apiScope)
        {
            try
            {
                var publicClient = PublicClientApplicationBuilder.Create(clienteId)
                    .WithAuthority(new Uri($"https://login.microsoftonline.com/{_tenantId}"))
                    .Build();

                var result = await publicClient.AcquireTokenByUsernamePassword(
                    new[] { apiScope },
                    email,
                    new NetworkCredential("", password).SecurePassword
                ).ExecuteAsync();

                return result.AccessToken;
            }
            catch (MsalUiRequiredException ex)
            {
                // Erro específico do MSAL quando é necessária uma interação do usuário
                Console.WriteLine($"MsalUiRequiredException: {ex.Message}");
                throw new Exception("Interação do usuário necessária. Verifique as permissões e consentimentos.");
            }
            catch (MsalServiceException ex)
            {
                // Erro específico do serviço MSAL
                Console.WriteLine($"MsalServiceException: {ex.Message}");
                throw new Exception("Erro no serviço de autenticação. Verifique as configurações do Azure AD.");
            }
            catch (MsalClientException ex)
            {
                // Erro específico do cliente MSAL
                Console.WriteLine($"MsalClientException: {ex.Message}");
                throw new Exception("Erro no cliente de autenticação. Verifique as credenciais e configurações.");
            }
            catch (Exception ex)
            {
                // Outros erros
                Console.WriteLine($"Exception: {ex.Message}");
                throw new Exception("Erro desconhecido. Verifique as configurações e tente novamente.");
            }
        }
    }
}
