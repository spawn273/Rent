using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentApi.Infrastructure.Database;
using SmartAnalytics.BASF.Backend.Application.Authorization.DTO;
using SmartAnalytics.BASF.Backend.Infrastructure;
using SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
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
        private readonly AppDbContext _context;

        public AuthorizationController(UserManager<User> userManager, IOptions<AuthorizationOptions> authOptions, AppDbContext context)
        {
            _userManager = userManager;
            _authOptions = authOptions.Value;
            _context = context;
        }

        [HttpPost("token")]
        public async Task<ActionResult<object>> Token(TokenRequest request)
        {
            var user = await _context.Users
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.UserName == request.Email);
            if (user == null || !user.EmailConfirmed || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return BadRequest();
            }

            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));

            var role = (await _userManager.GetRolesAsync(user)).First();
            claims.Add(new Claim(ClaimTypes.Role, role));

            if (user.Employee.ShopId.HasValue)
            {
                claims.Add(new Claim("shop", user.Employee.ShopId.ToString()));
            }


            var jwt = new JwtSecurityToken(
                issuer: _authOptions.Jwt.Issuer,
                audience: _authOptions.Jwt.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_authOptions.Jwt.Lifetime),
                signingCredentials: new SigningCredentials(_authOptions.Jwt.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );


            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new { Token = encodedJwt };
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
                FirstName = request.Name,
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
