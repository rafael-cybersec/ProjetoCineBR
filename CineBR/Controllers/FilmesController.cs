using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using CineBR.Models;
using System.Globalization;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace CineBR.Controllers
{
    public class FilmesController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly EmailService _emailService;

        private string CaminhoJson => Path.Combine(_env.WebRootPath, "data", "filmes.json");

        public FilmesController(IWebHostEnvironment env, EmailService emailService)
        {
            _env = env;
            _emailService = emailService;
        }

        private IActionResult VerificarAdmin(bool redirecionar = false)
        {
            if (User.IsInRole("Admin")) return null!;
            return redirecionar ? RedirectToAction("AcessoNegado", "Home") : Unauthorized();
        }

        private string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            var textoNormalizado = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in textoNormalizado)
            {
                var categoria = CharUnicodeInfo.GetUnicodeCategory(c);
                if (categoria != UnicodeCategory.NonSpacingMark && !char.IsPunctuation(c) && !char.IsWhiteSpace(c))
                {
                    sb.Append(char.ToLowerInvariant(c));
                }
            }

            return sb.ToString();
        }

        [HttpGet]
        public IActionResult Cadastro()
        {
            var resultado = VerificarAdmin(true);
            if (resultado != null) return resultado;

            var model = new Classification
            {
                Classifications = new List<ClassificationModel>
                {
                    new ClassificationModel { Codigo = "L", Descricao = "Livre", Logo = "/images/classifications/L.png" },
                    new ClassificationModel { Codigo = "10", Descricao = "Maiores de 10 anos", Logo = "/images/classifications/10.png" },
                    new ClassificationModel { Codigo = "12", Descricao = "Maiores de 12 anos", Logo = "/images/classifications/12.png" },
                    new ClassificationModel { Codigo = "14", Descricao = "Maiores de 14 anos", Logo = "/images/classifications/14.png" },
                    new ClassificationModel { Codigo = "16", Descricao = "Maiores de 16 anos", Logo = "/images/classifications/16.png" },
                    new ClassificationModel { Codigo = "18", Descricao = "Maiores de 18 anos", Logo = "/images/classifications/18.png" }
                }
            };

            return View("~/Views/AdminPages/CadastroFilmes.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(Filme filme, string DiretorLista, string ElencoLista,
            string GeneroLista, IFormFile capaUpload, IFormFile classificationUpload, bool Ativo = true, bool ExibirNoSite = true)
        {
            var resultado = VerificarAdmin();
            if (resultado != null) return resultado;

            var filmes = CarregarFilmes();

            filme.Id = filmes.Any() ? filmes.Max(f => f.Id) + 1 : 1;
            filme.DataHoraCadastro = DateTime.Now;
            filme.Ativo = Ativo;
            filme.ExibirNoSite = ExibirNoSite;

            filme.Diretor = ConverterParaLista(DiretorLista);
            filme.Elenco = ConverterParaLista(ElencoLista);
            filme.Genero = ConverterParaLista(GeneroLista);

            if (capaUpload != null && capaUpload.Length > 0)
            {
                filme.Capa = await ProcessarArquivo(capaUpload, "filme", filme.Id, "image");
            }

            filme.ClassificacaoSelecionada = filme.ClassificacaoSelecionada;
            var codigo = filme.ClassificacaoSelecionada;
            var caminhoLogo = $"/images/classifications/{codigo}.png";

            filmes.Add(filme);
            SalvarFilmes(filmes);

            // ✅ Envio de e-mail para os assinantes
            await _emailService.EnviarEmailNovoFilmeAsync(filme);

            return View("~/Views/AdminPages/CadastroFilmes.cshtml");
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var resultado = VerificarAdmin(true);
            if (resultado != null) return resultado;

            var filmes = CarregarFilmes();
            return View("~/Views/AdminPages/DashboardFilmes.cshtml", filmes);
        }

        [HttpGet]
        public JsonResult Buscar(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return Json(new List<object>());

            var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "filmes.json");
            var json = System.IO.File.ReadAllText(caminho);
            var filmes = JsonSerializer.Deserialize<List<Filme>>(json) ?? new List<Filme>();

            termo = NormalizarTexto(termo);

            var resultado = filmes.Where(f =>
                NormalizarTexto(f.Titulo ?? "").Contains(termo) ||
                f.Genero?.Any(g => NormalizarTexto(g).Contains(termo)) == true ||
                f.Diretor?.Any(d => NormalizarTexto(d).Contains(termo)) == true ||
                f.Elenco?.Any(e => NormalizarTexto(e).Contains(termo)) == true
            ).ToList();

            return Json(resultado);
        }

        private string RemoverAcentos(string texto)
        {
            return new string(texto.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }

        private List<string> ConverterParaLista(string entrada)
        {
            if (string.IsNullOrWhiteSpace(entrada))
                return new List<string>();

            try
            {
                var elementos = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(entrada);
                return elementos?.Select(x => x["value"]).ToList() ?? new List<string>();
            }
            catch
            {
                return entrada.Split(',')
                              .Select(x => x.Trim())
                              .Where(x => !string.IsNullOrEmpty(x))
                              .ToList();
            }
        }

        private List<Filme> CarregarFilmes()
        {
            try
            {
                if (!System.IO.File.Exists(CaminhoJson))
                    return new List<Filme>();

                var json = System.IO.File.ReadAllText(CaminhoJson);
                return JsonSerializer.Deserialize<List<Filme>>(json) ?? new List<Filme>();
            }
            catch
            {
                return new List<Filme>();
            }
        }

        private void SalvarFilmes(List<Filme> filmes)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(filmes, options);
            System.IO.File.WriteAllText(CaminhoJson, json);
        }

        private async Task<string> ProcessarArquivo(IFormFile arquivo, string prefixo, int id, string pasta)
        {
            if (!arquivo.ContentType.StartsWith("image"))
                throw new InvalidOperationException("O arquivo enviado não é uma imagem.");

            var caminhoPasta = Path.Combine(_env.WebRootPath, pasta);
            Directory.CreateDirectory(caminhoPasta);

            var extensao = Path.GetExtension(arquivo.FileName);
            var nomeArquivo = $"{prefixo}_{id}{extensao}";
            var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

            using var stream = new FileStream(caminhoCompleto, FileMode.Create);
            await arquivo.CopyToAsync(stream);

            return $"/{pasta}/{nomeArquivo}";

        }
    }
}
