using Microsoft.AspNetCore.Mvc;
using TrabalhoElvis2.Context;
using TrabalhoElvis2.Models;

namespace TrabalhoElvis2.Controllers
{
    public class ContratoController : Controller
    {
        private readonly LoginContext _context;

        public ContratoController(LoginContext context)
        {
            _context = context;
        }

        // GET: Contrato
        public IActionResult Index()
        {
            // 🔒 Verifica se o usuário é administrador
            var tipo = HttpContext.Session.GetString("TipoUsuario");
            if (tipo != "Administrador")
            {
                TempData["MensagemErro"] = "Acesso restrito a administradores.";
                return RedirectToAction("Login", "Usuario");
            }

            // Lista todos os contratos do banco
            var contratos = _context.Contratos.ToList();
            return View(contratos);
        }

        // POST: Contrato/Cadastrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(Contrato contrato)
        {
            var tipo = HttpContext.Session.GetString("TipoUsuario");
            if (tipo != "Administrador")
            {
                TempData["MensagemErro"] = "Acesso restrito a administradores.";
                return RedirectToAction("Login", "Usuario");
            }

            if (ModelState.IsValid)
            {
                _context.Contratos.Add(contrato);
                _context.SaveChanges();

                TempData["MensagemSucesso"] = "Contrato cadastrado com sucesso!";
                return RedirectToAction("Index");
            }

            TempData["MensagemErro"] = "Erro ao cadastrar o contrato.";
            return RedirectToAction("Index");
        }
    }
}