using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using PayphoneWallet.Api.Controllers;
using PayphoneWallet.Business.Contracts;
using PayphoneWallet.Entities.DTOs;

namespace PayphoneWallet.Tests.IntegrationTests
{
    public class WalletControllerTests
    {
        private readonly Mock<IWalletService> _walletServiceMock;
        private readonly WalletController _controller;

        public WalletControllerTests()
        {
            _walletServiceMock = new Mock<IWalletService>();
            _controller = new WalletController(_walletServiceMock.Object);
        }

        [Fact]
        public async Task GetById_ReturnsWallet_WhenExists()
        {
            // Arrange
            var walletId = 1;
            var expectedWallet = new WalletResponseDto
            {
                Id = walletId,
                DocumentId = "123456789",
                Name = "Test Wallet",
                Balance = 100
            };

            _walletServiceMock.Setup(s => s.GetWalletByIdAsync(walletId))
                .ReturnsAsync(expectedWallet);

            // Act
            var result = await _controller.Get(walletId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var wallet = Assert.IsType<WalletResponseDto>(okResult.Value);
            Assert.Equal(expectedWallet.Id, wallet.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenWalletNotFound()
        {
            // Arrange
            _walletServiceMock.Setup(s => s.GetWalletByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new System.Exception("La billetera no existe."));

            // Act
            var result = await _controller.Get(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedResult_WhenValid()
        {
            // Arrange
            var createDto = new CreateWalletDto
            {
                DocumentId = "987654321",
                Name = "New Wallet",
                InitialBalance = 50
            };

            var expectedResponse = new WalletResponseDto
            {
                Id = 1,
                DocumentId = createDto.DocumentId,
                Name = createDto.Name,
                Balance = createDto.InitialBalance
            };

            _walletServiceMock.Setup(s => s.CreateWalletAsync(createDto))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var wallet = Assert.IsType<WalletResponseDto>(createdResult.Value);
            Assert.Equal(expectedResponse.Id, wallet.Id);
        }
    }
}
