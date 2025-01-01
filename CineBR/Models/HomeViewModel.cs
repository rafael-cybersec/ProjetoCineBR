using System.Collections.Generic;

namespace CineBR.Models
{
    public class HomeViewModel
    {
        public List<Filme> Filmes { get; set; }
    }

    public class Filme
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Resumo { get; set; }
        public string Diretor { get; set; }
        public List<string> Elenco { get; set; }
        public int Duracao { get; set; }
        public List<string> Genero { get; set; }
        public string Capa { get; set; }
    }
}
