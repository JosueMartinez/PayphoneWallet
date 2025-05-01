using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using PayphoneWallet.Business.Services;
using PayphoneWallet.Business.Contracts;
using PayphoneWallet.Entities;
using PayphoneWallet.Entities.DTOs;
using PayphoneWallet.Core.Contracts;

namespace PayphoneWallet.Tests.UnitTests
{
    public class WalletServiceTests
    {
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly WalletService _walletService;

        public WalletServiceTests()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _walletService = new WalletService(_walletRepositoryMock.Object);
        }

        [Fact]
        public async Task GetWalletByIdAsync_ReturnsWallet_WhenWalletExists()
        {
            // Arrange
            var wallet = new Wallet
            {
                Id = 1,
                DocumentId = "123456789",
                Name = "Test Wallet",
                Balance = 100,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _walletRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(wallet);

            // Act
            var result = await _walletService.GetWalletByIdAsync(1);

            // Assert
            Assert.Equal(wallet.Id, result.Id);
            Assert.Equal(wallet.DocumentId, result.DocumentId);
            Assert.Equal(wallet.Balance, result.Balance);
        }

        [Fact]
        public async Task CreateWalletAsync_CreatesWalletSuccessfully()
        {
            // Arrange
            var dto = new CreateWalletDto
            {
                DocumentId = "987654321",
                Name = "New Wallet",
                InitialBalance = 50
            };

            _walletRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Wallet>()))
                .Returns(Task.CompletedTask)
                .Callback<Wallet>(w => w.Id = 1); // Simula asignación de ID tras guardado

            // Act
            var result = await _walletService.CreateWalletAsync(dto);

            // Assert
            Assert.Equal(dto.DocumentId, result.DocumentId);
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(dto.InitialBalance, result.Balance);
        }
    }
}
