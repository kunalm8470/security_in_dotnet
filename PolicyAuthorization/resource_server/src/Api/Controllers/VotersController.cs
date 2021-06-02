using Core.Entities;
using Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotersController : ControllerBase
    {
        [Authorize(Policy = "AtLeast18")]
        [HttpGet("Profile")]
        public ActionResult<VoterProfile> FetchProfile()
        {
            string firstName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            string lastName = HttpContext.User.FindFirst(ClaimTypes.Surname).Value;
            int age = HttpContext.User.FindFirst(ClaimTypes.DateOfBirth).Value.CalculateAge();

            return Ok(new VoterProfile(firstName, lastName, age, true));
        }
    }
}
