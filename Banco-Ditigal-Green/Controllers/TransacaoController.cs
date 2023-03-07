namespace Banco_Ditigal_Green.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Threading.Tasks;
    using Banco_Ditigal_Green.Models;
    using Confluent.Kafka;
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Newtonsoft.Json;


    namespace TransacaoController.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class TransacaoController : ControllerBase
        {
            private readonly ITransacaoRepository _transacaoRepository;
            private readonly ITransacaoProducer _transacaoProducer;

            public TransacaoController(ITransacaoRepository transacaoRepository, ITransacaoProducer transacaoProducer)
            {
                _transacaoRepository = transacaoRepository ?? throw new ArgumentNullException(nameof(transacaoRepository));
                _transacaoProducer = transacaoProducer ?? throw new ArgumentNullException(nameof(transacaoProducer));
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Transacao>>> GetAll()
            {
                var transacoes = await _transacaoRepository.GetAllAsync();
                return Ok(transacoes);
            }

            [HttpGet("{id:length(18)}", Name = "GetById")]
            public async Task<ActionResult<Transacao>> GetById(string id)
            {
                var transacao = await _transacaoRepository.GetByIdAsync(id);
                if (transacao == null)
                {
                    return NotFound();
                }
                return Ok(transacao);
            }

            [HttpPost]
            public async Task<ActionResult> Create([FromBody] Transacao transacao)
            {
                if (transacao == null)
                {
                    return BadRequest(nameof(transacao));
                }

                await _transacaoRepository.AddAsync(transacao);
                await _transacaoProducer.ProduceAsync(transacao);

                return CreatedAtRoute("GetById", new { id = transacao.Id }, transacao);
            }
        }

        public interface ITransacaoRepository
        {
            Task<IEnumerable<Transacao>> GetAllAsync();
            Task<Transacao> GetByIdAsync(string id);
            Task AddAsync(Transacao transacao);
        }

        public class TransacaoRepository : ITransacaoRepository
        {
            private readonly IMongoCollection<Transacao> _transacaoCollection;

            public TransacaoRepository(IMongoDatabase mongoDatabase)
            {
                _transacaoCollection = mongoDatabase.GetCollection<Transacao>("transacoes");
            }

            public async Task<IEnumerable<Transacao>> GetAllAsync()
            {
                var transacoes = await _transacaoCollection.FindAsync(new BsonDocument());
                return await transacoes.ToListAsync();
            }

            public async Task<Transacao> GetByIdAsync(string id)
            {
                var transacao = await _transacaoCollection.FindAsync(t => t.Id == id);
                return await transacao.FirstOrDefaultAsync();
            }

            public async Task AddAsync(Transacao transacao)
            {
                if (transacao == null)
                {
                    throw new ArgumentNullException(nameof(transacao));
                }

                await _transacaoCollection.InsertOneAsync(transacao);
            }
        }

        public interface ITransacaoProducer
        {
            Task ProduceAsync(Transacao transacao);
        }

        public class TransacaoProducer : ITransacaoProducer
        {
            private readonly IProducer<Null, string> _producer;
            private readonly string _topicName;

            public TransacaoProducer(IProducer<Null, string> producer, string topicName)
            {
                _producer = producer ?? throw new ArgumentNullException(nameof(producer));
                _topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            }

            public async Task ProduceAsync(Transacao transacao)
            {
                if (transacao == null)
                {
                    throw new ArgumentNullException(nameof(transacao));
                }

                var message = new Message<Null, string>
                {
                    Value = transacao.ToJson() // <--- Missing code
                };

                await _producer.ProduceAsync(_topicName, message);
            }

        }

    }
 }




