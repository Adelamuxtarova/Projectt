using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System;
using Project.Constants;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace Project.Helper
{
    public class JWTHelper
    {
        public TokenModel Create(IConfiguration configuration)
        {
            var key = Encoding.UTF8.GetBytes(configuration["JWTToken:Key"]);
            var ValidIssuer = configuration["JWTToken:Issuer"];
            var ValidAudience = configuration["JWTToken:Audience"];
            var claims = new List<Claim>() {new Claim(ClaimTypes.Name,ClaimsConfig.ClaimName),
                    new Claim(ClaimTypes.NameIdentifier,ClaimsConfig.ClaimId),
                    new Claim(ClaimTypes.Email,ClaimsConfig.ClaimEmail)};
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var TokenDescriptor = new JwtSecurityToken(
                issuer: ValidIssuer,
                audience: ValidAudience,
                signingCredentials: signingCredentials,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(30)
                );
            var Accesstoken = new JwtSecurityTokenHandler().WriteToken(TokenDescriptor);
            var RefreshToken = GenerateRefreshToken();
            return new TokenModel(Accesstoken, RefreshToken);
        }


        public string GenerateRefreshToken()
        {
            var bytes = new byte[32];
            using (var nums = RandomNumberGenerator.Create())
            {
                nums.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }
    }

    public class TokenModel
    {
        public TokenModel(string refreshToken, string accessToken)
        {
            RefreshToken = refreshToken;
            AccessToken = accessToken;
        }

        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}

