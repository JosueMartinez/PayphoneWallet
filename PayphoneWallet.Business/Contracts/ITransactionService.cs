using PayphoneWallet.Entities;
using PayphoneWallet.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayphoneWallet.Business.Contracts
{
    public interface ITransactionService
    {
        Task<TransactionResponseDto> CreateTransactionAsync(CreateTransactionDto transaction);
        Task<TransactionResponseDto> GetTransactionByIdAsync(int id);
        Task<IEnumerable<TransactionResponseDto>> GetTransactionsByWalletIdAsync(int walletId);
        Task<IEnumerable<TransactionResponseDto>> GetAllTransactionsAsync();
    }
}
