namespace Banco_Ditigal_Green.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using Confluent.Kafka;

    namespace TransferenciaController.Controllers
    {
        [ApiController]
        [Route("[controller]")]
        public class TransferenciaController : ControllerBase
        {
            private readonly ILogger<TransferenciaController> _logger;
            private readonly IMongoCollection<Transferencia> _transferenciasCollection;
            private readonly IProducer<Null, string> _producer;

            public TransferenciaController(ILogger<TransferenciaController> logger, IMongoClient mongoClient, ProducerConfig producerConfig)
            {
                _logger = logger;
                _transferenciasCollection = mongoClient.GetDatabase("transferencias").GetCollection<Transferencia>("transferencias");
                _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
            }

            [HttpPost]
            public async Task<ActionResult> Post([FromBody] Transferencia transferencia)
            {
                try
                {
                    // Salva a transferência no MongoDB
                    await _transferenciasCollection.InsertOneAsync(transferencia);

                    // Envia a transferência para o Kafka
                    var message = new Message<Null, string> { Value = transferencia.ToString() };
                    await _producer.ProduceAsync("transferencias-topic", message);

                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar transferência");
                    return StatusCode(500);
                }
            }
        }

        public class Transferencia
        {
            public string Origem { get; set; }
            public string Destino { get; set; }
            public decimal Valor { get; set; }

            public override string ToString()
            {
                return $"Origem: {Origem}, Destino: {Destino}, Valor: {Valor}";
            }
        }
    }

}
