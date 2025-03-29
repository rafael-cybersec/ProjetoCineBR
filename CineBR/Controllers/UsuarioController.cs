using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using CineBR.Services;
using CineBR.Models;

namespace CineBR.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly string _jsonFile;

        public UsuarioController(IWebHostEnvironment env)
        {
            _jsonFile = Path.Combine(env.WebRootPath, "data", "usuarios.json");
        }

        public IActionResult CadastroLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Usuario usuario)
        {
            var usuarios = UsuarioService.CarregarUsuarios(_jsonFile);

            if (usuarios.Any(u => u.Email == usuario.Email))
            {
                ViewBag.MensagemCadastroErro = "Email já cadastrado.";
                return View("CadastroLogin");
            }

            if (string.IsNullOrEmpty(usuario.Nome) || string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.Senha))
            {
                ViewBag.MensagemCadastroErro = "Todos os campos são obrigatórios.";
                return View("CadastroLogin");
            }

            usuario.Id = Guid.NewGuid().ToString();  // Garantindo um ID único para o usuário
            usuario.Tipo = "Usuario";
            usuarios.Add(usuario);
            UsuarioService.SalvarUsuarios(usuarios, _jsonFile);

            return RedirectToAction("CadastroLogin");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            var usuarios = UsuarioService.CarregarUsuarios(_jsonFile);
            var usuario = usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);

            if (usuario == null)
            {
                ViewBag.MensagemLoginErro = "Email ou senha incorretos.";
                return View("CadastroLogin");
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.Nome),
        new Claim(ClaimTypes.Email, usuario.Email),
        new Claim(ClaimTypes.Role, usuario.Tipo)
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Redirect("/");
        }




        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            if (!User.IsInRole("Admin")) return Unauthorized();
            var usuarios = UsuarioService.CarregarUsuarios(_jsonFile);
            return View(usuarios);
        }

        [HttpPost]
        public IActionResult CriarUsuario(Usuario usuario)
        {
            if (!User.IsInRole("Admin")) return Unauthorized();
            var usuarios = UsuarioService.CarregarUsuarios(_jsonFile);
            usuarios.Add(usuario);
            UsuarioService.SalvarUsuarios(usuarios, _jsonFile);
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult ExcluirUsuario(string email)
        {
            if (!User.IsInRole("Admin")) return Unauthorized();
            var usuarios = UsuarioService.CarregarUsuarios(_jsonFile);
            usuarios.RemoveAll(u => u.Email == email);
            UsuarioService.SalvarUsuarios(usuarios, _jsonFile);
            return RedirectToAction("Dashboard");
        }
    }
}
