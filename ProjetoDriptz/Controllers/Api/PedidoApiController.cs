using Microsoft.AspNetCore.Mvc;
using ProjetoDriptz.Enums;
using ProjetoDriptz.Models;
using ProjetoDriptz.Models.ViewModels;
using ProjetoDriptz.Repositorio;
using ProjetoDriptz.Repositorio.Interfaces;
using System.Text.Json;

namespace ProjetoDriptz.Controllers.Api
{
    // API JSON consumida pelo Angular (loja/cardápio do cliente).
    // Convive com o PedidoController (Razor) já existente, sem substituí-lo.
    [ApiController]
    [Route("api/pedido")]
    public class PedidoApiController : ControllerBase
    {
        private const string ChaveCarrinho = "carrinho_cliente";

        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public PedidoApiController(IProdutoRepositorio produtoRepositorio,
                                    IPedidoRepositorio pedidoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
            _pedidoRepositorio = pedidoRepositorio;
        }

        // GET api/pedido/cardapio?tipo=1
        [HttpGet("cardapio")]
        public IActionResult Cardapio(int? tipo)
        {
            var produtos = _produtoRepositorio.BuscarTodos();

            if (tipo.HasValue)
                produtos = produtos.Where(p => p.Tipo == tipo.Value).ToList();

            var categorias = Enum.GetValues<TipoProduto>()
                .Select(t => new { value = (int)t, nome = t.ToString() })
                .ToList();

            return Ok(new
            {
                produtos = produtos.Select(p => new
                {
                    p.Id,
                    p.NomeProduto,
                    p.Imagem,
                    p.Preco,
                    p.Tipo
                }),
                categorias,
                tipoSelecionado = tipo,
                qtdCarrinho = ObterCarrinho().Sum(i => i.Quantidade)
            });
        }

        // GET api/pedido/carrinho
        [HttpGet("carrinho")]
        public IActionResult Carrinho()
        {
            return Ok(ObterCarrinho());
        }

        // POST api/pedido/carrinho/adicionar
        [HttpPost("carrinho/adicionar")]
        public IActionResult AdicionarAoCarrinho([FromBody] AdicionarItemDto dto)
        {
            try
            {
                var quantidade = dto.Quantidade <= 0 ? 1 : dto.Quantidade;

                var produto = _produtoRepositorio.ListarPorIId(dto.ProdutoId);
                if (produto == null)
                    return NotFound(new { sucesso = false, mensagem = "Produto não encontrado." });

                var carrinho = ObterCarrinho();

                var itemExistente = carrinho.FirstOrDefault(i => i.ProdutoId == dto.ProdutoId);
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
                        Observacao = dto.Observacao
                    });
                }

                SalvarCarrinho(carrinho);

                return Ok(new
                {
                    sucesso = true,
                    qtdCarrinho = carrinho.Sum(i => i.Quantidade),
                    carrinho
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { sucesso = false, mensagem = ex.Message });
            }
        }

        // PUT api/pedido/carrinho/{produtoId}
        [HttpPut("carrinho/{produtoId:int}")]
        public IActionResult AtualizarQuantidade(int produtoId, [FromBody] AtualizarQuantidadeDto dto)
        {
            var carrinho = ObterCarrinho();
            var item = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);

            if (item != null)
            {
                if (dto.Quantidade <= 0)
                    carrinho.Remove(item);
                else
                    item.Quantidade = dto.Quantidade;
            }

            SalvarCarrinho(carrinho);
            return Ok(carrinho);
        }

        // DELETE api/pedido/carrinho/{produtoId}
        [HttpDelete("carrinho/{produtoId:int}")]
        public IActionResult RemoverDoCarrinho(int produtoId)
        {
            var carrinho = ObterCarrinho();
            carrinho.RemoveAll(i => i.ProdutoId == produtoId);
            SalvarCarrinho(carrinho);
            return Ok(carrinho);
        }

        // POST api/pedido/checkout
        [HttpPost("checkout")]
        public IActionResult Checkout([FromBody] CheckoutVm vm)
        {
            if (vm.TipoEntrega == TipoEntrega.Delivery && string.IsNullOrWhiteSpace(vm.Endereco))
                ModelState.AddModelError("Endereco", "Informe o endereço para entrega.");

            var carrinho = ObterCarrinho();

            if (!carrinho.Any())
                return BadRequest(new { sucesso = false, mensagem = "Seu carrinho está vazio." });

            vm.Itens = carrinho;

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

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

                return Ok(new { sucesso = true, pedidoId = pedido.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { sucesso = false, mensagem = $"Erro ao registrar pedido: {ex.Message}" });
            }
        }

        // GET api/pedido/confirmacao/{id}
        [HttpGet("confirmacao/{id:int}")]
        public IActionResult Confirmacao(int id)
        {
            var pedido = _pedidoRepositorio.BuscarPorId(id);
            if (pedido == null) return NotFound();

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

            return Ok(vm);
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

    public class AdicionarItemDto
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public string? Observacao { get; set; }
    }

    public class AtualizarQuantidadeDto
    {
        public int Quantidade { get; set; }
    }
}