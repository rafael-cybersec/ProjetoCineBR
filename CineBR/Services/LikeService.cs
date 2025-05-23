using System.Text.Json;
using CineBR.Models;

public static class LikeService
{
    private static string caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "likes.json");

    public static bool UsuarioJaCurtiu(int idFilme, string idUsuario)
    {
        if (!File.Exists(caminho)) return false;
        var lista = JsonSerializer.Deserialize<List<Like>>(File.ReadAllText(caminho));
        return lista?.Any(x => x.IdFilme == idFilme && x.IdUsuario == idUsuario && x.Ativo) ?? false;
    }


    public static void Curtir(Like novo)
    {
        var lista = File.Exists(caminho)
            ? JsonSerializer.Deserialize<List<Like>>(File.ReadAllText(caminho)) ?? new List<Like>()
            : new List<Like>();

        var likeExistente = lista.FirstOrDefault(x => x.IdFilme == novo.IdFilme && x.IdUsuario == novo.IdUsuario);

        if (likeExistente != null)
        {
            // Alterna o estado do like
            likeExistente.Ativo = !likeExistente.Ativo;
            likeExistente.DataAtualizacao = DateTime.Now;
        }
        else
        {
            // Novo like
            novo.IdLike = lista.Any() ? lista.Max(x => x.IdLike) + 1 : 1;
            novo.DataCadastro = DateTime.Now;
            lista.Add(novo);
        }

        File.WriteAllText(caminho, JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true }));
    }

    public static int ContarCurtidas(int idFilme)
    {
        if (!File.Exists(caminho))
            return 0;

        var json = File.ReadAllText(caminho);
        var listaLikes = System.Text.Json.JsonSerializer.Deserialize<List<Like>>(json) ?? new List<Like>();

        return listaLikes.Count(l => l.IdFilme == idFilme && l.Ativo);
    }


}
