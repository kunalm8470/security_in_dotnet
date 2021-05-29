using Api.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Todo[]> Get()
        {
            string username = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string email = User.FindFirstValue(ClaimTypes.Email);

            Todo[] todos = new[]
            {
                new Todo(1, "Buy groceries", false),
                new Todo(2, "Clean house", true)
            };
            return Ok(todos);
        }
    }
}
