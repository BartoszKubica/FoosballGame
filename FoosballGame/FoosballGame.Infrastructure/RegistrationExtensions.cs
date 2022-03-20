using Microsoft.Extensions.DependencyInjection;

namespace FoosballGame.Infrastructure
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection RegisterMediator(this IServiceCollection services)
        {
            return services.AddScoped<IMediator, Mediator>();
        }
    }
}