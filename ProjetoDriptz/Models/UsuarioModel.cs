using ProjetoDriptz.Helper;
using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Models
{
    public class UsuarioModel
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "O nome do usuario é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O login do usuario é obrigatório.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "O email do usuario é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O email do usuario é obrigatório.")]

        public int Perfil { get; set; }

        [Required(ErrorMessage = "a senha do usuario é obrigatório.")]
        public string Senha { get; set; }

        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public bool SenhaValida(string senha)
        {
            return Senha == senha.GerarHash();
        }

        public void setSenhaHash()
        {
           Senha = Senha.GerarHash();
        }

        public void SetNovaSenha(string novaSenha)
        {
            Senha = novaSenha.GerarHash();
        }

        public string GerarNovaSenha()
        {
            string novaSenha = Guid.NewGuid().ToString().Substring(0, 8);
            Senha = novaSenha.GerarHash();
     
            return novaSenha;
        }
    }
}
