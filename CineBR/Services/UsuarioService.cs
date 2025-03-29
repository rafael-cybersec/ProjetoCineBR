using CineBR.Models;
using System.Text.Json;

namespace CineBR.Services
{
    public static class UsuarioService
    {
        public static List<Usuario> CarregarUsuarios(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return new List<Usuario>();

            var json = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }

        public static void SalvarUsuarios(List<Usuario> usuarios, string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(filePath, json);
        }
    }
}
