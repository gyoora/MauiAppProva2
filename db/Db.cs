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
            await _conexao.CreateTableAsync<Historico>();
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
                                .Where(u => u.Email == email && u.Senha == senha)
                                .FirstOrDefaultAsync();
        }

        public async Task<int> AdicionarHistorico(Historico h)
        {
            await Init();
            return await _conexao.InsertAsync(h);
        }

        public async Task<List<Historico>> GetHistorico(int idUsuario)
        {
            await Init();
            return await _conexao.Table<Historico>()
                                .Where(h => h.IdUsuario == idUsuario) 
                                .OrderByDescending(x => x.DataConsulta)
                                .ToListAsync();
        }

        public async Task<List<Historico>> GetHistoricoFiltrado(int idUsuario, DateTime inicio, DateTime fim, string cidade = null)
        {
            await Init();

            DateTime fimDoDia = fim.Date.AddDays(1).AddTicks(-1);

            var query = _conexao.Table<Historico>()
                                .Where(h => h.IdUsuario == idUsuario && 
                                            h.DataConsulta >= inicio.Date &&
                                            h.DataConsulta <= fimDoDia);

            if (!string.IsNullOrEmpty(cidade))
            {
                query = query.Where(h => h.Cidade.ToLower().Contains(cidade.ToLower()));
            }

            return await query.OrderByDescending(x => x.DataConsulta)
                              .ToListAsync();
        }
    }
}