using LocationsAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LocationsAPI.Services
{
    public interface ITokenService
    {
        public string CreateToken(User user);

        public bool IsTokenValid(string key, string issuer, string audience, string token);

    }

    public class TokenService : ITokenService
    {
        private readonly LocationContext _dbContext;
        private readonly IConfiguration _config;
        private const int EXPIRATION_TIMER = 60;

        public TokenService(LocationContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        public string CreateToken(User user)
        {
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
             }),
                Expires = DateTime.UtcNow.AddMinutes(EXPIRATION_TIMER),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }

        public bool IsTokenValid(string key, string issuer, string audience, string token)
        {
            var mySecret = Encoding.ASCII.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = securityKey
                    }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
