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
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletDbContext _dbContext;

        public WalletRepository(WalletDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddAsync(Wallet wallet)
        {
            await _dbContext.Wallets.AddAsync(wallet);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var wallet = await GetByIdAsync(id);
            if (wallet == null)
                return false;

            _dbContext.Wallets.Remove(wallet);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Wallet>> GetAllAsync()
        {
            return await _dbContext.Wallets.ToListAsync();
        }

        public async Task<Wallet> GetByIdAsync(int id)
        {
            return await _dbContext.Wallets.FindAsync(id);
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            _dbContext.Wallets.Update(wallet);
            await _dbContext.SaveChangesAsync();
        }
    }
}
