using HotDinner.Application.Common.Interfaces.Authentication;
using HotDinner.Application.Common.Interfaces.Persistence;
using HotDinner.Domain.Entities;

namespace HotDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        //Check if user already exists
        if (_userRepository.GetByEmail(email) != null)
            throw new Exception("User with given email already exists");

        //Create user (generate unique id)

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        _userRepository.Add(user);

        //Generate JWT token

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }

    public AuthenticationResult Login(string email, string password)
    {

        if (_userRepository.GetByEmail(email) is not User user)
            throw new Exception("Wrong email or password");
        
        if (user.Password != password)
            throw new Exception("Wrong email or password");

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }
}