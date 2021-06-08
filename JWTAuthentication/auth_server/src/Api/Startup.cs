using Api.Models;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
            services.AddDbContext<UserContext>((options) =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddAutoMapper(typeof(Startup));

            // Register repositories
            services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // Register services
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAccessTokenService, AccessTokenService>();
            services.AddTransient<IRefreshTokenService, RefreshTokenService>();

            services.AddTransient(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile(provider.GetService<IPasswordService>()));
            }).CreateMapper());

            AuthenticationConfiguration authenticationConfiguration = new();
            Configuration.Bind("Authentication", authenticationConfiguration);
            authenticationConfiguration.AccessTokenKeys = new AsymmetricKeys
            {
                PrivateKey = File.ReadAllText(Path.Join(HostEnvironment.ContentRootPath, "Keys", "AccessToken", "private_key.pem")),
                PublicKey = File.ReadAllText(Path.Join(HostEnvironment.ContentRootPath, "Keys", "AccessToken", "public_key.pem"))
            };
            authenticationConfiguration.RefreshTokenKeys = new AsymmetricKeys
            {
                PrivateKey = File.ReadAllText(Path.Join(HostEnvironment.ContentRootPath, "Keys", "RefreshToken", "private_key.pem")),
                PublicKey = File.ReadAllText(Path.Join(HostEnvironment.ContentRootPath, "Keys", "RefreshToken", "public_key.pem"))
            };

            services.AddSingleton(authenticationConfiguration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer((options) =>
            {
                RSA publicKey = RSA.Create();
                publicKey.ImportFromPem(authenticationConfiguration.AccessTokenKeys.PublicKey);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new RsaSecurityKey(publicKey),
                    ValidIssuer = authenticationConfiguration.Issuer,
                    ValidAudience = authenticationConfiguration.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

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
