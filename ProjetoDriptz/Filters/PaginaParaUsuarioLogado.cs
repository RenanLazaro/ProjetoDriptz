using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjetoDriptz.Models;
using System.Text.Json;

public class PaginaParaUsuarioLogado : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // ✅ Ignora validação se for página de Login
        if (context.RouteData.Values["controller"].ToString().ToLower() == "login")
        {
            base.OnActionExecuting(context);
            return;
        }

        string sessaoUsuario = context.HttpContext.Session.GetString("sessaoUsuarioLogado");

        if (string.IsNullOrEmpty(sessaoUsuario))
        {
            context.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                    { "controller", "Login" },
                    { "action", "Index" }
                });
        }
        else
        {
            UsuarioModel usuario = JsonSerializer.Deserialize<UsuarioModel>(sessaoUsuario);
            if (usuario == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "controller", "Login" },
                        { "action", "Index" }
                    });
            }
        }

        base.OnActionExecuting(context);
    }
}