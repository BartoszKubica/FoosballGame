using FoosballGame.Contracts;
using FoosballGame.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace FoosballGame.Domain
{
    public static class Bootstrap
    {
        public static void BootstrapDomain(this IServiceCollection services)
        {
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<ITransactionExecutor, TransactionExecutor>();
            services.AddScoped<ICommandHandler<CreateGame>, CreateGameCommandHandler>();
        }
    }
}
