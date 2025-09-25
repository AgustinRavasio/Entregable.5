using TuBilletera.Data;

namespace TuBilletera.Responses
{
    public class BilleteraResponse
    {
        public string Cvu { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } = "activa";
        public List<TransaccionResponse> Transacciones { get; set; } = new List<TransaccionResponse>();

        public BilleteraResponse(Billetera billetera) //error en Billetera
        {
            Cvu = billetera.Cvu;
            Dni = billetera.Dni;
            Saldo = billetera.Saldo;
            FechaCreacion = billetera.FechaCreacion;
            Estado = billetera.Estado;
            Transacciones = billetera.Transacciones.Select(t => new TransaccionResponse(t)).ToList();
        }
    }
}
