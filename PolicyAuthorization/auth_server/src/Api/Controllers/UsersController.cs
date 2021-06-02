using Api.Models.Requests;
using Api.Models.Responses;
using AutoMapper;
using Core.Entities;
using Core.Services;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly AuthenticationConfiguration _authConfig;

        public UsersController(
            IUserService service,
            IPasswordService passwordService,
            ITokenService tokenService,
            IMapper mapper,
            AuthenticationConfiguration authConfig
        )
        {
            _service = service;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _mapper = mapper;
            _authConfig = authConfig;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserDto dto)
        {
            try
            {
                await _service.RegisterUserAsync(_mapper.Map<RegisterUserDto, User>(dto));
                return Ok();
            }
            catch (UniqueConstraintException)
            {
                return Conflict(new HttpError("ENTRY_ALREADY_EXISTS", "Username / email already exists!"));
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> LoginUserAsync([FromBody] LoginUserDto dto)
        {
            User found = await _service.FetchUserAsync(dto.Username);
            if (found == default)
            {
                return Unauthorized(new HttpError("invalid_client", "No user found associated with the login"));
            }

            if (!_passwordService.VerifyPassword(dto.Password, found.Password))
            {
                return Unauthorized(new HttpError("invalid_client", "Incorrect password!"));
            }

            string accessToken = _tokenService.GenerateAccessToken(found);
            string refreshToken = _tokenService.GenerateRefreshToken();
            await _tokenService.PersistRefreshToken(refreshToken, found.Id);

            LoginResponse response = new(accessToken, _authConfig.AccessTokenExpirationMinutes, refreshToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Token")]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshTokenAsync([FromBody] RefreshTokenDto dto)
        {
            // check refresh token is actually persisting or not
            RefreshToken token = await _tokenService.FetchRefreshToken(dto.RefreshToken);
            if (token == default)
            {
                return Unauthorized(new HttpError("invalid_client", "Refresh token not found!"));
            }

            // validate refresh token
            if (!_tokenService.ValidateRefreshToken(token))
            {
                return Unauthorized(new HttpError("invalid_client", "Invalid refresh token!"));
            }

            string accessToken = _tokenService.GenerateAccessToken(token.User);
            RefreshTokenResponse response = new(accessToken, _authConfig.AccessTokenExpirationMinutes);
            return Ok(response);
        }
      
        [AllowAnonymous]
        [HttpPost("Revoke")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] RevokeTokenDto dto)
        {
            // check refresh token is actually persisting or not
            RefreshToken token = await _tokenService.FetchRefreshToken(dto.Token);
            if (token == default)
            {
                return Unauthorized(new HttpError("invalid_client", "Refresh token not found!"));
            }

            await _tokenService.DeleteRefreshToken(dto.Token);
            return Ok();
        }

        [Authorize]
        [HttpDelete("Logout")]
        public async Task<IActionResult> LogoutUserAsync()
        {
            string rawUserId = HttpContext.User.FindFirstValue("id");
            if (!int.TryParse(rawUserId, out int userId))
            {
                return Unauthorized(new HttpError("invalid_client", "No user registered with the given id"));
            }

            await _tokenService.DeleteRefreshTokenForUser(userId);
            return NoContent();
        }
    }
}
