using FoosballGame.Contracts;
using FoosballGame.Infrastructure;

namespace FoosballGame.Domain.CommandHandlers
{
    internal class CreateGameCommandHandler : ICommandHandler<CreateGame>
    {
        private readonly IGameRepository repository;

        public CreateGameCommandHandler(IGameRepository repository)
        {
            this.repository = repository;
        }

        public Task Handle(CreateGame command)
        {
            var entity = new GameDb(command.Id, DateTime.UtcNow);
            repository.Add(entity);

            return Task.CompletedTask;
        }
    }
}