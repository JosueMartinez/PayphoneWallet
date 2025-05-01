using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayphoneWallet.Business.Contracts;
using PayphoneWallet.Entities;
using PayphoneWallet.Entities.DTOs;
using System.Net;

namespace PayphoneWallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IWalletService _walletService;

        public TransactionController(ITransactionService transactionService, IWalletService walletService)
        {
            _transactionService = transactionService;
            _walletService = walletService;
        }

        /// <summary>
        /// Obtiene todas las transacciones.
        /// </summary>
        /// <returns>Lista de todas las transacciones</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAll()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            if (transactions == null || !transactions.Any())
            {
                return NotFound("No existen transacciones realizadas.");
            }

            return Ok(transactions);
        }

        /// <summary>
        /// Obtiene una transacción por su ID.
        /// </summary>
        /// <param name="id">ID de la transacción</param>
        /// <returns>Transacción solicitada</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetById(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound($"No existen la transacción.");
            }

            return Ok(transaction);
        }

        /// <summary>
        /// Obtiene transacciones de una billetera específica.
        /// </summary>
        /// <param name="walletId">ID de la billetera</param>
        /// <returns>Lista de transacciones asociadas a la billetera</returns>
        [HttpGet("wallet/{walletId}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetByWalletId(int walletId)
        {
            var transactions = await _transactionService.GetTransactionsByWalletIdAsync(walletId);
            if (transactions == null || !transactions.Any())
            {
                return NotFound($"No existen transacciones registradas.");
            }

            return Ok(transactions);
        }

        /// <summary>
        /// Crea una nueva transacción.
        /// </summary>
        /// <param name="transaction">Objeto de transacción a crear</param>
        /// <returns>Resultado de la creación de la transacción</returns>
        [HttpPost]
        public async Task<ActionResult<Transaction>> Create([FromBody] CreateTransactionDto transaction)
        {
            // Validación de los datos de la transacción
            if (transaction == null)
            {
                return BadRequest("La transacción debe ser válida");
            }

            if (transaction.Amount <= 0)
            {
                return BadRequest("La transacción debe ser por un monto válido");
            }

            // Verificar si la billetera de origen y destino existen
            if (transaction.SourceWalletId == transaction.DestinationWalletId)
            {
                return BadRequest("La billetera origen y destino no pueden ser las mismas");
            }

            // Verificar si el saldo de la billetera es suficiente
            var sourceWalletBalance = await _walletService.GetWalletBalanceAsync(transaction.SourceWalletId);
            if (sourceWalletBalance < transaction.Amount)
            {
                return BadRequest("No cuenta con los fondos suficientes para realizar la transacción");
            }

            try
            {
                var created = await _transactionService.CreateTransactionAsync(transaction);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Ha ocurrido un error creando la transacción: {ex.Message}");
            }
        }
    }
}
