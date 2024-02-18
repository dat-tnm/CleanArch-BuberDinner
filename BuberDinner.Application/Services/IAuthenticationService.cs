using BuberDinner.Application.Common.Errors;
using OneOf;

namespace BuberDinner.Application.Services
{
    public interface IAuthenticationService
    {
        OneOf<AuthenticationResult, IServiceException> Register(string firstName, string lastName, string email, string password);
        AuthenticationResult Login(string email, string password);
    }
}
