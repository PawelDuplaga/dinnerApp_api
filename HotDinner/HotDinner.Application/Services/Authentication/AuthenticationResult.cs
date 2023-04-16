using HotDinner.Domain.Entities;

namespace HotDinner.Application.Services.Authentication;

    public record AuthenticationResult
    (
        User User,
        string Token
    );
