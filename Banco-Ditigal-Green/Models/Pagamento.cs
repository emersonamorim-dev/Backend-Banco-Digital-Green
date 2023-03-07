namespace Banco_Ditigal_Green.Models
{
    public class Pagamento
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
    }
}
