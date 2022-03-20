using FoosballGame.Infrastructure;

namespace FoosballGame.Domain
{
    internal class TransactionExecutor : ITransactionExecutor
    {
        private readonly FoosballGameDbContext context;

        public TransactionExecutor(FoosballGameDbContext context)
        {
            this.context = context;
        }

        public async Task Commit()
            => await context.SaveChangesAsync();
    }
}