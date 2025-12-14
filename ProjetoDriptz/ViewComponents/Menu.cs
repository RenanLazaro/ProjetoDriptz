using Microsoft.AspNetCore.Mvc;
using ProjetoDriptz.Models;
using System.Text.Json;

namespace ProjetoDriptz.ViewComponents
{
    public class Menu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sessaoUsuario = HttpContext.Session.GetString("sessaoUsuarioLogado");
            
            if (string.IsNullOrEmpty(sessaoUsuario)) return null;

            UsuarioModel usuario =   JsonSerializer.Deserialize<UsuarioModel>(sessaoUsuario);

            return View(usuario);
        }

    }
}
