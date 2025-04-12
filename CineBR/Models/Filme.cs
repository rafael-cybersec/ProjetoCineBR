namespace CineBR.Models
{
    public class Filme
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Resumo { get; set; }
        public List<string> Diretor { get; set; }
        public List<string> Elenco { get; set; }
        public int Duracao { get; set; }
        public List<string> Genero { get; set; }
        public string Capa { get; set; }
        public DateTime DataHoraCadastro { get; set; }
        public string trailerlink { get; set; }
        public int releasedate { get; set; }



        public bool Ativo { get; set; } = true;
        public bool ExibirNoSite { get; set; } = true;
    }
}
