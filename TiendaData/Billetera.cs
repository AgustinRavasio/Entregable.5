using System;
using System.Collections.Generic;

namespace TuBilletera.Data
{
    public class Billetera
    {
        public string Cvu { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } = "activa";
        public List<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    }
}
