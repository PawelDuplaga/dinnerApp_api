using HotDinner.Application.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace HotDinner.Api.Controllers
{
    [ApiController]
    [Route("errors")]
    public class ErrorsController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
            var (statusCode, message) = exception switch
            {
                IServiceException serviceException => ((int)serviceException.StatusCode, serviceException.ErrorMessage),
                _ => (StatusCodes.Status500InternalServerError, "Internal server error")
            };
            return Problem(statusCode : statusCode, detail : message);
        }
    }
}