namespace FoosballGame.Contracts.Exceptions
{
    public class EntityNotFound : Exception
    {
        private EntityNotFound(string message) : base(message)
        {
        }

        public static EntityNotFound Create<TEntity>(Guid id)
            where TEntity : class 
            => new ($"{typeof(TEntity).Name} with id: {id} not found");
    }
}