﻿using Azure.Core;
using CI_Platform.Entities.Auth;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CI_Platform_web.Auth
{
    public static class JwtTokenHelper
    {
      
        public static string GenerateToken(JwtSetting jwtSetting, User user)
        {
            if (jwtSetting == null)
                return string.Empty;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //string isActive = "false";
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.FirstName + user.LastName),
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Role, user.Role!),
                //new Claim("isActive", user.Status.ToString()!),
                new Claim("CustomClaimForUser", JsonSerializer.Serialize(user)),  // Additional Claims
                new Claim("exp", DateTime.UtcNow.AddMinutes(30).ToString()) // Expiration Time Claim
            };

            var token = new JwtSecurityToken(
                jwtSetting.Issuer,
                jwtSetting.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(30), // Default 5 mins, max 1 day
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
