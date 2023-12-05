using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult listaDeUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View(new UsuarioService().Listar());
        }

          public IActionResult RegistrarUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);
            return View();
        }

        [HttpPost]
       public IActionResult RegistrarUsuarios(Usuario novoUsuario)
       {
            novoUsuario.Senha = Criptografo.TextoCriptografado(novoUsuario.Senha);
            UsuarioService us = new UsuarioService();
            us.incluirUsuario(novoUsuario);
            return RedirectToAction("CadastroRealizado");
       }

       public IActionResult CadastroRealizado()
       {
           Autenticacao.CheckLogin(this);
           Autenticacao.verificaSeUsuarioEAdmin(this);
           return View();
       }

       public IActionResult EditarUsuario(int id)
       {
           Usuario u = new UsuarioService().Listar(id);
           return View(u);
       }
       [HttpPost]
       public IActionResult EditarUsuario(Usuario userEditado)
       {
            UsuarioService us = new UsuarioService();
            us.editarUsuario(userEditado);
            return RedirectToAction("ListaDeUsuarios");
       }

        public IActionResult ExcluirUsuario(int id)
        {
            return View(new UsuarioService().Listar(id));
        }

        [HttpPost]
         public IActionResult ExcluirUsuario(string decisao, int id)
         {
             if(decisao == "Excluir")
             {
                 ViewData["Mensagem"] = "Exclusão do usuário " + new UsuarioService().Listar(id).Nome + "realizada com sucesso";
                 new UsuarioService().excluirUsuario(id);
                 return View("ListaDeUsuarios", new UsuarioService().Listar());
             }
             else{
                 ViewData["Mensagem"] = "Exclusão Cancelada";
                 return View("ListaDeUsuarios", new UsuarioService().Listar());
             }
         }


      public IActionResult Sair()
      {
          HttpContext.Session.Clear();
          return RedirectToAction("Index", "Home");
      }

        public IActionResult NeedAdmin()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }

    }
}