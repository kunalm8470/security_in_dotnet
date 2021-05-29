using Api.Models.Requests;
using Api.Models.Responses;
using AutoMapper;
using Core.Entities;
using Core.Services;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;

        public UsersController(
            IUserService service,
            IPasswordService passwordService,
            IMapper mapper
        )
        {
            _service = service;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<UserResponseDto>> RegisterUserAsync([FromBody] RegisterUserDto dto)
        {
            try
            {
                User u = _mapper.Map<RegisterUserDto, User>(dto);
                u.Password = _passwordService.HashPassword(u.Password);
                User created = await _service.RegisterUserAsync(u);
                return Ok(_mapper.Map<User, UserResponseDto>(created));
            }
            catch (UniqueConstraintException)
            {
                return Conflict(new HttpError("ENTRY_ALREADY_EXISTS", "Username / email already exists!"));
            }
        }
    }
}
