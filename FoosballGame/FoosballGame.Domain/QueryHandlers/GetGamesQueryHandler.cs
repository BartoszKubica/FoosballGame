using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoosballGame.Contracts.Queries;
using FoosballGame.Infrastructure;

namespace FoosballGame.Domain.QueryHandlers
{
    internal class GetGamesQueryHandler : IQueryHandler<GetGames, IReadOnlyCollection<GameDetails>>
    {
        private readonly IGameRepository repository;

        public GetGamesQueryHandler(IGameRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IReadOnlyCollection<GameDetails>> HandleAsync(GetGames query)
        {
            var entities = await repository.List();

            return entities.Select(x => x.ToGameDetails()).ToArray();
        }
    }
}
