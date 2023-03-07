namespace Banco_Ditigal_Green.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using Confluent.Kafka;
    using Newtonsoft.Json;
    using Banco_Ditigal_Green.Models;

    namespace AcoesController.Controllers
    {
        [ApiController]
        [Route("[controller]")]
        public class AcoesController : ControllerBase
        {
            private readonly ILogger<AcoesController> _logger;
            private readonly IMongoCollection<Acoes> _acoesCollection;
            private readonly IProducer<Null, string> _kafkaProducer;

            public AcoesController(ILogger<AcoesController> logger, IMongoClient mongoClient, IProducer<Null, string> kafkaProducer)
            {
                _logger = logger;
                _acoesCollection = (IMongoCollection<Acoes>?)mongoClient.GetDatabase("acoes").GetCollection<Acoes>("acoes");
                _kafkaProducer = kafkaProducer;
            }

            [HttpPost]
            public async Task<ActionResult> Post([FromBody] Acoes acoes)
            {
                try
                {
                    // Salva a ação no MongoDB
                    await _acoesCollection.InsertOneAsync(acoes);

                    // Envia a mensagem para o tópico do Kafka
                    var message = new Message<Null, string>
                    {
                        Value = JsonConvert.SerializeObject(acoes)
                    };
                    await _kafkaProducer.ProduceAsync("acoes", message);

                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar ação");
                    return StatusCode(500);
                }
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Acoes>>> Get()
            {
                try
                {
                    var acoes = await _acoesCollection.Find(_ => true).ToListAsync();
                    return Ok(acoes);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao obter ações");
                    return StatusCode(500);
                }
            }
        }
    }

}
