using Microsoft.EntityFrameworkCore;
using PayphoneWallet.Core.Contracts;
using PayphoneWallet.Entities;
using PayphoneWallet.Infraestructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayphoneWallet.Infraestructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly WalletDbContext _dbContext;
        public TransactionRepository(WalletDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();

            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetAllFromWalletlAsync(int walletId)
        {
            return await _dbContext.Transactions
                .Where(t => t.SourceWalletId == walletId || t.DestinationWalletId == walletId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _dbContext.Transactions.ToListAsync();
        }

        public Task<Transaction> GetByIdAsync(int id)
        {
            return _dbContext.Transactions
                .Include(t => t.SourceWallet)
                .Include(t => t.DestinationWallet)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
