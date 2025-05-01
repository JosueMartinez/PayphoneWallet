using PayphoneWallet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayphoneWallet.Core.Contracts
{
    public interface IWalletRepository
    {
        Task<Wallet> GetByIdAsync(int id);
        Task<IEnumerable<Wallet>> GetAllAsync();
        Task AddAsync(Wallet wallet);
        Task UpdateAsync(Wallet wallet);
        Task<bool> DeleteAsync(int id);
    }
}
