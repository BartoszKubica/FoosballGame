using FoosballGame.Contracts.Exceptions;
using FoosballGame.Domain.AddPoint;
using Microsoft.EntityFrameworkCore;

namespace FoosballGame.Domain
{
    internal interface IGameRepository
    {
        void Add(GameDb entity);
        Task<GameDb> Get(Guid id);
        void Update(GameDb entity);
        Task<IReadOnlyCollection<GameDb>> List();
    }

    internal class GameRepository : IGameRepository
    {
        private readonly FoosballGameDbContext context;

        public GameRepository(FoosballGameDbContext context)
        {
            this.context = context;
        }

        public void Add(GameDb entity)
        {
            context.Game.Add(entity);
        }

        public async Task<GameDb> Get(Guid id)
        {
            var entity = await context.Game.SingleOrDefaultAsync(x => x.Id.Equals(id));

            if (entity is null)
                throw EntityNotFound.Create<Game>(id);

            return entity;
        }

        public async Task<IReadOnlyCollection<GameDb>> List()
            => await context.Game.OrderByDescending(x => x.StartDateTime).ToListAsync();

        public void Update(GameDb entity)
            => context.Update(entity);
    }
}