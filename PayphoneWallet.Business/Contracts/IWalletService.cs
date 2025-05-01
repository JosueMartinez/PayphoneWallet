using PayphoneWallet.Entities;
using PayphoneWallet.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayphoneWallet.Business.Contracts
{
    public interface IWalletService
    {
        Task<WalletResponseDto> CreateWalletAsync(CreateWalletDto wallet);
        Task<WalletResponseDto> GetWalletByIdAsync(int id);
        Task<IEnumerable<WalletResponseDto>> GetAllWalletsAsync();
        Task UpdateWalletAsync(UpdateWalletDto wallet);
        Task DeleteWalletAsync(int id);
        Task<decimal> GetWalletBalanceAsync(int sourceWalletId);
    }
}
