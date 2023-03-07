namespace Banco_Ditigal_Green.Views.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using Confluent.Kafka;
    using Newtonsoft.Json;
    using global::Banco_Ditigal_Green.Models;

    namespace AtivosDigitaisRepository.Repositories
    {
        public interface IAtivoDigitalRepository
        {
            Task<IEnumerable<AtivoDigital>> BuscarAtivos();
            Task<AtivoDigital> BuscarAtivoPorId(string id);
            Task SalvarAtivo(AtivoDigital ativo);
            Task AtualizarAtivo(AtivoDigital ativo);
            Task DeletarAtivo(string id);
        }
        public class AtivoDigitalRepository : IAtivoDigitalRepository
        {
            private readonly IMongoCollection<AtivoDigital> _ativos;
            private readonly IProducer<Null, string> _kafkaProducer;

            public AtivoDigitalRepository(IMongoDatabase database, IProducer<Null, string> kafkaProducer)
            {
                _ativos = database.GetCollection<AtivoDigital>("ativos");
                _kafkaProducer = kafkaProducer;
            }

            public async Task<IEnumerable<AtivoDigital>> BuscarAtivos()
            {
                var ativos = await _ativos.FindAsync(a => true);
                return await ativos.ToListAsync();
            }

            public async Task<AtivoDigital> BuscarAtivoPorId(string id)
            {
                var ativo = await _ativos.FindAsync(a => a.Id == id);
                return await ativo.FirstOrDefaultAsync();
            }

            public async Task SalvarAtivo(AtivoDigital ativo)
            {
                await _ativos.InsertOneAsync(ativo);
            }

            public async Task AtualizarAtivo(AtivoDigital ativo)
            {
                var filtro = Builders<AtivoDigital>.Filter.Eq(a => a.Id, ativo.Id);
                await _ativos.ReplaceOneAsync(filtro, ativo);
            }

            public async Task DeletarAtivo(string id)
            {
                var filtro = Builders<AtivoDigital>.Filter.Eq(a => a.Id, id);
                await _ativos.DeleteOneAsync(filtro);
            }
        }
    }

}
