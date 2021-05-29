using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult HandleError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            var errorMessage = exception.Message;
            var stackTrace = exception.StackTrace;

            return Problem(
                detail: stackTrace,
                type: $"Server Error - {errorMessage}",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error"
            );
        }
    }
}
