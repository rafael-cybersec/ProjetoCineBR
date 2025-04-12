using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CineBR.Models;

namespace CineBR.Controllers
{
    public class FilmesController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private string caminhoJson => Path.Combine(_env.WebRootPath, "data", "filmes.json");

        public FilmesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        private bool UsuarioEhAdmin()
        {
            var tipoUsuario = HttpContext.Session.GetString("TipoUsuario");
            return tipoUsuario == "Admin";
        }

        [HttpGet]
        public IActionResult Cadastro()
        {
            if (!UsuarioEhAdmin())
                return RedirectToAction("AcessoNegado", "Home");

            return View("~/Views/AdminPages/CadastroFilmes.cshtml");
        }

        [HttpPost]
        public IActionResult Cadastrar(
            Filme filme,
            string DiretorLista,
            string ElencoLista,
            string GeneroLista,
            IFormFile capaUpload,
            bool Ativo = true,
            bool ExibirNoSite = true)
        {
            if (!UsuarioEhAdmin())
                return Unauthorized();

            string imagemPath = Path.Combine(_env.WebRootPath, "image");
            var filmes = CarregarFilmes();

            filme.Id = Guid.NewGuid().ToString();
            filme.DataHoraCadastro = DateTime.Now;
            filme.Ativo = Ativo;
            filme.ExibirNoSite = ExibirNoSite;

            // Divide as listas
            filme.Diretor = DiretorLista?.Split(',').Select(x => x.Trim()).ToList() ?? new();
            filme.Elenco = ElencoLista?.Split(',').Select(x => x.Trim()).ToList() ?? new();
            filme.Genero = GeneroLista?.Split(',').Select(x => x.Trim()).ToList() ?? new();

            // Salvar imagem
            if (capaUpload != null && capaUpload.Length > 0)
            {
                string fileName = $"{filme.Id}_{Path.GetFileName(capaUpload.FileName)}";
                string filePath = Path.Combine(imagemPath, fileName);

                if (!Directory.Exists(imagemPath))
                    Directory.CreateDirectory(imagemPath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    capaUpload.CopyTo(stream);
                }

                filme.Capa = "/image/" + fileName;
            }

            filmes.Add(filme);
            SalvarFilmes(filmes);

            ViewBag.Mensagem = "Filme cadastrado com sucesso!";
            return View("~/Views/AdminPages/CadastroFilmes.cshtml");
        }

        [HttpGet]
        public IActionResult Detalhes(string id)
        {
            var filmes = CarregarFilmes();
            var filme = filmes.FirstOrDefault(f => f.Id == id);

            if (filme == null)
                return NotFound("Filme não encontrado.");

            return View(filme);
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            if (!UsuarioEhAdmin())
                return RedirectToAction("AcessoNegado", "Home");

            string jsonPath = Path.Combine(_env.WebRootPath, "data", "filmes.json");
            var filmes = FilmeService.CarregarFilmes(jsonPath);
            return View("~/Views/AdminPages/DashboardFilmes.cshtml", filmes);
        }



        // Métodos utilitários
        private List<Filme> CarregarFilmes()
        {
            if (!System.IO.File.Exists(caminhoJson))
                return new List<Filme>();

            var json = System.IO.File.ReadAllText(caminhoJson);
            return JsonConvert.DeserializeObject<List<Filme>>(json) ?? new List<Filme>();
        }

        private void SalvarFilmes(List<Filme> filmes)
        {
            var json = JsonConvert.SerializeObject(filmes, Formatting.Indented);
            System.IO.File.WriteAllText(caminhoJson, json);
        }
    }
}
