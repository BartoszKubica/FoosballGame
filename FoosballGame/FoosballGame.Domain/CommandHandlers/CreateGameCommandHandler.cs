﻿using FoosballGame.Contracts;
using FoosballGame.Domain.AddPoint;
using FoosballGame.Infrastructure;

namespace FoosballGame.Domain.CommandHandlers
{
    internal class CreateGameCommandHandler : ICommandHandler<CreateGame>
    {
        private readonly IGameRepository repository;
        private readonly ITransactionExecutor transactionExecutor;
        public CreateGameCommandHandler(IGameRepository repository, ITransactionExecutor transactionExecutor)
        {
            this.repository = repository;
            this.transactionExecutor = transactionExecutor;
        }

        public async Task Handle(CreateGame command)
        {
            var entity = new GameDb(command.Id, DateTime.UtcNow);
            repository.Add(entity);
            await transactionExecutor.Commit();
        }
    }
}