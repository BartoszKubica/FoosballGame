using FoosballGame.Contracts;
using FoosballGame.Contracts.Queries;
using FoosballGame.Domain.CommandHandlers;
using FoosballGame.Domain.QueryHandlers;
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
            services.AddScoped<ICommandHandler<AddPointToGame>, AddPointCommandHandler>();
            services.AddScoped<IQueryHandler<GetGames, IReadOnlyCollection<GameDetails>>, GetGamesQueryHandler>();
            services.AddScoped<IQueryHandler<GetGameDetails, GameDetails>, GetGameDetailsQueryHandler>();
        }
    }
}
