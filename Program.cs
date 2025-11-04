using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TrabalhoElvis2.Context;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// 🔹 CONFIGURAÇÕES GERAIS
// ==========================

// Conexão com o banco de dados
builder.Services.AddDbContext<LoginContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

// MVC + Razor Views
builder.Services.AddControllersWithViews();

// Sessão — mantém login do administrador
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // tempo de inatividade
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Autenticação baseada em cookie (para compatibilidade futura)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login";
        options.AccessDeniedPath = "/Usuario/AcessoNegado";
    });

// Autorização (precisa vir junto)
builder.Services.AddAuthorization();

var app = builder.Build();

// ==========================
// 🔹 PIPELINE DE EXECUÇÃO
// ==========================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redireciona HTTP → HTTPS (agora seguro)
app.UseHttpsRedirection();

// Permite arquivos estáticos (CSS, JS, imagens)
app.UseStaticFiles();

app.UseRouting();

// Ativa sessão (precisa vir antes do Auth)
app.UseSession();

// Ativa autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// ==========================
// 🔹 ROTAS PADRÃO
// ==========================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}"
);

app.Run();