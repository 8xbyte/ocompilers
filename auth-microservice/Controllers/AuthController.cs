using AuthMicroservice.Interfaces;
using AuthMicroservice.Services;
using MassTransit.Configuration;
using MassTransit.SagaStateMachine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Interfaces;
using Shared.Interfaces.Options;

namespace AuthMicroservice.Controllers {
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService, IOptions<IApiGatewayOptions> apiGatewayOptions) : ControllerBase {
        private readonly IAuthService _authService = authService;
        private readonly IApiGatewayOptions _apiGatewayOptions = apiGatewayOptions.Value;

        [HttpPost("login")]
        public async Task<ActionResult<ILoginHttpResponse>> Login([FromBody] ILoginHttpRequest body) {
            var token = await _authService.LoginAsync(body.Email, body.Password);

            if (token == null) {
                return BadRequest(new IError() {
                    Message = "Email or password incorrect"
                });
            }

            HttpContext.Response.Cookies.Append(_apiGatewayOptions.Http.Cookies.AuthToken, token, new CookieOptions {
                Expires = DateTime.Now.AddMonths(1)
            });

            return Ok(new ILoginHttpResponse() {
                Status = "Success"
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<IRegisterHttpResponse>> Register([FromBody] IRegisterHttpRequest body) {
            var token = await _authService.RegisterAsync(body.Email, body.Password);

            if (token == null) {
                return BadRequest(new IError() {
                    Message = "User already created"
                });
            }

            HttpContext.Response.Cookies.Append(_apiGatewayOptions.Http.Cookies.AuthToken, token, new CookieOptions {
                Expires = DateTime.Now.AddMonths(1)
            });


            return Ok(new ILoginHttpResponse() {
                Status = "Success"
            });
        }

        [HttpPost("verify")]
        public ActionResult<ITokenPayload> Verify() {
            var token = HttpContext.Request.Cookies[_apiGatewayOptions.Http.Cookies.AuthToken];

            if (token == null) {
                return BadRequest();
            }

            var payload = _authService.VerifyAsync(token);
            if (payload == null) {
                return BadRequest();
            } else {
                return Ok(payload);
            }
        }
    }
}
