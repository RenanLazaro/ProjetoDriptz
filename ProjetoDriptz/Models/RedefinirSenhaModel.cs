using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Models
{
    public class RedefinirSenhaModel
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Email { get; set; }
    }
}
