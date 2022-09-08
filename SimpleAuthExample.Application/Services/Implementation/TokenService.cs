﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpleAuthExample.Application.Services.Interfaces;
using SimpleAuthExample.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAuthExample.Application.Services.Implementation
{
    public class TokenService : ITokenService

    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(User user, List<string> roles)
        {
            TimeSpan? expiration = null;
            var claims = CreateJwtClaims(user, roles);
            var options = GetOptions();
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? options.Expiration),
                signingCredentials: options.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private List<Claim> CreateJwtClaims(User user, List<string> roles)
        {
            var claims = new List<Claim>();
            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.

            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, "029261DF-0E82-4E5E-9AA8-036606F70ECF"),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim("email", user.Email),
                new Claim("phoneNumber", user.PhoneNumber),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),

            });
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private TokenProviderOptions GetOptions()
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("Authentication:JwtBearer:SecretKey").Value));

            return new TokenProviderOptions
            {
                Audience = _configuration.GetSection("Authentication:JwtBearer:Audience").Value,
                Issuer = _configuration.GetSection("Authentication:JwtBearer:Issuer").Value,
                Expiration = TimeSpan.FromMinutes(Convert.ToInt32(_configuration.GetSection("Authentication:JwtBearer:AccessExpiration").Value)),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512)
            };
        }
    }

    public class TokenProviderOptions
    {
        public SymmetricSecurityKey SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
        public TimeSpan Expiration { get; set; }
    }
}
