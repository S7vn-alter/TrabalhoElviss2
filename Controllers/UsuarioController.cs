using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text;
using TrabalhoElvis2.Context;
using TrabalhoElvis2.Models;

namespace TrabalhoElvis2.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly LoginContext _context;

        public UsuarioController(LoginContext context)
        {
            _context = context;
        }

        // --- CADASTRO (GET) ---
        public IActionResult Cadastrar()
        {
            return View();
        }

        // --- CADASTRO (POST) ---
        [HttpPost]
        public IActionResult Cadastrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Usuarios.Add(usuario);
                    _context.SaveChanges();

                    TempData["MensagemSucesso"] = "Cadastro realizado com sucesso! Faça login para continuar.";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Erro ao salvar: {ex.Message}");
                    ModelState.AddModelError("", "Erro ao salvar no banco de dados.");
                }
            }
            return View(usuario);
        }

        // --- LOGIN (GET) ---
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // --- LOGIN (POST) ---
        [HttpPost]
        public IActionResult Login(string email, string senha, string tipoUsuario)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha) || string.IsNullOrWhiteSpace(tipoUsuario))
            {
                ViewBag.Erro = "Preencha todos os campos!";
                return View();
            }

            string Normalizar(string texto)
            {
                return new string(texto
                    .Normalize(NormalizationForm.FormD)
                    .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    .ToArray())
                    .ToLower();
            }

            string tipoNormalizado = Normalizar(tipoUsuario);
            string emailNormalizado = email.ToLower().Trim();
            string senhaNormalizada = senha.Trim();

            // ✅ Corrigido: agora o EF entende a consulta
            var usuarios = _context.Usuarios
                .Where(u => u.Email.ToLower() == emailNormalizado && u.Senha == senhaNormalizada)
                .ToList();

            var usuario = usuarios.FirstOrDefault(u =>
                Normalizar(u.TipoUsuario) == tipoNormalizado
            );

            if (usuario == null)
            {
                ViewBag.Erro = "E-mail, senha ou tipo de usuário incorretos!";
                return View();
            }

            // ✅ Salva dados na sessão
            HttpContext.Session.SetString("TipoUsuario", usuario.TipoUsuario);
            HttpContext.Session.SetString("NomeUsuario", usuario.NomeAdministrador ?? usuario.NomeCompleto ?? "Usuário");
            HttpContext.Session.SetInt32("IdUsuario", usuario.Id);

            // ✅ Redireciona conforme o tipo
            return RedirectToAction("Interface");
        }

        // --- INTERFACE ---
        public IActionResult Interface()
        {
            var tipo = HttpContext.Session.GetString("TipoUsuario");

            if (string.IsNullOrEmpty(tipo))
                return RedirectToAction("Login");

            return tipo switch
            {
                "Administrador" => RedirectToAction("PainelAdministrador"),
                "Síndico" => View("InterfaceSindico"),
                "Morador" => View("InterfaceMorador"),
                _ => RedirectToAction("Login")
            };
        }

        // --- PAINEL ADMIN ---
        public IActionResult PainelAdministrador()
        {
            var tipo = HttpContext.Session.GetString("TipoUsuario");
            var nome = HttpContext.Session.GetString("NomeUsuario");

            if (tipo != "Administrador")
                return RedirectToAction("AcessoNegado");

            ViewBag.NomeUsuario = nome;
            return View();
        }

        // --- ACESSO NEGADO ---
        public IActionResult AcessoNegado()
        {
            return View();
        }

        // --- LOGOUT ---
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}