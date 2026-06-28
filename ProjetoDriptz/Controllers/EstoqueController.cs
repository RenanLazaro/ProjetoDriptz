using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoDriptz.Enums;
using ProjetoDriptz.Models;
using ProjetoDriptz.Models.ViewModels;
using ProjetoDriptz.Repositorio.Interfaces;

namespace ProjetoDriptz.Controllers
{
    [PaginaParaAdminLogado]
    public class EstoqueController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IEstoqueRepositorio _estoqueRepositorio;
        public EstoqueController(IProdutoRepositorio produtoRepositorio, IEstoqueRepositorio estoqueRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
            _estoqueRepositorio = estoqueRepositorio;
        }

        public IActionResult Index()
        {
            var estoques = _estoqueRepositorio.BuscarTodos();
            return View(estoques);
        }

        public IActionResult Editar(int id)
        {
            try
            {
                EstoqueModel estoque = _estoqueRepositorio.ListarPorIId(id);

                if (estoque == null)
                {
                    TempData["MensagemErro"] = "Estoque não encontrado!";
                    return RedirectToAction("Index");
                }

                // Converte Model para ViewModel
                var estoqueVm = new EstoqueVm
                {
                    Id = estoque.Id,
                    ProdutoId = estoque.ProdutoId,
                    Tamanho = (TamanhoProduto)estoque.Tamanho, 
                    Cor = estoque.Cor,
                    Quantidade = estoque.Quantidade
                };

                // Carrega lista de produtos
                ViewData["Produtos"] = _produtoRepositorio.BuscarTodos()
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.NomeProduto
                    }).ToList();

                return View(estoqueVm);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao carregar estoque. Detalhes: {erro.Message}";
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
            EstoqueModel estoque = _estoqueRepositorio.ListarPorIId(id);
            return View(estoque);
        }

        public IActionResult Criar()
        {
            ViewBag.Produtos = _produtoRepositorio.BuscarTodos()
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.NomeProduto
                }).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Criar(EstoqueVm estoqueVm)
        {
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    // Converte o ViewModel para o Model
                    var estoque = new EstoqueModel
                    {
                        ProdutoId = estoqueVm.ProdutoId,
                        Tamanho = (int)estoqueVm.Tamanho, // ou (int)estoqueVm.Tamanho se for int no Model
                        Cor = estoqueVm.Cor,
                        Quantidade = estoqueVm.Quantidade
                    };

                    _estoqueRepositorio.Adicionar(estoque);
                    TempData["MensagemSucesso"] = "Estoque cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }

                // Recarrega a lista de produtos em caso de erro de validação
                ViewData["Produtos"] = _produtoRepositorio.BuscarTodos()
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.NomeProduto
                    }).ToList();

                return View(estoqueVm);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar o estoque. Detalhes: {erro.Message}";

                // Recarrega a lista de produtos
                ViewData["Produtos"] = _produtoRepositorio.BuscarTodos()
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.NomeProduto
                    }).ToList();

                return View(estoqueVm);
            }
        }

        [HttpPost]
        public IActionResult Alterar(EstoqueVm estoqueVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Converte ViewModel para Model
                    var estoque = new EstoqueModel
                    {
                        Id = estoqueVm.Id,
                        ProdutoId = estoqueVm.ProdutoId,
                        Tamanho = (int)estoqueVm.Tamanho, 
                        Cor = estoqueVm.Cor,
                        Quantidade = estoqueVm.Quantidade
                    };

                    _estoqueRepositorio.Editar(estoque);
                    TempData["MensagemSucesso"] = "Estoque alterado com sucesso!";
                    return RedirectToAction("Index");
                }

                // Recarrega lista de produtos em caso de erro
                ViewData["Produtos"] = _produtoRepositorio.BuscarTodos()
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.NomeProduto
                    }).ToList();

                return View("Editar", estoqueVm);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos atualizar o estoque, tente novamente. Detalhes do erro: {erro.Message}";

                ViewData["Produtos"] = _produtoRepositorio.BuscarTodos()
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.NomeProduto
                    }).ToList();

                return View("Editar", estoqueVm);
            }
        }

        public IActionResult Apagar(int id)
        {
            try
            {

                bool apagado = _estoqueRepositorio.Excluir(id);

                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Estoque Apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não conseguimos apagar seu estoque!";
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                TempData["MensagemErro"] = "Ops, não conseguimos apagar seu estoque!";
                return RedirectToAction("Index");
            }

        }

    }
}
