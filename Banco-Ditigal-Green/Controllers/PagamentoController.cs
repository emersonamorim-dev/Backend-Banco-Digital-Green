namespace Banco_Ditigal_Green.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using System.Runtime.Serialization;
    using Confluent.Kafka;
    using MongoDB.Driver;
    using MongoDB.Bson;
    using Banco_Ditigal_Green.Views.Services.Dto;

    namespace PagamentoController.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class PagamentoController : ControllerBase
        {
            private readonly IPagamentoRepository _pagamentoRepository;
            private readonly IProducer<string, string> _producer;

            public PagamentoController(IPagamentoRepository pagamentoRepository, IProducer<string, string> producer)
            {
                _pagamentoRepository = pagamentoRepository;
                _producer = producer;
            }

            // POST api/payment
            [HttpPost]
            public async Task<ActionResult> Post([FromBody] PagamentoDto pagamentoDto)
            {
                try
                {
                    var pagamento = new Pagamento(pagamentoDto);

                    await _pagamentoRepository.AddAsync(pagamento);

                    var message = new Message<string, string>
                    {
                        Key = pagamento.Id.ToString(),
                        Value = pagamento.ToJson(),

                    };
                    await _producer.ProduceAsync("pagamento-topic", message);

                    return Ok();
                }
                catch (PagamentoException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            // GET api/pagamento/{id}
            [HttpGet("{id}")]
            public async Task<ActionResult<PagamentoDto>> Get(string id)
            {
                try
                {
                    var pagamento = await _pagamentoRepository.GetAsync(id);

                    if (pagamento == null)
                    {
                        return NotFound();
                    }

                    return Ok(new PagamentoDto(pagamento));
                }
                catch (PagamentoException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        public interface IPagamentoRepository
        {
            Task AddAsync(Pagamento pagamento);
            Task<Pagamento> GetAsync(string id);
        }

        public class PagamentoRepository : IPagamentoRepository
        {
            private readonly IMongoCollection<Pagamento> _pagamentoCollection;

            public PagamentoRepository(IMongoClient mongoClient)
            {
                _pagamentoCollection = mongoClient.GetDatabase("pagamentos").GetCollection<Pagamento>("pagamentos");
            }

            public async Task AddAsync(Pagamento pagamento)
            {
                await _pagamentoCollection.InsertOneAsync(pagamento);
            }

            public async Task<Pagamento> GetAsync(string id)
            {
                var pagamento = await _pagamentoCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

                if (pagamento == null)
                {
                    throw new PagamentoException($"Pagamento com id {id} não encontrado.");
                }

                return pagamento;
            }
        }

        [Serializable]
        public class PagamentoException : Exception
        {
            public PagamentoException()
            {
            }

            public PagamentoException(string message) : base(message)
            {
            }

            public PagamentoException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected PagamentoException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }



        public class Pagamento
        {
            private PagamentoDto pagamentoDto;

            public Pagamento(PagamentoDto pagamentoDto)
            {
                this.pagamentoDto = pagamentoDto;
            }

            public string Id { get; set; }
            public decimal Valor { get; set; }
            public string Moeda { get; set; }
            public string NumeroCartao { get; set; }
            public string NomeCartao { get; set; }
            public DateTime DataExpericao { get; set; }
            public int CVV { get; set; }

            public BsonDocument ToBsonDocument()
            {
                return new BsonDocument
            {
                { "id", Id },
                { "amount", Valor },
                { "currency", Moeda  },
                { "cardNumber", NumeroCartao },
                { "cardHolderName", NomeCartao },
                { "expiryDate", DataExpericao },
                { "cvv", CVV }
            };
            }
        }
    }

}

