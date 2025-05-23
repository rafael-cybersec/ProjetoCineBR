using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o serviço de autenticação e cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/CadastroLogin";  // Página de login
        options.LogoutPath = "/Usuario/Logout";       // Página de logout
        options.AccessDeniedPath = "/Usuario/AccessDenied"; // Página de acesso negado, caso necessário
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Habilita a autenticação e autorização
app.UseAuthentication();  // Adiciona a autenticação
app.UseAuthorization();   // Adiciona a autorização

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "detalhes",
    pattern: "filme/detalhes/{id?}",
    defaults: new { controller = "Home", action = "Detalhes" });

app.MapControllerRoute(
    name: "filmes-curtidos",
    pattern: "Home/filmes-curtidos",
    defaults: new { controller = "FilmesCurtidos", action = "filmes-curtidos" });

app.Run();
