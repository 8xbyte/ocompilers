using AuthMicroservice.Interfaces;
using MassTransit;
using Shared.Interfaces;
using Shared.Interfaces.Users;

namespace AuthMicroservice.Services {
    public interface IAuthService {
        public Task<string> LoginAsync(string email, string password);
        public Task<string> RegisterAsync(string email, string password);
        public ITokenPayload VerifyAsync(string token);
    }
    public class AuthService(IRequestClient<IGetUserByEmailRequest> getUserByEmailRequestClient, IRequestClient<ICreateUserRequest> createUserRequestClient, IPasswordService passwordService, IJwtService jwtService) : IAuthService {
        private readonly IRequestClient<IGetUserByEmailRequest> _getUserByEmailRequestClient = getUserByEmailRequestClient;
        private readonly IRequestClient<ICreateUserRequest> _createUserRequestClient = createUserRequestClient;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly IJwtService _jwtService = jwtService;

        public async Task<string?> LoginAsync(string email, string password) {
            var getUserByEmailResponse = await _getUserByEmailRequestClient.GetResponse<IGetUserResponse, IError>(new() {
                Email = email
            });

            if (getUserByEmailResponse.Is(out Response<IGetUserResponse> userResponse)) {
                if (!_passwordService.Verify(password, userResponse.Message.Password)) {
                    return null;
                }
                try {
                    return _jwtService.GenerateToken(new() {
                        UserId = userResponse.Message.Id
                    });
                } catch {
                    return null;
                }
            } else {
                return null;
            }
        }

        public async Task<string?> RegisterAsync(string email, string password) {
            var createUserResponse = await _createUserRequestClient.GetResponse<ICreateUserResponse, IError>(new() {
                Email = email,
                Password = _passwordService.Hash(password)
            });

            if (createUserResponse.Is(out Response<ICreateUserResponse> userResponse)) {
                try {
                    return _jwtService.GenerateToken(new() {
                        UserId = userResponse.Message.Id
                    });
                } catch {
                    return null;
                }
            } else {
                return null;
            }
        }

        public ITokenPayload? VerifyAsync(string token) {
            try {
                var payload = _jwtService.VerifyToken(token);
                return payload;
            } catch {
                return null;
            }
        }
    }
}
