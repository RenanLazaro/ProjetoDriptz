using ProjetoDriptz.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Models.ViewModels
{
    public class EstoqueVm
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public TamanhoProduto Tamanho { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Cor { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Quantidade { get; set; }

    }
}
