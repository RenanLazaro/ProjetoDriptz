namespace ProjetoDriptz.Models.ViewModels.Relatorio
{
    public class RelatorioVendasMesVm
    {

        public int Mes { get; set; }
        public int Ano { get; set; }
        public List<VendaModel> Vendas { get; set; } = new();
        public List<ProdutoModel> Produtos { get; set; } = new();

    }
}
