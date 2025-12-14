using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoDriptz.Filters;
using ProjetoDriptz.Models;
using System.Diagnostics;

namespace ProjetoDriptz.Controllers
{
    //preciso resolver problema de autenticação
    //[PaginaParaUsuarioLogado]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HomeModel home = new HomeModel();

            home.Nome = "Renan";
            home.Email = "renanlazaro@gmail.com";
           
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
