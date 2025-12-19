using ProjetoDriptz.Enums;
using System.ComponentModel.DataAnnotations;


namespace ProjetoDriptz.Models
    {
    public class VendaItemModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int VendaId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int EstoqueId { get; set; }


        [Required(ErrorMessage = "Campo Obrigatório")]
        public TamanhoProduto Tamanho { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public decimal PrecoUnitario { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public decimal SubTotal { get; set; }


        public ProdutoModel? Produto { get; set; }
        public EstoqueModel? Estoque { get; set; }
        public VendaModel? Venda { get; set; }

    }
 }

