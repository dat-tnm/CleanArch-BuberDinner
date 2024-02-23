using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using ErrorOr;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BuberDinner.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler :
        IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepo;

        public LoginQueryHandler(
            IJwtTokenGenerator jwtTokenGenerator,
            IUserRepository userRepo)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepo = userRepo;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            //Validate if user exists
            var user = _userRepo.GetUserByEmail(query.Email);
            if (user == null)
            {
                return Domain.Common.Errors.Errors.Authentication.InvalidCredentials;
            }

            //Validate password is correct
            if (user.Password != query.Password)
            {
                return Domain.Common.Errors.Errors.Authentication.InvalidCredentials;
            }

            //Generate token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }
    }
}
