namespace FoosballGame.Contracts.Exceptions
{
    public class InvalidState : Exception
    {
        private InvalidState(string message) : base(message)
        {
        }

        public static InvalidState Create<TEntity, TExpected>()
            => new InvalidState($"Entity {typeof(TEntity).Name} does not have expected status" +
                                $" {typeof(TExpected).Name}");
    }
}