using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuberDinner.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepo;

        public AuthenticationService(
            IJwtTokenGenerator jwtTokenGenerator,
            IUserRepository userRepo)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepo = userRepo;
        }

        public AuthenticationResult Login(string email, string password)
        {
            //Validate if user exists
            var user = _userRepo.GetUserByEmail(email);
            if (user == null)
            {
                throw new DuplicateEmailException();
            }

            //Validate password is correct
            if (user.Password != password)
            {
                throw new Exception("Invalid password.");

            }

            //Generate token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }

        public OneOf<AuthenticationResult, IServiceException> Register(string firstName, string lastName, string email, string password)
        {
            //Check if user exists
            if (_userRepo.GetUserByEmail(email) != null)
            {
                return new DuplicateEmailException();
            }

            //Create user
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };

            //Create JWT token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }
    }
}
