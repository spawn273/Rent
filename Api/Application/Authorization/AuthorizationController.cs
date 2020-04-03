using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartAnalytics.BASF.Backend.Application.Authorization.DTO;
using SmartAnalytics.BASF.Backend.Infrastructure;
using SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartAnalytics.BASF.Backend.Application.Authorization
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AuthorizationOptions _authOptions;

        public AuthorizationController(UserManager<User> userManager, IOptions<AuthorizationOptions> authOptions)
        {
            _userManager = userManager;
            _authOptions = authOptions.Value;
        }
        
        [HttpPost("token")]
        public async Task<ActionResult<string>> Token(TokenRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !user.EmailConfirmed || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return BadRequest();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var roleClaims = userRoles.Select(x => new Claim(ClaimTypes.Role, x));

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }.Concat(roleClaims).ToArray();

            var jwt = new JwtSecurityToken(
                issuer: _authOptions.Jwt.Issuer,
                audience: _authOptions.Jwt.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_authOptions.Jwt.Lifetime),
                signingCredentials: new SigningCredentials(_authOptions.Jwt.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest request)
        {
            //var validation = request.Validate(_validator);
            //if (!validation.IsValid)
            //{
            //    return BadRequest(validation.ToString());
            //}

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    return StatusCode(StatusCodes.Status409Conflict);
                }
            }

            user = new User
            {
                Name = request.Name,
                Email = request.Email,
                UserName = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
    }
}
