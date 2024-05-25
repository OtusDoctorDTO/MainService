using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IApplicationConfig _config;
        public TokenService(IApplicationConfig config)
        {
            _config = config;
        }
        public async Task<bool> Validate(string? token)
        {
            if(string.IsNullOrEmpty(token))
                return false;
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationOptions = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = _config.AuthOptions.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.AuthOptions.Key))
            };
            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, validationOptions);
            return tokenValidationResult.IsValid;
        }
    }
}
