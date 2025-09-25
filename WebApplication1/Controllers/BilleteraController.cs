using Microsoft.AspNetCore.Mvc;
using TuBilletera.Requests;
using TuBilletera.Responses;
using TuBilletera.Services;

namespace TuBilletera.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BilleteraController : ControllerBase
    {
        private readonly BilleteraService _billeteraService;

        public BilleteraController(BilleteraService billeteraService)
        {
            _billeteraService = billeteraService;
        }

        // Crear cuenta digital
        [HttpPost]
        public IActionResult CrearBilletera([FromBody] CrearBilleteraRequest request)
        {
            try
            {
                var billetera = _billeteraService.CrearBilletera(request);
                return CreatedAtAction(nameof(ObtenerBilletera), new { cvu = billetera.Cvu }, billetera);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Obtener listado de billeteras
        [HttpGet]
        public IActionResult ObtenerBilleteras([FromQuery] string? ordenarPor, [FromQuery] bool asc = true, [FromQuery] string? estado = null)
        {
            var billeteras = _billeteraService.ObtenerBilleteras(ordenarPor, asc, estado);
            return Ok(billeteras);
        }

        // Obtener billetera por CVU
        [HttpGet("{cvu}")]
        public IActionResult ObtenerBilletera(string cvu)
        {
            try
            {
                var billetera = _billeteraService.ObtenerBilletera(cvu);
                return Ok(billetera);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        // Actualizar saldo
        [HttpPatch("{cvu}/saldo")]
        public IActionResult ActualizarSaldo(string cvu, [FromBody] ActualizarSaldoRequest request)
        {
            try
            {
                var billetera = _billeteraService.ActualizarSaldo(cvu, request);
                return Ok(billetera);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Cambiar estado (activar/suspender)
        [HttpPatch("{cvu}/estado")]
        public IActionResult CambiarEstado(string cvu, [FromQuery] string nuevoEstado)
        {
            try
            {
                _billeteraService.CambiarEstado(cvu, nuevoEstado);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
