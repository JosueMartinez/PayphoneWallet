using System;
using System.Threading.Tasks;
using Moq;
using PayphoneWallet.Business.Services;
using PayphoneWallet.Entities.DTOs;
using PayphoneWallet.Core.Contracts;
using PayphoneWallet.Entities;

namespace PayphoneWallet.Tests.UnitTests
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepoMock;
        private readonly Mock<IWalletRepository> _walletRepoMock;
        private readonly TransactionService _service;

        public TransactionServiceTests()
        {
            _transactionRepoMock = new Mock<ITransactionRepository>();
            _walletRepoMock = new Mock<IWalletRepository>();
            _service = new TransactionService(_transactionRepoMock.Object, _walletRepoMock.Object);
        }

        [Fact]
        public async Task CreateTransactionAsync_SuccessfulTransaction_ReturnsTransactionResponse()
        {
            // Arrange
            var sourceWallet = new Wallet { Id = 1, Balance = 100 };
            var destinationWallet = new Wallet { Id = 2, Balance = 50 };
            var createDto = new CreateTransactionDto
            {
                SourceWalletId = 1,
                DestinationWalletId = 2,
                Amount = 30,
                Comment = "Test Transfer",
                Type = Entities.enums.TransactionType.Credit
            };

            var savedTransaction = new Transaction
            {
                Id = 99,
                SourceWalletId = 1,
                DestinationWalletId = 2,
                Amount = 30,
                Comment = "Test Transfer",
                Type = Entities.enums.TransactionType.Credit,
                CreatedAt = DateTime.UtcNow
            };

            _walletRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sourceWallet);
            _walletRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(destinationWallet);
            _transactionRepoMock.Setup(r => r.CreateTransactionAsync(It.IsAny<Transaction>())).ReturnsAsync(savedTransaction);

            // Act
            var result = await _service.CreateTransactionAsync(createDto);

            // Assert
            Assert.Equal(99, result.Id);
            Assert.Equal(1, result.SourceWalletId);
            Assert.Equal(2, result.DestinationWalletId);
            Assert.Equal(30, result.Amount);
            _walletRepoMock.Verify(r => r.UpdateAsync(sourceWallet), Times.Once);
            _walletRepoMock.Verify(r => r.UpdateAsync(destinationWallet), Times.Once);
        }

        [Fact]
        public async Task CreateTransactionAsync_InsufficientFunds_ThrowsException()
        {
            var sourceWallet = new Wallet { Id = 1, Balance = 10 };
            var destinationWallet = new Wallet { Id = 2, Balance = 0 };
            var createDto = new CreateTransactionDto
            {
                SourceWalletId = 1,
                DestinationWalletId = 2,
                Amount = 50,
                Comment = "Transfer",
                Type = Entities.enums.TransactionType.Credit
            };

            _walletRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sourceWallet);
            _walletRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(destinationWallet);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateTransactionAsync(createDto));

            Assert.Equal("Fondos insuficientes.", ex.Message);
        }

        [Fact]
        public async Task CreateTransactionAsync_WalletNotFound_ThrowsException()
        {
            _walletRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Wallet)null);

            var createDto = new CreateTransactionDto
            {
                SourceWalletId = 1,
                DestinationWalletId = 2,
                Amount = 10,
                Comment = "Transfer",
                Type = Entities.enums.TransactionType.Credit
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateTransactionAsync(createDto));

            Assert.Equal("Una de las billeteras no existe.", ex.Message);
        }

        [Fact]
        public async Task CreateTransactionAsync_AmountIsZero_ThrowsException()
        {
            var sourceWallet = new Wallet { Id = 1, Balance = 100 };
            var destinationWallet = new Wallet { Id = 2, Balance = 100 };
            var createDto = new CreateTransactionDto
            {
                SourceWalletId = 1,
                DestinationWalletId = 2,
                Amount = 0,
                Comment = "Transfer",
                Type = Entities.enums.TransactionType.Credit
            };

            _walletRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sourceWallet);
            _walletRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(destinationWallet);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateTransactionAsync(createDto));

            Assert.Equal("El monto debe ser mayor a cero.", ex.Message);
        }
    }
}
