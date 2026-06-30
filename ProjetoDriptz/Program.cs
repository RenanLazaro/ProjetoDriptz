using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjetoDriptz.Data;
using ProjetoDriptz.Helper;
using ProjetoDriptz.Repositorio;
using ProjetoDriptz.Repositorio.Interfaces;
using QuestPDF.Infrastructure;
using Rotativa.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

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
builder.Services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
builder.Services.AddScoped<IPromocaoRepositorio, PromocaoRepositorio>();

builder.Services.AddScoped<IEmail, Email>();
builder.Services.AddScoped<ISessao, Sessao>();

// SESSION
builder.Services.AddSession(o =>
{
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
    // Necessário para o cookie de sessão ser enviado em requisições
    // cross-origin feitas pelo Angular (ex: localhost:4200 -> localhost:5000)
    o.Cookie.SameSite = SameSiteMode.None;
    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// CORS — libera o app Angular a consumir a API com cookies (credentials)
const string AngularCorsPolicy = "AngularCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularCorsPolicy, policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",   // ng serve (dev)
                "https://localhost:4200"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // obrigatório para o cookie de sessão/auth ir junto
    });
});

// AUTH
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/Index";
        // Mesmo motivo do cookie de sessão: precisa trafegar em requisições
        // cross-origin vindas do Angular.
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

// MVC
builder.Services.AddControllersWithViews();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 5 * 1024 * 1024; // 5MB
});


var app = builder.Build();

// ?? APLICA MIGRATIONS AUTOMATICAMENTE (PRODU��O)
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<BancoContext>();
//     db.Database.Migrate();
// }

app.UseStaticFiles();
// PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(AngularCorsPolicy);

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
// ?? CONFIGURA��O OBRIGAT�RIA
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