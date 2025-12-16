using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ProjetoDriptz.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Models.ViewModels
{
    public class VendaVm
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int EstoqueId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public TamanhoProduto Tamanho { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string PrecoItem { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public FormaDePagamento FormaDePagamento { get; set; }


        [Required(ErrorMessage = "Campo Obrigatório")]
        public FormaDePagamento? FormaDePagamentoAdicional { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public bool PossuiMaisDeUmaFormaPagamento { get; set; }


        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? ValorAdicional { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime DataVenda { get; set; }


        //Campos de Exibição

        // PDV
        public List<ProdutoEstoqueVm> ProdutosEmEstoque { get; set; } = new();

        public List<VendaItemVm> Itens { get; set; } = new();

        [ValidateNever]
        public string NomeProduto { get; set; }


    }

    public class VendaItemVm
    {
        public int ProdutoId { get; set; }
        public int? EstoqueId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public TamanhoProduto Tamanho { get; set; }
    }


}
