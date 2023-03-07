using Banco_Ditigal_Green.Controllers.PagamentoController.Controllers;

namespace Banco_Ditigal_Green.Views.Services.Dto
{
    public class PagamentoDto
    {
        private Pagamento pagamento;

        public PagamentoDto(Pagamento pagamento)
        {
            this.pagamento = pagamento;
        }

        public string Id { get; set; }
        public decimal Valor { get; set; }
        public string Moeda { get; set; }
        public string NumeroCartao { get; set; }
        public string NomeCartao { get; set; }
        public DateTime DataExpericao { get; set; }
        public int CVV { get; set; }
    }

}
