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
        private readonly IVendaItemRepositorio _vendaItemRepositorio;

        public VendaController(IEstoqueRepositorio estoqueRepositorio, IVendaRepositorio vendaRepositorio, IProdutoRepositorio produtoRepositorio, IVendaItemRepositorio vendaItemRepositorio)
        {
            _estoqueRepositorio = estoqueRepositorio;
            _vendaRepositorio = vendaRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _vendaItemRepositorio = vendaItemRepositorio;
        }


        public IActionResult Index()
        {
            var vendas = _vendaRepositorio.BuscarTodosComItens();

            var produtos = _produtoRepositorio.BuscarTodos()
                .ToDictionary(p => p.Id);

            var estoques = _estoqueRepositorio.BuscarTodos()
                .ToDictionary(e => e.Id);

            var vendasVm = vendas.SelectMany(v => v.VendaItens.Select(item =>
            {
                var produto = produtos[item.ProdutoId];
                var estoque = estoques[item.EstoqueId];

                return new VendaVm
                {
                    Id = v.Id,
                    DataVenda = v.DataVenda,
                    FormaDePagamento = v.FormaDePagamento,
                    PossuiMaisDeUmaFormaPagamento = v.PossuiMaisDeUmaFormaPagamento,

                    NomeProduto = produto.NomeProduto,
                    ImagemProduto = produto.Imagem,
                    ValorProduto = item.Venda.ValorTotal,
                    Quantidade = estoque.Quantidade
                };
            })).ToList();

            return View(vendasVm);
        }





        public IActionResult Editar(int id)
        {
            try
            {
                var venda = _vendaRepositorio.BuscarTodosComItens()
                    .FirstOrDefault(v => v.Id == id);

                if (venda == null)
                {
                    TempData["MensagemErro"] = "Venda não encontrada.";
                    return RedirectToAction("Index");
                }

                // Usar o método auxiliar
                CarregarEstoques();

                var vendaVm = new VendaVm
                {
                    Id = venda.Id,
                    DataVenda = venda.DataVenda,
                    FormaDePagamento = venda.FormaDePagamento,
                    FormaDePagamentoAdicional = venda.FormaDePagamentoAdicional,
                    PossuiMaisDeUmaFormaPagamento = venda.PossuiMaisDeUmaFormaPagamento,
                    ValorAdicional = venda.ValorAdicional,
                    ValorTotal = venda.ValorTotal,
                    DescontoGeral = venda.DescontoGeral, // Adicione se existir
                    Itens = venda.VendaItens.Select(i => new VendaItemVm
                    {
                        ProdutoId = i.ProdutoId,
                        EstoqueId = i.EstoqueId,
                        Quantidade = i.Quantidade,
                        PrecoUnitario = i.PrecoUnitario,
                        Tamanho = i.Tamanho,
                        NomeProduto = i.Produto?.NomeProduto,
                        DescontoPercentual = i.DescontoPercentual// Adicione para exibir na view
                    }).ToList()
                };

                return View("EditarVenda", vendaVm);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }


        public IActionResult ApagarConfirmacao(int id)
        {
            try
            {
                var venda = _vendaRepositorio.BuscarTodosComItens()
                    .FirstOrDefault(v => v.Id == id);

                if (venda == null)
                {
                    TempData["MensagemErro"] = "Venda não encontrada.";
                    return RedirectToAction("Index");
                }

                // Carrega os estoques para exibir informações dos produtos
                var estoques = _estoqueRepositorio.BuscarTodos()
                    .Where(e => e.Quantidade >= 0) // Mostra todos, mesmo sem estoque
                    .Select(e => new
                    {
                        EstoqueId = e.Id,
                        ProdutoId = e.ProdutoId,
                        e.Tamanho,
                        TamanhoNome = e.Tamanho.ToString(),
                        NomeProduto = e.Produto.NomeProduto,
                        Preco = e.Produto.Preco,
                        QuantidadeDisponivel = e.Quantidade,
                        ImagemUrl = e.Produto.Imagem
                    })
                    .ToList();

                ViewBag.Estoques = estoques;

                // Converte para ViewModel para manter consistência
                var vendaVm = new VendaVm
                {
                    Id = venda.Id,
                    DataVenda = venda.DataVenda,
                    FormaDePagamento = venda.FormaDePagamento,
                    FormaDePagamentoAdicional = venda.FormaDePagamentoAdicional,
                    PossuiMaisDeUmaFormaPagamento = venda.PossuiMaisDeUmaFormaPagamento,
                    ValorAdicional = venda.ValorAdicional,
                    ValorTotal = venda.ValorTotal,
                  //  DescontoPercentual = venda.DescontoPercentual,
                    Itens = venda.VendaItens.Select(i => new VendaItemVm
                    {
                        ProdutoId = i.ProdutoId,
                        EstoqueId = i.EstoqueId,
                        Quantidade = i.Quantidade,
                        PrecoUnitario = i.PrecoUnitario,
                        Tamanho = i.Tamanho,
                        NomeProduto = i.Produto?.NomeProduto
                    }).ToList()
                };

                return View(vendaVm);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao carregar venda: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public IActionResult Criar()
        {
            try
            {
                // Busca todos os estoques com quantidade > 0 e inclui o Produto relacionado
                var estoquesDisponiveis = _estoqueRepositorio.BuscarTodos()
                    .Where(e => e.Quantidade > 0)
                    .ToList();

                if (!estoquesDisponiveis.Any())
                {
                    TempData["MensagemAlerta"] = "Não há produtos com estoque disponível no momento.";
                    return RedirectToAction("Index", "Estoque");
                }

                // Cria estrutura simplificada para JS
                var estoquesParaView = estoquesDisponiveis.Select(e => new
                {
                    EstoqueId = e.Id,
                    ProdutoId = e.ProdutoId,
                    NomeProduto = e.Produto?.NomeProduto ?? "Produto sem nome",
                    Tamanho = (int)e.Tamanho,
                    TamanhoNome = ((TamanhoProduto)e.Tamanho).ToString(),
                    Preco = e.Produto?.Preco ?? 0,
                    QuantidadeDisponivel = e.Quantidade,
                    ImagemUrl = e.Produto?.Imagem
                }).ToList();

                ViewBag.Estoques = estoquesParaView;

                // Inicializa o ViewModel para evitar null
                var vendaVm = new VendaVm
                {
                    DataVenda = DateTime.Now,
                    Itens = new List<VendaItemVm>(),  // essencial!
                    DescontoGeral = 0
                };

                return View("CriarVenda", vendaVm);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao carregar tela de venda. Detalhes: {erro.Message}";
                return RedirectToAction("Index", "Home");
            }
        }




        [HttpPost]
        public IActionResult Criar(VendaVm vendaVm)
        {
            try
            {
                // Remove validações desnecessárias para itens
                if (vendaVm.Itens != null)
                {
                    for (int i = 0; i < vendaVm.Itens.Count; i++)
                    {
                        ModelState.Remove($"Itens[{i}].EstoqueId");
                        ModelState.Remove($"Itens[{i}].NomeProduto");
                        vendaVm.Itens[i].EstoqueId = 0; // ou null se for nullable
                    }
                }

                // Valida ModelState
                if (!ModelState.IsValid)
                {
                    CarregarProdutos();
                    return View("CriarVenda", vendaVm);
                }

                if (vendaVm.Itens == null || !vendaVm.Itens.Any())
                {
                    CarregarProdutos();
                    ModelState.AddModelError("", "Nenhum item foi adicionado à venda.");
                    return View("CriarVenda", vendaVm);
                }

                // 1️⃣ Valida estoque de cada item
                var estoquesPorItem = new Dictionary<string, EstoqueModel>();
                foreach (var itemVm in vendaVm.Itens)
                {
                    var chave = $"{itemVm.ProdutoId}_{(int)itemVm.Tamanho}";
                    if (estoquesPorItem.ContainsKey(chave))
                        continue;

                    var estoque = _estoqueRepositorio.BuscarPorProdutoETamanho(itemVm.ProdutoId, (int)itemVm.Tamanho);
                    if (estoque == null)
                    {
                        var produto = _produtoRepositorio.ListarPorIId(itemVm.ProdutoId);
                        throw new Exception($"Estoque não encontrado para: {produto?.NomeProduto ?? "Produto"} - Tamanho: {itemVm.Tamanho}");
                    }

                    var quantidadeTotal = vendaVm.Itens
                        .Where(i => i.ProdutoId == itemVm.ProdutoId && i.Tamanho == itemVm.Tamanho)
                        .Sum(i => i.Quantidade);

                    if (estoque.Quantidade < quantidadeTotal)
                    {
                        throw new Exception($"Estoque insuficiente para {estoque.Produto?.NomeProduto} (Tamanho: {itemVm.Tamanho}). Disponível: {estoque.Quantidade}, Solicitado: {quantidadeTotal}");
                    }

                    estoquesPorItem[chave] = estoque;
                }

                // 2️⃣ Cria a venda (sem ValorTotal inicial)
                var venda = new VendaModel
                {
                    DataVenda = DateTime.Now,
                    FormaDePagamento = (int)vendaVm.FormaDePagamento,
                    FormaDePagamentoAdicional = vendaVm.FormaDePagamentoAdicional,
                    PossuiMaisDeUmaFormaPagamento = vendaVm.PossuiMaisDeUmaFormaPagamento,
                    ValorAdicional = vendaVm.ValorAdicional
                };

                _vendaRepositorio.Adicionar(venda);

                decimal subtotal = 0;

                // 3️⃣ Processa os itens
                foreach (var itemVm in vendaVm.Itens)
                {
                    var chave = $"{itemVm.ProdutoId}_{(int)itemVm.Tamanho}";
                    var estoque = estoquesPorItem[chave];

                    // Calcula subtotal do item já aplicando desconto individual
                    decimal subtotalItem = itemVm.Quantidade * itemVm.PrecoUnitario;
                    decimal valorDescontoItem = subtotalItem * ((decimal)(itemVm.DescontoPercentual ?? 0) / 100m);
                    subtotalItem -= valorDescontoItem;

                    subtotal += subtotalItem;

                    var vendaItem = new VendaItemModel
                    {
                        VendaId = venda.Id,
                        ProdutoId = itemVm.ProdutoId,
                        EstoqueId = estoque.Id,
                        Tamanho = itemVm.Tamanho,
                        Quantidade = itemVm.Quantidade,
                        PrecoUnitario = itemVm.PrecoUnitario,
                        SubTotal = subtotalItem,
                        DescontoPercentual = itemVm.DescontoPercentual ?? 0 // ✅ desconto individual
                    };

                    _vendaItemRepositorio.Adicionar(vendaItem);

                    // Atualiza estoque
                    estoque.Quantidade -= itemVm.Quantidade;
                    _estoqueRepositorio.Editar(estoque);
                }

                // 4️⃣ Calcula total da venda aplicando desconto geral
                decimal valorDescontoGeral = subtotal * ((decimal)(vendaVm.DescontoGeral ?? 0) / 100m);
                decimal valorTotal = subtotal - valorDescontoGeral;

                venda.ValorTotal = valorTotal;
                venda.DescontoGeral = vendaVm.DescontoGeral ?? 0; // ✅ desconto global

                // Valida pagamento adicional
                if (vendaVm.PossuiMaisDeUmaFormaPagamento && (vendaVm.ValorAdicional > valorTotal || vendaVm.ValorAdicional < 0))
                {
                    throw new Exception("O valor adicional deve ser entre 0 e o total da venda.");
                }

                _vendaRepositorio.Editar(venda);

                TempData["MensagemSucesso"] = "Venda realizada com sucesso!";
                return RedirectToAction("Index", "Estoque");
            }
            catch (Exception erro)
            {
                CarregarProdutos();
                ModelState.AddModelError("", $"Erro ao processar venda: {erro.Message}");
                return View("CriarVenda", vendaVm);
            }
        }



        [HttpPost]
        public IActionResult Alterar(VendaVm vendaVm)
        {
            try
            {
                // Remove erros de validação do NomeProduto (campo não obrigatório)
                for (int i = 0; i < vendaVm.Itens?.Count; i++)
                {
                    ModelState.Remove($"Itens[{i}].NomeProduto");
                }

                if (!ModelState.IsValid)
                {
                    CarregarEstoques();
                    return View("EditarVenda", vendaVm);
                }

                if (vendaVm.Itens == null || !vendaVm.Itens.Any())
                {
                    TempData["MensagemErro"] = "A venda deve conter pelo menos um item.";
                    CarregarEstoques();
                    return View("EditarVenda", vendaVm);
                }

                var vendaExistente = _vendaRepositorio.BuscarTodosComItens()
                    .FirstOrDefault(v => v.Id == vendaVm.Id);

                if (vendaExistente == null)
                {
                    TempData["MensagemErro"] = "Venda não encontrada.";
                    return RedirectToAction("Index");
                }

                // ===== Devolver estoque dos itens antigos e removê-los =====
                var itensAntigos = vendaExistente.VendaItens.ToList();
                foreach (var itemAntigo in itensAntigos)
                {
                    var estoque = _estoqueRepositorio.ListarPorIId(itemAntigo.EstoqueId);
                    if (estoque != null)
                    {
                        estoque.Quantidade += itemAntigo.Quantidade;
                        _estoqueRepositorio.Editar(estoque);
                    }

                    _vendaItemRepositorio.Excluir(itemAntigo.Id);
                }

                // ===== Validar disponibilidade de estoque =====
                foreach (var itemVm in vendaVm.Itens)
                {
                    var estoque = _estoqueRepositorio.ListarPorIId(itemVm.EstoqueId);

                    if (estoque == null)
                    {
                        TempData["MensagemErro"] = "Estoque não encontrado para o item.";
                        foreach (var itemAntigo in itensAntigos)
                        {
                            var estoqueReverter = _estoqueRepositorio.ListarPorIId(itemAntigo.EstoqueId);
                            if (estoqueReverter != null)
                            {
                                estoqueReverter.Quantidade -= itemAntigo.Quantidade;
                                _estoqueRepositorio.Editar(estoqueReverter);
                            }
                        }
                        CarregarEstoques();
                        return View("EditarVenda", vendaVm);
                    }

                    if (estoque.Quantidade < itemVm.Quantidade)
                    {
                        TempData["MensagemErro"] = $"Quantidade insuficiente em estoque para o produto '{estoque.Produto?.NomeProduto}'. Disponível: {estoque.Quantidade}";
                        foreach (var itemAntigo in itensAntigos)
                        {
                            var estoqueReverter = _estoqueRepositorio.ListarPorIId(itemAntigo.EstoqueId);
                            if (estoqueReverter != null)
                            {
                                estoqueReverter.Quantidade -= itemAntigo.Quantidade;
                                _estoqueRepositorio.Editar(estoqueReverter);
                            }
                        }
                        CarregarEstoques();
                        return View("EditarVenda", vendaVm);
                    }
                }

                // ===== Baixar estoque e criar novos itens com desconto =====
                foreach (var itemVm in vendaVm.Itens)
                {
                    var estoque = _estoqueRepositorio.ListarPorIId(itemVm.EstoqueId);

                    // Baixa do estoque
                    estoque.Quantidade -= itemVm.Quantidade;
                    _estoqueRepositorio.Editar(estoque);

                    // Cria novo item com DescontoPercentual
                    var novoItem = new VendaItemModel
                    {
                        VendaId = vendaExistente.Id,
                        ProdutoId = itemVm.ProdutoId,
                        EstoqueId = itemVm.EstoqueId,
                        Quantidade = itemVm.Quantidade,
                        PrecoUnitario = itemVm.PrecoUnitario,
                        Tamanho = itemVm.Tamanho,
                        DescontoPercentual = itemVm.DescontoPercentual // ✅ Salva desconto individual
                    };

                    _vendaItemRepositorio.Adicionar(novoItem);
                }

                // ===== Atualizar dados da venda =====
                vendaExistente.DataVenda = DateTime.Now;
                vendaExistente.FormaDePagamento = vendaVm.FormaDePagamento;
                vendaExistente.PossuiMaisDeUmaFormaPagamento = vendaVm.PossuiMaisDeUmaFormaPagamento;
                vendaExistente.FormaDePagamentoAdicional = vendaVm.FormaDePagamentoAdicional;
                vendaExistente.ValorAdicional = vendaVm.ValorAdicional;
                vendaExistente.ValorTotal = vendaVm.ValorTotal;
                vendaExistente.DescontoGeral = vendaVm.DescontoGeral; // ✅ Salva desconto global

                _vendaRepositorio.Editar(vendaExistente);

                TempData["MensagemSucesso"] = "Venda atualizada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao atualizar venda: {ex.Message}";
                CarregarEstoques();
                return View("EditarVenda", vendaVm);
            }
        }


        [HttpPost]
        public IActionResult Apagar(int id)
        {
            try
            {
                // 1. Buscar a venda com seus itens
                var venda = _vendaRepositorio.BuscarTodosComItens()
                    .FirstOrDefault(v => v.Id == id);

                if (venda == null)
                {
                    TempData["MensagemErro"] = "Venda não encontrada!";
                    return RedirectToAction("Index");
                }

                // 2. Devolver o estoque de cada item da venda
                foreach (var item in venda.VendaItens.ToList())
                {
                    var estoque = _estoqueRepositorio.ListarPorIId(item.EstoqueId);

                    if (estoque != null)
                    {
                        // Devolve a quantidade ao estoque
                        estoque.Quantidade += item.Quantidade;
                        _estoqueRepositorio.Editar(estoque);
                    }
                    else
                    {
                        TempData["MensagemAlerta"] = $"Estoque não encontrado para o item {item.ProdutoId}. A exclusão continuará.";
                    }
                }

                // 3. Excluir os itens da venda primeiro
                foreach (var item in venda.VendaItens.ToList())
                {
                    _vendaItemRepositorio.Excluir(item.Id);
                }

                // 4. Excluir a venda
                bool apagado = _vendaRepositorio.Excluir(id);

                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Venda excluída e estoque devolvido com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Não foi possível excluir a venda!";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao excluir venda: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        private void CarregarEstoques()
        {
            var estoques = _estoqueRepositorio.BuscarTodos()
                .Where(e => e.Quantidade > 0)
                .Select(e => new
                {
                    EstoqueId = e.Id,
                    ProdutoId = e.ProdutoId,
                    e.Tamanho,
                    TamanhoNome = e.Tamanho.ToString(),
                    NomeProduto = e.Produto.NomeProduto,
                    Preco = e.Produto.Preco,
                    QuantidadeDisponivel = e.Quantidade,
                    ImagemUrl = e.Produto.Imagem
                })
                .ToList();

            ViewBag.Estoques = estoques;
        }

        private void CarregarProdutos()
        {
            try
            {
                // ✅ Busca todos os estoques com quantidade > 0
                var estoquesDisponiveis = _estoqueRepositorio.BuscarTodos()
                    .Where(e => e.Quantidade > 0)
                    .ToList();

                // ✅ Cria uma estrutura otimizada para a view
                var estoquesParaView = estoquesDisponiveis.Select(e => new
                {
                    EstoqueId = e.Id,
                    ProdutoId = e.ProdutoId,
                    NomeProduto = e.Produto?.NomeProduto ?? "Produto sem nome",
                    Tamanho = (int)e.Tamanho,
                    TamanhoNome = ((TamanhoProduto)e.Tamanho).ToString(),
                    Preco = e.Produto?.Preco ?? 0,
                    QuantidadeDisponivel = e.Quantidade,
                    ImagemUrl = e.Produto?.Imagem
                }).ToList();

                ViewBag.Estoques = estoquesParaView;
            }
            catch (Exception ex)
            {
                // Em caso de erro ao carregar, inicializa com lista vazia para evitar null
                ViewBag.Estoques = new List<object>();
                TempData["MensagemAlerta"] = $"Erro ao carregar produtos: {ex.Message}";
            }
        }

    }
}
