using Microsoft.AspNetCore.Mvc;
using ProjetoDriptz.Models;
using ProjetoDriptz.Repositorio;
using ProjetoDriptz.Repositorio.Interfaces;

namespace ProjetoDriptz.Controllers
{
    [PaginaParaAdminLogado]
    public class PromocaoController : Controller
    {
        private readonly IPromocaoRepositorio _promocaoRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public PromocaoController(IPromocaoRepositorio promocaoRepositorio,
                                   IProdutoRepositorio produtoRepositorio)
        {
            _promocaoRepositorio = promocaoRepositorio;
            _produtoRepositorio = produtoRepositorio;
        }

        // ── Lista todas ──────────────────────────────────────────────
        public IActionResult Index()
        {
            var promocoes = _promocaoRepositorio.BuscarTodas();
            return View(promocoes);
        }

        // ── Criar ────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Criar()
        {
            ViewBag.Produtos = _produtoRepositorio.BuscarTodos();
            return View(new PromocaoModel());
        }

        [HttpPost]
        public IActionResult Criar(PromocaoModel promocao)
        {
            ValidarDesconto(promocao);

            if (!ModelState.IsValid)
            {
                ViewBag.Produtos = _produtoRepositorio.BuscarTodos();
                return View(promocao);
            }

            try
            {
                _promocaoRepositorio.Adicionar(promocao);
                TempData["MensagemSucesso"] = "Promoção criada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Produtos = _produtoRepositorio.BuscarTodos();
                return View(promocao);
            }
        }

        // ── Editar ───────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var promocao = _promocaoRepositorio.BuscarPorId(id);
            if (promocao == null) return NotFound();

            ViewBag.Produtos = _produtoRepositorio.BuscarTodos();
            return View(promocao);
        }

        [HttpPost]
        public IActionResult Editar(PromocaoModel promocao)
        {
            ValidarDesconto(promocao);

            if (!ModelState.IsValid)
            {
                ViewBag.Produtos = _produtoRepositorio.BuscarTodos();
                return View(promocao);
            }

            try
            {
                _promocaoRepositorio.Editar(promocao);
                TempData["MensagemSucesso"] = "Promoção atualizada!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Produtos = _produtoRepositorio.BuscarTodos();
                return View(promocao);
            }
        }

        // ── Excluir ──────────────────────────────────────────────────
        [HttpPost]
        public IActionResult Excluir(int id)
        {
            try
            {
                _promocaoRepositorio.Excluir(id);
                TempData["MensagemSucesso"] = "Promoção removida.";
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        // ── Helper ───────────────────────────────────────────────────
        private void ValidarDesconto(PromocaoModel p)
        {
            if (p.TipoDesconto == Enums.TipoDesconto.Percentual && !p.DescontoPercentual.HasValue)
                ModelState.AddModelError("DescontoPercentual", "Informe o percentual de desconto.");

            if (p.TipoDesconto == Enums.TipoDesconto.PrecoFixo && !p.PrecoFixo.HasValue)
                ModelState.AddModelError("PrecoFixo", "Informe o preço promocional.");

            if (p.DataFim < p.DataInicio)
                ModelState.AddModelError("DataFim", "A data de fim deve ser após a data de início.");
        }
    }
}