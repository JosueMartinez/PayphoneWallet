using PayphoneWallet.Business.Contracts;
using PayphoneWallet.Core.Contracts;
using PayphoneWallet.Entities;
using PayphoneWallet.Entities.DTOs;
using PayphoneWallet.Entities.enums;
using System.Data.Common;
using System.Transactions;
using Transaction = PayphoneWallet.Entities.Transaction;

namespace PayphoneWallet.Business.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletRepository _walletRepository;

        public TransactionService(ITransactionRepository transactionRepository, IWalletRepository walletRepository)
        {
            _transactionRepository = transactionRepository;
            _walletRepository = walletRepository;
        }

        public async Task<TransactionResponseDto> CreateTransactionAsync(CreateTransactionDto dto)
        {
            //Validaciones
            var sourceWallet = await _walletRepository.GetByIdAsync(dto.SourceWalletId);
            var destinationWallet = await _walletRepository.GetByIdAsync(dto.DestinationWalletId);

            if (sourceWallet == null || destinationWallet == null)
                throw new InvalidOperationException("Una de las billeteras no existe.");

            if (sourceWallet.Balance < dto.Amount)
                throw new InvalidOperationException("Fondos insuficientes.");

            if(dto.Amount <= 0)
                throw new InvalidOperationException("El monto debe ser mayor a cero.");

            // Actualizar balances
            sourceWallet.Balance -= dto.Amount;
            destinationWallet.Balance += dto.Amount;

            var trans = new Transaction
            {
                SourceWalletId = dto.SourceWalletId,
                DestinationWalletId = dto.DestinationWalletId,
                Type = dto.Type,
                Amount = dto.Amount,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            // Guardar cambios
            await _walletRepository.UpdateAsync(sourceWallet);
            await _walletRepository.UpdateAsync(destinationWallet);
            var createdTransaction = await _transactionRepository.CreateTransactionAsync(trans);

            var transactionDto = new TransactionResponseDto
            {
                Id = createdTransaction.Id,
                SourceWalletId = createdTransaction.SourceWalletId,
                DestinationWalletId = createdTransaction.DestinationWalletId,
                Amount = createdTransaction.Amount,
                Comment = createdTransaction.Comment,
                TransactionType = createdTransaction.Type, // Incluir tipo de transacción en la respuesta
                CreatedAt = createdTransaction.CreatedAt
            };

            return transactionDto;
        }

        public async Task<IEnumerable<TransactionResponseDto>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllAsync();

            var transactionDtos = transactions.Select(t => new TransactionResponseDto
            {
                Id = t.Id,
                SourceWalletId = t.SourceWalletId,
                DestinationWalletId = t.DestinationWalletId,
                Amount = t.Amount,
                Comment = t.Comment,
                TransactionType = t.Type,
                CreatedAt = t.CreatedAt
            });

            return transactionDtos;
        }

        public async Task<TransactionResponseDto> GetTransactionByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);

            if (transaction == null)
            {
                throw new InvalidOperationException($"No existe la transacción.");
            }

            var transactionDto = new TransactionResponseDto
            {
                Id = transaction.Id,
                SourceWalletId = transaction.SourceWalletId,
                DestinationWalletId = transaction.DestinationWalletId,
                Amount = transaction.Amount,
                Comment = transaction.Comment,
                TransactionType = transaction.Type,
                CreatedAt = transaction.CreatedAt
            };

            return transactionDto;
        }

        public async Task<IEnumerable<TransactionResponseDto>> GetTransactionsByWalletIdAsync(int walletId)
        {
            var transactions = await _transactionRepository.GetAllFromWalletlAsync(walletId);

            var transactionDtos = transactions.Select(t => new TransactionResponseDto
            {
                Id = t.Id,
                SourceWalletId = t.SourceWalletId,
                DestinationWalletId = t.DestinationWalletId,
                Amount = t.Amount,
                Comment = t.Comment,
                TransactionType = t.Type,
                CreatedAt = t.CreatedAt
            });

            return transactionDtos;        
        }
    }
}
