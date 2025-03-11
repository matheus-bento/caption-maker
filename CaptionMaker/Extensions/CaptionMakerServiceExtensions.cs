using CaptionMaker.Data.Repository;
using CaptionMaker.Service.ImageStorage;
using CaptionMaker.Service.JwtService;

namespace CaptionMaker.Extensions
{
    /// <summary>
    ///     Class containing extension methods to make it easier to register all the services
    ///     created for this project
    /// </summary>
    public static class CaptionMakerServiceExtensions
    {
        /// <summary>
        ///     Registers all the data access repostories as services in the <see cref="IServiceCollection"/>
        /// </summary>
        public static IServiceCollection AddCaptionMakerRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICaptionRepository, CaptionRepository>();

            return services;
        }

        /// <summary>
        ///     Registers all the application logic classes as services in the <see cref="IServiceCollection"/>
        /// </summary>
        public static IServiceCollection AddCaptionMakerServices(this IServiceCollection services)
        {
            services.AddScoped<JwtService>();

            // TODO: Use an env var to choose the image storage method
            services.AddScoped<IImageStorageService, HttpImageStorageService>();

            return services;
        }
    }
}
