namespace Banco_Ditigal_Green.Views.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Banco_Ditigal_Green.Models;
    using MongoDB.Driver;

    namespace TransferenciaApi.Repositories
    {
        public class TransferenciaRepository
        {
            private readonly IMongoCollection<Transferencia> _transferenciasCollection;

            public TransferenciaRepository(IMongoClient mongoClient)
            {
                _transferenciasCollection = mongoClient.GetDatabase("transferencias").GetCollection<Transferencia>("transferencias");
            }

            public async Task<IEnumerable<Transferencia>> ObterTransferencias()
            {
                return await _transferenciasCollection.Find(_ => true).ToListAsync();
            }
        }
    }

}
