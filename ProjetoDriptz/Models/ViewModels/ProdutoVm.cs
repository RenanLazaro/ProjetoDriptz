using ProjetoDriptz.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Models.ViewModels
{
    public class ProdutoVm
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string NomeProduto { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public TipoProduto Tipo { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public TamanhoProduto Tamanho { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Preco { get; set; }
    }
}