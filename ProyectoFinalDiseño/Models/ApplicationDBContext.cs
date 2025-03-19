using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinalDiseño.Models
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public Microsoft.EntityFrameworkCore.DbSet<Categoria> Categorias { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Inventario> Inventario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la relación entre Inventario y Categoría
            modelBuilder.Entity<Inventario>()
                .HasOne(i => i.Categoria)
                .WithMany()
                .HasForeignKey(i => i.CategoriaID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}