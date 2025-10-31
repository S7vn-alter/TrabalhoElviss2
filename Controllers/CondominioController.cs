using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoElvis2.Context;
using TrabalhoElvis2.Models;

namespace TrabalhoElvis2.Controllers
{
    public class CondominioController : Controller
    {
        private readonly LoginContext _context;

        public CondominioController(LoginContext context)
        {
            _context = context;
        }

        // --- Cadastrar Condomínio ---
        public IActionResult Cadastrar(int adminId)
        {
            var admin = _context.Usuarios.FirstOrDefault(u => u.Id == adminId && u.TipoUsuario == "Administrador");
            if (admin == null)
                return RedirectToAction("Login", "Usuario");

            var condominio = new Condominio
            {
                AdminUsuarioId = admin.Id
            };

            return View(condominio);
        }

        [HttpPost]
        public IActionResult Cadastrar(Condominio condominio)
        {
            if (!ModelState.IsValid)
                return View(condominio);

            bool existe = _context.Condominios.Any(c => c.AdminUsuarioId == condominio.AdminUsuarioId);
            if (existe)
            {
                TempData["MensagemErro"] = "Você já possui um condomínio cadastrado!";
                return RedirectToAction("Dashboard", new { adminId = condominio.AdminUsuarioId });
            }

            _context.Condominios.Add(condominio);
            _context.SaveChanges();

            TempData["MensagemSucesso"] = "Condomínio cadastrado com sucesso!";
            return RedirectToAction("Dashboard", new { adminId = condominio.AdminUsuarioId });
        }

        // --- Dashboard ---
        public IActionResult Dashboard(int adminId)
        {
            var condominio = _context.Condominios
                .Include(c => c.Admin)
                .FirstOrDefault(c => c.AdminUsuarioId == adminId);

            if (condominio == null)
                return RedirectToAction("Cadastrar", new { adminId });

            return View(condominio);
        }
    }
}