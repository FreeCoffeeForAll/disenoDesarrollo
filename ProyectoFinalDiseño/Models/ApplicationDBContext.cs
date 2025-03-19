using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YourApp.Models;

namespace ProyectoFinalDiseño.Models
{
    public class ApplicationDbContext : IdentityDbContext<UserApplication>
    {
        public DbSet<Subscription> Subscriptions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Ensure identity tables are created
                                           // Define the relationship between Subscription and UserApplication
            builder.Entity<Subscription>()
                .HasOne(s => s.UserApplication) // A Subscription has one UserApplication
                .WithMany(u => u.Subscriptions) // A UserApplication can have many Subscriptions
                .HasForeignKey(s => s.UserId) // The foreign key in Subscription is UserId
                .OnDelete(DeleteBehavior.Cascade); // Optional: Define the delete behavior

            // You can configure other properties and relationships here as needed
        }
    }
}