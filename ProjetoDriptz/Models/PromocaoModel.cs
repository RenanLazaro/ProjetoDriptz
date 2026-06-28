using ProjetoDriptz.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Models
{
    public class PromocaoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione o produto.")]
        [Display(Name = "Produto")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Informe o nome da promoção.")]
        [Display(Name = "Nome da promoção")]
        public string Nome { get; set; } // ex: "Combo Família", "Oferta do dia"

        [Required(ErrorMessage = "Escolha o tipo de desconto.")]
        [Display(Name = "Tipo de desconto")]
        public TipoDesconto TipoDesconto { get; set; }

        // Preenchido se TipoDesconto == Percentual
        [Display(Name = "Desconto (%)")]
        [Range(1, 100, ErrorMessage = "Informe entre 1 e 100%.")]
        public int? DescontoPercentual { get; set; }

        // Preenchido se TipoDesconto == PrecoFixo
        [Display(Name = "Preço promocional (R$)")]
        [Range(0.01, 9999, ErrorMessage = "Informe um valor válido.")]
        public decimal? PrecoFixo { get; set; }

        [Required(ErrorMessage = "Informe a data de início.")]
        [Display(Name = "Válido de")]
        public DateTime DataInicio { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Informe a data de fim.")]
        [Display(Name = "Válido até")]
        public DateTime DataFim { get; set; } = DateTime.Today.AddDays(7);

        public bool Ativo { get; set; } = true;

        // Navegação
        public ProdutoModel? Produto { get; set; }

        // Helper: retorna o preço final calculado
        public decimal CalcularPrecoFinal(decimal precoOriginal)
        {
            if (!EstaAtiva()) return precoOriginal;

            return TipoDesconto switch
            {
                TipoDesconto.Percentual when DescontoPercentual.HasValue =>
                    precoOriginal * (1 - DescontoPercentual.Value / 100m),
                TipoDesconto.PrecoFixo when PrecoFixo.HasValue =>
                    PrecoFixo.Value,
                _ => precoOriginal
            };
        }

        public bool EstaAtiva()
        {
            var hoje = DateTime.Today;
            return Ativo && DataInicio <= hoje && hoje <= DataFim;
        }
    }
}