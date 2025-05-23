using CineBR.Models;
using System.Text.Json;

public static class ComentarioService
{
    private static string caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "comentarios.json");

    public static List<Comentario> ObterPorFilme(int idFilme)
    {
        if (!File.Exists(caminho)) return new List<Comentario>();
        var json = File.ReadAllText(caminho);
        var lista = JsonSerializer.Deserialize<List<Comentario>>(json);
        return lista?.Where(c => c.IdFilme == idFilme && c.Ativo).OrderByDescending(c => c.DataCadastro).ToList() ?? new();
    }

    public static void Salvar(Comentario novo)
    {
        var lista = File.Exists(caminho)
            ? JsonSerializer.Deserialize<List<Comentario>>(File.ReadAllText(caminho)) ?? new List<Comentario>()
            : new List<Comentario>();

        // Limite de 3 comentários por usuário por filme
        var totalComentarios = lista.Count(c => c.IdFilme == novo.IdFilme && c.IdUsuario == novo.IdUsuario && c.Ativo);
        if (totalComentarios >= 3)
            throw new Exception("Limite de 3 comentários por filme atingido");

        novo.IdComentario = lista.Any() ? lista.Max(x => x.IdComentario) + 1 : 1;
        lista.Add(novo);
        SalvarLista(lista);
    }

    public static void Editar(int idComentario, string idUsuario, string novoTexto)
    {
        var lista = CarregarLista();
        var comentario = lista.FirstOrDefault(c => c.IdComentario == idComentario && c.IdUsuario == idUsuario && c.Ativo);

        if (comentario == null)
            throw new Exception("Comentário não encontrado ou não pertence ao usuário");

        comentario.ComentarioTexto = novoTexto;
        comentario.DataAtualizacao = DateTime.Now;
        comentario.Editado = true;

        SalvarLista(lista);
    }

    public static void Desativar(int idComentario, string idUsuario)
    {
        var lista = CarregarLista();
        var comentario = lista.FirstOrDefault(c => c.IdComentario == idComentario && c.IdUsuario == idUsuario && c.Ativo);

        if (comentario != null)
        {
            comentario.Ativo = false;
            comentario.DataAtualizacao = DateTime.Now;

            SalvarLista(lista);
        }
    }


    public static Comentario ObterPorId(int idComentario)
    {
        var lista = CarregarLista();
        return lista.FirstOrDefault(c => c.IdComentario == idComentario && c.Ativo);
    }

    private static List<Comentario> CarregarLista()
    {
        if (!File.Exists(caminho)) return new List<Comentario>();
        var json = File.ReadAllText(caminho);
        return JsonSerializer.Deserialize<List<Comentario>>(json) ?? new List<Comentario>();
    }

    private static void SalvarLista(List<Comentario> lista)
    {
        File.WriteAllText(caminho, JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true }));
    }
}