using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using ProjetoDriptz.Data;
using ProjetoDriptz.Helper;
using ProjetoDriptz.Repositorio;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// BANCO
builder.Services.AddDbContext<BancoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));

// DI
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IEstoqueRepositorio, EstoqueRepositorio>();
builder.Services.AddScoped<IVendaRepositorio, VendaRepositorio>();
builder.Services.AddScoped<IVendaItemRepositorio, VendaItemRepositorio>();

builder.Services.AddScoped<IEmail, Email>();
builder.Services.AddScoped<ISessao, Sessao>();

// SESSION
builder.Services.AddSession(o =>
{
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

// AUTH
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/Index";
    });

// MVC
builder.Services.AddControllersWithViews();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 5 * 1024 * 1024; // 5MB
});


var app = builder.Build();

// ?? APLICA MIGRATIONS AUTOMATICAMENTE (PRODUÇÃO)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BancoContext>();
    db.Database.Migrate();
}

// PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
// ?? CONFIGURAÇÃO OBRIGATÓRIA
RotativaConfiguration.Setup(
    app.Environment.WebRootPath,
    "Rotativa"
);
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
