using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjetoDriptz.Models;
using ProjetoDriptz.Enums;
using System.Text.Json;

public class PaginaParaAdminLogado : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string sessaoUsuario = context.HttpContext.Session.GetString("sessaoUsuarioLogado");

        // Não está logado → vai para o login
        if (string.IsNullOrEmpty(sessaoUsuario))
        {
            context.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                    { "controller", "Login" },
                    { "action", "Index" }
                });
            return;
        }

        UsuarioModel usuario = JsonSerializer.Deserialize<UsuarioModel>(sessaoUsuario);

        // Sessão corrompida → vai para o login
        if (usuario == null)
        {
            context.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                    { "controller", "Login" },
                    { "action", "Index" }
                });
            return;
        }

        // Está logado mas não é admin → vai para o acesso negado (ou home)
        if ((PerfilUsuario)usuario.Perfil != PerfilUsuario.Administrador)
        {
            context.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                    { "controller", "Home" },
                    { "action", "AcessoNegado" }
                });
            return;
        }

        base.OnActionExecuting(context);
    }
}