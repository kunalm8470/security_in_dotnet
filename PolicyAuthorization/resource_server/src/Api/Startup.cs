using Api.Authorization.Handlers;
using Api.Authorization.Requirements;
using Core.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AuthenticationConfiguration authenticationConfiguration = new();
            Configuration.Bind("Authentication", authenticationConfiguration);
            authenticationConfiguration.AccessTokenKeys = new AsymmetricKeys
            {
                PublicKey = File.ReadAllText(Path.Join(HostEnvironment.ContentRootPath, "Keys", "AccessToken", "public_key.pem"))
            };
            
            services.AddSingleton(authenticationConfiguration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer((options) =>
            {
                RSA publicKey = RSA.Create();
                publicKey.ImportFromPem(authenticationConfiguration.AccessTokenKeys.PublicKey);

                SecurityKey key = new RsaSecurityKey(publicKey);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key, 
                    ValidIssuer = authenticationConfiguration.Issuer,
                    ValidAudience = authenticationConfiguration.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Register policies with requirements
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AtLeast18", policy =>
                {
                    policy.Requirements.Add(new CastVoteRequirement(18));
                });
            });

            // Register requirement handler
            services.AddSingleton<IAuthorizationHandler, CastVoteHandler>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Error");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
