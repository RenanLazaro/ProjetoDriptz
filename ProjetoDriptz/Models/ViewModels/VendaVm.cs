using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProjetoDriptz.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Models.ViewModels
{
    public class VendaVm
    {
        public int Id { get; set; }
        public DateTime DataVenda { get; set; } = DateTime.Now;

        public int FormaDePagamento { get; set; }
        public int? FormaDePagamentoAdicional { get; set; }
        public bool PossuiMaisDeUmaFormaPagamento { get; set; }

        public int? DescontoGeral { get; set; }
        public decimal ValorAdicional { get; set; }

        public decimal Subtotal { get; set; }
        public decimal ValorTotal { get; set; }




        // Produto
        [ValidateNever]
        public string NomeProduto { get; set; }
        [ValidateNever]
        public string ImagemProduto { get; set; }
        public decimal ValorProduto { get; set; }

        // Estoque
        public int Quantidade { get; set; }


        public List<VendaItemVm> Itens { get; set; } = new();

        public ProdutoModel? Produto { get; set; }
    }


    public class VendaItemVm
    {
        public int ProdutoId { get; set; }
        public int EstoqueId { get; set; }
        public string NomeProduto { get; set; }
        public TamanhoProduto Tamanho { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int? DescontoPercentual { get; set; }


        public decimal SubTotal => Quantidade * PrecoUnitario;

    }




}
