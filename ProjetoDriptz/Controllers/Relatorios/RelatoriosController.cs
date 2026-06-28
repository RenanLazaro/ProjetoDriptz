using Microsoft.AspNetCore.Mvc;
using ProjetoDriptz.Models.ViewModels.Relatorio;
using ProjetoDriptz.Reports;
using ProjetoDriptz.Repositorio.Interfaces;
using QuestPDF.Fluent;
using Rotativa.AspNetCore;


[PaginaParaAdminLogado]
public class RelatoriosController : Controller
{
    private readonly IVendaRepositorio _vendaRepositorio;
    private readonly IProdutoRepositorio _produtoRepositorio;   

    public RelatoriosController(IVendaRepositorio vendaRepositorio, IProdutoRepositorio produtoRepositorio)
    {
        _vendaRepositorio = vendaRepositorio;
        _produtoRepositorio = produtoRepositorio;       
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View("RelatorioVendas", new RelatorioVendasMesVm
        {
            Ano = DateTime.Now.Year
        });
    }

    /*[HttpGet]
    public IActionResult GerarRelatorio(int mes, int ano)
    {
        var vendas = _vendaRepositorio
            .BuscarTodosComItens()
            .Where(v => v.DataVenda.Month == mes && v.DataVenda.Year == ano)
            .OrderBy(v => v.DataVenda)
            .ToList();


        var vm = new RelatorioVendasMesVm
        {
            Mes = mes,
            Ano = ano,
            Vendas = vendas
        };



        return new ViewAsPdf("~/Views/Relatorios/Pdf/RelatoriosVendasPdf.cshtml", vm)
        {
            FileName = $"Relatorio_Vendas_{mes}_{ano}.pdf",
             PageSize = Rotativa.AspNetCore.Options.Size.A4
        };

    }*/

    [HttpGet]
    public IActionResult GerarRelatorio(int mes, int ano)
    {
        var vendas = _vendaRepositorio
            .BuscarTodosComItens()
            .Where(v => v.DataVenda.Month == mes && v.DataVenda.Year == ano)
            .OrderBy(v => v.DataVenda)
            .ToList();

        var vm = new RelatorioVendasMesVm
        {
            Mes = mes,
            Ano = ano,
            Vendas = vendas
        };

        var documento = new RelatorioVendasMesPdf(vm);
        var pdfBytes = documento.GeneratePdf();

        return File(
            pdfBytes,
            "application/pdf",
            $"Relatorio_Vendas_{mes:D2}_{ano}.pdf"
        );
    }

}
