using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o servi�o de autentica��o e cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/CadastroLogin";  // P�gina de login
        options.LogoutPath = "/Usuario/Logout";       // P�gina de logout
        options.AccessDeniedPath = "/Usuario/AccessDenied"; // P�gina de acesso negado, caso necess�rio
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

// Habilita a autentica��o e autoriza��o
app.UseAuthentication();  // Adiciona a autentica��o
app.UseAuthorization();   // Adiciona a autoriza��o

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
