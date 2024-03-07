using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RatingMusciAPI.DTO;
using RatingMusciAPI.Models;
using RatingMusciAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RatingMusciAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager
        , RoleManager<IdentityRole> roleManager, IConfiguration config, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _config = config;
        _logger = logger;
    }

    [HttpPost]
    [Route("create-role")]
    [Authorize(Policy ="AdminOnly")]
    public async Task<IActionResult> CrateRole(string roleName)
    {
        var roleExists = await _roleManager.FindByNameAsync(roleName);
        
        if(roleExists is not null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = $"The role {roleName} already exists " });
        }

        var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

        if (roleResult.Succeeded)
        {
            _logger.LogInformation(1, "Role added");
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Sucess", Message = $"Role {roleName} added successfully" });
        }
        else
        {
            _logger.LogInformation(2, "Error");
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = $"The role {roleName} couldn't be added!" });
        }
    }

    [HttpPost]
    [Route("add-user-role")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AddUserRole(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var role = await _roleManager.FindByNameAsync(roleName);

        if(user is not null && role is not null)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded) { 

                _logger.LogInformation(1, $"User {user.Email} added to the role {roleName} role");
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Sucess", Message = $"Role {roleName} added to user {user.Email} successfully" });
            }
            else
            {
                _logger.LogInformation(1, $"Error! Unable to add user {user.Email} to role {roleName}");
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = $"Error! Unable to add user {user.Email} to role {roleName}" });
            }
        }
        return BadRequest(new { error = "Unable to find user/role!" });

    }


    [HttpPost]
    [Route("login")]

    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username!);

        if(user is not null && await _userManager.CheckPasswordAsync(user,model.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("id", user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            foreach(var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAcessToken(authClaims, _config);

            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_config["JWT:RefreshTokenValidMinutes"], out int refreshTokenValidMinutes);

            user.RefreshTokenExpireTime = DateTime.Now.AddMinutes(refreshTokenValidMinutes);

            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo.ToLocalTime()
            });
        }
        return Unauthorized();
    }


    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody]RegisterModel model)
    {
        var userExists = await _userManager.FindByNameAsync(model.UserName!);

        if(userExists is not null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
        }

        ApplicationUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.UserName
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response
            {
                Status = "Error",
                Message = "User couldn't be created!"
            });
        }

        return Ok(new Response { Status = "Sucess", Message = "User created successfully" });
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel model)
    {
        if(model is null)
        {
            return BadRequest();
        }

        string? acessToken = model.AcessToken ?? throw new ArgumentNullException(nameof(model));

        string refreshToken = model.RefreshToken ?? throw new ArgumentNullException(nameof(model));

        var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken!,_config);

        if(principal is null)
        {
            return BadRequest();
        }

        string username = principal.Identity.Name;

        var user = await _userManager.FindByNameAsync(username);

        if(user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpireTime <= DateTime.Now)
        {
            return BadRequest("Invalid acess/ refresh token");
        }

        var newAcessToken = _tokenService.GenerateAcessToken(principal.Claims.ToList(), _config);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            AcessToken = new JwtSecurityTokenHandler().WriteToken(newAcessToken),
            RefreshToken = newRefreshToken,
        });
    }

    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if(user is null)
        {
            return BadRequest();
        }

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);
        return NoContent();
    }
}
