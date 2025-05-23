namespace CineBR.Models
{
    public class Filme
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Resumo { get; set; }
        public List<string>? Diretor { get; set; }
        public List<string>? Elenco { get; set; }
        public List<string>? Genero { get; set; }
        public int Duracao { get; set; }
        public string? Capa { get; set; }
        public string? ClassificacaoSelecionada { get; set; }
        public DateTime DataHoraCadastro { get; set; } = DateTime.Now;
        public string? TrailerLink { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool Ativo { get; set; } = true;
        public bool ExibirNoSite { get; set; } = true;
    }
}
