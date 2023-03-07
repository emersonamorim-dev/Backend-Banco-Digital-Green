namespace Banco_Ditigal_Green.Views.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using Confluent.Kafka;
    using Newtonsoft.Json;
    using static Banco_Ditigal_Green.Repositories.AcoesRepository;

    namespace Banco_Ditigal_Green.Repositories
    {
        public class AcoesRepository : IAcoesRepository
        {
            private readonly IMongoCollection<Acoes> _acoesCollection;
            private readonly IProducer<Null, string> _kafkaProducer;

            public AcoesRepository(IMongoClient mongoClient, IProducer<Null, string> kafkaProducer)
            {
                _acoesCollection = mongoClient.GetDatabase("acoes").GetCollection<Acoes>("acoes");
                _kafkaProducer = kafkaProducer;
            }

            public async Task Add(Acoes acoes)
            {
                await _acoesCollection.InsertOneAsync(acoes);
                await EnviarMensagemKafka(acoes);
            }

            public async Task Update(Acoes acoes)
            {
                var filter = Builders<Acoes>.Filter.Eq(a => a.Id, acoes.Id);
                await _acoesCollection.ReplaceOneAsync(filter, acoes);
                await EnviarMensagemKafka(acoes);
            }

            public async Task Delete(int id)
            {
                var filter = Builders<Acoes>.Filter.Eq(a => a.Id, id);
                await _acoesCollection.DeleteOneAsync(filter);
            }

            public async Task<IEnumerable<Acoes>> GetAll()
            {
                return await _acoesCollection.Find(_ => true).ToListAsync();
            }

            public async Task<Acoes> GetById(int id)
            {
                var filter = Builders<Acoes>.Filter.Eq(a => a.Id, id);
                return await _acoesCollection.Find(filter).FirstOrDefaultAsync();
            }

            private async Task EnviarMensagemKafka(Acoes acoes)
            {
                try
                {
                    var message = new Message<Null, string>
                    {
                        Value = JsonConvert.SerializeObject(acoes)
                    };
                    await _kafkaProducer.ProduceAsync("acoes", message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao enviar mensagem para o Kafka", ex);
                }
            }

            public class Acoes
            {
                public int Id { get; set; }
                public string Nome { get; set; }
                public decimal Valor { get; set; }
            }

            public interface IAcoesRepository
            {
                Task<IEnumerable<Acoes>> GetAll();
                Task<Acoes> GetById(int id);
                Task Add(Acoes acoes);
                Task Update(Acoes acoes);
                Task Delete(int id);
            }
        }
    }

}

