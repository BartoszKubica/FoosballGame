using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoosballGame.Domain
{
    internal interface ITransactionExecutor
    {
        Task Commit();
    }

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
