using ProjetoDriptz.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public decimal PrecoCusto { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Preco { get; set; }


        public string? Imagem { get; set; }

        [NotMapped]
        public IFormFile? ImagemUpload { get; set; }
    }

    public class ProdutoEstoqueVm
    {
        public int EstoqueId { get; set; }
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public string Preco { get; set; }
        public TamanhoProduto Tamanho { get; set; }
        public int QuantidadeDisponivel { get; set; }
        public string? Imagem { get; set; }
    }

}