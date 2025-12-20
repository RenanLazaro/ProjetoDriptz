using ProjetoDriptz.Models.ViewModels.Relatorio;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace ProjetoDriptz.Reports
{
    public class RelatorioVendasMesPdf : IDocument
    {
        private readonly RelatorioVendasMesVm _vm;
        private readonly CultureInfo _ptBr = CultureInfo.GetCultureInfo("pt-BR");

        public RelatorioVendasMesPdf(RelatorioVendasMesVm vm)
        {
            _vm = vm;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Content().Column(col =>
                {
                    col.Item().Element(Cabecalho);
                    col.Item().PaddingVertical(20).Element(Tabela);
                });

                page.Footer()
      .AlignRight()
      .PaddingTop(5)
      .Text(text =>
      {
          text.DefaultTextStyle(x =>
              x.FontSize(9)
               .FontColor(Colors.Grey.Darken2));

          text.Span("Relatório gerado em ");
          text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).Bold();
      });


            });
        }

        // ================= CABEÇALHO =================
        void Cabecalho(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Relatório de Vendas")
                        .FontSize(16)
                        .Bold();

                    col.Item().Text($"Mês: {_vm.Mes:D2}/{_vm.Ano}")
                        .FontSize(10);
                });

                row.ConstantItem(80).AlignRight().AlignMiddle()
                    .Image("wwwroot/img/driptz-logo.png", ImageScaling.FitArea);
            });
        }

        // ================= TABELA =================
        void Tabela(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3); // Produto
                    columns.RelativeColumn(2); // Data
                    columns.RelativeColumn(2); // Quantidade
                    columns.RelativeColumn(2); // Valor Unitário
                    columns.RelativeColumn(2); // Valor Total
                });

                // ===== HEADER =====
                table.Header(header =>
                {
                    HeaderCell(header.Cell(), "Produto");
                    HeaderCell(header.Cell(), "Data");
                    HeaderCell(header.Cell(), "Quantidade");
                    HeaderCell(header.Cell(), "Valor Unitário");
                    HeaderCell(header.Cell(), "Valor Total");
                });

                decimal totalGeral = _vm.Vendas
                    .SelectMany(v => v.VendaItens)
                    .Sum(i => i.Quantidade * i.PrecoUnitario);


                // ===== LINHAS =====
                foreach (var venda in _vm.Vendas)
                {
                    foreach (var item in venda.VendaItens)
                    {
                        BodyCell(table.Cell(), item.Produto.NomeProduto);
                        BodyCell(table.Cell(), venda.DataVenda.ToString("dd/MM/yyyy"));
                        BodyCell(table.Cell(), item.Quantidade.ToString(), c => c.AlignCenter());
                        BodyCell(table.Cell(), item.PrecoUnitario.ToString("C", _ptBr), c => c.AlignRight());

                        var total = item.Quantidade * item.PrecoUnitario;
                        BodyCell(table.Cell(), total.ToString("C", _ptBr), c => c.AlignRight());

                    }
                }

                // ===== TOTAL GERAL =====
                table.Cell().ColumnSpan(4)
                    .Border(1)
                    .Background(Colors.Grey.Lighten2)
                    .Padding(5)
                    .AlignRight()
                    .Text("TOTAL DO MÊS")
                    .Bold();

                table.Cell()
                    .Border(1)
                    .Background(Colors.Grey.Lighten2)
                    .Padding(5)
                    .AlignRight()
                    .Text(totalGeral.ToString("C", _ptBr))
                    .Bold();

            });
        }

        // ================= ESTILOS =================
        void HeaderCell(IContainer cell, string text)
        {
            cell
                .Border(1)
                .Background(Colors.Grey.Lighten3)
                .Padding(5)
                .AlignCenter()
                .Text(text)
                .Bold();
        }
        void BodyCell(IContainer cell, string text, Action<IContainer>? alignment = null)
        {
            var container = cell
                .Border(1)
                .Padding(5)
                .AlignMiddle();

            alignment?.Invoke(container);

            container.Text(text);
        }


    }
}
