namespace FoosballGame.Infrastructure
{
    public interface ITransactionExecutor
    {
        Task Commit();
    }
}