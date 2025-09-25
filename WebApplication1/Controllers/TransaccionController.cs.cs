using Microsoft.AspNetCore.Mvc;
using TuBilletera.Requests;
using TuBilletera.Responses;
using TuBilletera.Services;

namespace TuBilletera.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionController : ControllerBase
    {
        private readonly TransaccionService _transaccionService;

        public TransaccionController(TransaccionService transaccionService)
        {
            _transaccionService = transaccionService;
        }

        // Crear nueva transacción
        [HttpPost]
        public IActionResult CrearTransaccion([FromBody] CrearTransaccionRequest request)
        {
            try
            {
                var transaccion = _transaccionService.CrearTransaccion(request);
                return Created("", transaccion);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Obtener transacciones de una billetera
        [HttpGet("billetera/{cvu}")]
        public IActionResult ObtenerTransacciones(
            string cvu,
            [FromQuery] string? ordenarPor,
            [FromQuery] bool asc = true,
            [FromQuery] string? tipo = null)
        {
            var transacciones = _transaccionService.ObtenerTransacciones(cvu, ordenarPor, asc, tipo);
            return Ok(transacciones);
        }
    }
}
