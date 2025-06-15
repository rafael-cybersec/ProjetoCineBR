using MimeKit;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Text;
using CineBR.Models;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    // Envia email simples notificando um novo título
    public async Task EnviarEmailParaUsuariosAsync(string tituloFilme)
    {
        var destinatarios = await ObterDestinatariosAsync();
        var remetente = _config["EmailSettings:Remetente"];

        string corpo = $@"
            <h3>🎬 Novo filme adicionado ao Portal CineBR:</h3>
            <p><strong>{tituloFilme}</strong></p>
            <p>Acesse agora para conferir!</p>";

        await EnviarEmailsAsync(destinatarios, remetente, "Novo Filme Cadastrado!", corpo);
    }

    // Envia email com todos os dados do novo filme
    public async Task EnviarEmailNovoFilmeAsync(Filme filme)
    {
        var destinatarios = await ObterDestinatariosAsync();
        var remetente = _config["EmailSettings:Remetente"];

        string baseUrl = _config["AppSettings:BaseUrl"] ?? "https://localhost:7202";
        string linkFilme = $"{baseUrl}/filme/detalhes/{filme.Id}";
        string capaUrl = !string.IsNullOrEmpty(filme.Capa)
            ? $"{baseUrl}/image/{filme.Capa}"
            : $"{baseUrl}/images/default-poster.jpg";

        var corpoBuilder = new StringBuilder();
        corpoBuilder.AppendLine("<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; background-color: #1a1a1a; color: #e0e0e0; padding: 20px; border-radius: 8px;'>");

        // Cabeçalho
        corpoBuilder.AppendLine("<div style='border-bottom: 1px solid #2e7d32; padding-bottom: 10px; margin-bottom: 20px;'>");
        corpoBuilder.AppendLine("<h2 style='color: #2e7d32; margin: 0;'>🎬 Novo filme cadastrado no Portal CineBR!</h2>");
        corpoBuilder.AppendLine("</div>");

        // Título e link
        corpoBuilder.AppendLine($"<h3 style='margin-bottom: 5px;'><a href='{linkFilme}' style='color: #4caf50; text-decoration: none;'>{filme.Titulo}</a></h3>");

        // Capa do filme
        corpoBuilder.AppendLine("<div style='margin: 20px 0; text-align: center;'>");
        corpoBuilder.AppendLine($"<a href='{linkFilme}'><img src='{capaUrl}' alt='Capa do filme {filme.Titulo}' style='max-width: 300px; border-radius: 5px; border: 2px solid #2e7d32;'/></a>");
        corpoBuilder.AppendLine("</div>");

        // Informações do filme
        corpoBuilder.AppendLine("<div style='background-color: #2d2d2d; padding: 15px; border-radius: 5px; margin-bottom: 20px;'>");
        corpoBuilder.AppendLine($"<p style='margin: 8px 0;'><strong style='color: #81c784;'>📅 Data de cadastro:</strong> {filme.DataHoraCadastro:dd/MM/yyyy HH:mm:ss}</p>");
        corpoBuilder.AppendLine($"<p style='margin: 8px 0;'><strong style='color: #81c784;'>🎭 Gêneros:</strong> {string.Join(", ", filme.Genero ?? new List<string>())}</p>");
        corpoBuilder.AppendLine($"<p style='margin: 8px 0;'><strong style='color: #81c784;'>🎥 Diretor(es):</strong> {string.Join(", ", filme.Diretor ?? new List<string>())}</p>");
        corpoBuilder.AppendLine($"<p style='margin: 8px 0;'><strong style='color: #81c784;'>🌟 Elenco:</strong> {string.Join(", ", filme.Elenco ?? new List<string>())}</p>");
        corpoBuilder.AppendLine($"<p style='margin: 8px 0;'><strong style='color: #81c784;'>🔞 Classificação:</strong> {filme.ClassificacaoSelecionada}</p>");
        corpoBuilder.AppendLine("</div>");

        // Botão de ação
        corpoBuilder.AppendLine($"<div style='text-align: center;'>");
        corpoBuilder.AppendLine($"<a href='{linkFilme}' style='background-color: #2e7d32; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block;'>Ver Detalhes</a>");
        corpoBuilder.AppendLine("</div>");

        corpoBuilder.AppendLine("</div>");

        await EnviarEmailsAsync(destinatarios, remetente, $"Novo Filme Cadastrado: {filme.Titulo}", corpoBuilder.ToString());
    }

    // Lê os destinatários a partir do arquivo usuarios.json
    private async Task<List<string>> ObterDestinatariosAsync()
    {
        var usuariosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "usuarios.json");

        if (!File.Exists(usuariosPath))
            return new List<string>();

        var json = await File.ReadAllTextAsync(usuariosPath);
        var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json) ?? new List<Usuario>();

        return usuarios
            .Where(u => u.ReceberEmails && !string.IsNullOrWhiteSpace(u.Email))
            .Select(u => u.Email.Trim())
            .ToList();
    }

    // Envia email para todos os destinatários de uma vez
    public async Task EnviarEmailsAsync(List<string> destinatarios, string remetente, string assunto, string corpo)
    {
        var senha = _config["EmailSettings:Senha"];

        using (var smtp = new SmtpClient())
        {
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(remetente, senha);

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(remetente, "CINEBR");

                foreach (var destinatario in destinatarios)
                {
                    message.To.Add(destinatario);
                }

                message.Subject = assunto;
                message.Body = corpo;
                message.IsBodyHtml = true;

                await smtp.SendMailAsync(message);
            }
        }
    }
}