using Banco_Ditigal_Green.Models;
using MongoDB.Driver;


namespace Banco_Ditigal_Green.Views.Services.Repository
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllUsersAsync();
        Task<Usuario> GetUserByIdAsync(string id);
        Task CreateUserAsync(Usuario usuario);
        Task UpdateUserAsync(string id, Usuario usuario);
        Task DeleteUserAsync(Usuario usuario, string id);
    }

    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IMongoCollection<Usuario> _usuarios;

        public UsuarioRepository(IMongoClient client)
        {
            var database = client.GetDatabase("Bank");
            _usuarios = database.GetCollection<Usuario>("Usuarios");
        }

        public async Task<IEnumerable<Usuario>> GetAllUsersAsync()
        {
            return await _usuarios.Find(u => true).ToListAsync();
        }

        public async Task<Usuario> GetUserByIdAsync(string id)
        {
            return await _usuarios.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(Usuario usuario)
        {
            await _usuarios.InsertOneAsync(usuario);
        }

        public async Task UpdateUserAsync(string id, Usuario usuario)
        {
            await _usuarios.ReplaceOneAsync(u => u.Id == id, usuario);
        }

        public async Task DeleteUserAsync(string id)
        {
            await _usuarios.DeleteOneAsync(u => u.Id == id);
        }

        public Task DeleteUserAsync(Usuario usuario, string id)
        {
            throw new NotImplementedException();
        }
    }
}


