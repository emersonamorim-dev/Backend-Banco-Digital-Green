namespace Banco_Ditigal_Green.Views.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using MongoDB.Bson;
    using Confluent.Kafka;

    namespace PagamentoRepository
    {
        public interface IPagamentoRepository
        {
            Task InserirPagamento(Pagamento pagamento);
            Task<Pagamento> ObterPagamentoPorId(string id);
        }

        public class PagamentoRepository : IPagamentoRepository
        {
            private readonly IMongoCollection<Pagamento> _pagamentoCollection;
            private readonly IProducer<string, string> _producer;

            public PagamentoRepository(IMongoClient mongoClient, IProducer<string, string> producer)
            {
                _pagamentoCollection = mongoClient.GetDatabase("pagamentos").GetCollection<Pagamento>("pagamentos");
                _producer = producer;
            }

            public async Task InserirPagamento(Pagamento pagamento)
            {
                try
                {
                    await _pagamentoCollection.InsertOneAsync(pagamento);
                    var message = new Message<string, string>
                    {
                        Key = pagamento.Id,
                        Value = pagamento.ToJson(),

                    };
                    await _producer.ProduceAsync("pagamento-topic", message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao inserir pagamento", ex);
                }
            }

            public async Task<Pagamento> ObterPagamentoPorId(string id)
            {
                try
                {
                    var filter = Builders<Pagamento>.Filter.Eq(p => p.Id, id);
                    var pagamento = await _pagamentoCollection.Find(filter).FirstOrDefaultAsync();
                    return pagamento;
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao obter pagamento por ID", ex);
                }
            }
        }

        public class Pagamento
        {
            public string Id { get; set; }
            public decimal Valor { get; set; }
            public string Moeda { get; set; }
            public string NumeroCartao { get; set; }
            public string NomeCartao { get; set; }
            public DateTime DataExpericao { get; set; }
            public int CVV { get; set; }

            public Pagamento()
            {
                Id = Guid.NewGuid().ToString();
            }
        }
    }

}
