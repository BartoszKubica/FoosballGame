using FoosballGame.Infrastructure;

namespace FoosballGame.Contracts
{
    public record AddPointToGame(Guid Id, Team Team) : ICommand;
}