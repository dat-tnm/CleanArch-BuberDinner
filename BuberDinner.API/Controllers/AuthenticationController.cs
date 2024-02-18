using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Services;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace BuberDinner.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authService = authenticationService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest registerRequest)
        {
            OneOf<AuthenticationResult, IServiceException> result = _authService.Register(
                registerRequest.FirstName,
                registerRequest.LastName,
                registerRequest.Email,
                registerRequest.Password
                );

            return result.Match(
                authResult => Ok(MapAuthResult(authResult)),
                error => Problem(statusCode: (int)error.StatusCode, title: error.ErrorMessage)
                );
        }

        private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
        {
            return new AuthenticationResponse(
                        authResult.user.Id,
                        authResult.user.FirstName,
                        authResult.user.LastName,
                        authResult.user.Email,
                        authResult.Token);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var result = _authService.Login(
                loginRequest.Email, 
                loginRequest.Password
                );

            var response = new AuthenticationResponse(
                result.user.Id,
                result.user.FirstName,
                result.user.LastName,
                result.user.Email,
                result.Token);

            return Ok(response);
        }
    }
}
