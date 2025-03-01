using Microsoft.EntityFrameworkCore;

namespace ProyectoFinalDiseño.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Inventario> Inventario { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().HasKey(c => c.CategoriaID); // Define la clave primaria si no la detecta
        }

    }
}
