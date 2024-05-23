using AuthMicroservice.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Shared.Interfaces.Options;
using Microsoft.Extensions.Options;

namespace AuthMicroservice.Services {
    public interface IJwtService {
        public string GenerateToken(ITokenPayload payload);
        public ITokenPayload VerifyToken(string token);
    }
    public class JwtService : IJwtService {
        private readonly IAuthOptions _authOptions;
        private readonly JwtSecurityTokenHandler _jwtHandler;
        private readonly SymmetricSecurityKey _securityKey;
        private readonly JwtHeader _jwtHeader;

        public JwtService(IOptions<IAuthOptions> authOptions) {
            _authOptions = authOptions.Value;
            _jwtHandler = new JwtSecurityTokenHandler();
            _securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authOptions.Jwt.SecurityKey));
            var credentials = new SigningCredentials(_securityKey, _authOptions.Jwt.EncryptionAlgorithm);
            _jwtHeader = new JwtHeader(credentials);
        }

        public string GenerateToken(ITokenPayload payload) {
            var jwtPayload = new JwtPayload() {
                { _authOptions.Jwt.Token.Fields.UserId, Convert.ToInt32(payload.UserId) },
                { _authOptions.Jwt.Token.Fields.Expiration, DateTime.UtcNow.AddSeconds(_authOptions.Jwt.Token.Expiration).Ticks },
            };
            var jwtToken = new JwtSecurityToken(_jwtHeader, jwtPayload);
            var token = _jwtHandler.WriteToken(jwtToken);
            return token;
        }

        public ITokenPayload VerifyToken(string token) {
            _jwtHandler.ValidateToken(token, new TokenValidationParameters() {
                IssuerSigningKey = _securityKey,
                ValidateAudience = false,
                ValidateIssuer = false,
            }, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            return new() {
                UserId = Convert.ToInt32(jwtSecurityToken.Payload[_authOptions.Jwt.Token.Fields.UserId])
            };
        }
    }
}
