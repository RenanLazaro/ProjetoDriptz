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

                // Converte Model → ViewModel
                var produtoVm = new ProdutoVm
                {
                    Id = produto.Id,
                    NomeProduto = produto.NomeProduto,
                    Tipo = (TipoProduto)produto.Tipo,
                    PrecoCusto = produto.PrecoCusto,
                    Preco = produto.Preco,

                    Imagem = produto.Imagem
                };

                return View(produtoVm);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] =
                    $"Erro ao carregar produto. Detalhes: {erro.Message}";
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
                var produto = _produtoRepositorio.ListarPorIId(id);

                if (produto == null)
                {
                    TempData["MensagemErro"] = "Produto não encontrado!";
                    return RedirectToAction("Index");
                }

                if (!string.IsNullOrEmpty(produto.Imagem))
                {
                    var caminhoImagem = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "imagens",
                        "produtos",
                        produto.Imagem
                    );

                    if (System.IO.File.Exists(caminhoImagem))
                    {
                        System.IO.File.Delete(caminhoImagem);
                    }
                }

                // 3️⃣ Remove o registro do banco
                bool apagado = _produtoRepositorio.Excluir(id);

                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Produto apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não conseguimos apagar o produto!";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] =
                    $"Erro ao apagar produto. Detalhes: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public IActionResult Criar([FromForm] ProdutoVm produtoVm)
        {
            try
            {
                var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(),
    "wwwroot", "imagens", "produtos");
                // Verificar se o arquivo foi enviado
                if (produtoVm.ImagemUpload == null || produtoVm.ImagemUpload.Length == 0)
                {
                    ModelState.AddModelError("ImagemUpload", "Selecione uma imagem.");
                    return View(produtoVm);
                }

                // Verificar a extensão do arquivo
                var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extensao = Path.GetExtension(produtoVm.ImagemUpload.FileName).ToLower();

                if (!extensoesPermitidas.Contains(extensao))
                {
                    ModelState.AddModelError("ImagemUpload", "Formato de imagem inválido.");
                    return View(produtoVm);
                }

                // Verificar o tamanho da imagem (máximo de 5MB)
                if (produtoVm.ImagemUpload.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImagemUpload", "Imagem maior que 5MB.");
                    return View(produtoVm);
                }

                // Gerar um nome único para a imagem
                var nomeImagem = $"{Guid.NewGuid()}{extensao}";

                // Caminho completo onde a imagem será salva
                var caminhoCompleto = Path.Combine(caminhoPasta, nomeImagem);

                // Salvar a imagem no diretório
                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    produtoVm.ImagemUpload.CopyTo(stream);
                }

                // Continuar o fluxo normal
                var produto = new ProdutoModel
                {
                    NomeProduto = produtoVm.NomeProduto,
                    Tipo = (int)produtoVm.Tipo,
                    PrecoCusto = produtoVm.PrecoCusto, 
                     Preco = produtoVm.Preco,
                    Imagem = nomeImagem // Aqui você salva o nome da imagem
                };

                _produtoRepositorio.Adicionar(produto);

                TempData["MensagemSucesso"] = "Produto cadastrado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Logar o erro
                Console.WriteLine($"Erro ao salvar a imagem: {ex.Message}");
                TempData["MensagemErro"] = $"Ops, houve um erro ao salvar a imagem. Detalhes: {ex.Message}";
                return View(produtoVm);
            }

        }




        [HttpPost]
        public IActionResult Alterar([FromForm] ProdutoVm produtoVm)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(produtoVm);

                var produto = _produtoRepositorio.ListarPorIId(produtoVm.Id);
                if (produto == null)
                    return NotFound();

                produto.NomeProduto = produtoVm.NomeProduto;
                produto.Tipo = (int)produtoVm.Tipo;
                produto.PrecoCusto = produtoVm.PrecoCusto;
                produto.Preco = produtoVm.Preco;

                // 🖼️ Troca de imagem (se houver)
                if (produtoVm.ImagemUpload != null && produtoVm.ImagemUpload.Length > 0)
                {
                    var extensao = Path.GetExtension(produtoVm.ImagemUpload.FileName);
                    var nomeImagem = $"{Guid.NewGuid()}{extensao}";

                    var pasta = Path.Combine(Directory.GetCurrentDirectory(),
                                             "wwwroot", "imagens", "produtos");

                    var caminhoNova = Path.Combine(pasta, nomeImagem);

                    using (var stream = new FileStream(caminhoNova, FileMode.Create))
                    {
                        produtoVm.ImagemUpload.CopyTo(stream);
                    }

                    // 🧹 Remove imagem antiga
                    if (!string.IsNullOrEmpty(produto.Imagem))
                    {
                        var caminhoAntigo = Path.Combine(pasta, produto.Imagem);
                        if (System.IO.File.Exists(caminhoAntigo))
                            System.IO.File.Delete(caminhoAntigo);
                    }

                    produto.Imagem = nomeImagem;
                }

                _produtoRepositorio.Editar(produto);

                TempData["MensagemSucesso"] = "Produto alterado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao alterar produto: {ex.Message}";
                return RedirectToAction("Index");
            }
        }




    }
}
