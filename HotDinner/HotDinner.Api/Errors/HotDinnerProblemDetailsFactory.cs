using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace  HotDinner.Api.Errors;

//default ProblemDetails from Microsoft.AspNetCore.Mvc.ProblemDetailsFactory
//changed for our needs
public class HotDinnerProblemDetailsFactory : ProblemDetailsFactory
{

    private readonly ApiBehaviorOptions _options;

    public HotDinnerProblemDetailsFactory(IOptions<ApiBehaviorOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        statusCode ??= 500;

        var problemDetails = new ProblemDetails
        {
            Type = type,
            Title = title,
            Status = statusCode,
            Detail = detail,
            Instance = instance
        };

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        throw new NotImplementedException();
    }

     private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
    {
        problemDetails.Status ??= statusCode;

        if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
        {
            problemDetails.Type ??= clientErrorData.Link;
            problemDetails.Title ??= clientErrorData.Title;
        }

        var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;

        if (traceId != null)
        {
            problemDetails.Extensions["traceId"] = traceId;
        }
        
        problemDetails.Extensions.Add("customProperty", "customValue");



    }
}