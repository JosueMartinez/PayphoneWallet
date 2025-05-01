using PayphoneWallet.Business.Contracts;
using PayphoneWallet.Core.Contracts;
using PayphoneWallet.Entities;
using PayphoneWallet.Entities.DTOs;

namespace PayphoneWallet.Business.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<WalletResponseDto> GetWalletByIdAsync(int id)
        {
            var wallet = await _walletRepository.GetByIdAsync(id);

            if (wallet == null)
            {
                throw new InvalidOperationException("La billetera no existe.");
            }

            var walletDto = new WalletResponseDto
            {
                Id = wallet.Id,
                DocumentId = wallet.DocumentId,
                Name = wallet.Name,
                Balance = wallet.Balance,
                CreatedAt = wallet.CreatedAt,
                UpdatedAt = wallet.UpdatedAt
            };

            return walletDto;
        }

        public async Task<IEnumerable<WalletResponseDto>> GetAllWalletsAsync()
        {
            var wallets = await _walletRepository.GetAllAsync();

            var walletDtos = wallets.Select(wallet => new WalletResponseDto
            {
                Id = wallet.Id,
                DocumentId = wallet.DocumentId,
                Name = wallet.Name,
                Balance = wallet.Balance,
                CreatedAt = wallet.CreatedAt,
                UpdatedAt = wallet.UpdatedAt
            });

            return walletDtos;
        }

        public async Task<WalletResponseDto> CreateWalletAsync(CreateWalletDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
            {
                throw new InvalidOperationException("Es necesario un nombre para su billetera.");
            }

            if (dto.InitialBalance <= 0)
            {
                throw new InvalidOperationException("Su billetera debe contar con un balance inicial para la creación");
            }

            try
            {
                var wallet = new Wallet
                {
                    DocumentId = dto.DocumentId,
                    Name = dto.Name,
                    Balance = dto.InitialBalance,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _walletRepository.AddAsync(wallet);

                var walletDto = new WalletResponseDto
                {
                    Id = wallet.Id,
                    DocumentId = wallet.DocumentId,
                    Name = wallet.Name,
                    Balance = wallet.Balance,
                    CreatedAt = wallet.CreatedAt,
                    UpdatedAt = wallet.UpdatedAt
                };

                return walletDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateWalletAsync(UpdateWalletDto dto)
        {
            var existing = await _walletRepository.GetByIdAsync(dto.Id);
            if (existing == null)
            {
                throw new Exception($"La billetera no existe.");
            }

            existing.DocumentId = dto.DocumentId;
            existing.Name = dto.Name;
            existing.Balance = dto.Balance;
            existing.UpdatedAt = DateTime.UtcNow;

            await _walletRepository.UpdateAsync(existing);
        }

        public async Task DeleteWalletAsync(int id)
        {
            var wallet = await _walletRepository.GetByIdAsync(id);
            if (wallet == null)
            {
                throw new Exception($"La billetera no existe.");
            }

            await _walletRepository.DeleteAsync(id);
        }

        public async Task<decimal> GetWalletBalanceAsync(int sourceWalletId)
        {
            var wallet = await _walletRepository.GetByIdAsync(sourceWalletId);

            if (wallet == null)
            {
                throw new InvalidOperationException($"No existe la billetera origen.");
            }

            return wallet.Balance;
        }
    }
}
