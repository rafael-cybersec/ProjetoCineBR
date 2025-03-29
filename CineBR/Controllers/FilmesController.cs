using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CineBR.Controllers
{
    public class FilmesController : Controller
    {
        public IActionResult Detalhes(int id)
        {
            // Caminho fixo para o arquivo JSON (sem Path.Combine)
            string filePath = "wwwroot/data/filmes.json";

            // Validar se o arquivo existe
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Arquivo JSON não encontrado.");
            }

            // Ler e deserializar o JSON
            var filmesJson = System.IO.File.ReadAllText(filePath);
            var filmes = JsonConvert.DeserializeObject<List<CineBR.Models.Filme>>(filmesJson);

            // Encontrar o filme pelo ID
            var filme = filmes.FirstOrDefault(f => f.Id == id);

            if (filme == null)
            {
                return NotFound("Filme não encontrado.");
            }

            // Passar o filme para a View
            return View(filme);
        }
    }
}
