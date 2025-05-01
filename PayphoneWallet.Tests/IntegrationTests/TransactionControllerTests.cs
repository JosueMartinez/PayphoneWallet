using Moq;
using Microsoft.AspNetCore.Mvc;
using PayphoneWallet.Entities.DTOs;
using PayphoneWallet.Business.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayphoneWallet.Api.Controllers;
using System;

namespace PayphoneWallet.Tests.IntegrationTests
{
    public class TransactionControllerTests
    {
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly Mock<IWalletService> _walletServiceMock;
        private readonly TransactionController _controller;

        public TransactionControllerTests()
        {
            _transactionServiceMock = new Mock<ITransactionService>();
            _walletServiceMock = new Mock<IWalletService>();
            _controller = new TransactionController(_transactionServiceMock.Object, _walletServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WhenTransactionsExist()
        {
            // Arrange
            var transactions = new List<TransactionResponseDto>
            {
                new TransactionResponseDto
                {
                    Id = 1,
                    SourceWalletId = 1,
                    DestinationWalletId = 2,
                    Amount = 100,
                    TransactionType = Entities.enums.TransactionType.Debit,
                    CreatedAt = DateTime.UtcNow
                }
            };

            _transactionServiceMock.Setup(s => s.GetAllTransactionsAsync()).ReturnsAsync(transactions);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TransactionResponseDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenNoTransactionsExist()
        {
            // Arrange
            _transactionServiceMock.Setup(s => s.GetAllTransactionsAsync()).ReturnsAsync(new List<TransactionResponseDto>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No existen transacciones realizadas.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenTransactionExists()
        {
            // Arrange
            var transaction = new TransactionResponseDto
            {
                Id = 1,
                SourceWalletId = 1,
                DestinationWalletId = 2,
                Amount = 100,
                TransactionType = Entities.enums.TransactionType.Debit,
                CreatedAt = DateTime.UtcNow
            };

            _transactionServiceMock.Setup(s => s.GetTransactionByIdAsync(1)).ReturnsAsync(transaction);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<TransactionResponseDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenTransactionDoesNotExist()
        {
            // Arrange
            _transactionServiceMock.Setup(s => s.GetTransactionByIdAsync(999)).ReturnsAsync((TransactionResponseDto)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Transaction with ID 999 not found.", notFoundResult.Value);
        }


        [Fact]
        public async Task Create_ReturnsBadRequest_WhenTransactionIsInvalid()
        {
            // Arrange
            var invalidTransaction = new CreateTransactionDto
            {
                SourceWalletId = 1,
                DestinationWalletId = 1,
                Amount = 100,
                Type = Entities.enums.TransactionType.Debit,
                Comment = "Invalid transaction"
            };

            // Act
            var result = await _controller.Create(invalidTransaction);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("La billetera origen y destino no pueden ser las mismas", badRequestResult.Value);
        }
    }

}
