using TuBilletera.Data;

namespace TuBilletera.Responses
{
    public class TransaccionResponse
    {
        public string Codigo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string TipoOperacion { get; set; } = string.Empty;
        public string CvuOrigen { get; set; } = string.Empty;
        public string CvuDestino { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string Descripcion { get; set; } = string.Empty;

        public TransaccionResponse(Transaccion transaccion) //error en transaccion
        {
            Codigo = transaccion.Codigo;
            Monto = transaccion.Monto;
            TipoOperacion = transaccion.TipoOperacion;
            CvuOrigen = transaccion.CvuOrigen;
            CvuDestino = transaccion.CvuDestino;
            FechaHora = transaccion.FechaHora;
            Descripcion = transaccion.Descripcion;
        }

      
    }
}
