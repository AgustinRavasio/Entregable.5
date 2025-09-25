using TuBilletera.Data;
using TuBilletera.Requests;
using TuBilletera.Responses;
using System.Text.Json;

namespace TuBilletera.Services
{
    public class TransaccionService
    {
        private readonly string _filePath = "Data/transacciones.json";
        private List<Transaccion> _transacciones;
        private readonly BilleteraService _billeteraService;

        public TransaccionService(BilleteraService billeteraService)
        {
            _billeteraService = billeteraService;

            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _transacciones = JsonSerializer.Deserialize<List<Transaccion>>(json) ?? new List<Transaccion>();
            }
            else
            {
                _transacciones = new List<Transaccion>();
            }
        }

        private void Guardar()
        {
            var json = JsonSerializer.Serialize(_transacciones, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public TransaccionResponse CrearTransaccion(CrearTransaccionRequest request)
        {
            var billeteraOrigen = _billeteraService.ObtenerBilletera(request.CvuOrigen);
            var billeteraDestino = _billeteraService.ObtenerBilletera(request.CvuDestino);

            if (billeteraOrigen.Estado == "suspendida" || billeteraDestino.Estado == "suspendida")
                throw new Exception("Una de las billeteras está suspendida");

            if ((request.TipoOperacion == "pago" || request.TipoOperacion == "transferencia") && billeteraOrigen.Saldo < request.Monto)
                throw new Exception("Saldo insuficiente");

            var transaccion = new Transaccion
            {
                Codigo = Guid.NewGuid().ToString(),
                CvuOrigen = request.CvuOrigen, //error
                CvuDestino = request.CvuDestino, //error
                Monto = request.Monto,
                TipoOperacion = request.TipoOperacion, //error
                Descripcion = request.Descripcion,
                FechaHora = DateTime.Now
            };

            // Actualizar saldos
            if (request.TipoOperacion != "recarga")
            {
                billeteraOrigen.Saldo -= request.Monto;
            }
            billeteraDestino.Saldo += request.Monto;

            // Guardar transacción
            _transacciones.Add(transaccion);
            Guardar();

            return new TransaccionResponse(transaccion); //error
        }

        public List<TransaccionResponse> ObtenerTransacciones(string cvu, string? ordenarPor = null, bool asc = true, string? tipo = null)
        {
            var query = _transacciones.Where(t => t.CvuOrigen == cvu || t.CvuDestino == cvu); //error

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(t => t.TipoOperacion == tipo);

            query = ordenarPor?.ToLower() switch
            {
                "fecha" => asc ? query.OrderBy(t => t.FechaHora) : query.OrderByDescending(t => t.FechaHora),
                "monto" => asc ? query.OrderBy(t => t.Monto) : query.OrderByDescending(t => t.Monto),
                _ => query
            };

            return query.Select(t => new TransaccionResponse(t)).ToList();
        }
    }
}
