using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Models
{
    public class EstoqueModel
    {

        public int Id { get; set; }
        
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Tamanho { get; set; }
        
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Cor { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Quantidade { get; set; }

        public ProdutoModel? Produto { get; set; }
        public virtual List<VendaItemModel> VendasItens { get; set; }
    }
}
