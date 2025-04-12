using CineBR.Models;
using System.Text.Json;

public static class FilmeService
{
    public static List<Filme> CarregarFilmes(string path)
    {
        if (!File.Exists(path)) return new List<Filme>();
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<Filme>>(json) ?? new List<Filme>();
    }

    public static void SalvarFilmes(List<Filme> filmes, string path)
    {
        var json = JsonSerializer.Serialize(filmes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
}
