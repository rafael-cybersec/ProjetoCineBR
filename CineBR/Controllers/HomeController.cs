using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CineBR.Models;
using CineBR.Services;
using System.Security.Claims;

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
            var filmeSelecionado = filmes.FirstOrDefault(f => f.Id.ToString() == id);

            if (filmeSelecionado == null)
            {
                return NotFound("Filme não encontrado.");
            }

            // Carrega os comentários
            var comentarios = ComentarioService.ObterPorFilme(filmeSelecionado.Id);

            // Carrega os usuários (se necessário)
            var usuarios = UsuarioService.CarregarUsuarios(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "usuarios.json"));

            ViewBag.Comentarios = comentarios;
            ViewBag.Usuarios = usuarios;
            ViewBag.UsuarioAutenticado = User.Identity.IsAuthenticated;

            if (User.Identity.IsAuthenticated)
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                ViewBag.Usuario = usuarios.FirstOrDefault(u => u.Email == email);
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

        [HttpPost]
        public IActionResult Comentar([FromForm] Comentario request)
        {
            if (!User.Identity.IsAuthenticated || User.IsInRole("Admin"))
                return RedirectToAction("CadastroLogin", "Usuario");

            if (string.IsNullOrWhiteSpace(request.ComentarioTexto))
            {
                TempData["ErroComentario"] = "O comentário não pode estar vazio.";
                return RedirectToAction("Detalhes", new { id = request.IdFilme });
            }

            try
            {
                var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var caminhoUsuarios = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "usuarios.json");
                var usuarios = UsuarioService.CarregarUsuarios(caminhoUsuarios);
                var usuario = usuarios.FirstOrDefault(u => u.Email == email);

                if (usuario == null)
                {
                    TempData["ErroComentario"] = "Usuário não encontrado.";
                    return RedirectToAction("Detalhes", new { id = request.IdFilme });
                }

                var novoComentario = new Comentario
                {
                    IdFilme = request.IdFilme,
                    IdUsuario = usuario.Id,
                    ComentarioTexto = request.ComentarioTexto.Trim(),
                    DataCadastro = DateTime.Now,
                    Ativo = true
                };

                ComentarioService.Salvar(novoComentario);

                TempData["SucessoComentario"] = "Comentário adicionado com sucesso!";
                return RedirectToAction("Detalhes", new { id = request.IdFilme });
            }
            catch (Exception ex)
            {
                TempData["ErroComentario"] = "Erro ao salvar o comentário: " + ex.Message;
                return RedirectToAction("Detalhes", new { id = request.IdFilme });
            }
        }

        private string caminhoArquivoComentarios = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "usuarios.json");

        private List<Comentario> LerComentarios()
        {
            if (!System.IO.File.Exists(caminhoArquivoComentarios))
                return new List<Comentario>();

            var json = System.IO.File.ReadAllText(caminhoArquivoComentarios);
            return JsonConvert.DeserializeObject<List<Comentario>>(json) ?? new List<Comentario>();
        }

        private void SalvarComentarios(List<Comentario> comentarios)
        {
            var json = JsonConvert.SerializeObject(comentarios, Formatting.Indented);
            System.IO.File.WriteAllText(caminhoArquivoComentarios, json);
        }


        [HttpPost]
        public IActionResult EditarComentario([FromBody] ComentarioEditadoModel model)
        {
            try
            {
                var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var caminhoUsuarios = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "usuarios.json");
                var usuarios = UsuarioService.CarregarUsuarios(caminhoUsuarios);
                var usuario = usuarios.FirstOrDefault(u => u.Email == email);

                if (usuario == null)
                    return Unauthorized();

                ComentarioService.Editar(model.IdComentario, usuario.Id, model.NovoTexto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPost]
        public IActionResult ExcluirComentario([FromBody] ComentarioExcluirModel model)
        {
            try
            {
                var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var caminhoUsuarios = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "usuarios.json");
                var usuarios = UsuarioService.CarregarUsuarios(caminhoUsuarios);
                var usuario = usuarios.FirstOrDefault(u => u.Email == email);

                if (usuario == null)
                    return Unauthorized();

                ComentarioService.Desativar(model.IdComentario, usuario.Id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }





        [HttpPost]
        public IActionResult Curtir(int idFilme, string idUsuario)
        {
            if (!User.Identity.IsAuthenticated || User.IsInRole("Admin"))
                return RedirectToAction("CadastroLogin", "Usuario");

            if (string.IsNullOrWhiteSpace(idUsuario))
                return RedirectToAction("Detalhes", new { id = idFilme });

            LikeService.Curtir(new Like
            {
                IdFilme = idFilme,
                IdUsuario = idUsuario
            });

            return RedirectToAction("Detalhes", new { id = idFilme });
             }
        }

}

