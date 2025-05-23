namespace CineBR.Models
{
    public class Comentario
    {
        public int IdComentario { get; set; }
        public int IdFilme { get; set; }
        public string IdUsuario { get; set; }
        public string ComentarioTexto { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Ativo { get; set; } = true;
        public bool Editado { get; set; } = false;
    }

    public class ComentarioEditadoModel
    {
        public int IdComentario { get; set; }
        public string NovoTexto { get; set; }
    }

    public class ComentarioExcluirModel
    {
        public int IdComentario { get; set; }
    }
}