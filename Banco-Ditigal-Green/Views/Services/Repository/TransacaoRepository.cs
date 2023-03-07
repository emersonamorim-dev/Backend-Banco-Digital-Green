namespace Banco_Ditigal_Green.Views.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using Confluent.Kafka;
    using MongoDB.Driver;

    namespace TransacaoRepository
    {
        public class TransacaoRepository
        {
            private readonly MongoClient _mongoClient;
            private readonly IMongoCollection<Transacao> _transacoesCollection;
            private readonly IProducer<string, Transacao> _producer;

            public TransacaoRepository(string connectionString, string databaseName, string collectionName, string kafkaBrokerList)
            {
                _mongoClient = new MongoClient(connectionString);
                var database = _mongoClient.GetDatabase(databaseName);
                _transacoesCollection = database.GetCollection<Transacao>(collectionName);

                var config = new ProducerConfig { BootstrapServers = kafkaBrokerList };
                _producer = new ProducerBuilder<string, Transacao>(config)
                    .SetValueSerializer(new TransacaoSerializer())
                    .Build();
            }

            public void Add(Transacao transacao)
            {
                _transacoesCollection.InsertOne(transacao);
                var message = new Message<string, Transacao>
                {
                    Key = transacao.Id.ToString(),
                    Value = transacao
                };
                _producer.ProduceAsync("transacoes", message);
            }

            public List<Transacao> GetAll()
            {
                return _transacoesCollection.Find(_ => true).ToList();
            }

            public Transacao GetById(Guid id)
            {
                var filter = Builders<Transacao>.Filter.Eq(t => t.Id, id);
                return _transacoesCollection.Find(filter).FirstOrDefault();
            }
        }

        public class Transacao
        {
            public Guid Id { get; set; }
            public string Descricao { get; set; }
            public decimal Valor { get; set; }
        }

        public class TransacaoSerializer : Confluent.Kafka.ISerializer<Transacao>
        {
            public byte[] Serialize(Transacao data, SerializationContext context)
            {
                return JsonSerializer.SerializeToUtf8Bytes(data);
            }
        }
    }

}
