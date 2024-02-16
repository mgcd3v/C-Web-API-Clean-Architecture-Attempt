using Application.Interfaces.Services;
using Application.Services;
using Domain.Interfaces.Repositories;
using dotenv.net;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services) 
        {
            DotEnv.Load(); // Load environment variables from .env file

            var serviceProvider = new ServiceCollection()
                                                        .AddSingleton<IAppSettingService, EnvSettingService>()
                                                        .AddSingleton<IAppSettingService, AppSettingService>()
                                                        .BuildServiceProvider();

            var appSettingsServices = serviceProvider.GetServices<IAppSettingService>();
            var appSettingService = appSettingsServices.First(o => o.GetType() == typeof(AppSettingService));
            var envSettingsService = appSettingsServices.First(o => o.GetType() == typeof(EnvSettingService));

            var connectionString = envSettingsService?.Get<string>("DefaultConnection", "ConnectionStrings");
            services.AddDbContext<DataContext>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseSqlServer(connectionString);
            });

            var tokenKey = envSettingsService?.Get<string>("Token", "Jwt") ?? "";
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                        ClockSkew = TimeSpan.Zero // Set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    };
                });

            var allowedRequestors = appSettingService?.Get<string[]>("AllowedRequestors");
            services.AddCors(options =>
            {
                options.AddPolicy("Default",
                      corsBuilder =>
                      {
                          corsBuilder.WithOrigins(allowedRequestors ?? Array.Empty<string>())  // maintain a json object of allowed requestor
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
            });

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<ICmbRepository, CmbRepository>();

            return services;
        }
    }
}
