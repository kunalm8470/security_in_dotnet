using Api.Models.Requests;
using Api.Models.Responses;
using AutoMapper;
using Core.Entities;
using Core.Exceptions;
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
    public class AccountsController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IPasswordService _passwordService;
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMapper _mapper;
        private readonly AuthenticationConfiguration _authConfig;

        public AccountsController(
            IUserService service,
            IPasswordService passwordService,
            IAccessTokenService accessTokenService,
            IRefreshTokenService tokenService,
            IMapper mapper,
            AuthenticationConfiguration authConfig
        )
        {
            _service = service;
            _passwordService = passwordService;
            _accessTokenService = accessTokenService;
            _refreshTokenService = tokenService;
            _mapper = mapper;
            _authConfig = authConfig;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserDto dto)
        {
            try
            {
                User created = await _service.RegisterUserAsync(_mapper.Map<RegisterUserDto, User>(dto));
                return CreatedAtAction("LoginUser", _mapper.Map<User, RegisterUserResponseDto>(created));
            }
            catch (UniqueConstraintException)
            {
                return Conflict(new HttpError("ENTRY_ALREADY_EXISTS", "Username / email already exists!"));
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<RefreshTokenResponse>> LoginUserAsync([FromBody] LoginUserDto dto)
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

            string accessToken = _accessTokenService.Generate(found);
            string refreshToken = _refreshTokenService.Generate();
            await _refreshTokenService.PersistTokenAsync(refreshToken, found.Id);

            RefreshTokenResponse response = new(accessToken, _authConfig.AccessTokenExpirationMinutes, refreshToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Token")]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshTokenAsync([FromBody] RefreshTokenDto dto)
        {
            RefreshToken token = await _refreshTokenService.FetchTokenAsync(dto.RefreshToken);
            if (token == default)
            {
                return Unauthorized(new HttpError("invalid_client", "Refresh token not found!"));
            }

            try
            {
                if (!_refreshTokenService.Validate(token))
                    return Unauthorized(new HttpError("invalid_client", "Invalid refresh token!"));
            }
            catch(RefreshTokenExpiredException)
            {
                return Unauthorized(new HttpError("invalid_client", "Refresh token expired!"));
            }
            
            string accessToken = _accessTokenService.Generate(token.User);
            AccessTokenResponse response = new(accessToken, _authConfig.AccessTokenExpirationMinutes);
            return Ok(response);
        }
      
        [Authorize]
        [HttpPost("Revoke")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] RevokeTokenDto dto)
        {
            if (!await _refreshTokenService.TryDeleteTokenAsync(dto.Token))
            {
                return Unauthorized(new HttpError("invalid_client", "Refresh token not found!"));
            }

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

            await _refreshTokenService.DeleteTokenByUserAsync(userId);
            return NoContent();
        }
    }
}
