using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly IHostEnvironment _hostEnvironment;
        public ErrorController(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        [Route("/Error")]
        public IActionResult HandleError()
        {
            if (_hostEnvironment.IsDevelopment())
            {
                var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
                Exception exception = context.Error;

                return Problem(
                    detail: exception.Message,
                    type: "Server Error",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal Server Error"
                );
            }

            return Problem(
                type: "Server Error",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error"
            );
        }
    }
}
