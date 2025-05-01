using Azure.Core;
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
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        /// <summary>
        /// Obtiene todas las billeteras.
        /// </summary>
        /// <returns>Lista de todas las billeteras</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WalletResponseDto>>> GetAll()
        {
            var wallets = await _walletService.GetAllWalletsAsync();

            if (wallets == null || !wallets.Any())
            {
                return NotFound("No hay información de billeteras existentes");
            }

            return Ok(wallets);
        }

        /// <summary>
        /// Obtiene una billetera por su ID.
        /// </summary>
        /// <param name="id">ID de la billetera</param>
        /// <returns>Billetera solicitada</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WalletResponseDto>> Get(int id)
        {
            try
            {
                var wallet = await _walletService.GetWalletByIdAsync(id);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Crea una nueva billetera.
        /// </summary>
        /// <param name="dto">Objeto billetera a crear</param>
        /// <returns>Resultado de la creación de la billetera</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWalletDto dto)
        {
            try
            {
                var wallet = await _walletService.CreateWalletAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = wallet.Id }, wallet);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Ocurrió un error creando la billetera: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza los datos de una billetera.
        /// </summary>
        /// <param name="id">ID de la billetera a actualizar</param>
        /// <param name="dto">Objeto billetera con datos actualizados</param>
        /// <returns>Resultado de la actualización</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWalletDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Debe proporcionar los datos a actualizar");
            }

            if (id != dto.Id)
            {
                return BadRequest("Debe proporcionar el mismo Id.");
            }

            var existingWallet = await _walletService.GetWalletByIdAsync(id);
            if (existingWallet == null)
            {
                return NotFound($"La billetera no existe.");
            }

            try
            {
                await _walletService.UpdateWalletAsync(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Ha ocurrido un error actualizando la billetera: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina una billetera.
        /// </summary>
        /// <param name="id">ID de la billetera a eliminar</param>
        /// <returns>Resultado de la eliminación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {            
            try
            {
                await _walletService.DeleteWalletAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Ha ocurrido un error eliminando la billetera: {ex.Message}");
            }
        }
    }
}
