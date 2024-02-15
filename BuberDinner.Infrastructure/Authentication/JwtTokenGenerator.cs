using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Services;
using BuberDinner.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BuberDinner.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly JwtSetting _jwtOptions;

        public JwtTokenGenerator(
            IDateTimeProvider dateTimeProvider,
            IOptions<JwtSetting> jwtOptions)
        {
            _dateTimeProvider = dateTimeProvider;
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(User user)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
                SecurityAlgorithms.HmacSha256
                );

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                 new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                 new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                 new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
            };

            var securityToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
                claims: claims,
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
