using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjetoDriptz.Models;
using System.Text.Json;

namespace ProjetoDriptz.Filters
{
    public class PaginaParaUsuarioLogado : ActionFilterAttribute
    {
          public override void OnActionExecuting(ActionExecutingContext context)
          {
              string sessaoUsuario = context.HttpContext.Session.GetString("UsuarioLogado");

              if (string.IsNullOrEmpty(sessaoUsuario))
              {
                  context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "login" }, { "action", "index" } });
              }
              else 
              { 
                  UsuarioModel usuario = JsonSerializer.Deserialize<UsuarioModel>(sessaoUsuario);

                  if(usuario == null)
                  {
                     context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "login" }, { "action", "index" } });
                  }
              }

                  base.OnActionExecuting(context);
          }

    }
       
 }
