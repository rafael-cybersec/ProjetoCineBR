using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CineBR.Models;

namespace CineBR.Controllers
{
    public class HomeController : Controller
    {
        private readonly string jsonPath;

        public HomeController(IWebHostEnvironment env)
        {
            jsonPath = Path.Combine(env.WebRootPath, "data", "filmes.json");
        }

        public IActionResult Index()
        {
            var filmes = CarregarFilmes();
            var viewModel = new HomeViewModel { Filmes = filmes };
            return View(viewModel);
        }

        public IActionResult Detalhes(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            var filmes = CarregarFilmes();
            var filmeSelecionado = filmes.FirstOrDefault(f => f.Id == id);

            if (filmeSelecionado == null)
            {
                return NotFound("Filme não encontrado.");
            }

            return View(filmeSelecionado);
        }

        private List<Filme> CarregarFilmes()
        {
            if (!System.IO.File.Exists(jsonPath))
            {
                return new List<Filme>();
            }

            var json = System.IO.File.ReadAllText(jsonPath);
            return JsonConvert.DeserializeObject<List<Filme>>(json) ?? new List<Filme>();
        }
    }
}
