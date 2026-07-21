using AGP.Security.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Security.ServiceLayer
{
    public interface IAuthService
    {
        string GenerateAccessToken(int userId, DateTime expires);
        string GenerateRefreshToken();
        ClaimsPrincipal ValidateToken(string token);
    }
    public class AuthService: IAuthService
    {

        private readonly string _accessTokenSecret;
        private readonly string _refreshTokenSecret;

        public AuthService(IConfiguration configuration)
        {
            // _accessTokenSecret = accessTokenSecret;
            // _refreshTokenSecret = refreshTokenSecret;
            var jwtToken = configuration.GetSection("TokenSettings");
            _accessTokenSecret = jwtToken["AccessTokenSecret"];
            _refreshTokenSecret = jwtToken["RefreshTokenSecret"];

        }

        public string GenerateAccessToken(int userId, DateTime expires)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_accessTokenSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("userId", userId.ToString()) }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_accessTokenSecret);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }

        public Token SaveRefreshToken(Token token)
        {
            try
            {
                using (var context = new AgpSecurityContext())
                {
                    context.Tokens.Add(token);

                   // token.User = context.Users.Find(token.UserId);

                    // user.Area = context.Areas.Find(user.AreaId);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }


            return token;
        }

        public User GetUserByLogin(string Username,string Password)
        {
            User user = new User();
            try
            {
                using (var context = new AgpSecurityContext())
                {
                    user =  context.Users.Where(x => x.UserName == Username && x.Password == Password).FirstOrDefault();
                    if (user == null) throw new System.ArgumentException("User or Password incorrect !", "");
                  //  user.Area = context.Areas.Find(user.AreaId);
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return user;
        }

        public User GetUserByUserName(string Username)
        {
            User user = new User();
            try
            {
                using (var context = new AgpSecurityContext())
                {
                    user = context.Users.Where(x => x.UserName == Username).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return user;
        }
    }
}

