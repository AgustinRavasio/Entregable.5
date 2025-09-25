namespace TuBilletera.Requests
{
    public class CrearBilleteraRequest
    {
        public string Dni { get; set; } = string.Empty;
        public decimal SaldoInicial { get; set; } = 0;
    }
}
