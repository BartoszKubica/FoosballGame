namespace FoosballGame.Domain
{
    internal interface IGameRepository
    {
        void Add(GameDb entity);
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
    }
}
