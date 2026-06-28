using ProjetoDriptz.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Models
{
    public class PedidoModel
    {
        public int Id { get; set; }

        public DateTime DataPedido { get; set; } = DateTime.Now;

        // Dados do cliente (sem cadastro)
        [Required(ErrorMessage = "Informe seu nome.")]
        public string NomeCliente { get; set; }

        [Required(ErrorMessage = "Informe seu telefone.")]
        public string Telefone { get; set; }

        // Entrega
        [Required]
        public TipoEntrega TipoEntrega { get; set; }

        // Preenchido só se TipoEntrega == Delivery
        public string? Endereco { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }

        // Pagamento (pago na entrega/retirada)
        [Required(ErrorMessage = "Escolha a forma de pagamento.")]
        public FormaDePagamento FormaDePagamento { get; set; }

        // Troco (só se pagamento = Dinheiro)
        public decimal? TrocoParа { get; set; }

        // Valores
        public decimal ValorTotal { get; set; }
        public string? Observacao { get; set; }

        // Status do pedido (gerenciado pelo admin)
        public StatusPedido Status { get; set; } = StatusPedido.Aguardando;

        public virtual List<PedidoItemModel> Itens { get; set; } = new();
    }
}