using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HotDinner.Contracts.Authentication;
using HotDinner.Application.Services.Authentication;
using HotDinner.Api.Filters;
using OneOf;
using HotDinner.Application.Common.Errors;

namespace HotDinner.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    //[ErrorHandlingFilter]
    public class AuthenticationController : ControllerBase
    {

        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            OneOf<AuthenticationResult, DuplicateEmailError> registerResult = _authenticationService.Register(
                request.FirstName, 
                request.LastName ,
                request.Email, 
                request.Password);
            
            return registerResult.Match(
                authResult => Ok(MapAuthResult(authResult)),
                _ => Problem(statusCode: StatusCodes.Status409Conflict, title : "Email is already used")
            );

        }

        private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
        {
            return new AuthenticationResponse(
                                authResult.User.Id,
                                authResult.User.FirstName,
                                authResult.User.LastName,
                                authResult.User.Email,
                                authResult.Token);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
              var authResult = _authenticationService.Login(
                request.Email, 
                request.Password);

            var response = new AuthenticationResponse(
                authResult.User.Id,
                authResult.User.FirstName,
                authResult.User.LastName,
                authResult.User.Email,
                authResult.Token);

            return Ok(response);
        }
    }
        
}