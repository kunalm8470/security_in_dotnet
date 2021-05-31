using Api.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public ActionResult<Todo[]> Get()
        {
            string username = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string email = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            Todo[] todos = new[]
            {
                new Todo(1, "Buy groceries", false),
                new Todo(2, "Clean house", true)
            };
            return Ok(todos);
        }
    }
}
