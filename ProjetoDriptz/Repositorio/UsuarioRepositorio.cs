using ProjetoDriptz.Data;
using ProjetoDriptz.Models;
using ProjetoDriptz.Repositorio.Interfaces;

namespace ProjetoDriptz.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly BancoContext _bancoContext;
        public UsuarioRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public UsuarioModel ListarPorIId(int id)
        {
           return _bancoContext.Usuarios.FirstOrDefault(x => x.Id == id);
            
        }

        public List<UsuarioModel> BuscarTodos()
        {             
            return _bancoContext.Usuarios.ToList();
        }

        public UsuarioModel Adicionar(UsuarioModel usuario)
        {

            usuario.DataCadastro = DateTime.Now;
            
            //gravar no banco de dados
            usuario.setSenhaHash();
            _bancoContext.Usuarios.Add(usuario);
            _bancoContext.SaveChanges();    

            return usuario;

        }

        public UsuarioModel Editar(UsuarioModel usuario)
        {
            UsuarioModel usuarioDb = _bancoContext.Usuarios.FirstOrDefault(x => x.Id == usuario.Id);

            if (usuarioDb == null)
                throw new Exception("Houve um erro na atualização do usuario.");

            usuarioDb.Nome = usuario.Nome;
            usuarioDb.Email = usuario.Email;
            usuarioDb.Login = usuario.Login;
            usuarioDb.Perfil = usuario.Perfil;
            usuarioDb.DataAtualizacao = DateTime.Now;


            _bancoContext.Usuarios.Update(usuarioDb);
            _bancoContext.SaveChanges();

            return usuarioDb;
        }

        public UsuarioModel AlterarSenha(AlterarSenhaModel alterarSenhaModel)
        {
            UsuarioModel usuarioDb = ListarPorIId(alterarSenhaModel.Id);

            if (usuarioDb == null)  throw new Exception("Houve um erro na atualização da senha. usuário não encontrado!");
            
            if (!usuarioDb.SenhaValida(alterarSenhaModel.SenhaAtual)) throw new Exception("Senha Atual não confere!");

            if(usuarioDb.SenhaValida(alterarSenhaModel.NovaSenha)) throw new Exception("A nova senha não pode ser igual a senha atual!");

            usuarioDb.SetNovaSenha(alterarSenhaModel.NovaSenha);
            usuarioDb.DataAtualizacao = DateTime.Now;
        
            _bancoContext.Usuarios.Update(usuarioDb);
            _bancoContext.SaveChanges();
            return usuarioDb;
        }


        //   public ProdutoModel Excluir(ProdutoModel produto)
        //   {
        //     _bancoContext.Produtos.Remove(produto);
        //     _bancoContext.SaveChanges();

            //            return produto;
            //       }

        public bool Excluir(int id)
        {
            UsuarioModel usuarioDb = ListarPorIId(id);

            if(usuarioDb == null) throw new Exception("Houve um erro na exclusão do usuario!");

            _bancoContext.Usuarios.Remove(usuarioDb);
            _bancoContext.SaveChanges();

            return true;
        }

      
        
        public UsuarioModel BuscarPorLogin(string login)
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Login.ToUpper() == login.ToUpper());
        }

        public UsuarioModel BuscarPorEmailELogin(string email, string login)
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Email.ToUpper() == email.ToUpper() && x.Login.ToUpper() == login.ToUpper());
        }
    }
}
