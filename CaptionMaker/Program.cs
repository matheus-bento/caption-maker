using System.Text;
using System.Text.Json;
using CaptionMaker.Data;
using CaptionMaker.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CaptionMaker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplication app = CreateWebApplicationBuilder(args).Build();

            // Configure the HTTP request pipeline.

            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static WebApplicationBuilder CreateWebApplicationBuilder(string[] args)
        {
            WebApplicationBuilder webAppBuilder = WebApplication.CreateBuilder(args);

            ConfigureConfigurationProviders(webAppBuilder);
            ConfigureServices(webAppBuilder);

            return webAppBuilder;
        }

        private static void ConfigureConfigurationProviders(IHostApplicationBuilder builder)
        {
            builder.Configuration.AddEnvironmentVariables("CAPTION_MAKER_");
        }

        private static void ConfigureServices(IHostApplicationBuilder builder)
        {
            builder.Services.AddCaptionMakerRepositories();
            builder.Services.AddCaptionMakerServices();

            // Application options service (IOptions)

            builder.Services.Configure<CaptionMakerOptions>(builder.Configuration);

            // EF Service

            builder.Services.AddDbContext<CaptionContext>((services, dbContextOptions) =>
            {
                var appOptions = services.GetRequiredService<IOptions<CaptionMakerOptions>>();

                dbContextOptions.UseMySQL(
                    appOptions.Value.DbConnectionString,
                    mySqlOptions => mySqlOptions.MigrationsAssembly("CaptionMaker.Data")
                );
            });

            // ASP.NET services

            builder.Services
                .AddAuthentication(authOptions =>
                {
                    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtOptions =>
                {
                    string jwtSecret = builder.Configuration.GetValue<string>("JWT_SECRET");

                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),

                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    jwtOptions.RequireHttpsMetadata = false;
                    jwtOptions.SaveToken = true;
                });

            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                });
        }
    }
}
