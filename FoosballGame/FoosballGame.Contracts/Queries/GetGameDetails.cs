using FoosballGame.Infrastructure;

namespace FoosballGame.Contracts.Queries
{
    public record GetGameDetails(Guid Id) : IQuery<GameDetails>;

    public record GameDetails(Guid Id, DateTime StartDate, IReadOnlyCollection<SetDetails> Sets);

    public record SetDetails(short Team1Score, short Team2Score);
}