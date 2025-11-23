using SQLite;

namespace MauiAppProva2.Models
{
    public class Historico
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Cidade { get; set; }
        public DateTime DataConsulta { get; set; }
        public string Temperatura { get; set; } 
        public string Descricao { get; set; }
    }
}
