using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Serviço de email (mantido)
builder.Services.AddTransient<EmailService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)

.AddCookie(options =>
{
    options.LoginPath = "/Usuario/CadastroLogin";
    options.LogoutPath = "/Usuario/Logout";
    options.AccessDeniedPath = "/Usuario/AccessDenied";
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

app.UseAuthentication(); // Habilita autenticação
app.UseAuthorization();  // Habilita autorização

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
