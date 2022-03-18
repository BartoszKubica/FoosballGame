using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoosballGame.Contracts.Queries;
using FoosballGame.Infrastructure;

namespace FoosballGame.Domain.QueryHandlers
{
    internal class GetGameDetailsQueryHandler : IQueryHandler<GetGameDetails, GameDetails>
    {
        private readonly IGameRepository repository;

        public GetGameDetailsQueryHandler(IGameRepository repository)
        {
            this.repository = repository;
        }

        public async Task<GameDetails> HandleAsync(GetGameDetails query)
        {
            var entity = await repository.Get(query.Id);

            return entity.ToGameDetails();
        }
    }
}
