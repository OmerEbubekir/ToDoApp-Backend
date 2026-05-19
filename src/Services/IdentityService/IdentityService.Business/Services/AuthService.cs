using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IdentityService.Business.Interfaces;
using IdentityService.Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Data.Entities;

namespace IdentityService.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthService(UserManager<AppUser> userManager, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<AuthResult?> LoginAsync(LoginArgs args)
        {
            var user=await _userManager.FindByEmailAsync(args.Email);
            if (user == null)
                return null;
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, args.Password);
            if (!isPasswordValid)
                return null;
            var token= GenerateJwtToken(user);
            return new AuthResult
            {
                Token = token.Token,
                
                Expiration = DateTime.UtcNow.AddHours(1)
            };
        }

        private AuthResult GenerateJwtToken(AppUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

            
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.FullName)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpirationInMinutes"]!)),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResult
            {
                Token = tokenHandler.WriteToken(token)
                
            };
        }

        public async Task<bool> RegisterAsync(RegisterArgs args)
        {
            var user = _mapper.Map<AppUser>(args);
            var result = await _userManager.CreateAsync(user, args.Password);
            return result.Succeeded;
        }
    }
}
