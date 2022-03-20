using FoosballGame.Infrastructure;

namespace FoosballGame.Contracts
{
    public record CreateGame(Guid Id) : ICommand;
}