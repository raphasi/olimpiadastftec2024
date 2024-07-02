using Azure;
using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Application.Services;
using CRM.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CRM.API.BEND.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService,
                          UserManager<ApplicationUser> userManager,
                          RoleManager<IdentityRole> roleManager,
                          IConfiguration configuration,
                          ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("CreateRole")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return BadRequest(new ResponseDTO { Status = "Error", Message = "Role name cannot be empty." });
        }

        if (await _roleManager.RoleExistsAsync(roleName))
        {
            return BadRequest(new ResponseDTO { Status = "Error", Message = "Role already exists." });
        }

        var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
        if (roleResult.Succeeded)
        {
            _logger.LogInformation("Role {RoleName} added successfully", roleName);
            return Ok(new ResponseDTO { Status = "Success", Message = $"Role {roleName} added successfully" });
        }

        _logger.LogError("Error adding role {RoleName}", roleName);
        return BadRequest(new ResponseDTO { Status = "Error", Message = $"Issue adding the new {roleName} role" });
    }

    [HttpPost("addusertorole")]
    public async Task<IActionResult> AddUserToRole(string email, string roleName)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(roleName))
        {
            return BadRequest(new ResponseDTO { Status = "Error", Message = "Email and role name cannot be empty." });
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return NotFound(new ResponseDTO { Status = "Error", Message = "User not found." });
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (result.Succeeded)
        {
            _logger.LogInformation("User {Email} added to the {RoleName} role", email, roleName);
            return Ok(new ResponseDTO { Status = "Success", Message = $"User {email} added to the {roleName} role" });
        }

        _logger.LogError("Error adding user {Email} to the {RoleName} role", email, roleName);
        return BadRequest(new ResponseDTO { Status = "Error", Message = $"Error adding user {email} to the {roleName} role" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(model.Email!);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password!))
        {
            return Unauthorized();
        }

        var tokenResponse = await GenerateTokenResponse(user);
        return Ok(tokenResponse);
    }

    [HttpPost("login_ad")]
    public async Task<IActionResult> Login_AD([FromBody] LoginDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(model.Email!);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password!))
        {
            var tokenResponse = await GenerateTokenResponse(user);
            if (tokenResponse.UserInfo.Roles.Contains("AdminMaster"))
            {
                return Ok(tokenResponse);
            }
        }

        // Validação no Active Directory
        var ldapService = new LdapService(_configuration);
        if (ldapService.ValidateUser(model.Email!, model.Password!) && ldapService.IsUserInGroup(model.Email!))
        {
            var objectId = ldapService.GetUserObjectId(model.Email!);
            user = await EnsureUserExists(user, model.Email!, model.Password!, objectId);

            var tokenResponse = await GenerateTokenResponse(user);
            return Ok(tokenResponse);
        }

        return Unauthorized();
    }

    private async Task<ApplicationUser> EnsureUserExists(ApplicationUser? user, string email, string password, string objectId)
    {
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                SecurityIdentifierString = objectId
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new Exception("Erro ao criar o usuário.");
            }

            // Adiciona o usuário ao papel "Cliente" após o registro
            var addToRoleResult = await AddUserToRole(user.Email, "Admin");
            if (addToRoleResult is BadRequestObjectResult || addToRoleResult is NotFoundObjectResult)
            {
                throw new Exception("Erro ao adicionar o usuário na role.");
            }
        }
        else
        {
            var passwordHasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, password);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Erro ao atualizar o usuário.");
            }
        }

        return user;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModelDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userExists = await _userManager.FindByNameAsync(model.UserName!);
        if (userExists != null)
        {
            return Conflict(new ResponseDTO { Status = "Error", Message = "User already exists!" });
        }

        var user = new ApplicationUser
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.UserName
        };

        var result = await _userManager.CreateAsync(user, model.Password!);
        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User creation failed." });
        }

        return Ok(new ResponseDTO { Status = "Success", Message = "User created successfully!" });
    }

    [HttpPost("register_loja")]
    public async Task<IActionResult> Register_Loja([FromBody] RegisterModelDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userExists = await _userManager.FindByNameAsync(model.UserName!);
        if (userExists != null)
        {
            return Conflict(new ResponseDTO { Status = "Error", Message = "User already exists!" });
        }

        var user = new ApplicationUser
        {
            Email = model.Email,
            RefreshTokenExpiryTime = DateTime.Now.AddDays(1),
            UserName = model.UserName,
            SecurityIdentifierString = "S-1-0-0",
            LeadID = model.LeadID
        };

        var result = await _userManager.CreateAsync(user, model.Password!);
        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User creation failed." });
        }

        // Adiciona o usuário ao papel "Cliente" após o registro
        var addToRoleResult = await AddUserToRole(user.Email, "Cliente");
        if (addToRoleResult is BadRequestObjectResult || addToRoleResult is NotFoundObjectResult)
        {
            return addToRoleResult;
        }


        return Ok(new ResponseDTO { Status = "Success", Message = "User created successfully!" });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenDTO tokenModel)
    {
        if (tokenModel == null)
        {
            return BadRequest("Invalid client request");
        }

        var principal = _tokenService.GetClaimsPrincipalFromExpiredToken(tokenModel.AccessToken!, _configuration);
        if (principal == null)
        {
            return BadRequest("Invalid access token/refresh token");
        }

        var username = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(username!);
        if (user == null || user.RefreshToken != tokenModel.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid access token/refresh token");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken
        });
    }

    [Authorize]
    [HttpPost("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return NotFound("Invalid user name");
        }

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);

        return NoContent();
    }

    private async Task<TokenDTO> GenerateTokenResponse(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName!),
        new Claim(ClaimTypes.Email, user.Email!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
        user.RefreshToken = refreshToken;

        await _userManager.UpdateAsync(user);

        var userInfo = new UserInfoDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            LeadID = user.LeadID,
            SecurityIdentifier = user.SecurityIdentifierString,
            Roles = userRoles
        };

        return new TokenDTO
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            UserInfo = userInfo
        };
    }
}