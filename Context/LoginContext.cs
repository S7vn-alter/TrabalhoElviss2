using Microsoft.EntityFrameworkCore;
using TrabalhoElvis2.Models;

namespace TrabalhoElvis2.Context
{
    public class LoginContext : DbContext
    {
        public LoginContext(DbContextOptions<LoginContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Condominio> Condominios { get; set; }
    }
}