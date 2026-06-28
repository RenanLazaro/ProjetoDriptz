using Microsoft.AspNetCore.Mvc;
using ProjetoDriptz.Models;
using ProjetoDriptz.Repositorio.Interfaces;

namespace ProjetoDriptz.Controllers
{
   // [PaginaParaUsuarioLogado]

    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public IActionResult Index()
        {
            var usuarios = _usuarioRepositorio.BuscarTodos();
            return View(usuarios);
        }

        public IActionResult Criar()
        {
            return View();
        }

        public IActionResult ApagarConfirmacao(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorIId(id);
            return View(usuario);
        }

        public IActionResult Editar(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorIId(id);
            return View(usuario);
        }

        public IActionResult Apagar(int id)
        {
            try
            {

                bool apagado = _usuarioRepositorio.Excluir(id);

                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Usuario Apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não conseguimos apagar seu usuario!";
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                TempData["MensagemErro"] = "Ops, não conseguimos apagar seu usuario!";
                return RedirectToAction("Index");
            }

        }


        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _usuarioRepositorio.Adicionar(usuario);
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
                    return RedirectToAction("Index");

                }

                return View(usuario);

            }
            catch (Exception erro)
            {


                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar seu usuário, tente novamente. Detalhes do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public IActionResult Alterar(UsuarioModel usuarioSemSenhaModel)
        {
            try
            {
                // Remover a validação de senha ANTES de validar o Model
                ModelState.Remove("Senha");

                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.Editar(usuarioSemSenhaModel);

                    TempData["MensagemSucesso"] = "Usuário alterado com sucesso!";
                    return RedirectToAction("Index");
                }

                // Se der erro de validação, retorna com os dados
                return View("Editar", usuarioSemSenhaModel);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos atualizar o usuário. Detalhes: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

    }
}
