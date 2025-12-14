using ProjetoDriptz.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Models
{
    public class ProdutoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        public string NomeProduto { get; set; }

        [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
        public int Tipo { get; set; }

        [Required(ErrorMessage = "O tamanho do produto é obrigatório.")]
        public int Tamanho { get; set; }

        [Required(ErrorMessage = "O tamanho do produto é obrigatório.")]
        public string Preco { get; set; }


        public virtual List<EstoqueModel> Estoques { get; set; }
        public virtual List<VendaModel> Vendas { get; set; }
    }
}
