namespace CineBR.Models
{
    public class Usuario
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Gera um ID único automaticamente
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Tipo { get; set; } = "Usuario"; // Padrão "Usuario"
        public bool ReceberEmails { get; set; }
    }
}
