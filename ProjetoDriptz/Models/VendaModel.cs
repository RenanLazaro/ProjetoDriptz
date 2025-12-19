using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Models
{
    public class VendaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime DataVenda { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int FormaDePagamento { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int? FormaDePagamentoAdicional { get; set; }
        
        public bool PossuiMaisDeUmaFormaPagamento { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public decimal ValorAdicional { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public decimal ValorTotal { get; set; }

        public int? DescontoGeral { get; set; }

        public virtual List<VendaItemModel> VendaItens { get; set; }

    }
}
