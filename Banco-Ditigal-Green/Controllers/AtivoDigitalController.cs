namespace Banco_Ditigal_Green.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Driver;
    using Confluent.Kafka;
    using Newtonsoft.Json;
    using Banco_Ditigal_Green.Models;
    using Banco_Ditigal_Green.Views.Repository.AtivosDigitaisRepository.Repositories;

    namespace AtivoDigitalController.Controllers
    {
        [ApiController]
        [Route("[controller]")]
        public class AtivoDigitalController : ControllerBase
        {
            private readonly AtivoDigitalRepository _ativodigitalRepository;
            private readonly IProducer<Null, string> _kafkaProducer;

            public AtivoDigitalController(AtivoDigitalRepository repository, IProducer<Null, string> kafkaProducer)
            {
                _ativodigitalRepository = repository;
                _kafkaProducer = kafkaProducer;
            }

            [HttpGet]
            public async Task<IEnumerable<AtivoDigital>> Get()
            {
                return await _ativodigitalRepository.BuscarAtivos();
            }

            [HttpGet("{id}")]
            public async Task<AtivoDigital> GetById(string id)
            {
                return await _ativodigitalRepository.BuscarAtivoPorId(id);
            }

            [HttpPost]
            public async Task<IActionResult> Post([FromBody] AtivoDigital ativo)
            {
                if (ativo == null)
                {
                    return BadRequest();
                }

                await _ativodigitalRepository.SalvarAtivo(ativo);

                var message = new Message<Null, string>
                {
                    Value = JsonConvert.SerializeObject(ativo)
                };
                await _kafkaProducer.ProduceAsync("ativos", message);

                return Ok(ativo);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Put(string id, [FromBody] AtivoDigital ativo)
            {
                if (ativo == null || id != ativo.Id)
                {
                    return BadRequest();
                }

                var ativoExistente = await _ativodigitalRepository.BuscarAtivoPorId(id);
                if (ativoExistente == null)
                {
                    return NotFound();
                }

                await _ativodigitalRepository.AtualizarAtivo(ativo);

                var message = new Message<Null, string>
                {
                    Value = JsonConvert.SerializeObject(ativo)
                };
                await _kafkaProducer.ProduceAsync("ativos", message);

                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(string id)
            {
                var ativoExistente = await _ativodigitalRepository.BuscarAtivoPorId(id);
                if (ativoExistente == null)
                {
                    return NotFound();
                }

                await _ativodigitalRepository.DeletarAtivo(id);

                var message = new Message<Null, string>
                {
                    Value = JsonConvert.SerializeObject(ativoExistente)
                };
                await _kafkaProducer.ProduceAsync("ativos", message);

                return NoContent();
            }
        }
    }

}
