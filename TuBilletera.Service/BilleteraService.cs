using System.Linq;
using System.Text.Json;
using TuBilletera.Data;
using TuBilletera.Requests;
using TuBilletera.Responses;

namespace TuBilletera.Services
{
    public class BilleteraService
    {
        private readonly string _filePath = "Data/billeteras.json";
        private List<Billetera> _billeteras;

        public BilleteraService()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _billeteras = JsonSerializer.Deserialize<List<Billetera>>(json) ?? new List<Billetera>();
            }
            else
            {
                _billeteras = new List<Billetera>();
            }
        }

        private void Guardar()
        {
            var json = JsonSerializer.Serialize(_billeteras, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public BilleteraResponse CrearBilletera(CrearBilleteraRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Dni) || request.Dni.Length != 8)
                throw new Exception("DNI inválido");

            if (_billeteras.Any(b => b.Dni == request.Dni)) //error
                throw new Exception("El DNI ya está registrado");

            var billetera = new Billetera
            {
                Cvu = Guid.NewGuid().ToString(), //error
                Dni = request.Dni, //error
                Saldo = request.SaldoInicial,
                FechaCreacion = DateTime.Now,
                Estado = "activa", //error
                Transacciones = new List<Transaccion>()
            };

            _billeteras.Add(billetera);
            Guardar();

            return new BilleteraResponse(billetera); //error
        }

        public List<BilleteraResponse> ObtenerBilleteras(string? ordenarPor = null, bool asc = true, string? estado = null)
        {
            var query = _billeteras.AsEnumerable();

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(b => b.Estado == estado); //error

            query = ordenarPor?.ToLower() switch
            {
                "dni" => asc ? query.OrderBy(b => b.Dni) : query.OrderByDescending(b => b.Dni), //error
                "saldo" => asc ? query.OrderBy(b => b.Saldo) : query.OrderByDescending(b => b.Saldo),
                "fecha" => asc ? query.OrderBy(b => b.FechaCreacion) : query.OrderByDescending(b => b.FechaCreacion),
                _ => query
            };

            return query.Select(b => new BilleteraResponse(b)).ToList(); //error
        }

        public BilleteraResponse ObtenerBilletera(string cvu)
        {
            var billetera = _billeteras.FirstOrDefault(b => b.Cvu == cvu); //error
            if (billetera == null) throw new Exception("Billetera no encontrada");
            return new BilleteraResponse(billetera);
        }

        public BilleteraResponse ActualizarSaldo(string cvu, ActualizarSaldoRequest request)
        {
            var billetera = _billeteras.FirstOrDefault(b => b.Cvu == cvu); //error
            if (billetera == null) throw new Exception("Billetera no encontrada");

            if (request.NuevoSaldo < 0)
                throw new Exception("El saldo no puede ser negativo");

            billetera.Saldo = request.NuevoSaldo;
            Guardar();

            return new BilleteraResponse(billetera);
        }

        public void CambiarEstado(string cvu, string nuevoEstado)
        {
            var billetera = _billeteras.FirstOrDefault(b => b.Cvu == cvu); //error
            if (billetera == null) throw new Exception("Billetera no encontrada");

            billetera.Estado = nuevoEstado;
            Guardar();
        }
    }
}
