using FoosballGame.Contracts;
using FoosballGame.Domain.AddPoint;
using FoosballGame.Infrastructure;

namespace FoosballGame.Domain.CommandHandlers
{
    internal class AddPointCommandHandler : ICommandHandler<AddPointToGame>
    {
        private readonly IGameRepository repository;

        public AddPointCommandHandler(IGameRepository repository)
        {
            this.repository = repository;
        }


        public async Task Handle(AddPointToGame command)
        {
            var entity = await repository.Get(command.Id);

            var game = AddPointWorkflow.AddPoint(command.Team, entity.State.ResolveState())
                .GetResult();

            entity.State.ApplyState(game);

            repository.Update(entity);
        }
    }
}