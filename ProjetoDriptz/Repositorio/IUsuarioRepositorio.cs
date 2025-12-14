using ProjetoDriptz.Models;

namespace ProjetoDriptz.Repositorio
{
    public interface IUsuarioRepositorio
    {
       
        UsuarioModel BuscarPorLogin(string login);
        UsuarioModel BuscarPorEmailELogin(string email,string login);
        UsuarioModel ListarPorIId(int id);
        List<UsuarioModel> BuscarTodos();
        UsuarioModel Adicionar(UsuarioModel produto);

        UsuarioModel Editar(UsuarioModel produto);

        UsuarioModel AlterarSenha(AlterarSenhaModel alterarSenhaModel);
        bool Excluir(int id);

    }
}
