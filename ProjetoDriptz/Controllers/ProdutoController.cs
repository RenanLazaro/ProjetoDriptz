using Microsoft.AspNetCore.Mvc;
using ProjetoDriptz.Enums;
using ProjetoDriptz.Filters;
using ProjetoDriptz.Models;
using ProjetoDriptz.Models.ViewModels;
using ProjetoDriptz.Repositorio;

namespace ProjetoDriptz.Controllers
{
//    [PaginaParaUsuarioLogado]

    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        public ProdutoController(IProdutoRepositorio produtoRepositorio)
        {
               _produtoRepositorio = produtoRepositorio;
        }

        public IActionResult Index()
        {
          var produtos =   _produtoRepositorio.BuscarTodos();
            return View(produtos);
        }
        public IActionResult Criar()
        {
            return View();
        }

        public IActionResult Editar(int id)
        {
            try
            {
                ProdutoModel produto = _produtoRepositorio.ListarPorIId(id);

                if (produto == null)
                {
                    TempData["MensagemErro"] = "Produto não encontrado!";
                    return RedirectToAction("Index");
                }

                // Converte Model para ViewModel
                var produtoVm = new ProdutoVm
                {
                    Id = produto.Id,
                    NomeProduto = produto.NomeProduto,
                    Tipo = (TipoProduto)produto.Tipo, // <- Cast explícito
                    Tamanho = (TamanhoProduto)produto.Tamanho, // <- Cast explícito
                    Preco = produto.Preco
                };

                return View(produtoVm);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao carregar produto. Detalhes: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
        public IActionResult ApagarConfirmacao(int id)
        {
            ProdutoModel produto = _produtoRepositorio.ListarPorIId(id);
            return View(produto);
        }

     


        public IActionResult Apagar(int id)
        {
            try
            {

              bool apagado =   _produtoRepositorio.Excluir(id);

                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Produto Apagado com sucesso!";
                }else
                {
                    TempData["MensagemErro"] = "Ops, não conseguimos apagar seu produto!";
                }
               
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                TempData["MensagemErro"] = "Ops, não conseguimos apagar seu produto!";
                return RedirectToAction("Index");   
            }

        }

        [HttpPost]
        public IActionResult Criar(ProdutoVm produtoVm)
        {
            try
            {
                ModelState.Remove("Id");
                if (ModelState.IsValid)
                {
                    // Converte o ViewModel para o Model
                    var produto = new ProdutoModel
                    {
                        NomeProduto = produtoVm.NomeProduto,
                        Tipo = (int)produtoVm.Tipo,
                        Tamanho = (int)produtoVm.Tamanho,
                        Preco = produtoVm.Preco
                    };
                    _produtoRepositorio.Adicionar(produto);
                    TempData["MensagemSucesso"] = "Produto cadastrado com sucesso!";
                    return RedirectToAction("Index");

                }

                return View(produtoVm);

            }
            catch (Exception erro)
            {


                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar seu produto, tente novamente. Detalhes do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }



        [HttpPost]
        public IActionResult Alterar(ProdutoVm produtoVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Converte ViewModel para Model
                    var produto = new ProdutoModel
                    {
                        Id = produtoVm.Id,
                        NomeProduto = produtoVm.NomeProduto,
                        Tipo = (int)produtoVm.Tipo,
                        Tamanho = (int)produtoVm.Tamanho,
                        Preco = produtoVm.Preco
                    };

                    _produtoRepositorio.Editar(produto);
                    TempData["MensagemSucesso"] = "Produto alterado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View("Editar", produtoVm);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos atualizar seu produto, tente novamente. Detalhes do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }



    }
}
