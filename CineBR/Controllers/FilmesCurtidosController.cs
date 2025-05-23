using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CineBR.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CineBR.Controllers
{
    [Authorize]
    public class FilmesCurtidosController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public FilmesCurtidosController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            // Obter o ID do usuário logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Carregar os likes do usuário
            var likes = CarregarLikes()
                .Where(l => l.IdUsuario == userId && l.Ativo)
                .ToList();

            // Carregar todos os filmes
            var filmes = CarregarFilmes();

            // Filtrar os filmes curtidos pelo usuário
            var filmesCurtidos = filmes
                .Where(f => likes.Any(l => l.IdFilme == f.Id))
                .ToList();

            return View(filmesCurtidos);
        }

        private List<Like> CarregarLikes()
        {
            var likesPath = Path.Combine(_env.WebRootPath, "wwwroot", "data", "likes.json");

            if (!System.IO.File.Exists(likesPath))
            {
                return new List<Like>();
            }

            var json = System.IO.File.ReadAllText(likesPath);
            return JsonConvert.DeserializeObject<List<Like>>(json) ?? new List<Like>();
        }

        private List<Filme> CarregarFilmes()
        {
            var filmesPath = Path.Combine(_env.WebRootPath, "wwwroot", "data", "filmes.json");

            if (!System.IO.File.Exists(filmesPath))
            {
                return new List<Filme>();
            }

            var json = System.IO.File.ReadAllText(filmesPath);
            return JsonConvert.DeserializeObject<List<Filme>>(json) ?? new List<Filme>();
        }
    }
}