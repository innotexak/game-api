using Microsoft.Extensions.DependencyInjection;
using VideoGameApi.Services;
using VideoGameApi.Services.Interfaces;

namespace VideoGameApi.Configurations
{
    public static class ServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IVideoGameService, VideoGameService>();
            services.AddScoped<IAuthService, AuthService>();
           
        }
    }
}
