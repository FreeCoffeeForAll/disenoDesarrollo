using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ProyectoFinalDiseño.Models
{
    public class ApplicationDbContext : IdentityDbContext<UserApplication>
    {
        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<UserVisit> UserVisits { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingExercise> TrainingExercises { get; set; }
        public DbSet<ClientTraining> ClientTrainings { get; set; }
        public DbSet<ExerciseProgress> ExerciseProgresses { get; set; }

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

            builder.Entity<TrainingExercise>()
        .HasKey(te => new { te.TrainingId, te.ExerciseId });

            builder.Entity<TrainingExercise>()
                .HasOne(te => te.Training)
                .WithMany(t => t.TrainingExercises)
                .HasForeignKey(te => te.TrainingId);

            builder.Entity<TrainingExercise>()
                .HasOne(te => te.Exercise)
                .WithMany()
                .HasForeignKey(te => te.ExerciseId);
        }
    }
}