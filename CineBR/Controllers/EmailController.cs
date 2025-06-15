using CineBR.Models;
using CineBR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CineBR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(IConfiguration config)
        {
            _emailService = new EmailService(config);
        }

        /// <summary>
        /// Envia um e-mail para todos os usuários que optaram por receber notificações.
        /// </summary>
        /// <param name="filme">Objeto do novo filme cadastrado</param>
        [HttpPost("EnviarEmailsNovoFilme")]
        public async Task<IActionResult> EnviarEmailsNovoFilme([FromBody] Filme filme)
        {
            try
            {
                if (filme == null || string.IsNullOrWhiteSpace(filme.Titulo))
                    return BadRequest("Dados do filme inválidos.");

                await _emailService.EnviarEmailNovoFilmeAsync(filme);

                return Ok("Emails enviados com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao enviar emails: {ex.Message}");
            }
        }

        /// <summary>
        /// Envia um e-mail simples para notificar que um novo filme foi adicionado (sem detalhes).
        /// </summary>
        /// <param name="titulo">Título do novo filme</param>
        [HttpPost("NotificarNovoTitulo")]
        public async Task<IActionResult> NotificarNovoTitulo([FromBody] string titulo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(titulo))
                    return BadRequest("Título do filme inválido.");

                await _emailService.EnviarEmailParaUsuariosAsync(titulo);

                return Ok("Notificação enviada com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao enviar notificação: {ex.Message}");
            }
        }
    }
}
