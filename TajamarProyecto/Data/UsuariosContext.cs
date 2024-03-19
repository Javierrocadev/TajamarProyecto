using Microsoft.EntityFrameworkCore;
using TajamarProyecto.Models;

namespace TajamarProyecto.Data
{
    public class UsuariosContext : DbContext
    {
        public UsuariosContext(DbContextOptions<UsuariosContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Empresa> Empresas { get; set; }

        public DbSet<EntrevistaAlumno> EntrevistasAlumnos { get; set; }
    }

}
