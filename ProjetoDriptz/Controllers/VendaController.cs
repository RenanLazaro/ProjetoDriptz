using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoDriptz.Enums;
using ProjetoDriptz.Models;
using ProjetoDriptz.Models.ViewModels;
using ProjetoDriptz.Repositorio;

namespace ProjetoDriptz.Controllers
{
    public class VendaController : Controller
    {
        private readonly IEstoqueRepositorio _estoqueRepositorio;
        private readonly IVendaRepositorio _vendaRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public VendaController(IEstoqueRepositorio estoqueRepositorio, IVendaRepositorio vendaRepositorio, IProdutoRepositorio produtoRepositorio)
        {
            _estoqueRepositorio = estoqueRepositorio;
            _vendaRepositorio = vendaRepositorio;
            _produtoRepositorio = produtoRepositorio;
        }


        public IActionResult Index()
        {
            var estoques = _vendaRepositorio.BuscarTodos();
            return View(estoques);
        }



        public IActionResult Editar(int id)
        {
            try
            {
                // Busca a venda pelo Id
                var venda = _vendaRepositorio.ListarPorIId(id);
                if (venda == null)
                {
                    TempData["MensagemErro"] = "Venda não encontrada.";
                    return RedirectToAction("Index");
                }

                // Busca o estoque vinculado à venda
                var estoque = _estoqueRepositorio.ListarPorIId(venda.EstoqueId);
                if (estoque == null)
                {
                    TempData["MensagemErro"] = "Estoque não encontrado.";
                    return RedirectToAction("Index", "Estoque");
                }

                // Converte Model para ViewModel
                var vendaVm = new VendaVm
                {
                    Id = venda.Id,
                    ProdutoId = venda.ProdutoId,
                    EstoqueId = venda.EstoqueId,
                    Tamanho = (TamanhoProduto)venda.Tamanho,
                    Quantidade = venda.Quantidade,
                    PrecoItem = venda.PrecoItem,
                    FormaDePagamento = (FormaDePagamento)venda.FormaDePagamento,
                    FormaDePagamentoAdicional = (FormaDePagamento?)venda.FormaDePagamentoAdicional,
                    PossuiMaisDeUmaFormaPagamento = venda.PossuiMaisDeUmaFormaPagamento,
                    ValorAdicional = venda.ValorAdicional,
                    NomeProduto = venda.Produto?.NomeProduto
                };

                // Carrega lista de produtos para o select
                var produtos = _produtoRepositorio.BuscarTodos()
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.NomeProduto
                    }).ToList();

                ViewData["Produtos"] = produtos;

                return View("EditarVenda", vendaVm);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao carregar venda. Detalhes: {erro.Message}";
                return RedirectToAction("Index");
            }
        }


        public IActionResult ApagarConfirmacao(int id)
        {
            ViewBag.Produtos = _produtoRepositorio.BuscarTodos()
             .Select(p => new SelectListItem
             {
                 Value = p.Id.ToString(),
                 Text = p.NomeProduto
             }).ToList();
            VendaModel venda = _vendaRepositorio.ListarPorIId(id);
            return View(venda);
        }

      /*  public IActionResult Criar()
        {
            ViewBag.Produtos = _produtoRepositorio.BuscarTodos()
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.NomeProduto
                }).ToList();

            return View();
        }*/
        public IActionResult Criar(int id)
        {
            try
            {
                var estoque = _estoqueRepositorio.ListarPorIId(id);

                if (estoque == null)
                {
                    TempData["MensagemErro"] = "Produto não encontrado no estoque.";
                    return RedirectToAction("Index", "Estoque");
                }

                // Busca o produto separadamente
                var produto = _produtoRepositorio.ListarPorIId(estoque.ProdutoId);

                if (produto == null)
                {
                    TempData["MensagemErro"] = "Produto não encontrado.";
                    return RedirectToAction("Index", "Estoque");
                }

                // Converte para ViewModel
                var vendaVm = new VendaVm
                {
                    ProdutoId = estoque.ProdutoId,
                    EstoqueId = estoque.Id,
                    Tamanho = (TamanhoProduto)estoque.Tamanho,
                    Quantidade = 1,
                    PrecoItem = produto.Preco, // Usa o produto buscado
                    DataVenda = DateTime.Now,

                    // Dados auxiliares para exibição
                    NomeProduto = produto.NomeProduto,
                 //   CorEstoque = estoque.Cor,
                 //   QuantidadeDisponivel = estoque.Quantidade
                };

                return View("CriarVenda", vendaVm);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao carregar dados. Detalhes: {erro.Message}";
                return RedirectToAction("Index", "Estoque");
            }
        }

        [HttpPost]
        public IActionResult Criar(VendaVm vendaVm)
        {
            try
            {
                if(vendaVm.ValorAdicional == null)
                {
                    ModelState.Remove("ValorAdicional");
                }

                if (ModelState.IsValid)
                {
                    // Busca o estoque atual
                    var estoque = _estoqueRepositorio.ListarPorIId(vendaVm.EstoqueId);

                    if (estoque == null)
                    {
                        TempData["MensagemErro"] = "Estoque não encontrado.";
                        return RedirectToAction("Index", "Estoque");
                    }

                    // Verifica se há quantidade suficiente
                    if (estoque.Quantidade < vendaVm.Quantidade)
                    {
                        TempData["MensagemErro"] = $"Quantidade insuficiente em estoque. Disponível: {estoque.Quantidade}";
                        return RedirectToAction("Index", "Estoque");
                    }

                    // Converte ViewModel para Model
                    var venda = new VendaModel
                    {
                        ProdutoId = vendaVm.ProdutoId,
                        EstoqueId = vendaVm.EstoqueId,
                        Tamanho = (int)vendaVm.Tamanho,
                        Quantidade = vendaVm.Quantidade,
                        PrecoItem = vendaVm.PrecoItem,
                        FormaDePagamento = (int)vendaVm.FormaDePagamento,
                        FormaDePagamentoAdicional = (int?)vendaVm.FormaDePagamentoAdicional,
                        PossuiMaisDeUmaFormaPagamento =  vendaVm.PossuiMaisDeUmaFormaPagamento,
                        ValorAdicional = vendaVm.ValorAdicional,
                        DataVenda = DateTime.Now
                    };

                    // Adiciona a venda
                    _vendaRepositorio.Adicionar(venda);

                    // DIMINUI O ESTOQUE
                    estoque.Quantidade -= vendaVm.Quantidade;
                    _estoqueRepositorio.Editar(estoque);

                    TempData["MensagemSucesso"] = $"Venda realizada com sucesso! Estoque atualizado.";
                    return RedirectToAction("Index", "Estoque");
                }

                return View("CriarVenda", vendaVm);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao realizar venda. Detalhes: {erro.Message}";
                return RedirectToAction("Index", "Estoque");
            }
        }

        [HttpPost]
        public IActionResult Alterar(VendaVm vendaVm)
        {
            try
            {
                if (vendaVm.ValorAdicional == null)
                {
                    ModelState.Remove("ValorAdicional");
                }

                if (ModelState.IsValid)
                {
                    // Busca a venda existente
                    var vendaExistente = _vendaRepositorio.ListarPorIId(vendaVm.Id);
                    if (vendaExistente == null)
                    {
                        TempData["MensagemErro"] = "Venda não encontrada.";
                        return RedirectToAction("Index", "Venda");
                    }

                    // Busca o estoque atual
                    var estoque = _estoqueRepositorio.ListarPorIId(vendaVm.EstoqueId);
                    if (estoque == null)
                    {
                        TempData["MensagemErro"] = "Estoque não encontrado.";
                        return RedirectToAction("Index", "Estoque");
                    }

                    // Calcula diferença de quantidade
                    int diferenca = vendaVm.Quantidade - vendaExistente.Quantidade;

                    // Se aumentar quantidade, verificar se há estoque suficiente
                    if (diferenca > 0 && estoque.Quantidade < diferenca)
                    {
                        TempData["MensagemErro"] = $"Quantidade insuficiente em estoque. Disponível: {estoque.Quantidade}";
                        return RedirectToAction("Index", "Venda");
                    }

                    // Atualiza estoque
                    estoque.Quantidade -= diferenca;
                    _estoqueRepositorio.Editar(estoque);

                    // Atualiza venda
                    vendaExistente.ProdutoId = vendaVm.ProdutoId;
                    vendaExistente.EstoqueId = vendaVm.EstoqueId;
                    vendaExistente.Tamanho = (int)vendaVm.Tamanho;
                    vendaExistente.Quantidade = vendaVm.Quantidade;
                    vendaExistente.PrecoItem = vendaVm.PrecoItem;
                    vendaExistente.FormaDePagamento = (int)vendaVm.FormaDePagamento;
                    vendaExistente.FormaDePagamentoAdicional = (int?)vendaVm.FormaDePagamentoAdicional;
                    vendaExistente.PossuiMaisDeUmaFormaPagamento = vendaVm.PossuiMaisDeUmaFormaPagamento;
                    vendaExistente.ValorAdicional = vendaVm.ValorAdicional;

                    _vendaRepositorio.Editar(vendaExistente);

                    TempData["MensagemSucesso"] = "Venda atualizada com sucesso!";
                    return RedirectToAction("Index", "Venda");
                }

                return View("EditarVenda", vendaVm);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao editar venda. Detalhes: {erro.Message}";
                return RedirectToAction("Index", "Venda");
            }
        }

        public IActionResult Apagar(int id)
        {
            try
            {
                // 1. Buscar a venda
                var venda = _vendaRepositorio.ListarPorIId(id);
                if (venda == null)
                {
                    TempData["MensagemErro"] = "Venda não encontrada!";
                    return RedirectToAction("Index");
                }

                var estoque = _estoqueRepositorio.BuscarPorProdutoId(venda.ProdutoId);

                if (estoque == null)
                {
                    TempData["MensagemErro"] = $"Estoque do produto {venda.ProdutoId} não encontrado!";
                    return RedirectToAction("Index");
                }

                // 3. Devolver a quantidade
                estoque.Quantidade += venda.Quantidade;
                _estoqueRepositorio.Editar(estoque);

                // 4. Excluir a venda
                bool apagado = _vendaRepositorio.Excluir(id);

                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Venda apagada e estoque atualizado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não conseguimos apagar a venda!";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ops, ocorreu um erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

    }
}
