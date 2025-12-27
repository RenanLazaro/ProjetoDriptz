using Microsoft.AspNetCore.Mvc;
using ProjetoDriptz.Models;
using ProjetoDriptz.Repositorio;
using System.Diagnostics;
using System.Text.Json;

namespace ProjetoDriptz.Controllers
{
    //preciso resolver problema de autenticação
   
 //   [PaginaParaUsuarioLogado]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public HomeController(ILogger<HomeController> logger,IUsuarioRepositorio usuarioRepositorio)
        {
            _logger = logger;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public IActionResult Index()
        {
            HomeModel home = new HomeModel();

            // Pega o usuário da sessão
            var sessaoUsuario = HttpContext.Session.GetString("sessaoUsuarioLogado");

            if (!string.IsNullOrEmpty(sessaoUsuario))
            {
                var usuario = JsonSerializer.Deserialize<UsuarioModel>(sessaoUsuario);
                home.Nome = usuario.Nome;
                home.Email = usuario.Email;
            }

            return View(home);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
