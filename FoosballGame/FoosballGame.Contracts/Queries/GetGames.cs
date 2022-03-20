using FoosballGame.Infrastructure;

namespace FoosballGame.Contracts.Queries
{
    public record GetGames : IQuery<IReadOnlyCollection<GameDetails>>;
}