namespace CineBR.Models
{
    public class Like
    {
        public int IdLike { get; set; }
        public int IdFilme { get; set; }
        public string IdUsuario { get; set; }  // Agora é string
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime? DataAtualizacao { get; set; }
    }

}
