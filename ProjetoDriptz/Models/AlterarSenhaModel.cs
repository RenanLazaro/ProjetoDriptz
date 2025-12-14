using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Models
{
    public class AlterarSenhaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A senha atual é obrigatória.")]
        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [Compare("NovaSenha", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmarNovaSenha { get; set; }
    }
}
