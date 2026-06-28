using Microsoft.AspNetCore.Mvc;
using ProjetoDriptz.Enums;
using ProjetoDriptz.Models;
using ProjetoDriptz.Models.ViewModels;
using ProjetoDriptz.Repositorio;
using ProjetoDriptz.Repositorio.Interfaces;
using System.Text.Json;

namespace ProjetoDriptz.Controllers
{
    // Área pública — sem [PaginaParaAdminLogado]
    public class PedidoController : Controller
    {
        private const string ChaveCarrinho = "carrinho_cliente";

        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public PedidoController(IProdutoRepositorio produtoRepositorio,
                                 IPedidoRepositorio pedidoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
            _pedidoRepositorio = pedidoRepositorio;
        }

        // ---------------------------------------------------------------
        // CARDÁPIO
        // ---------------------------------------------------------------
        public IActionResult Cardapio(int? tipo)
        {
            var produtos = _produtoRepositorio.BuscarTodos();

            if (tipo.HasValue)
                produtos = produtos.Where(p => p.Tipo == tipo.Value).ToList();

            ViewBag.TipoSelecionado = tipo;
            ViewBag.Categorias = Enum.GetValues<TipoProduto>()
                .Select(t => new { Value = (int)t, Nome = t.ToString() })
                .ToList();

            ViewBag.QtdCarrinho = ObterCarrinho().Sum(i => i.Quantidade);

            return View(produtos);
        }

        // ---------------------------------------------------------------
        // CARRINHO — visualizar
        // ---------------------------------------------------------------
        public IActionResult Carrinho()
        {
            var itens = ObterCarrinho();
            return View(itens);
        }

        // ---------------------------------------------------------------
        // CARRINHO — adicionar item
        // ---------------------------------------------------------------
        [HttpPost]
        public IActionResult AdicionarAoCarrinho(int produtoId, int quantidade, string? observacao)
        {
            try
            {
                if (quantidade <= 0) quantidade = 1;

                var produto = _produtoRepositorio.ListarPorIId(produtoId);
                if (produto == null)
                    return Json(new { sucesso = false, mensagem = "Produto não encontrado." });

                var carrinho = ObterCarrinho();

                var itemExistente = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);
                if (itemExistente != null)
                {
                    itemExistente.Quantidade += quantidade;
                }
                else
                {
                    carrinho.Add(new CarrinhoItemVm
                    {
                        ProdutoId = produto.Id,
                        NomeProduto = produto.NomeProduto,
                        Imagem = produto.Imagem,
                        PrecoUnitario = produto.Preco,
                        Quantidade = quantidade,
                        Observacao = observacao
                    });
                }

                SalvarCarrinho(carrinho);

                return Json(new
                {
                    sucesso = true,
                    qtdCarrinho = carrinho.Sum(i => i.Quantidade),
                    mensagem = "Item adicionado ao carrinho!"
                });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = ex.Message });
            }
        }

        // ---------------------------------------------------------------
        // CARRINHO — remover item
        // ---------------------------------------------------------------
        [HttpPost]
        public IActionResult RemoverDoCarrinho(int produtoId)
        {
            var carrinho = ObterCarrinho();
            carrinho.RemoveAll(i => i.ProdutoId == produtoId);
            SalvarCarrinho(carrinho);
            return RedirectToAction("Carrinho");
        }

        // ---------------------------------------------------------------
        // CARRINHO — atualizar quantidade
        // ---------------------------------------------------------------
        [HttpPost]
        public IActionResult AtualizarQuantidade(int produtoId, int quantidade)
        {
            var carrinho = ObterCarrinho();
            var item = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);

            if (item != null)
            {
                if (quantidade <= 0)
                    carrinho.Remove(item);
                else
                    item.Quantidade = quantidade;
            }

            SalvarCarrinho(carrinho);
            return RedirectToAction("Carrinho");
        }

        // ---------------------------------------------------------------
        // CHECKOUT — formulário
        // ---------------------------------------------------------------
        [HttpGet]
        public IActionResult Checkout()
        {
            var carrinho = ObterCarrinho();

            if (!carrinho.Any())
            {
                TempData["MensagemAlerta"] = "Seu carrinho está vazio.";
                return RedirectToAction("Cardapio");
            }

            var vm = new CheckoutVm { Itens = carrinho };
            return View(vm);
        }

        // ---------------------------------------------------------------
        // CHECKOUT — confirmar pedido
        // ---------------------------------------------------------------
        [HttpPost]
        public IActionResult Checkout(CheckoutVm vm)
        {
            // Validação de endereço obrigatório para delivery
            if (vm.TipoEntrega == TipoEntrega.Delivery && string.IsNullOrWhiteSpace(vm.Endereco))
                ModelState.AddModelError("Endereco", "Informe o endereço para entrega.");

            var carrinho = ObterCarrinho();

            if (!carrinho.Any())
            {
                TempData["MensagemAlerta"] = "Seu carrinho está vazio.";
                return RedirectToAction("Cardapio");
            }

            vm.Itens = carrinho;

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var pedido = new PedidoModel
                {
                    NomeCliente = vm.NomeCliente,
                    Telefone = vm.Telefone,
                    TipoEntrega = vm.TipoEntrega,
                    Endereco = vm.Endereco,
                    Complemento = vm.Complemento,
                    Bairro = vm.Bairro,
                    FormaDePagamento = vm.FormaDePagamento,
                    TrocoParа = vm.TrocoPara,
                    Observacao = vm.Observacao,
                    ValorTotal = vm.ValorTotal,
                    DataPedido = DateTime.Now,
                    Status = StatusPedido.Aguardando,
                    Itens = carrinho.Select(i => new PedidoItemModel
                    {
                        ProdutoId = i.ProdutoId,
                        Quantidade = i.Quantidade,
                        PrecoUnitario = i.PrecoUnitario,
                        Observacao = i.Observacao
                    }).ToList()
                };

                _pedidoRepositorio.Adicionar(pedido);

                // Limpa o carrinho
                SalvarCarrinho(new List<CarrinhoItemVm>());

                return RedirectToAction("Confirmacao", new { id = pedido.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao registrar pedido: {ex.Message}");
                return View(vm);
            }
        }

        // ---------------------------------------------------------------
        // CONFIRMAÇÃO
        // ---------------------------------------------------------------
        public IActionResult Confirmacao(int id)
        {
            var pedido = _pedidoRepositorio.BuscarPorId(id);
            if (pedido == null) return RedirectToAction("Cardapio");

            var vm = new PedidoConfirmadoVm
            {
                Id = pedido.Id,
                NomeCliente = pedido.NomeCliente,
                Telefone = pedido.Telefone,
                TipoEntrega = pedido.TipoEntrega,
                Endereco = pedido.Endereco,
                Bairro = pedido.Bairro,
                FormaDePagamento = pedido.FormaDePagamento,
                ValorTotal = pedido.ValorTotal,
                Status = pedido.Status,
                DataPedido = pedido.DataPedido,
                Itens = pedido.Itens.Select(i => new CarrinhoItemVm
                {
                    ProdutoId = i.ProdutoId,
                    NomeProduto = i.Produto?.NomeProduto ?? "",
                    Imagem = i.Produto?.Imagem,
                    PrecoUnitario = i.PrecoUnitario,
                    Quantidade = i.Quantidade,
                    Observacao = i.Observacao
                }).ToList()
            };

            return View(vm);
        }

        // ---------------------------------------------------------------
        // HELPERS
        // ---------------------------------------------------------------
        private List<CarrinhoItemVm> ObterCarrinho()
        {
            var json = HttpContext.Session.GetString(ChaveCarrinho);
            if (string.IsNullOrEmpty(json)) return new List<CarrinhoItemVm>();
            return JsonSerializer.Deserialize<List<CarrinhoItemVm>>(json) ?? new();
        }

        private void SalvarCarrinho(List<CarrinhoItemVm> carrinho)
        {
            HttpContext.Session.SetString(ChaveCarrinho, JsonSerializer.Serialize(carrinho));
        }
    }
}