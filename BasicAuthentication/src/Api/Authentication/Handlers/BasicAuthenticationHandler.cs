using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Api.Authentication.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private const string _realm = "Default realm";

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserService userService,
            IPasswordService passwordService
        )
        : base(options,
              logger,
              encoder,
              clock)
        {
            _userService = userService;
            _passwordService = passwordService;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Bypass authentication for anonymous routes
            Endpoint endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != default)
                return AuthenticateResult.NoResult();

            if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                Response.Headers.Add(HeaderNames.WWWAuthenticate, string.Format(@"Basic realm=""{0}""", _realm));
                return AuthenticateResult.Fail("Authorization Header missing!");
            }
                
            // Extract username and password from authorization header
            string encodedCredentials = AuthenticationHeaderValue.Parse(Request.Headers[HeaderNames.Authorization]).Parameter;
            string credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            string[] credentialsSplit = credentials.Split(':', 2);
            string username = credentialsSplit[0];
            string password = credentialsSplit[1];

            // Validate user credentials
            User found = await _userService.FetchUserAsync(username);
            if (found == default)
            {
                Response.Headers.Add(HeaderNames.WWWAuthenticate, string.Format(@"Basic realm=""{0}""", _realm));
                return AuthenticateResult.Fail("User not found!");
            }

            if (!_passwordService.VerifyPassword(password, found.Password))
            {
                Response.Headers.Add(HeaderNames.WWWAuthenticate, string.Format(@"Basic realm=""{0}""", _realm));
                return AuthenticateResult.Fail("Incorrect password!");
            }

            // Build claims and claim principal
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name, found.FirstName),
                new Claim(ClaimTypes.Surname, found.LastName),
                new Claim(ClaimTypes.DateOfBirth, found.DateOfBirth.ToString("yyyy-MM-dd")),
                new Claim(ClaimTypes.MobilePhone, found.Phone),
                new Claim(ClaimTypes.NameIdentifier, found.Username),
                new Claim(ClaimTypes.Email, found.Email)
            };
            
            ClaimsPrincipal principal = new(new[]
            { 
                new ClaimsIdentity(claims, Scheme.Name)
            });

            AuthenticationTicket ticket = new(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
