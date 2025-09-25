namespace TuBilletera.Requests
{
    public class CrearTransaccionRequest
    {
        public string CvuOrigen { get; set; } = string.Empty;
        public string CvuDestino { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string TipoOperacion { get; set; } = string.Empty; 
        public string Descripcion { get; set; } = string.Empty;
    }
}
