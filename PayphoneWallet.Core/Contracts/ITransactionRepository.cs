

using PayphoneWallet.Entities;

namespace PayphoneWallet.Core.Contracts
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetAllFromWalletlAsync(int walletId);
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction> GetByIdAsync(int id);
    }
}
