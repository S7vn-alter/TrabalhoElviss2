using Microsoft.AspNetCore.Mvc;

using System.Globalization;
using System.Text;
using TrabalhoElvis2.Context;
using TrabalhoElvis2.Models;

namespace TrabalhoElvis.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly LoginContext _context;

        public UsuarioController(LoginContext context)
        {
            _context = context;
        }

        // --- CADASTRO ---
        public IActionResult Cadastrar()
        {
            return View();
        }

       [HttpPost]
public IActionResult Cadastrar(Usuario usuario)
{
    // Verifica se todos os campos obrigatórios foram preenchidos corretamente
    if (ModelState.IsValid)
    {
        try
        {
            // Adiciona o usuário ao banco
            _context.Usuarios.Add(usuario);

            // Salva as alterações no banco
            int registros = _context.SaveChanges();

            // Log no console para depuração
            Console.WriteLine($"✅ Usuário cadastrado com sucesso! Registros salvos: {registros}");
            Console.WriteLine($"📧 Email: {usuario.Email} | Tipo: {usuario.TipoUsuario}");

            // Mensagem de sucesso temporária (para o Login)
            TempData["MensagemSucesso"] = "Cadastro realizado com sucesso! Faça login para continuar.";

            // Redireciona para a tela de login
            return RedirectToAction("Login", "Usuario");
        }
        catch (Exception ex)
        {
            // Se der algum erro no banco, mostra no console
            Console.WriteLine($"❌ Erro ao salvar no banco: {ex.Message}");
            ModelState.AddModelError("", "Erro ao salvar o usuário no banco de dados.");
        }
    }
    else
    {
        Console.WriteLine("⚠️ ModelState inválido (algum campo obrigatório está vazio).");
    }

    // Se algo deu errado, retorna pra view com os dados digitados
    return View(usuario);
}

        [HttpPost]
        public IActionResult Login(string email, string senha, string tipoUsuario)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha) || string.IsNullOrWhiteSpace(tipoUsuario))
            {
                ViewBag.Erro = "Preencha todos os campos!";
                return View();
            }

            // 🔹 Função para normalizar acentuação e letras
            string Normalizar(string texto)
            {
                return new string(texto
                    .Normalize(NormalizationForm.FormD)
                    .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    .ToArray())
                    .ToLower();
            }

            string tipoNormalizado = Normalizar(tipoUsuario);

            // 🔹 Carrega todos os usuários na memória antes do filtro (para poder usar Normalizar)
            var usuarios = _context.Usuarios.ToList();

            var usuario = usuarios.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                u.Senha == senha &&
                Normalizar(u.TipoUsuario) == tipoNormalizado
            );

            if (usuario == null)
            {
                ViewBag.Erro = "E-mail, senha ou tipo de usuário incorretos!";
                return View();
            }

            TempData["TipoUsuario"] = usuario.TipoUsuario;
            TempData["Nome"] = usuario.NomeAdministrador ?? usuario.NomeCompleto;
            TempData["IdUsuario"] = usuario.Id;

            return RedirectToAction("Interface");
        }

        // --- INTERFACE PRINCIPAL ---
        public IActionResult Interface()
        {
            var tipo = TempData["TipoUsuario"]?.ToString();
            var idUsuario = TempData["IdUsuario"]?.ToString();

            if (tipo == null || idUsuario == null)
                return RedirectToAction("Login");

            int id = int.Parse(idUsuario);

            switch (tipo)
            {
                case "Administrador":
                    bool temCondominio = _context.Condominios.Any(c => c.AdminUsuarioId == id);

                    if (temCondominio)
                        return RedirectToAction("Dashboard", "Condominio", new { adminId = id });
                    else
                        return RedirectToAction("Cadastrar", "Condominio", new { adminId = id });

                case "Síndico":
                    return View("InterfaceSindico");

                case "Morador":
                    return View("InterfaceMorador");

                default:
                    return RedirectToAction("Login");
            }
        }

        // --- LOGOUT ---
        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}