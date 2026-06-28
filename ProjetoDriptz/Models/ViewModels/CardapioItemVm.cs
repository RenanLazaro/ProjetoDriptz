// =====================================================================
// ARQUIVO: Models/ViewModels/CardapioItemVm.cs  — ARQUIVO NOVO
// =====================================================================
namespace ProjetoDriptz.Models.ViewModels
{
    // Representa um produto no cardápio, já com desconto calculado
    public class CardapioItemVm
    {
        public int    ProdutoId     { get; set; }
        public string NomeProduto   { get; set; }
        public string? Imagem       { get; set; }
        public int    Tipo          { get; set; }   // TipoProduto (int)

        // Preço original do produto
        public decimal PrecoOriginal { get; set; }

        // Preço após desconto (= PrecoOriginal se não tiver promoção)
        public decimal PrecoFinal    { get; set; }

        // Dados da promoção (null se não houver)
        public bool   TemPromocao   { get; set; }
        public string? NomePromocao { get; set; }  // ex: "Oferta do dia"
        public string? DescricaoDesconto { get; set; } // ex: "10% off"
    }
}
