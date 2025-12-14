using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoDriptz.Models
{
    public class VendaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int EstoqueId { get; set; }
        
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Tamanho { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string PrecoItem { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int FormaDePagamento { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int? FormaDePagamentoAdicional { get; set; }
        
        public bool PossuiMaisDeUmaFormaPagamento { get; set; }


        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? ValorAdicional { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime DataVenda { get; set; }


        public ProdutoModel? Produto { get; set; }
        public EstoqueModel? Estoque { get; set; }
      
    }
}
