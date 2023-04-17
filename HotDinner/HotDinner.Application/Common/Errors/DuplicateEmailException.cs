using System.Net;

namespace HotDinner.Application.Common.Errors;

public class DuplicateEmailException : Exception, IServiceException
{
    public HttpStatusCode StatusCode { get; } = HttpStatusCode.Conflict;
    public string ErrorMessage { get; } = "Email is already used";
}

