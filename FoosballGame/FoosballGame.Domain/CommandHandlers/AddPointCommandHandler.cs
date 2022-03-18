using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoosballGame.Contracts;
using FoosballGame.Domain.AddPoint;
using FoosballGame.Infrastructure;

namespace FoosballGame.Domain.CommandHandlers
{
    internal class AddPointCommandHandler : ICommandHandler<AddPointToGame>
    {
        private readonly IGameRepository repository;
        private readonly ITransactionExecutor transactionExecutor;

        public AddPointCommandHandler(IGameRepository repository, ITransactionExecutor transactionExecutor)
        {
            this.repository = repository;
            this.transactionExecutor = transactionExecutor;
        }


        public async Task Handle(AddPointToGame command)
        {
            var entity = await repository.Get(command.Id);

            var game = AddPointWorkflow.AddPoint(command.Team, entity.State.ResolveState())
                .GetResult();

            entity.State.ApplyState(game);

            repository.Update(entity);
            await transactionExecutor.Commit();
        }
    }
}
