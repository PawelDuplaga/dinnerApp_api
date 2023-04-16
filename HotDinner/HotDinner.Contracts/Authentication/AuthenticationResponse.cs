using System;
namespace HotDinner.Contracts.Authentication;

public record AuthenticationResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Token
);
