using SQLite;
using MauiAppProva2.Models;

namespace MauiAppProva2.db
{
    public class Db
    {
        private SQLiteAsyncConnection _conexao;

        async Task Init()
        {
            if (_conexao != null)
                return;

            var caminhoBanco = Path.Combine(FileSystem.AppDataDirectory, "database.db3");

            _conexao = new SQLiteAsyncConnection(caminhoBanco);

            await _conexao.CreateTableAsync<Usuario>();
        }

        public async Task<int> SalvarUsuarioAsync(Usuario usuario)
        {
            await Init(); 
            return await _conexao.InsertAsync(usuario);
        }

        public async Task<Usuario> GetUsuarioAsync(string email, string senha)
        {
            await Init();

            return await _conexao.Table<Usuario>()
                .Where(usuario => usuario.Email == email && usuario.Senha == senha)
                .FirstOrDefaultAsync();
        }
    }
}