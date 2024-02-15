using BuberDinner.Application.Services;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

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
            var result = _authService.Register(
                registerRequest.FirstName, 
                registerRequest.LastName,
                registerRequest.Email,
                registerRequest.Password
                );

            var response = new AuthenticationResponse(
                result.Id,
                result.FirstName,
                result.LastName,
                result.Email,
                result.Token);

            return Ok(response);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var result = _authService.Login(
                loginRequest.Email, 
                loginRequest.Password
                );

            var response = new AuthenticationResponse(
                result.Id,
                result.FirstName,
                result.LastName,
                result.Email,
                result.Token);

            return Ok(result);
        }
    }
}
