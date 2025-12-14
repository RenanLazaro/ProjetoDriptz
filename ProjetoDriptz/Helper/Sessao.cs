using ProjetoDriptz.Models;
using System.Text.Json;

namespace ProjetoDriptz.Helper
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Sessao(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public UsuarioModel BuscarSessaoUsuario()
        {
            string sessaoUsuario = _httpContextAccessor.HttpContext.Session.GetString("sessaoUsuarioLogado");

            if (string.IsNullOrEmpty(sessaoUsuario)) return null;
      
            return JsonSerializer.Deserialize<UsuarioModel>(sessaoUsuario);
        }

        public void CriarSessaoUsuario(UsuarioModel usuario)
        {
            string valor = JsonSerializer.Serialize(usuario);
            _httpContextAccessor.HttpContext.Session.SetString("sessaoUsuarioLogado",valor);
        }

        public void RemoverSessaoUsuario()
        {
            _httpContextAccessor.HttpContext.Session.Remove("sessaoUsuarioLogado");
        }
    }
}