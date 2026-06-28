using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoDriptz.Helper;
using ProjetoDriptz.Models;
using ProjetoDriptz.Repositorio.Interfaces;

namespace ProjetoDriptz.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private IUsuarioRepositorio _usuarioRepositorio;
        private ISessao _sessao;
        private IEmail _email;
        public LoginController(IUsuarioRepositorio usuarioRepositorio,ISessao sessao, IEmail email)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _email = email;
        }

        public IActionResult Index()
        {
            try
            {
                // Se o usuário já estiver logado, redireciona para a home
                var usuarioLogado = _sessao.BuscarSessaoUsuario();
                if (usuarioLogado != null)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                // Se houver erro ao buscar sessão, limpa tudo
                HttpContext.Session.Clear();
            }

            return View();
        }

        public IActionResult RedefinirSenha()
        {
            return View();
        }
        public IActionResult Sair()
        {
            _sessao.RemoverSessaoUsuario();
            return RedirectToAction("Index", "Login");
        }

        public IActionResult LimparSessao()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Session");
            TempData["MensagemSucesso"] = "Sessão limpa com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorLogin(loginModel.Login);

                    if (usuario != null)
                    {

                        if (usuario.SenhaValida(loginModel.Senha))
                        {
                            _sessao.CriarSessaoUsuario(usuario);
                            return RedirectToAction("Index", "Home");
                        }
                                            
                            TempData["MensagemErro"] = $"Senha inválida, tente novamente.";
                            return View("Index");
                                       
                    }
                   
                    TempData["MensagemErro"] = $"Senha ou Usuario Inválido, tente novamente.";
                }
                return View("Index");
            }
            catch (Exception erro)
            {

                TempData["MensagemErro"] = $"Ops, não conseguimos realizar seu login, tente novamente. Detalhes do erro: {erro.Message}";
                return RedirectToAction("Index");

            }
                                     
        }

        [HttpPost]
        
        public IActionResult EnviarLinkParaRedefinirSenha(RedefinirSenhaModel redefinirSenhaModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorEmailELogin(redefinirSenhaModel.Email,redefinirSenhaModel.Login);

                    if (usuario != null)
                    {

                        string novaSenha = usuario.GerarNovaSenha();

                   
                        string mensagem = $"Sua nova senha é: {novaSenha}";

                      bool emailEnviado =   _email.Enviar(usuario.Email, "Driptz - Redefinição de Senha", mensagem);

                        if(emailEnviado)
                        {
                            _usuarioRepositorio.Editar(usuario);
                            TempData["MensagemSucesso"] = $"Um link para redefinição de senha foi enviado para o email cadastrado.";
                        }
                        else
                        {
                            TempData["MensagemErro"] = $"Não foi possível enviar o email de redefinição de senha. Por favor, tente novamente mais tarde.";
                        }

                         return RedirectToAction("Index", "Login");

                    }

                    TempData["MensagemErro"] = $"Não conseguimos redefinir sua senha. por favor, verifique os dados informados.";
                }
                return View("Index");
            }
         /*   catch (Exception erro)
            {

                TempData["MensagemErro"] = $"Ops, não conseguimos redefinir sua senha, tente novamente. Detalhes do erro: {erro.Message}";
                return RedirectToAction("Index");

            }*/

            catch (Exception ex)
            {
                // TEMPORÁRIO - para debug
                Console.WriteLine($"Erro ao enviar email: {ex.Message}");
                Console.WriteLine($"InnerException: {ex.InnerException?.Message}");
                throw; // Vai mostrar o erro completo
            }
        }


    }
}
