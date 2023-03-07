namespace Banco_Ditigal_Green.Models
{
    public class Transacao
    {
        public string Id { get; set; }
        public string UsuarioId { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
    }
}
