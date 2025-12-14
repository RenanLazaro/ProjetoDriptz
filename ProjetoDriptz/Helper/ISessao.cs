using ProjetoDriptz.Models;

namespace ProjetoDriptz.Helper
{
    public interface ISessao
    {
        void CriarSessaoUsuario(Models.UsuarioModel usuario);
        void RemoverSessaoUsuario();
        UsuarioModel BuscarSessaoUsuario();
    }
}
