using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinalDiseño.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Add your application tables
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            base.OnModelCreating(builder); // Ensure identity tables are created
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
            modelBuilder.Entity<Inventario>()
                .HasOne(i => i.Categoria)
                .WithMany()
                .HasForeignKey(i => i.CategoriaID);
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
=======

            base.OnModelCreating(modelBuilder);
        }
>>>>>>> Stashed changes

            base.OnModelCreating(modelBuilder);
        }
>>>>>>> Stashed changes

            base.OnModelCreating(modelBuilder);
        }
>>>>>>> Stashed changes

            // Define primary key for Categoria explicitly if necessary
            builder.Entity<Categoria>().HasKey(c => c.CategoriaID);
        }
    }
}