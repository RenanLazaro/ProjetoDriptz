using ProjetoDriptz.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Models.ViewModels
{
    // Item dentro do carrinho (guardado na Session)
    public class CarrinhoItemVm
    {
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public string? Imagem { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public string? Observacao { get; set; }

        public decimal SubTotal => Quantidade * PrecoUnitario;
    }

    // Usado no checkout (dados do cliente + carrinho)
    public class CheckoutVm
    {
        [Required(ErrorMessage = "Informe seu nome.")]
        [Display(Name = "Nome completo")]
        public string NomeCliente { get; set; }

        [Required(ErrorMessage = "Informe seu telefone.")]
        [Display(Name = "Telefone / WhatsApp")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Escolha o tipo de entrega.")]
        [Display(Name = "Tipo de entrega")]
        public TipoEntrega TipoEntrega { get; set; }

        [Display(Name = "Endereço")]
        public string? Endereco { get; set; }

        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [Display(Name = "Bairro")]
        public string? Bairro { get; set; }

        [Required(ErrorMessage = "Escolha a forma de pagamento.")]
        [Display(Name = "Forma de pagamento")]
        public FormaDePagamento FormaDePagamento { get; set; }

        [Display(Name = "Troco para (R$)")]
        public decimal? TrocoPara { get; set; }

        [Display(Name = "Observação geral")]
        public string? Observacao { get; set; }

        // Preenchido pelo controller, não pelo form
        public List<CarrinhoItemVm> Itens { get; set; } = new();
        public decimal ValorTotal => Itens.Sum(i => i.SubTotal);
    }

    // Exibição do pedido confirmado
    public class PedidoConfirmadoVm
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; }
        public string Telefone { get; set; }
        public TipoEntrega TipoEntrega { get; set; }
        public string? Endereco { get; set; }
        public string? Bairro { get; set; }
        public FormaDePagamento FormaDePagamento { get; set; }
        public decimal ValorTotal { get; set; }
        public StatusPedido Status { get; set; }
        public DateTime DataPedido { get; set; }
        public List<CarrinhoItemVm> Itens { get; set; } = new();
    }
}