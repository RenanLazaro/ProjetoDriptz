using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Models
{
    public class PedidoItemModel
    {
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        public decimal PrecoUnitario { get; set; }

        public decimal SubTotal => Quantidade * PrecoUnitario;

        public string? Observacao { get; set; } // ex: "sem cebola"

        // Navegação
        public PedidoModel? Pedido { get; set; }
        public ProdutoModel? Produto { get; set; }
    }
}