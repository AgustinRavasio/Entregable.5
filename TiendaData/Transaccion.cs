using System;

namespace TuBilletera.Data
{
    public class Transaccion
    {
        public string Codigo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string TipoOperacion { get; set; } = string.Empty; 
        public string CvuOrigen { get; set; } = string.Empty;
        public string CvuDestino { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}
