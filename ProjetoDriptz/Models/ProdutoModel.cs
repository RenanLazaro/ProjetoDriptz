using ProjetoDriptz.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public decimal PrecoCusto { get; set; }

        [Required(ErrorMessage = "O tamanho do produto é obrigatório.")]
        public decimal Preco { get; set; }


        [Required(ErrorMessage = "O tamanho do produto é obrigatório.")]
        public string? Imagem { get; set; }

        // Propriedade NotMapped para upload (não salva no banco)
        [NotMapped]
        public IFormFile? ImagemUpload { get; set; }

        public virtual List<EstoqueModel> Estoques { get; set; }
        public virtual List<VendaItemModel> VendaItems { get; set; }
    }
}
