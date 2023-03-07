namespace Banco_Ditigal_Green.Models
{
    public class Usuario
    {
    public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public decimal Balanco { get; set; }

        internal void Remove(Usuario user)
        {
            throw new NotImplementedException();
        }
    }
}
