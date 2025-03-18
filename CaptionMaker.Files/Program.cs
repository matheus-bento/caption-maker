using System.Text.Json;
using CaptionMaker.Files.Authorization;

namespace CaptionMaker.Files
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
            builder.Configuration.AddEnvironmentVariables("CAPTION_MAKER_FILES_");
        }

        private static void ConfigureServices(IHostApplicationBuilder builder)
        {
            // Application options service (IOptions)

            builder.Services.Configure<CaptionMakerFilesOptions>(builder.Configuration);

            builder.Services.AddSingleton<ApiKeyAuthorizationFilter>();

            // ASP.NET Core services

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
